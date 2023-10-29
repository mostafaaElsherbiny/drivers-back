using Driver.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity.Core.Common.EntitySql;

namespace Driver.Controllers;
[Route("/alphabetizer")]
[ApiController]
public class AlphabetizerController : ControllerBase
{
    [HttpGet("")]
    public ActionResult<APIResponse<string>> AlphabetizeString([FromQuery] string input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return BadRequest(APIResponse<string>.Error("Input string is empty or null."));
            }

            return Ok(APIResponse<string>.Success(Alphabetize(input)));
        }
        catch (Exception ex)
        {
            return StatusCode(500, APIResponse<string>.Error(ex.Message));
        }
    }


    private string Alphabetize(string input)
    {
        var words = input.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            var characters = words[i].ToCharArray();

            Array.Sort(characters, (x, y) => char.ToLower(x).CompareTo(char.ToLower(y)));

            words[i] = new string(characters);
        }

        return string.Join(" ", words);

    }
}
