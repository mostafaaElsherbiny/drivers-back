using Moq;
using Driver.Controllers;
using Driver.Entities;
using Driver.Services;
using Xunit;
namespace Driver.Tests.Controllers;
public class DriverControllerTest
{
    private readonly DriverController _controller;
    private readonly Mock<IDriverService> _driverServiceMock;
    public DriverControllerTest()
    {
        _controller = new DriverController(_driverServiceMock.Object);

        _driverServiceMock = new Mock<IDriverService>();
    }

    [Fact]
    public void Get_ReturnsOkResultWithDrivers()
    {

        var expectedDrivers = new List<Driver>
            {

                new Driver { FirstName = "mustafa", LastName = "elbadawy", Email = "mustafa.sherra@gmail.com", PhoneNumber = "01000942765"},
                new Driver { FirstName = "mustafa2", LastName = "elbadawy2", Email = "mustafa.sherra@gmail.com2", PhoneNumber = "010009427652"},

            };
        _driverServiceMock.Setup(x => x.Index()).Returns(expectedDrivers);

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result);

        var actualDrivers = Assert.IsAssignableFrom<IEnumerable<Driver>>(okResult.Value);

        Assert.Equal(expectedDrivers, actualDrivers);
    }

    [Fact]
    public void Create_Driver_ReturnsOkResult()
    {
        var validDriver = new Driver { FirstName = "mustafa", LastName = "elbadawy", Email = "mustafa.sherra@gmail.com", PhoneNumber = "01000942765" };

        _driverServiceMock.Setup(x => x.Create(validDriver))
            .Returns(validDriver);


        var controller = new DriverController(_driverServiceMock.Object);


        var result = controller.Create(validDriver);

        Assert.IsType<OkObjectResult>(result.Result);

        var okResult = result.Result as OkObjectResult;

        Assert.IsType<Driver>(okResult?.Value);


        Assert.NotNull(okResult?.Value);

        Assert.Equal(validDriver.FirstName, (okResult.Value as Driver).FirstName);

    }

    [Fact]
    public void GetById_ReturnsOkResultWithDriver()
    {

        var validDriver = new Driver { Id = 1, FirstName = "mustafa", LastName = "elbadawy", Email = "mustafa.sherra@gmail.com", PhoneNumber = "01000942765" };

        _driverServiceMock.Setup(x => x.Show(validDriver.Id)).Returns(validDriver);

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.GetById(validDriver.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);

        var actualTodo = Assert.IsType<Driver>(okResult.Value);

        Assert.Equal(validDriver, actualTodo);
    }


    [Fact]
    public void Update_ReturnsOkResultWithUpdatedDriver()
    {
        var validDriver = new Driver { Id = 1, FirstName = "mustafa", LastName = "elbadawy", Email = "mustafa.sherra@gmail.com", PhoneNumber = "01000942765" };

        _driverServiceMock.Setup(x => x.Update(validDriver.Id, validDriver)).Returns(validDriver);

        var controller = new DriverController(_driverServiceMock.Object);

        // Act
        var result = controller.Update(validDriver.Id, validDriver);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var actualDriver = Assert.IsType<Driver>(okResult.Value);

        Assert.Equal(validDriver, actualDriver);


    }

    [Fact]
    public void Delete_ReturnsOkResultWhenDriverIsDeleted()
    {

        _driverServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(true);

        var controller = new DriverController(_driverServiceMock.Object);

        var result = controller.Delete(1);

        Assert.IsType<OkResult>(result);
    }


}
