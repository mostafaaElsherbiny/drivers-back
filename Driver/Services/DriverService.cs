using System.Data.SQLite;
using Driver.Database;
using Driver.Database.Seeder;
using Driver.Entities;
using Microsoft.OpenApi.Models;
namespace Driver.Services;

public class DriverService : IDriverService
{
    private readonly ISqliteConnectionFactory _connectionFactory;



    public DriverService(ISqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;

    }

    public IEnumerable<DriverEntity> Index()
    {
        using var connection = _connectionFactory.CreateConnection();


        connection.Open();

        using var command = new SQLiteCommand("SELECT *  FROM Drivers", connection);

        using var reader = command.ExecuteReader();

        var drivers = new List<DriverEntity>();

        while (reader.Read())
        {
            if (reader.HasRows)
            {
                var driver = new DriverEntity
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    Email = reader["Email"].ToString() ?? string.Empty,
                    PhoneNumber = reader["PhoneNumber"].ToString() ?? string.Empty,
                };

                drivers.Add(driver);
            }
        }

        connection.Close();
        return drivers;

    }


    public DriverEntity Show(int id)
    {

        using var connection = _connectionFactory.CreateConnection();

        connection.Open();

        using var command = new SQLiteCommand("SELECT * FROM Drivers WHERE Id = @Id", connection);

        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        var driver = new DriverEntity();

        if (!reader.HasRows)
        {
            throw new Exception("Driver not found");
        }

        while (reader.Read())
        {
            driver = new DriverEntity
            {
                Id = Convert.ToInt32(reader["Id"]),

                FirstName = reader["FirstName"].ToString() ?? string.Empty,
                LastName = reader["LastName"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                PhoneNumber = reader["PhoneNumber"].ToString() ?? string.Empty,

            };
        }

        connection.Close();

        return driver;

    }



    public DriverEntity Create(DriverEntity driver)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        // Check if the email already exists
        using (var checkEmailCommand = new SQLiteCommand("SELECT Id FROM Drivers WHERE Email = @Email", connection))
        {
            checkEmailCommand.Parameters.AddWithValue("@Email", driver.Email);

            var existingEmailDriverId = checkEmailCommand.ExecuteScalar();

            if (existingEmailDriverId != null)
            {
                // Email already exists, so you can return an error or throw an exception as needed
                // For example, you can throw an exception like this:
                throw new Exception("Email already exists");
            }
        }

        // Check if the phone number already exists
        using (var checkPhoneNumberCommand = new SQLiteCommand("SELECT Id FROM Drivers WHERE PhoneNumber = @PhoneNumber", connection))
        {
            checkPhoneNumberCommand.Parameters.AddWithValue("@PhoneNumber", driver.PhoneNumber);

            var existingPhoneNumberDriverId = checkPhoneNumberCommand.ExecuteScalar();

            if (existingPhoneNumberDriverId != null)
            {
                // Phone number already exists, so you can return an error or throw an exception as needed
                // For example, you can throw an exception like this:
                throw new Exception("Phone number already exists");
            }
        }

        // If the code reaches this point, it means neither email nor phone number already exists, so you can proceed with the insertion.
        using (var insertCommand = new SQLiteCommand("INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber)", connection))
        {
            insertCommand.Parameters.AddWithValue("@FirstName", driver.FirstName);
            insertCommand.Parameters.AddWithValue("@LastName", driver.LastName);
            insertCommand.Parameters.AddWithValue("@Email", driver.Email);
            insertCommand.Parameters.AddWithValue("@PhoneNumber", driver.PhoneNumber);

            var rowsAffected = insertCommand.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new Exception("Failed to create driver");
            }

            driver.Id = (int)connection.LastInsertRowId;
        }

        return driver;
    }


    public DriverEntity Update(int id, DriverEntity driver)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        // Check if the email already exists
        using (var checkEmailCommand = new SQLiteCommand("SELECT Id FROM Drivers WHERE Email = @Email AND Id != @Id", connection))
        {
            checkEmailCommand.Parameters.AddWithValue("@Email", driver.Email);
            checkEmailCommand.Parameters.AddWithValue("@Id", id);

            var existingEmailDriverId = checkEmailCommand.ExecuteScalar();

            if (existingEmailDriverId != null)
            {
                // Email already exists for a different driver, so you can throw an exception
                throw new Exception("Email already exists for another driver");
            }
        }

        // Check if the phone number already exists
        using (var checkPhoneNumberCommand = new SQLiteCommand("SELECT Id FROM Drivers WHERE PhoneNumber = @PhoneNumber AND Id != @Id", connection))
        {
            checkPhoneNumberCommand.Parameters.AddWithValue("@PhoneNumber", driver.PhoneNumber);
            checkPhoneNumberCommand.Parameters.AddWithValue("@Id", id);

            var existingPhoneNumberDriverId = checkPhoneNumberCommand.ExecuteScalar();

            if (existingPhoneNumberDriverId != null)
            {
                // Phone number already exists for a different driver, so you can throw an exception
                throw new Exception("Phone number already exists for another driver");
            }
        }

        // Check if the driver with the given ID exists
        using (var checkDriverExistsCommand = new SQLiteCommand("SELECT COUNT(*) FROM Drivers WHERE Id = @Id", connection))
        {
            checkDriverExistsCommand.Parameters.AddWithValue("@Id", id);

            var driverExistsCount = Convert.ToInt32(checkDriverExistsCommand.ExecuteScalar());

            if (driverExistsCount == 0)
            {
                // Driver with the specified ID does not exist
                throw new Exception("Driver with the specified ID does not exist");
            }
        }

        // If all checks pass, proceed with the update
        using (var updateCommand = new SQLiteCommand("UPDATE Drivers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber WHERE Id = @Id", connection))
        {
            updateCommand.Parameters.AddWithValue("@Id", id);
            updateCommand.Parameters.AddWithValue("@FirstName", driver.FirstName);
            updateCommand.Parameters.AddWithValue("@LastName", driver.LastName);
            updateCommand.Parameters.AddWithValue("@Email", driver.Email);
            updateCommand.Parameters.AddWithValue("@PhoneNumber", driver.PhoneNumber);

            var rowsAffected = updateCommand.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new Exception("Failed to update driver");
            }
        }

        driver.Id = id;

        return driver;
    }


    public bool Delete(int id)
    {
        if (id <= 0)
        {
            throw new Exception("Invalid driver ID");
        }

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using (var checkDriverExistsCommand = new SQLiteCommand("SELECT COUNT(*) FROM Drivers WHERE Id = @Id", connection))
        {
            checkDriverExistsCommand.Parameters.AddWithValue("@Id", id);

            var driverExistsCount = Convert.ToInt32(checkDriverExistsCommand.ExecuteScalar());

            if (driverExistsCount == 0)
            {
                throw new Exception("Driver with the specified ID does not exist");
            }
        }

        using var deleteCommand = new SQLiteCommand("DELETE FROM Drivers WHERE Id = @Id", connection);
        deleteCommand.Parameters.AddWithValue("@Id", id);

        var rowsAffected = deleteCommand.ExecuteNonQuery();

        connection.Close();

        return rowsAffected > 0;
    }




    // GenerateDrivers

    public IEnumerable<DriverEntity> GenerateDrivers()
    {
        var seed = new DriverSeeder(_connectionFactory);

        var drivers = seed.Seed();

        return drivers;

    }


}
