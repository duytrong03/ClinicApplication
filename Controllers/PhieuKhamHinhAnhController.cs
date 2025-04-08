using ClinicApplication.ViewModels;
using ClinicApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Threading.Tasks;

[Route("api/phieu-kham-hinh-anh")]
public class PhieuKhamHinhAnhController : ControllerBase
{
    private readonly ILogger<PhieuKhamHinhAnhController> _logger;
    private readonly PhieuKhamHinhAnhService _phieuKhamHinhAnhService;

    public PhieuKhamHinhAnhController(ILogger<PhieuKhamHinhAnhController> logger, PhieuKhamHinhAnhService phieuKhamHinhAnhService)
    {
        _logger = logger;
        _phieuKhamHinhAnhService = phieuKhamHinhAnhService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadHinhAnh(IFormFile file,int phieuKhamId, string? tenFile, string? tenFileLuuTru)
    {
        try
        {
            var result = await _phieuKhamHinhAnhService.UploadHinhAnh(file, phieuKhamId, tenFile, tenFileLuuTru);
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }
            return Ok(new { success = true, message = result.Message, filePath = result.FilePath });
        }
        catch (PostgresException ex)
        {
            _logger.LogError(ex, "Lỗi database - Mã lỗi SQL: {SqlState}", ex.SqlState, ex.Message);
            return StatusCode(500, new { success = false, message = "Lỗi truy xuất dữ liệu, vui lòng thử lại sau." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Lỗi không xác định từ server",ex.Message);
            return StatusCode(500,new{ success = false, message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau." });
        }
    }
}