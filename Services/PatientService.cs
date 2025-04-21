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
    public class PatientService
    {
        private readonly IMemoryCache _cache;
        
        private readonly ILogger<PatientService> _logger;
        private readonly PatientRepository _patientRepository;

        public PatientService(IMemoryCache cache, PatientRepository patientRepository, ILogger<PatientService> logger)
        {
            _cache = cache;
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Patient>> GetPatient()
        {
            object value = null;
            if (!_cache.TryGetValue("patients", out value))
            {
                _logger.LogInformation("Dữ liệu lấy từ cơ sở dữ liệu");
                var patients = await _patientRepository.GetPatient();
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                _cache.Set("patients", patients, cacheOptions);
                value = patients;
            }
            else
            {
                _logger.LogInformation("Dữ liệu lấy từ cache");
            }
            return (IEnumerable<Patient>)value;
        }
        public async Task AddPatient(PatientViewModel model)
        {
            await _patientRepository.AddPatient(model);
            _cache.Remove("patients");
            if (!_cache.TryGetValue("patients", out var cacheValue))
            {
                _logger.LogInformation("Cache đã bị xóa");
            }
        }
        public async Task<bool> UpdatePatient(int id, PatientViewModel model)
        {
            int count = await _patientRepository.GetPatientById(id);
            if (count == 0)
            {
                return false;
            }
            await _patientRepository.UpdatePatient(id, model);
            _cache.Remove("patients");
            if (!_cache.TryGetValue("patients", out var value))
            {
                _logger.LogInformation("cache đã bị xóa");
            }
            return true;
        }
        public async Task<bool> DeletePatient(int id)
        {
            var existingPatient = await _patientRepository.GetPatientById(id);
            if (existingPatient == null)
            {
                return false;
            }
            var rowAffected = await _patientRepository.DeletePatient(id); 
            if (rowAffected > 0)
            {
                _cache.Remove("patients");
                return true;
            }
            return false;
        }
    } 
}
