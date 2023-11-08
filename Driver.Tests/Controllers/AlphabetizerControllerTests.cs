using Xunit;
using Driver.Controllers;
using Microsoft.AspNetCore.Mvc;
using Driver.Helper;

public class AlphabetizerControllerTests
{
    [Fact]
    public void AlphabetizeString_WithValidInput_ReturnsOkResultWithAlphabetizedString()
    {
        // Arrange
        var input = "Oliver Johnson";

        var expectedOutput = "eilOrv hJnnoos";

        var controller = new AlphabetizerController();

        // Act
        var result = controller.AlphabetizeString(input);

        // Assert
        var actionResult = Assert.IsType<ActionResult<APIResponse<string>>>(result);

        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<string>>(okObjectResult.Value);

        var actualOutput = apiResponse.Data;

        Assert.Equal(expectedOutput, actualOutput);
    }

    [Fact]
    public void AlphabetizeString_EmptyInput_ReturnsBadRequest()
    {
        var controller = new AlphabetizerController();

        string input = "";

        var result = controller.AlphabetizeString(input);


        var actionResult = Assert.IsType<ActionResult<APIResponse<string>>>(result);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<string>>(badRequestObjectResult.Value);

        Assert.Equal("Input string is empty or null.", apiResponse.Message);

    }

    [Fact]
    public void AlphabetizeString_NullInput_ReturnsBadRequest()
    {
        var controller = new AlphabetizerController();

        string input = "";


        var result = controller.AlphabetizeString(input);


        var actionResult = Assert.IsType<ActionResult<APIResponse<string>>>(result);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

        var apiResponse = Assert.IsType<APIResponse<string>>(badRequestObjectResult.Value);

        Assert.Equal("Input string is empty or null.", apiResponse.Message);
    }
}
