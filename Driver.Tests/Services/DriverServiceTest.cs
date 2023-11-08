using Xunit;
using Driver.Services;
using Moq;
using System.Collections.Generic;
using Driver.Entities;
using Driver.Database;
using Driver.Database.Seeder;
using Driver.Repositories;

namespace Driver.Tests.Services;


public class DriverServiceTests
{
    private readonly DriverSeeder _driverSeeder;
    private readonly SqliteConnectionFactory _connectionFactory;

    public DriverServiceTests()
    {
        _connectionFactory = new SqliteConnectionFactory();

        _driverSeeder = new DriverSeeder(_connectionFactory);
    }

    [Fact]
    public void Index_ReturnsListOfDrivers()
    {
        var driverRepositoryMock = new Mock<IDriverRepository>();

        var expectedDrivers = _driverSeeder.getDummyDriversData();

        driverRepositoryMock.Setup(repo => repo.Index()).Returns(expectedDrivers);

        var driverService = new DriverService(driverRepositoryMock.Object);

        var result = driverService.Index();

        Assert.NotNull(result);

        Assert.IsAssignableFrom<IEnumerable<DriverEntity>>(result);

        var drivers = result as IEnumerable<DriverEntity>;

        Assert.Equal(expectedDrivers, drivers);
    }

    [Fact]
    public void Show_ExistingDriver_ReturnsDriver()
    {
        var driverRepositoryMock = new Mock<IDriverRepository>();

        var expectedDriver = _driverSeeder.getDummyDriversData().First();
        expectedDriver.Id = 1;

        driverRepositoryMock.Setup(repo => repo.Show(expectedDriver.Id)).Returns(expectedDriver);

        var driverService = new DriverService(driverRepositoryMock.Object);

        var result = driverService.Show(expectedDriver.Id);

        Assert.NotNull(result);
        Assert.IsType<DriverEntity>(result);
        Assert.Equal(expectedDriver, result);
    }

    [Fact]
    public void Show_NonExistentDriver_ThrowsException()
    {
        var driverRepositoryMock = new Mock<IDriverRepository>();

        var nonExistentDriverId = 999;

        driverRepositoryMock.Setup(repo => repo.Show(nonExistentDriverId)).Returns(new DriverEntity { Id = 0 });

        var driverService = new DriverService(driverRepositoryMock.Object);

        Assert.Throws<Exception>(() => driverService.Show(nonExistentDriverId));
    }



    [Fact]
    public void Create_DuplicateEmail_ThrowsException()
    {
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var driverService = new DriverService(driverRepositoryMock.Object);

        var existingDriverWithDuplicateEmail = new DriverEntity
        {
            Id = 1,
            Email = "existing@example.com",
            PhoneNumber = "111-222-3333"
        };

        driverRepositoryMock.Setup(repo => repo.FindBy("Email", existingDriverWithDuplicateEmail.Email))
            .Returns(existingDriverWithDuplicateEmail);

        var newDriverWithDuplicateEmail = new DriverEntity
        {
            Email = "existing@example.com",
            PhoneNumber = "444-555-6666"
        };

        Assert.Throws<Exception>(() => driverService.Create(newDriverWithDuplicateEmail));
    }

    [Fact]
    public void Create_DuplicatePhoneNumber_ThrowsException()
    {
        var driverRepositoryMock = new Mock<IDriverRepository>();

        var driverService = new DriverService(driverRepositoryMock.Object);

        var existingDriverWithDuplicatePhoneNumber = new DriverEntity
        {
            Id = 2,
            Email = "new@example.com",
            PhoneNumber = "555-666-7777"
        };

        driverRepositoryMock.Setup(repo => repo.FindBy("PhoneNumber", existingDriverWithDuplicatePhoneNumber.PhoneNumber))
            .Returns(existingDriverWithDuplicatePhoneNumber);

        var newDriverWithDuplicatePhoneNumber = new DriverEntity
        {
            Email = "another@example.com",
            PhoneNumber = "555-666-7777"
        };

        Assert.Throws<Exception>(() => driverService.Create(newDriverWithDuplicatePhoneNumber));

    }

    [Fact]
    public void Create_ValidDriver_ReturnsCreatedDriver()
    {
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var driverService = new DriverService(driverRepositoryMock.Object);

        var newDriver = new DriverEntity
        {
            Email = "new@example.com",
            PhoneNumber = "777-888-9999"
        };

        driverRepositoryMock.Setup(repo => repo.FindBy("Email", newDriver.Email))
            .Returns(new DriverEntity());
        driverRepositoryMock.Setup(repo => repo.FindBy("PhoneNumber", newDriver.PhoneNumber))
            .Returns(new DriverEntity());
        driverRepositoryMock.Setup(repo => repo.Create(newDriver))
            .Returns(new DriverEntity { Id = 3, Email = newDriver.Email, PhoneNumber = newDriver.PhoneNumber });

        var result = driverService.Create(newDriver);

        Assert.NotNull(result);

        Assert.IsType<DriverEntity>(result);

        Assert.Equal(3, result.Id);
    }

    [Fact]
    public void Update_DriverExistsAndValidUpdate_ReturnsUpdatedDriver()
    {
        // Arrange
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var driverService = new DriverService(driverRepositoryMock.Object);

        var existingDriver = new DriverEntity
        {
            Id = 1,
            Email = "existing@example.com",
            PhoneNumber = "111-222-3333"
        };

        var updatedDriver = new DriverEntity
        {
            Id = 1,
            Email = "new@example.com",
            PhoneNumber = "444-555-6666"
        };

        driverRepositoryMock.Setup(repo => repo.Show(existingDriver.Id))
            .Returns(existingDriver);
        driverRepositoryMock.Setup(repo => repo.FindBy("Email", updatedDriver.Email))
            .Returns(new DriverEntity());
        driverRepositoryMock.Setup(repo => repo.FindBy("PhoneNumber", updatedDriver.PhoneNumber))
            .Returns(new DriverEntity());
        driverRepositoryMock.Setup(repo => repo.Update(existingDriver.Id, updatedDriver))
            .Returns(true);

        // Act
        var result = driverService.Update(existingDriver.Id, updatedDriver);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<DriverEntity>(result);
        Assert.Equal(updatedDriver.Id, result.Id);
        Assert.Equal(updatedDriver.Email, result.Email);
        Assert.Equal(updatedDriver.PhoneNumber, result.PhoneNumber);
    }

    [Fact]
    public void Update_DriverNotExists_ThrowsException()
    {
        // Arrange
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var driverService = new DriverService(driverRepositoryMock.Object);

        var nonExistentDriverId = 999; // Assume a driver with this ID does not exist
        driverRepositoryMock.Setup(repo => repo.Show(nonExistentDriverId))
            .Returns(new DriverEntity { Id = 0 });

        var updatedDriver = new DriverEntity
        {
            Id = nonExistentDriverId,
            Email = "new@example.com",
            PhoneNumber = "444-555-6666"
        };

        // Act and Assert
        Assert.Throws<Exception>(() => driverService.Update(updatedDriver.Id, updatedDriver));
    }

    [Fact]
    public void Update_DuplicateEmail_ThrowsException()
    {
        // Arrange
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var driverService = new DriverService(driverRepositoryMock.Object);

        var existingDriver = new DriverEntity
        {
            Id = 2,
            Email = "new@example.com",
            PhoneNumber = "555-666-7777"
        };

        var updatedDriverWithDuplicateEmail = new DriverEntity
        {
            Id = 1,
            Email = "new@example.com",
            PhoneNumber = "888-999-0000"
        };

        driverRepositoryMock.Setup(repo => repo.Show(existingDriver.Id))
            .Returns(existingDriver);
        driverRepositoryMock.Setup(repo => repo.FindBy("Email", updatedDriverWithDuplicateEmail.Email))
            .Returns(new DriverEntity { Id = updatedDriverWithDuplicateEmail.Id });

        Assert.Throws<Exception>(() => driverService.Update(existingDriver.Id, updatedDriverWithDuplicateEmail));
    }

    [Fact]
    public void Update_DuplicatePhoneNumber_ThrowsException()
    {
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var driverService = new DriverService(driverRepositoryMock.Object);

        var existingDriver = new DriverEntity
        {
            Id = 3,
            Email = "another@example.com",
            PhoneNumber = "777-888-9999"
        };

        var updatedDriverWithDuplicatePhoneNumber = new DriverEntity
        {
            Id = 1,
            Email = "new@example.com",
            PhoneNumber = "777-888-9999"
        };

        driverRepositoryMock.Setup(repo => repo.Show(existingDriver.Id))
            .Returns(existingDriver);
        driverRepositoryMock.Setup(repo => repo.FindBy("PhoneNumber", updatedDriverWithDuplicatePhoneNumber.PhoneNumber))
            .Returns(new DriverEntity { Id = updatedDriverWithDuplicatePhoneNumber.Id });

        Assert.Throws<Exception>(() => driverService.Update(existingDriver.Id, updatedDriverWithDuplicatePhoneNumber));
    }

    [Fact]
    public void Update_FailedToUpdate_ThrowsException()
    {
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var driverService = new DriverService(driverRepositoryMock.Object);

        var existingDriver = new DriverEntity
        {
            Id = 4,
            Email = "existing@example.com",
            PhoneNumber = "111-222-3333"
        };

        var updatedDriver = new DriverEntity
        {
            Id = 4,
            Email = "new@example.com",
            PhoneNumber = "444-555-6666"
        };

        driverRepositoryMock.Setup(repo => repo.Show(existingDriver.Id))
            .Returns(existingDriver);
        driverRepositoryMock.Setup(repo => repo.FindBy("Email", updatedDriver.Email))
            .Returns(new DriverEntity());
        driverRepositoryMock.Setup(repo => repo.FindBy("PhoneNumber", updatedDriver.PhoneNumber))
            .Returns(new DriverEntity());
        driverRepositoryMock.Setup(repo => repo.Update(existingDriver.Id, updatedDriver))
            .Returns(false);

        Assert.Throws<Exception>(() => driverService.Update(existingDriver.Id, updatedDriver));
    }
}

