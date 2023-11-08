
using System.Data.SQLite;

namespace Driver.Database;


public class SqliteConnectionFactory : ISqliteConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqliteConnectionFactory(IConfiguration configuration = null)
    {
        _configuration = configuration;
    }

    public SQLiteConnection CreateConnection()
    {
        string connectionString = _configuration?.GetConnectionString("DefaultConnection") ?? "Data Source=mydatabase.db;Version=3;";

        return new SQLiteConnection(connectionString);
    }
}





