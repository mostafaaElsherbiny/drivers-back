
using System.Data.SQLite;

namespace Driver.Database;


public class SqliteConnectionFactory : ISqliteConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqliteConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public SQLiteConnection CreateConnection()
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(connectionString));

        return new SQLiteConnection(connectionString);
    }
}





