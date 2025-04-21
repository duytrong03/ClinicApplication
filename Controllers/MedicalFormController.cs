using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using ClinicApplication.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using ClinicApplication.Services;
using ClinicApplication.Repositories;

[Route("api/medical-forms")]
public class MedicalFormController : ControllerBase
{
    private readonly ILogger<MedicalFormController> _logger;
    private readonly MedicalFormService _medicalFormService;

    public MedicalFormController(ILogger<MedicalFormController> logger, MedicalFormService medicalFormService)
    {
        _logger = logger;
        _medicalFormService = medicalFormService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMedicalForms(
        string? keyword,
        DateTime? fromDate,
        DateTime? toDate,
        int? page,
        int? pageSize
    )
    {
        try
        {
            var medicalForms = await _medicalFormService.GetMedicalForms(keyword, fromDate, toDate, page, pageSize);
            if (!medicalForms.Any())
            {
                return NotFound(new { success = false, message = "Không tìm thấy phiếu khám nào trong hệ thống!" });
            }

            return Ok(new { success = true, message = "Lấy dữ liệu thành công!", data = medicalForms });
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
            _logger.LogError(ex, "Lỗi không xác định từ server");
            return StatusCode(500, new {
                success = false,
                message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau."
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddMedicalFormAndUpload(
        IFormFile file,
        int? id,
        MedicalFormViewModel model,
        string? fileName,
        string? storageFileName
    )
    {
        try
        {
            await _medicalFormService.AddMedicalFormAndUpload(file, id, model, fileName, storageFileName);
            return Ok(new {
                success = true,
                message = "Phiếu khám đã được tạo thành công.",
                insertedData = model
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định từ server", ex.Message);
            return StatusCode(500, new {
                success = false,
                message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau."
            });
        }
    }
}
