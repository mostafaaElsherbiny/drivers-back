using System.Data.SQLite;
using Driver.Database.Seeder;
using Driver.Entities;
using Driver.Repositories;
namespace Driver.Services;

public class DriverService : IDriverService
{
    private readonly IDriverRepository _driverRepository;
    // private readonly DriverSeeder _driverSeeder;

    public DriverService(IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;

        // _driverSeeder = driverSeeder;
    }

    public IEnumerable<DriverEntity> Index()
    {
        return _driverRepository.Index();

    }


    public DriverEntity Show(int id)
    {
        var driver = _driverRepository.Show(id);
        if (driver.Id == 0)
        {
            throw new Exception("Driver not found");
        }
        return driver;
    }



    public DriverEntity Create(DriverEntity driver)
    {
        var existingEmailDriver = _driverRepository.FindBy("Email", driver.Email);

        if (existingEmailDriver.Email != null)
        {
            throw new Exception("Email already exists");
        }
        var existingPhoneDriver = _driverRepository.FindBy("PhoneNumber", driver.PhoneNumber);

        if (existingPhoneDriver.PhoneNumber != null)
        {
            throw new Exception("Phone number already exists");
        }

        return _driverRepository.Create(driver);
    }


    public DriverEntity Update(int id, DriverEntity driver)
    {
        driver.Id = id;

        var driverExist = _driverRepository.Show(id);

        if (driverExist.Id == 0)
        {
            throw new Exception("driver not exist");
        }

        var existingEmailDriver = _driverRepository.FindBy("Email", driver.Email);

        if (existingEmailDriver.Email != null && existingEmailDriver.Id != driver.Id)
        {
            throw new Exception("Email already exists");
        }
        var existingPhoneDriver = _driverRepository.FindBy("PhoneNumber", driver.PhoneNumber);

        if (existingPhoneDriver.PhoneNumber != null && existingPhoneDriver.Id != driver.Id)
        {
            throw new Exception("Phone number already exists");
        }

        var updated = _driverRepository.Update(id, driver);

        return updated ? driver : throw new Exception("Field Update Driver");
    }


    public bool Delete(int id)
    {
        var driverExist = _driverRepository.Show(id);
        if (driverExist.Id == 0)
        {
            throw new Exception("driver not exist");
        }
        return _driverRepository.Delete(id);

    }

    // public IEnumerable<DriverEntity> GenerateDrivers()
    // {
    //     return _driverSeeder.Seed();
    // }
}
