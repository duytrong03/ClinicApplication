using Microsoft.AspNetCore.Mvc;
using ClinicApplication.Models;
using ClinicApplication.Services;

[Route("api/auth/login")]
public class LoginController : ControllerBase
{
    private readonly LoginService _loginService;

    public LoginController(LoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var token = await _loginService.Authenticate(username, password);
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid username or password"});
        }
        return Ok(new { token });
    }
}