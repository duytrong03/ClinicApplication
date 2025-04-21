using System.Data;
using Npgsql;

namespace ClinicApplication.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}