using Driver.Entities;

namespace Driver.Services;
public interface IDriverService
{
    IEnumerable<DriverEntity> Index();

    DriverEntity Show(int id);

    DriverEntity Create(DriverEntity driver);

    DriverEntity Update(int id, DriverEntity driver);

    bool Delete(int id);
    bool GenerateDrivers();

}
