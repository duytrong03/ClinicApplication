using Dapper;
using Microsoft.Extensions.Caching.Memory;
using ClinicApplication.Helpers;
using System.Data;
using ClinicApplication.Models;
using ClinicApplication.ViewModels;
using ClinicApplication.Repositories;
using Dapper.FastCrud;

namespace ClinicApplication.Repositories
{
    public class PhieuKhamService
    {
        private readonly IMemoryCache _cache;
        
        private readonly ILogger<PhieuKhamService> _logger;
        private readonly PhieuKhamRepository _phieuKhamRepository;


        public PhieuKhamService(IMemoryCache cache, PhieuKhamRepository phieuKhamRepository, ILogger<PhieuKhamService> logger)
        {
            _cache = cache;
            _phieuKhamRepository = phieuKhamRepository;
            _logger = logger;
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
            await _phieuKhamRepository.AddPhieuKham(model);
        }
    }
}