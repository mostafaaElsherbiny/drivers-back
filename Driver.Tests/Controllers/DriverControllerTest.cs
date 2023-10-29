using Moq;

using Driver.Controllers;

using Driver.Entities;

using Driver.Services;

using Xunit;

using Driver.Database.Seeder;

using Driver.Database;

using Microsoft.AspNetCore.Mvc;

using Driver.Helper;

using System.Data.SQLite;

using System.Data.Common;

namespace Driver.Tests.Controllers;

public class DriverControllerTest
{
    private readonly DriverController _controller;
    private readonly Mock<IDriverService> _driverServiceMock;
    private readonly DriverSeeder _driverSeeder;
    private readonly SqliteConnectionFactory _connectionFactory;


    public DriverControllerTest()
    {
        _driverServiceMock = new Mock<IDriverService>();


        _controller = new DriverController(_driverServiceMock.Object);



        _connectionFactory = new SqliteConnectionFactory();


        _driverSeeder = new DriverSeeder(_connectionFactory);
    }

    [Fact]
    public void Get_ReturnsOkResultWithDrivers()
    {

        var expectedDrivers = _driverSeeder.getDummyDriversData();

        Console.WriteLine(expectedDrivers.Count());

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Index()).Returns(expectedDrivers);

        var controller = new DriverController(_driverServiceMock.Object);

        // Act
        var result = controller.Get();

        // Assert
        var actionResult = Assert.IsType<ActionResult<APIResponse<IEnumerable<DriverEntity>>>>(result);



        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<IEnumerable<DriverEntity>>>(okObjectResult.Value);

        var actualDrivers = apiResponse.Data;

        Assert.Equal(expectedDrivers.Count(), actualDrivers.Count());

        Assert.Equal(expectedDrivers, actualDrivers);


    }

    public void GetById_ReturnsOkResultWithDriver()
    {

        var expectedDriver = _driverSeeder.getDummyDriversData().First();

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Show(expectedDriver.Id)).Returns(expectedDriver);

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.GetById(expectedDriver.Id);

        var actionResult = Assert.IsType<ActionResult<APIResponse<DriverEntity>>>(result);

        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<DriverEntity>>(okObjectResult.Value);

        var actualDriver = apiResponse.Data;

        Assert.Equal(expectedDriver, actualDriver);
    }

    [Fact]
    public void Create_WithExistingEmail_ThrowsException()
    {
        var existingDriver = _driverSeeder.getDummyDriversData().First();

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Create(existingDriver)).Throws(new Exception("Email already exists"));

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Create(existingDriver);

        var actionResult = Assert.IsType<ActionResult<APIResponse<DriverEntity>>>(result);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<DriverEntity>>(badRequestObjectResult.Value);

        Assert.True(apiResponse.HasError);

        Assert.Equal("Failed to create driver: Email already exists", apiResponse.Message);

    }

    [Fact]

    public void Create_WithExistingPhoneNumber_ThrowsException()
    {
        var existingDriver = _driverSeeder.getDummyDriversData().First();

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Create(existingDriver)).Throws(new Exception("Phone number already exists"));

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Create(existingDriver);

        var actionResult = Assert.IsType<ActionResult<APIResponse<DriverEntity>>>(result);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<DriverEntity>>(badRequestObjectResult.Value);

        Assert.True(apiResponse.HasError);

        Assert.Equal("Failed to create driver: Phone number already exists", apiResponse.Message);

    }

    //test update

    [Fact]
    public void Update_WithExistingEmail_ThrowsException()
    {
        var existingDriver = _driverSeeder.getDummyDriversData().First();

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Update(existingDriver.Id, existingDriver)).Throws(new Exception("Email already exists"));

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Update(existingDriver.Id, existingDriver);

        var actionResult = Assert.IsType<ActionResult<APIResponse<DriverEntity>>>(result);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<DriverEntity>>(badRequestObjectResult.Value);

        Assert.True(apiResponse.HasError);

        Assert.Equal("Failed to update driver: Email already exists", apiResponse.Message);

    }

    [Fact]

    public void Update_WithExistingPhoneNumber_ThrowsException()
    {
        var existingDriver = _driverSeeder.getDummyDriversData().First();

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Update(existingDriver.Id, existingDriver)).Throws(new Exception("Phone number already exists"));

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Update(existingDriver.Id, existingDriver);

        var actionResult = Assert.IsType<ActionResult<APIResponse<DriverEntity>>>(result);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<DriverEntity>>(badRequestObjectResult.Value);

        Assert.True(apiResponse.HasError);

        Assert.Equal("Failed to update driver: Phone number already exists", apiResponse.Message);

    }

    [Fact]

    public void Update_WithNonExistingDriver_ThrowsException()
    {
        var existingDriver = _driverSeeder.getDummyDriversData().First();

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Update(existingDriver.Id, existingDriver)).Throws(new Exception("Driver with the specified ID does not exist"));

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Update(existingDriver.Id, existingDriver);

        var actionResult = Assert.IsType<ActionResult<APIResponse<DriverEntity>>>(result);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<DriverEntity>>(badRequestObjectResult.Value);

        Assert.True(apiResponse.HasError);

        Assert.Equal("Failed to update driver: Driver with the specified ID does not exist", apiResponse.Message);

    }

    [Fact]
    public void Update_WithValidDriver_ReturnsOkResultWithDriver()
    {
        var existingDriver = _driverSeeder.getDummyDriversData().First();

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Update(existingDriver.Id, existingDriver)).Returns(existingDriver);

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Update(existingDriver.Id, existingDriver);

        var actionResult = Assert.IsType<ActionResult<APIResponse<DriverEntity>>>(result);

        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<DriverEntity>>(okObjectResult.Value);

        var actualDriver = apiResponse.Data;

        Assert.Equal(existingDriver, actualDriver);

    }

    [Fact]
    public void Delete_WithNonExistingDriver_ThrowsException()
    {
        var existingDriver = _driverSeeder.getDummyDriversData().First();

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Delete(existingDriver.Id)).Throws(new Exception("Driver with the specified ID does not exist"));

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Delete(existingDriver.Id);

        var actionResult = Assert.IsType<BadRequestObjectResult>(result);

        var apiResponse = Assert.IsType<APIResponse<DriverEntity>>(actionResult.Value);

        Assert.True(apiResponse.HasError);

        Assert.Equal("Failed to delete driver: Driver with the specified ID does not exist", apiResponse.Message);

    }

    [Fact]
    public void Delete_WithValidDriver_ReturnsOkResultWithDriver()
    {
        var existingDriver = _driverSeeder.getDummyDriversData().First();

        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.Delete(existingDriver.Id)).Returns(true);

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Delete(existingDriver.Id);

        var actionResult = Assert.IsType<OkObjectResult>(result);

        var apiResponse = Assert.IsType<APIResponse<DriverEntity>>(actionResult.Value);

        Assert.False(apiResponse.HasError);

        Assert.Equal("Deleted successfully", apiResponse.Message);

    }

    [Fact]
    public void GenerateDrivers_ReturnsOkResultWithDriver()
    {
        var _driverServiceMock = new Mock<IDriverService>();

        _driverServiceMock.Setup(x => x.GenerateDrivers()).Returns(_driverSeeder.getDummyDriversData());

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.GenerateDrivers();

        var actionResult = Assert.IsType<ActionResult<DriverEntity>>(result);

        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<IEnumerable<DriverEntity>>>(okObjectResult.Value);

        Assert.False(apiResponse.HasError);

        Assert.Equal("Drivers generated successfully", apiResponse.Message);

    }

    [Fact]
    public void AlphabetizeName_ReturnsAlphabetizedName()
    {
        // Arrange
        string name = "Oliver Johnson";

        // Act
        string alphabetizedName = Alphabetizer.AlphabetizeName(name);

        // Assert
        Assert.Equal("eilOrv hJnnoos", alphabetizedName);
    }

}
