using Driver.Entities;

namespace Driver.Repositories;
public interface IDriverRepository
{
    IEnumerable<DriverEntity> Index();

    DriverEntity Show(int id);
    DriverEntity FindBy(string column, string value);
    DriverEntity Create(DriverEntity driver);

    bool Update(int id, DriverEntity driver);

    bool Delete(int id);

}
