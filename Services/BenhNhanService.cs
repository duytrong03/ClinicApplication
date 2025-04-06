using Dapper;
using Microsoft.Extensions.Caching.Memory;
using ClinicApplication.Helpers;
using System.Data;
using ClinicApplication.Models;
using ClinicApplication.ViewModels;
using ClinicApplication.Repositories;
using Dapper.FastCrud;

namespace ClinicApplication.Services
{
    public class BenhNhanService
    {
        private readonly IMemoryCache _cache;
        
        private readonly ILogger<BenhNhanService> _logger;
        private readonly BenhNhanRepository _benhNhanRepository;

        public BenhNhanService(IMemoryCache cache, BenhNhanRepository benhNhanRepository, ILogger<BenhNhanService> logger)
        {
            _cache = cache;
            _benhNhanRepository = benhNhanRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BenhNhan>> GetAllBenhNhan()
        {
            if (!_cache.TryGetValue("benhNhans", out IEnumerable<BenhNhan> benhNhans))
            {
                _logger.LogInformation("Dữ liệu lấy từ cơ sở dữ liệu.");
                benhNhans = await _benhNhanRepository.GetAllBenhNhan();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _cache.Set("benhNhans", benhNhans, cacheOptions);
            }
            else
            {
                _logger.LogInformation("Dữ liệu lấy từ cache");
            }
            
            return benhNhans;
        }

        public async Task AddBenhNhan(BenhNhanViewModel model)

        {
            await _benhNhanRepository.AddBenhNhan(model);
            _cache.Remove("benhNhans");
            if (!_cache.TryGetValue("benhNhans", out var cacheValue))
            {
                Console.WriteLine("Cache đã bị xóa."); 
            }
            else
            {
                Console.WriteLine("Cache vẫn còn."); 
            }
            
        }
        public async Task<bool> UpdateBenhNhan(int id,BenhNhanViewModel model)
        {
            var count = await _benhNhanRepository.CountBenhNhanById(id);
            if (count == 0)
            {
                return false;
            }
            await _benhNhanRepository.UpdateBenhNhan(id, model);
            _cache.Remove("benhNhans");
            
            return true;
        }
        public async Task<bool> DeleteBenhNhan(int id)
        {
            var existingBenhNhan = await _benhNhanRepository.GetBenhNhanById(id);
            if (existingBenhNhan == null)
            {
                return false;
            }
            var rowAffected = await _benhNhanRepository.DeleteBenhNhan(id); 
            if (rowAffected > 0)
            {
                _cache.Remove("benhNhans");
                return true;
            }
            return false;
        }
    } 
}
