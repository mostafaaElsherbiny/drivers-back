using System.Data.SQLite;

namespace Driver.Database;
public interface ISqliteConnectionFactory
{
    SQLiteConnection CreateConnection();
}
