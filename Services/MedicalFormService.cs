using Dapper;
using Microsoft.Extensions.Caching.Memory;
using ClinicApplication.Helpers;
using System.Data;
using ClinicApplication.Models;
using ClinicApplication.ViewModels;
using ClinicApplication.Repositories;
using Microsoft.Extensions.Options;

namespace ClinicApplication.Repositories
{
    public class MedicalFormService
    {
        private readonly IMemoryCache _cache;
        private readonly MedicalFormRepository _medicalFormRepository;
        private readonly ImageMedicalFormRepository _imageMedicalFormRepository;
        private readonly FileSettingOptions _fileSettings;
        private readonly IWebHostEnvironment _env;

        public MedicalFormService(
            IMemoryCache cache,
            MedicalFormRepository medicalFormRepository,
            ImageMedicalFormRepository imageMedicalFormRepository,
            IOptions<FileSettingOptions> fileSettings,
            IWebHostEnvironment env
        )
        {
            _cache = cache;
            _medicalFormRepository = medicalFormRepository;
            _imageMedicalFormRepository = imageMedicalFormRepository;
            _fileSettings = fileSettings.Value;
            _env = env;
        }

        public async Task<IEnumerable<MedicalForm>> GetMedicalForms(
            string? keyword,
            DateTime? fromDate,
            DateTime? toDate,
            int? page,
            int? pageSize
        )
        {
            return await _medicalFormRepository.GetPhieuKham(keyword, fromDate, toDate, page, pageSize);
        }

        public async Task AddMedicalFormAndUpload(IFormFile file, int? id, MedicalFormViewModel model, string? fileName, string? storageFileName)
        {
            if (id == null)
            {
                var count = await _medicalFormRepository.GetPhieuKhamCountInYear();
                string maPhieu = $"PK{DateTime.Now:yy}{count + 1:D2}";

                var medicalFormId = await _medicalFormRepository.AddPhieuKham(maPhieu, model);

                if (file != null && file.Length > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_fileSettings.AllowedExtensions.Contains(extension))
                    {
                        throw new Exception("Định dạng file không hợp lệ.");
                    }

                    var generatedFileName = !string.IsNullOrEmpty(storageFileName)
                        ? $"{storageFileName}{extension}"
                        : $"{maPhieu}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";

                    var uploadPath = Path.Combine(_env.WebRootPath, _fileSettings.ImagePath);
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filePath = Path.Combine(uploadPath, generatedFileName);
                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileUrl = $"/uploads/images/{generatedFileName}";

                    await _imageMedicalFormRepository.AddImageMedicalForm(medicalFormId, fileName, generatedFileName, file.Length, fileUrl);
                }
            }
            else
            {
                await _medicalFormRepository.UpdatePhieuKham(id.Value, model);
            }
        }
    }
}
