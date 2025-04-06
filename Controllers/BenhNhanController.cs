using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using ClinicApplication.Helpers;
using ClinicApplication.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using ClinicApplication.Services;

[Route("api/benh-nhan")]
public class BenhNhanController : ControllerBase
{

    private readonly ILogger<BenhNhanController> _logger;
    private readonly BenhNhanService _benhNhanService;

    public BenhNhanController( ILogger<BenhNhanController> logger,  BenhNhanService benhNhanService)
    {
        
        _logger = logger;
        _benhNhanService = benhNhanService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBenhNhan()
    {
        try
        {
            var benhNhans = await _benhNhanService.GetAllBenhNhan();
            if (!benhNhans.Any())
            {
                return NotFound(new { success = false, message = "Không tìm thấy bệnh nhân nào trong hệ thống!" });
            }

            return Ok(new { success = true, message = "Lấy dữ liệu thành công!", data = benhNhans });
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
    public async Task<IActionResult> AddBenhNhan(BenhNhanViewModel model)
    {
        try
        {
            await _benhNhanService.AddBenhNhan(model);

            return Ok(new 
            { 
                success = true, 
                message = "Thêm bệnh nhân thành công!",
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
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBenhNhan(int id, BenhNhanViewModel model)
    {
        try
        {
            var updated = await _benhNhanService.UpdateBenhNhan(id, model);
            if (!updated)
            {
                return NotFound(new 
                { 
                    success = false, 
                    message = "Không tìm thấy bệnh nhân để cập nhật." 
                });
            }
            return Ok(new 
            { 
                success = true,
                message = "Cập nhật thành công!",
                updatedData = model
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
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBenhNhan(int id)
    {
        var result = await _benhNhanService.DeleteBenhNhan(id);
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
