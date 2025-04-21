using ClinicApplication.ViewModels;
using ClinicApplication.Repositories;
using ClinicApplication.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

public class ImageMedicalFormService
{
    private readonly ImageMedicalFormRepository _imageMedicalFormRepository;
    private readonly FileSettingOptions _fileSettingOptions;
    private readonly IWebHostEnvironment _env;

    public ImageMedicalFormService(ImageMedicalFormRepository imageMedicalFormRepository, IOptions<FileSettingOptions> fileSettingOptions, IWebHostEnvironment env)
    {
        _imageMedicalFormRepository = imageMedicalFormRepository;
        _fileSettingOptions = fileSettingOptions.Value;
        _env = env;
    }

    public async Task<UploadResult> UploadImageMedicalForm(IFormFile file, int medicalFormId, string? fileName, string? storageName)
    {
        if (file == null || file.Length == 0)
        {
            return new UploadResult(false, "Không có file nào được upload!", null);
        }

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_fileSettingOptions.AllowedExtensions.Contains(extension))
        {
            return new UploadResult(false, "Định dạng file không hợp lệ!", null);
        }

        if (file.Length > _fileSettingOptions.MaxFileSize)
        {
            return new UploadResult(false, "Kích thước file quá lớn!", null);
        }

        var uploadPath = Path.Combine(_env.WebRootPath, _fileSettingOptions.ImagePath);
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var generatedFileName = $"{medicalFormId}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
        var filePath = Path.Combine(uploadPath, generatedFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileUrl = $"/uploads/images/{generatedFileName}";

        await _imageMedicalFormRepository.AddImageMedicalForm(medicalFormId, fileName, storageName, file.Length, fileUrl);
        return new UploadResult(true, "Upload thành công!", fileUrl);
    }
}
