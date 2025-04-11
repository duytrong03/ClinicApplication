using Dapper;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using ClinicApplication.Models;
using ClinicApplication.ViewModels;
using ClinicApplication.Repositories;
using Microsoft.Extensions.Options;


    public class PhieuKhamService
    {
        private readonly IMemoryCache _cache;
        private readonly FileSettingOptions _fileSettingOptions;
        private readonly ILogger<PhieuKhamService> _logger;
        private readonly PhieuKhamRepository _phieuKhamRepository;
        private readonly IWebHostEnvironment _env;
        private readonly PhieukhamHinhAnhRepository _phieuKhamHinhAnhRepository;


        public PhieuKhamService(IMemoryCache cache, PhieuKhamRepository phieuKhamRepository, ILogger<PhieuKhamService> logger, IOptions<FileSettingOptions> fileSettingOptions, IWebHostEnvironment env, PhieukhamHinhAnhRepository phieukhamHinhAnhRepository)
        {
            _cache = cache;
            _phieuKhamRepository = phieuKhamRepository;
            _logger = logger;
            _fileSettingOptions = fileSettingOptions.Value;
            _env = env; 
            _phieuKhamHinhAnhRepository = phieukhamHinhAnhRepository;
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

        public async Task AddPhieuKham(PhieuKhamViewModel model)
        {
            var count = await _phieuKhamRepository.GetPhieuKhamCountInYear();
            var maPhieu = $"PK{DateTime.Now:yy}{count + 1:D2}";
            await _phieuKhamRepository.AddPhieuKham(model, maPhieu);
        }

        public async Task<UploadResult> PostOrUpdatePhieuKhamAndUpload(int? id ,PhieuKhamViewModel model, IFormFile file, string? tenFile, string? tenFileLuuTru)
        {
            int phieuKhamId;
            if (id == null)
            {
                var count = await _phieuKhamRepository.GetPhieuKhamCountInYear();
                var maPhieu = $"PK{DateTime.Now:yy}{count + 1:D2}";
                phieuKhamId = await _phieuKhamRepository.AddPhieuKham(model, maPhieu);
                if (file != null && file.Length > 0)
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (!_fileSettingOptions.AllowedExtensions.Contains(extension))
                    {
                        return new UploadResult(false, "Định dạng file không hợp lệ!",null);
                    }
                    var fileName = !string.IsNullOrEmpty(tenFileLuuTru) 
                                ? $"{tenFileLuuTru}{extension}" 
                                : $"{maPhieu}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
                    var uploadPath = Path.Combine(_env.WebRootPath, _fileSettingOptions.ImagePath);
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var filePath = Path.Combine(uploadPath, fileName);
                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileUrl = $"/uploads/images/{fileName}";
                    await _phieuKhamHinhAnhRepository.AddPhieuKhamHinhAnh(phieuKhamId, tenFile, tenFileLuuTru ?? fileName, file.Length, fileUrl);
                    return new UploadResult(true, "Thêm phiếu khám thành công!", fileUrl);
                }
                else
                {
                    return new UploadResult(true, "Thêm phiếu khám thành công, nhưng không có file được tải lên.", null);
                }
            }
            else
            {
                var count = await _phieuKhamRepository.GetPhieuKhamById(id.Value);
                if (count == null)
                {
                    return new UploadResult(false, "Không tìm thấy phiếu khám với ID đã cho.",null);
                }
                else
                {
                    await _phieuKhamRepository.UpdatePhieuKham(id.Value, model);
                    return new UploadResult(true, "Cập nhật phiếu khám thành công!",null);   
                }
                
            }
        }
    }
