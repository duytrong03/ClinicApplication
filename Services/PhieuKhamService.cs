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
    public class PhieuKhamService
    {
        private readonly IMemoryCache _cache;
        private readonly PhieuKhamRepository _phieuKhamRepository;
        private readonly PhieukhamHinhAnhRepository _phieuKhamHinhAnhRepository;
        private readonly FileSettingOptions _fileSettings;
        private readonly IWebHostEnvironment _env;


        public PhieuKhamService(
            IMemoryCache cache,
            PhieuKhamRepository phieuKhamRepository,
            PhieukhamHinhAnhRepository phieuKhamHinhAnhRepository,
            IOptions<FileSettingOptions> fileSettings,
            IWebHostEnvironment env
        )
        {
            _cache = cache;
            _phieuKhamRepository = phieuKhamRepository;
            _phieuKhamHinhAnhRepository = phieuKhamHinhAnhRepository;
            _fileSettings = fileSettings.Value;
            _env = env;
        }
        public async Task<IEnumerable<PhieuKham>> GetPhieuKham(
            string? keyword,
            DateTime? fromDate,
            DateTime? toDate,
            int? page,
            int? pageSize
        )
        {
            var phieuKhams = await _phieuKhamRepository.GetPhieuKham(keyword, fromDate, toDate, page, pageSize);
            return phieuKhams;
        }

        public async Task AddPhieuKhamAndUpload(IFormFile file, int? id, PhieuKhamViewModel model, string? tenFile, string? tenFileLuuTru)
        {
            if (id == null)
            {
                var count = await _phieuKhamRepository.GetPhieuKhamCountInYear();
                string maPhieu = $"PK{DateTime.Now:yy}{count + 1:D2}";
                var phieuKhamId = await _phieuKhamRepository.AddPhieuKham(maPhieu, model);
                if (file != null && file.Length >0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (! _fileSettings.AllowedExtensions.Contains(extension))
                    {
                        throw new Exception("Định dạng file không hợp lệ.");
                    }
                    var fileName = !string.IsNullOrEmpty(tenFileLuuTru)
                                ? $"{tenFileLuuTru}{extension}"
                                : $"{maPhieu}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
                    var uploadPath = Path.Combine(_env.WebRootPath, _fileSettings.ImagePath);
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var filePath = Path.Combine(uploadPath, fileName);
                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileUrl = $"wwwroot/uploads/images/{fileName}";
                    await _phieuKhamHinhAnhRepository.AddPhieuKhamHinhAnh(phieuKhamId, tenFile, fileName, file.Length, fileUrl);
                }
            }
            else 
            {
                await _phieuKhamRepository.UpdatePhieuKham(id.Value, model);
            }
        }
    }
}