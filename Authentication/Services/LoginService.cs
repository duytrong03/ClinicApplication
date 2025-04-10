using ClinicApplication.Models;
using ClinicApplication.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

public class LoginService
{
    private readonly UserRepository _repository;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public LoginService(UserRepository repository, PasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }
    public async Task<string?> Authenticate(string username, string password)
    {
        var user = await _repository.GetUserByUsername(username);
        Console.WriteLine("PasswordHash from DB: " + user?.PasswordHash);
        if (user == null)
        {
            return null;
        }
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result != PasswordVerificationResult.Success)
        {
            return null;
        }
        return GenerateJwtToken(user);
    }
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}