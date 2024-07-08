using MySql.Data.MySqlClient;
using System.Data;

namespace MyRestfulApi.Services
{
    public class DatabaseService(IConfiguration configuration) : IDatabaseService
    {
        #pragma warning disable CS8601 // Possible null reference assignment.
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");
        #pragma warning restore CS8601 // Possible null reference assignment.
        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }

    public interface IDatabaseService
    {
        IDbConnection CreateConnection();
    }
}
