using ClinicApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.X509Certificates;

[Authorize]
[Route("api/auth/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UserController> _logger;
    public UserController(UserService userService, ILogger<UserController> logger)
    {
        _logger = logger;
        _userService = userService;
    }
    [Authorize(Roles = "Administrator")]
    [HttpPut("update-password")]
    public async Task<IActionResult> UpdatePassword(int id, string username, string oldPassword, string newPassword, string confirmPassword)
    {
        try
        {
            await _userService.UpdatePassword(id, username, oldPassword, newPassword, confirmPassword);
            return Ok(new
            {
                success = true,
                message = "Cập nhật mật khẩu thành công!"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định từ server", ex.Message);
            return StatusCode(500, new
            {
                success = false,
                message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau."
            });
        }
    }
    [Authorize(Roles = "Administrator")]
    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUser(int id, string username)
    {
        try
        {
            await _userService.DeleteUser(id, username);
            return Ok(new
            {
                success = true,
                message = "Xóa người dùng thành công!"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định từ server", ex.Message);
            return StatusCode(500, new
            {
                success = false,
                message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau."
            });
        }
    }
}