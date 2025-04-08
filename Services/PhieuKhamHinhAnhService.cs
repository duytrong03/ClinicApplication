using ClinicApplication.ViewModels;
using ClinicApplication.Repositories;
using ClinicApplication.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

public class PhieuKhamHinhAnhService
{
    private readonly PhieukhamHinhAnhRepository _phieuKhamHinhAnhRepository;
    private readonly FileSettingOptions _fileSettingOptions;
    private readonly IWebHostEnvironment _env;

    public PhieuKhamHinhAnhService(PhieukhamHinhAnhRepository phieukhamHinhAnhRepository, IOptions<FileSettingOptions> fileSettingOptions, IWebHostEnvironment env)
    {
        _phieuKhamHinhAnhRepository = phieukhamHinhAnhRepository;
        _fileSettingOptions = fileSettingOptions.Value;
        _env = env;
    }
    public async Task<UploadResult> UploadHinhAnh(IFormFile file, int phieuKhamId, string? tenFile, string? tenFileLuuTru)
    {
        if(file == null || file.Length == 0)
        {
            return new UploadResult(false, "Không có thư mục nào được up lên!", null);
        }
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_fileSettingOptions.AllowedExtensions.Contains(extension))
        {
            return new UploadResult(false, "Định dạng file không hợp lệ!", null);
        }
        if (file.Length > _fileSettingOptions.MaxFileSize)
        {
            return new UploadResult(false, "kích thước file quá lớn!", null);
        }
        var uploadPath = Path.Combine(_env.WebRootPath,_fileSettingOptions.ImagePath);      
        if(!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }
        var fileName = $"{phieuKhamId}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";       
        var filePath = Path.Combine(uploadPath, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var fileUrl = $"/uploads/images/{fileName}";
        
        await _phieuKhamHinhAnhRepository.AddPhieuKhamHinhAnh(phieuKhamId, tenFile, tenFileLuuTru, file.Length, fileUrl);
        return new UploadResult(true,"Upload thành công!", fileUrl);
    }
}