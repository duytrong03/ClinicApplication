using ClinicApplication.Repositories;
using Microsoft.AspNetCore.Identity;
using ClinicApplication.Models;
public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(UserRepository userRepository, PasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }   
    public async Task UpdatePassword(int id, string username, string oldPassword ,string newPassword, string confirmPassword)
    {
        var user = await _userRepository.GetUserByUserName(username);
        if (user == null)
        {
            throw new Exception("Nguời dùng không tồn tại.");
        }
        if (user.Id != id)
        {
            throw new Exception("Người dùng không hợp lệ.");
        }
        if (newPassword != confirmPassword)
        {
            throw new Exception("Mật khẩu không khớp.");
        }
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, oldPassword);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new Exception("Mật khẩu cũ không đúng.");
        }
        newPassword = _passwordHasher.HashPassword(user, newPassword);
        await _userRepository.UpdatePassword(id, newPassword);
    }
    
    public async Task DeleteUser(int id, string username)
    {
        var user = await _userRepository.GetUserByUserName(username);
        if (user == null)
        {
            throw new Exception("Người dùng không tồn tại.");
        }
        if (user.Id != id)
        {
            throw new Exception("Người dùng không hợp lệ.");
        }
        await _userRepository.DeleteUser(id);   
    }
}
