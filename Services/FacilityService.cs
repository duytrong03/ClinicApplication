using ClinicApplication.Models;
using ClinicApplication.Repositories;
using ClinicApplication.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicApplication.Services
{
    public class FacilityService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<FacilityService> _logger;
        private readonly FacilityRepository _facilityRepository;
        public FacilityService (FacilityRepository facilityRepository, IMemoryCache cache, ILogger<FacilityService> logger)
        {
            _facilityRepository = facilityRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<Facility>> GetAllFacility()
        {
            object value = null;
            if (!_cache.TryGetValue("facilities", out value))
            {
                _logger.LogInformation("Dữ liệu lấy từ cơ sở dữ liệu");
                var facilities = await _facilityRepository.GetAllFacility();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                _cache.Set("facilities", facilities, cacheOptions);
                value = facilities;
            }
            else
            {
                _logger.LogInformation("Dữ liệu lấy từ cache");
            }
            return (IEnumerable<Facility>)value;
            
        }
        public async Task AddFacility(FacilityViewModel model)
        {
            await _facilityRepository.AddFacility(model);
            _cache.Remove("facilities");
            if (!_cache.TryGetValue("facilities", out var cacheValue))
            {
                _logger.LogInformation("Cache đã được xóa");
            }
        }
        public async Task UpdateFacility(int id ,FacilityViewModel model)
        {
            var existingFacility = await _facilityRepository.IsFacilityExists(id);
            if (existingFacility == 0)
            {
                throw new Exception("Cơ sở không tồn tại");
            }
            _cache.Remove("facilities");
            if (!_cache.TryGetValue("facilities", out var cacheValue))
            {
                _logger.LogInformation("Cache đã được xóa");
            }
            await _facilityRepository.UpdateFacility(id,model);
        }
        public async Task DeleteFacility(int id)
        {
            var existingFacility = await _facilityRepository.IsFacilityExists(id);
            if (existingFacility == 0)
            {
                throw new Exception ("Cơ sở không tồn tại");
            }
            _cache.Remove("facilities");
            if (!_cache.TryGetValue("facilities", out var cacheValue))
            {
                _logger.LogInformation("Cache đã được xóa");
            }
            await _facilityRepository.DeleteFacility(id);
        }
        public async Task<List<Facility>> SearchNearByFacilities(double kinhDo, double viDo, double radius)
        {
            var facilities = await _facilityRepository.SearchNearbyFacilities(kinhDo, viDo, radius);
            return facilities;
        }
    }
}