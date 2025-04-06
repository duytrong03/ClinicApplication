using System.Data;
using Npgsql;

namespace ClinicApplication.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connnectionString;
        public DatabaseHelper(IConfiguration configuration)
        {
            _connnectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_connnectionString);
        }
    }
}