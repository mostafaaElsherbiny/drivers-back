using System.Data.SQLite;
using Driver.Database;
using Driver.Entities;
namespace Driver.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly ISqliteConnectionFactory _connectionFactory;

    public DriverRepository(ISqliteConnectionFactory connectionFactory)
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


    public DriverEntity FindBy(string column, string value)
    {

        using var connection = _connectionFactory.CreateConnection();

        connection.Open();

        var query = "SELECT * FROM Drivers WHERE " + column + " = @" + column;
        Console.WriteLine(query);
        using var command = new SQLiteCommand(query, connection);

        command.Parameters.AddWithValue("@" + column, value);

        using var reader = command.ExecuteReader();

        var driver = new DriverEntity();

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
    public DriverEntity Show(int id)
    {
        return FindBy("id", id.ToString());
    }



    public DriverEntity Create(DriverEntity driver)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

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


    public bool Update(int id, DriverEntity driver)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        var rowsAffected = 0;

        using (var updateCommand = new SQLiteCommand("UPDATE Drivers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber WHERE Id = @Id", connection))
        {
            updateCommand.Parameters.AddWithValue("@Id", id);
            updateCommand.Parameters.AddWithValue("@FirstName", driver.FirstName);
            updateCommand.Parameters.AddWithValue("@LastName", driver.LastName);
            updateCommand.Parameters.AddWithValue("@Email", driver.Email);
            updateCommand.Parameters.AddWithValue("@PhoneNumber", driver.PhoneNumber);

            rowsAffected = updateCommand.ExecuteNonQuery();


        }

        return rowsAffected == 0;
    }


    public bool Delete(int id)
    {

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var deleteCommand = new SQLiteCommand("DELETE FROM Drivers WHERE Id = @Id", connection);
        deleteCommand.Parameters.AddWithValue("@Id", id);

        var rowsAffected = deleteCommand.ExecuteNonQuery();

        connection.Close();

        return rowsAffected > 0;
    }
}
