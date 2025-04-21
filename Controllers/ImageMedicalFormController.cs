using ClinicApplication.ViewModels;
using ClinicApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Threading.Tasks;

[Route("api/medical-form-images")]
public class MedicalFormImageController : ControllerBase
{
    private readonly ILogger<MedicalFormImageController> _logger;
    private readonly ImageMedicalFormService _imageMedicalFormService;

    public MedicalFormImageController(ILogger<MedicalFormImageController> logger, ImageMedicalFormService imageMedicalFormService)
    {
        _logger = logger;
        _imageMedicalFormService = imageMedicalFormService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImageMedicalForm(IFormFile file, int medicalFormId, string? fileName, string? storageFileName)
    {
        try
        {
            var result = await _imageMedicalFormService.UploadImageMedicalForm(file, medicalFormId, fileName, storageFileName);
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
            _logger.LogError(ex, "Lỗi không xác định từ server", ex.Message);
            return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau." });
        }
    }
}
