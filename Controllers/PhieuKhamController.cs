using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using ClinicApplication.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using ClinicApplication.Services;
using ClinicApplication.Repositories;

[Route("api/phieu-kham")]
public class PhieuKhamController : ControllerBase
{
    private readonly ILogger<PhieuKhamController> _logger;
    private readonly PhieuKhamService _phieuKhamService;

    public PhieuKhamController(ILogger<PhieuKhamController> logger, PhieuKhamService phieuKhamService)
    {
        _logger = logger;
        _phieuKhamService = phieuKhamService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPhieuKham(
        string? keyword,
        DateTime? fromDate, 
        DateTime? toDate,
        int? page,
        int? pageSize
    )
    {
        try
        {
            var phieuKhams = await _phieuKhamService.GetPhieuKham(keyword, fromDate, toDate, page, pageSize);
            if (!phieuKhams.Any())
                {
                    return NotFound(new { success = false, message = "Không tìm thấy bệnh nhân nào trong hệ thống!" });
                }

            return Ok(new { success = true, message = "Lấy dữ liệu thành công!", data = phieuKhams });
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

    [HttpPost]
    public async Task<IActionResult> CreatePhieuKham(PhieuKhamViewModel model)
    {
        try
        {
            await _phieuKhamService.AddPhieuKham(model);
            return Ok(new { 
                success = true,
                message = "Phiếu khám đã được tạo thành công.",
                insertedData = model
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