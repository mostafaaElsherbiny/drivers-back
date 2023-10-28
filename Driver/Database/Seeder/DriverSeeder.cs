

using System.Data.SQLite;
using System.Text;
using Driver.Entities;


namespace Driver.Database.Seeder;
public class DriverSeeder
{

    private readonly ISqliteConnectionFactory _connectionFactory;

    public DriverSeeder(ISqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<DriverEntity> Seed()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var countCommand = new SQLiteCommand("SELECT COUNT(*) FROM Drivers", connection);
        var count = Convert.ToInt32(countCommand.ExecuteScalar());

        if (count > 0)
        {
            using var deleteCommand = new SQLiteCommand("DELETE FROM Drivers", connection);
        }

        var driversList = getDummyDriversData();

        var createdDrivers = new List<DriverEntity>();

        foreach (var driver in driversList)
        {
            // Check if the email or phone number already exists
            using (var checkCommand = new SQLiteCommand(
                "SELECT COUNT(*) FROM Drivers WHERE Email = @Email OR PhoneNumber = @PhoneNumber;",
                connection))
            {
                checkCommand.Parameters.AddWithValue("@Email", driver.Email);
                checkCommand.Parameters.AddWithValue("@PhoneNumber", driver.PhoneNumber);

                var existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());


                if (existingCount == 0)
                {
                    // Insert the driver if it doesn't exist in the database
                    using (var insertCommand = new SQLiteCommand(
                        "INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber);",
                        connection))
                    {
                        insertCommand.Parameters.AddWithValue("@FirstName", driver.FirstName);
                        insertCommand.Parameters.AddWithValue("@LastName", driver.LastName);
                        insertCommand.Parameters.AddWithValue("@Email", driver.Email);
                        insertCommand.Parameters.AddWithValue("@PhoneNumber", driver.PhoneNumber);
                        insertCommand.ExecuteNonQuery();

                        driver.Id = (int)connection.LastInsertRowId;
                        createdDrivers.Add(driver);
                    }


                }
            }
        }

        connection.Close();

        return createdDrivers;

    }


    public string GenerateRandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        var random = new Random();
        var result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(chars.Length);
            result.Append(chars[index]);
        }

        return result.ToString();
    }


    public IEnumerable<DriverEntity> getDummyDriversData()
    {

        var driversList = new List<DriverEntity>();
        for (int i = 0; i < 5; i++)
        {
            var FirstName = GenerateRandomString(5);
            var LastName = GenerateRandomString(5);
            var Email = FirstName + "." + i + "@gmail.com";
            var driver = new DriverEntity
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = "10000000" + "" + i
            };

            driversList.Add(driver);
        }

        return driversList;

    }

}
