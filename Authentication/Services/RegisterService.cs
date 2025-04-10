using Microsoft.AspNetCore.Identity;
using ClinicApplication.Models;
using ClinicApplication.Repositories;
using Dapper;

public class RegisterService
{
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly UserRepository _repository;

    public RegisterService(UserRepository repository, PasswordHasher<User> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task AddUser(string username, string password)
    {
        var user = new User { Username = username };
        var hashedPassword = _passwordHasher.HashPassword(user, password);
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be empty", nameof(password));
        }
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be empty", nameof(username));
        }
        await _repository.AddUser(username, hashedPassword);
    }
}