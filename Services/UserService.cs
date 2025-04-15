using Dapper;
using ClinicApplication.Helpers;
using ClinicApplication.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ClinicApplication.Services
{
    public class UserService
    {
        private readonly DatabaseHelper _databaseHelper;

        public UserService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        
        public ApplicationUser GetUserByEmail(string email)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                var sql = "SELECT * FROM users WHERE email = @Email";
                
                return conn.QueryFirstOrDefault<ApplicationUser>(sql, new{Email = email}); 
            }
        }
        public void CreateUser (RegisterViewModel model)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                var sql = @"INSERT INTO users(email,password_hash,hoten,ngaytao,ngaycapnhat)
                            VALUES (@Email, @PasswordHash,  @HoTen, @NgayTao, @NgayCapNhat)";
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var parameters = new
                {
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    HoTen = model.HoTen,
                    NgayTao = DateTime.UtcNow,
                    NgayCapNhat = DateTime.UtcNow
                };
                conn.Execute(sql, parameters);
            }
        }
    }
}