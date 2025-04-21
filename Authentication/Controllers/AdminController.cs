using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // API dành riêng cho Admin để quản lý người dùng
        [Authorize(Roles = "Administrator")]
        [HttpGet("admin-only")]
        public IActionResult GetAdminOnlyData()
        {
            return Ok("Chỉ Admin mới thấy được cái này");
        }
    }
}