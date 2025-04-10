using ClinicApplication.Models;
using ClinicApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

[Route("api/register")]
public class RegisterController : ControllerBase
{
    private readonly RegisterService _registerService;
    private readonly ILogger<RegisterController> _logger;
    public RegisterController(RegisterService registerService, ILogger<RegisterController> logger)
    {
        _logger = logger;
        _registerService = registerService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(string username, string password)
    {
        try
        {
            await _registerService.AddUser(username, password);
            return Ok(new
            {
                success = true,
                message = "User registered successfully!"
            });
        }
        catch (PostgresException ex)
        {
            _logger.LogError(ex, "Lỗi database - Mã lỗi SQL: {SqlState}", ex.SqlState, ex.Message);
            return StatusCode(500, new {
                success = false,
                message = "Lỗi truy xuất dữ liệu, vui lòng thử lại sau."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Lỗi không xác định từ server",ex.Message);
            return StatusCode(500, new{
                success = false,
                message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau."
            });
        }
    }
}