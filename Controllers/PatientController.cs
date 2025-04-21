using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using ClinicApplication.Services;
using ClinicApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;

[Route ("api/patient")]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly PatientService _patientService;
    private readonly ILogger<PatientController> _logger;
    public PatientController(PatientService patientService, ILogger<PatientController> logger)
    {
        _patientService = patientService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatient()
    {
        try 
        {
            var patients = await _patientService.GetPatient();
            if (!patients.Any())
            {
                return NotFound(new {success = false, message = "Không tìm thấy bệnh nhân nào trong hệ thống!"});
            }
            return Ok(new {success = true, message = "Lấy dữ liệu thành công!", data = patients });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định từ server");
            return StatusCode(500, new 
            {
                success = false,
                message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau"
            });
        }
    }
    [HttpPost]
    public async Task<IActionResult> AddPatient(PatientViewModel model)
    {
        try
        {
            await _patientService.AddPatient(model);
            return Ok(new
            {
                success = true,
                message = "Thêm bệnh nhân thành công",
                insertedData = model
            });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định từ server", ex.Message);
            return StatusCode(500, new 
            {
                success = false,
                message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau"
            });
        }
    }
    [HttpPut ("{id}")]
    public async Task<IActionResult> UpdatePatients(int id, PatientViewModel model)
    {
        try
        {
            bool result = await _patientService.UpdatePatient(id, model);
            if (!result)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Không tìm thấy bệnh nhân với ID = {id}"
                });
            }
            return Ok(new
            {
                success = true,
                message = "Cập nhật thông tin bệnh nhân thành công"
            });
        }
        catch(Exception ex)
        {
            _logger.LogInformation(ex, "Lỗi không xác định từ server");
            return StatusCode(500, new
            {
                success = false,
                message = "Đã xảy ra lỗi từ hệ thống, vui lòng thử lại sau"
            });
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var result = await _patientService.DeletePatient(id);
        if (!result)
        {
            return NotFound(new 
            {
                success = false,
                message = "Dữ liệu xóa thất bại!"
            });
        }
        return Ok(new
        {
            success = true,
            message = "Dữ liệu đã được xóa thành công!",
        });
    }
}
