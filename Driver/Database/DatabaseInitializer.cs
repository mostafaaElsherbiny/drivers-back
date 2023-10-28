using System.Data.SQLite;

namespace Driver.Database;

public class DatabaseInitializer
{
    public void InitializeDatabase()
    {
        using (var connection = new SQLiteConnection("Data Source=mydatabase.db;Version=3;"))
        {
            connection.Open();
            using (var command = new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS Drivers (Id INTEGER PRIMARY KEY AUTOINCREMENT, FirstName TEXT , LastName TEXT, Email TEXT UNIQUE, PhoneNumber TEXT UNIQUE);",
                connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}











