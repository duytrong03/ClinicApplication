using ClinicApplication.Helpers;
using ClinicApplication.Models;
using System.Threading.Tasks;
using Dapper;

namespace ClinicApplication.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseHelper _databaseHelper;
        public UserRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        public async Task AddUser(string username, string password)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = @"INSERT INTO users (username, password_hash) VALUES (@Username, @Password)";
            var parameters = new
            {
                Username = username,
                Password = password
            };
            await conn.ExecuteAsync(sql, parameters);
        }
        public async Task<User?> GetUserByUsername(string username)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = @"SELECT id, username, password_hash AS PasswordHash FROM users WHERE username = @Username";
            var parameters = new { Username = username };
            return await conn.QueryFirstOrDefaultAsync<User>(sql, parameters);
        }
    } 
}