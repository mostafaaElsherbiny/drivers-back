using System.Net;
using Microsoft.AspNetCore.Mvc;
using Driver.Entities;
using Driver.Services;

namespace Driver.Controllers;


[ApiController]
[Route("[controller]")]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;


    public DriverController(IDriverService driverService)
    {
        _driverService = driverService;
    }


    [HttpGet(Name = "GetDriver")]
    public ActionResult<APIResponse<IEnumerable<DriverEntity>>> Get()
    {
        try
        {
            return Ok(APIResponse<IEnumerable<DriverEntity>>.Success(_driverService.Index()));
        }
        catch (Exception ex)
        {
            return BadRequest(APIResponse<IEnumerable<DriverEntity>>.Error("Failed to get drivers: " + ex.Message));
        }
    }


    [HttpGet("{id}", Name = "GetDriverById")]
    public ActionResult<DriverEntity> GetById(int id)
    {
        try
        {
            var driver = _driverService.Show(id);

            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred: " + ex.Message);
        }
    }


    [HttpPost(Name = "CreateDriver")]
    public ActionResult<APIResponse<DriverEntity>> Create(DriverEntity driver)
    {
        try
        {
            var createdDriver = _driverService.Create(driver);

            return Ok(APIResponse<DriverEntity>.Created(createdDriver));
        }
        catch (Exception ex)
        {
            return BadRequest(APIResponse<DriverEntity>.Error("Failed to create driver: " + ex.Message));
        }
    }

    [HttpPut("{id}", Name = "UpdateDriver")]
    public ActionResult<APIResponse<DriverEntity>> Update(int id, DriverEntity driver)
    {
        try
        {
            var updatedDriver = _driverService.Update(id, driver);

            if (updatedDriver == null)
            {
                return BadRequest(APIResponse<DriverEntity>.Error("Invalid data for updating a Driver"));
            }

            return Ok(APIResponse<DriverEntity>.Updated(updatedDriver));
        }
        catch (Exception ex)
        {
            return BadRequest(APIResponse<DriverEntity>.Error("Failed to update driver: " + ex.Message));
        }
    }


    [HttpDelete("{id}", Name = "DeleteDriver")]
    public ActionResult Delete(int id)
    {
        try
        {
            var deleted = _driverService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok(APIResponse<DriverEntity>.Deleted());
        }
        catch (Exception ex)
        {
            return BadRequest(APIResponse<DriverEntity>.Error("Failed to delete driver: " + ex.Message));
        }
    }



    //Generate Drivers

    [HttpGet("generate-data", Name = "GetDriverByCar")]
    public ActionResult<DriverEntity> GenerateDrivers()
    {

        try
        {
            var drivers = _driverService.GenerateDrivers();

            if (!drivers)
            {
                return NotFound();
            }

            return Ok(drivers);
        }
        catch (Exception ex)
        {
            return BadRequest(APIResponse<DriverEntity>.Error("Failed to generate drivers: " + ex.Message));
        }
    }


}
