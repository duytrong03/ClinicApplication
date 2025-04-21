using Microsoft.AspNetCore.Mvc;
using ClinicApplication.Services;
using System.Net.Http.Headers;
using ClinicApplication.ViewModels;
using Microsoft.AspNetCore.Routing.Tree;

[Route ("api/facility")]
public class FacilityController : ControllerBase
{
    private readonly FacilityService _facilityService;
    private readonly ILogger<FacilityController> _logger;
    public FacilityController(FacilityService facilityService, ILogger<FacilityController> logger)
    {
        _facilityService = facilityService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllFacility()
    {
        try
        {
            var facilities = await _facilityService.GetAllFacility();
            if (!facilities.Any())
            {
                return NotFound(new {success = false, message = "Không tìm thấy dữ liệu cơ sở nào"});
            }
            return Ok (new {success = true, message = "Lấy dữ liệu cơ sở thành công", data = facilities});
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định từ server");
            return StatusCode(500, new {success = false, message = "Lỗi hệ thống, vui lòng thử lại sau"});
        }
    }
    [HttpPost]
    public async Task<IActionResult> AddFacility(FacilityViewModel model)
    {
        try
        {
            await _facilityService.AddFacility(model);
            return Ok(new {success = true, message = "Cơ sở mới được thêm thành công", dataInserted = model});
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Lỗi không xác định từ server");
            return StatusCode(500, new {success = false,message = "Lỗi hệ thống, vui lòng thử lại sau"});
        }

    }
    [HttpPut ("{id}")]
    public async Task<IActionResult> UpdateFacility(int id, FacilityViewModel model)
    {
        try
        {
            await _facilityService.UpdateFacility(id, model);
            return Ok(new {success = true, message = "Cập nhật dữ liệu thành công"});
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định từ hệ thống");
            return StatusCode(500, new {success = false, message = "Đã xảy ra lỗi từ hệ thống, vui lòng thử lại sau"});
        }
    }
    [HttpDelete ("{id}")]
    public async Task<IActionResult> DeleteFacility(int id)
    {
        try
        {
            await _facilityService.DeleteFacility(id);
            return Ok(new {success = true, message = "Đã xóa cơ sở thành công"});
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định từ hệ thỗng");
            return StatusCode(500, new {success = false, message = "Đã xảy ra lỗi từ hệ hệ thống, vui lòng thử lại sau"});
        }
    }

    [HttpGet ("near-by")]
    public async Task<IActionResult> SearchNearbyFacilities(double kinhDo, double viDo, double radius)
    {
        try
        {
            var facilities = await _facilityService.SearchNearByFacilities(kinhDo, viDo, radius);
            return Ok(new {
                success = true , 
                message = "Đã tra cứu cơ sở nằm trong bán kính được chỉ thành công",
                data = facilities
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định từ hệ thỗng");
            return StatusCode(500, new {success = false, message = "Đã xảy ra lỗi từ hệ hệ thống, vui lòng thử lại sau"});
        }
    }
}