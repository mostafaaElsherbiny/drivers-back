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
    public void Create_Driver_ReturnsOkResult()
    {
        // Arrange
        var validDriver = new Driver { FirstName = "mustafa", LastName = "elbadawy", Email = "mustafa.sherra@gmail.com", PhoneNumber = "01000942765" };

        _driverServiceMock.Setup(x => x.Create(validDriver))
            .Returns(validDriver);

        // Act
        var result = _controller.Create(validDriver);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);

        var okResult = result.Result as OkObjectResult;

        Assert.IsType<Todo>(okResult?.Value);


        Assert.NotNull(okResult?.Value);

        Assert.Equal(validDriver.FirstName, (okResult.Value as Driver).FirstName);

    }


}
