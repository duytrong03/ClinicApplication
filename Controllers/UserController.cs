using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using Dapper;
using ClinicApplication.ViewModels;
using ClinicApplication.Services;

[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userservice;

    public UserController(UserService userservice)
    {
        _userservice = userservice;
    }

    // [HttpPost("login")]
    // public IActionResult Login(LoginModel model)
    // {
        
    // }
    [HttpPost("Register")]
    public IActionResult Register(RegisterViewModel model)
    {
        try
        {
            var existingUser = _userservice.GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                return BadRequest("Email đã được sử dụng");
            }
            _userservice.CreateUser(model);
            return Ok(new 
            { 
                success = true, 
                message = "Bạn đã đăng kí tài khoản thành công!",
                insertedData = model
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            {
                success = false,
                message = "Lỗi hệ thống! Vui lòng thử lại sau.",
                details = ex.Message
            });
        }

    }
}