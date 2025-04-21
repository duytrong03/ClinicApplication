using ClinicApplication.Helpers;
using Dapper;
using ClinicApplication.Models;
using ClinicApplication.ViewModels;
using System.Threading.Tasks;
using System.Data.SqlTypes;
namespace ClinicApplication.Repositories
{
    public class PatientRepository
    {
        private readonly DatabaseHelper _databaseHelper;
        public PatientRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<IEnumerable<Patient>> GetPatient()
    {
            using var conn = _databaseHelper.GetConnection();
            var patients = await conn.QueryAsync<Patient>(
                @"SELECT id, hovaten, namsinh, sohoso, diachi, gioitinh, 
                nghenghiep, sodienthoai, email, ngaytao, ngaycapnhat
                FROM benhnhan
                ORDER BY id"
            );
            return patients;
            }
        public async Task AddPatient(PatientViewModel model)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = @"INSERT INTO benhnhan (hovaten, socon, namsinh, sohoso, diachi, gioitinh, nghenghiep, 
                        sodienthoai, email, ngaytao, ngaycapnhat)
                        VALUES (@HoVaTen, @SoCon, @NamSinh, @SoHoSo, @DiaChi, @GioiTinh, @NgheNghiep, 
                        @SoDienThoai, @Email, @NgayTao, @NgayCapNhat)";
            var parameters = new 
            {
                HoVaTen = model.HoVaTen,
                SoCon = model.SoCon, 
                NamSinh = model.NamSinh,
                SoHoSo = model.SoHoSo,
                DiaChi = model.DiaChi,
                GioiTinh = model.GioiTinh,
                NgheNghiep = model.NgheNghiep,
                SoDienThoai = model.SoDienThoai,
                Email = model.Email,
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };
            await conn.ExecuteAsync(sql, parameters);
        }
        public async Task UpdatePatient(int id, PatientViewModel model)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = @"UPDATE benhnhan
                        SET hovaten = @HoVaTen, socon = @SoCon, namsinh = @NamSinh, sohoso = @SoHoSo, 
                                    diachi = @DiaChi, gioitinh = @GioiTinh, nghenghiep = @NgheNghiep, 
                                    sodienthoai = @SoDienThoai, email = @Email, ngaycapnhat = @NgayCapNhat
                        WHERE id = @Id";
            var parameters = new 
            {
                Id = id,
                HoVaTen = model.HoVaTen,
                SoCon = model.SoCon, 
                NamSinh = model.NamSinh,
                SoHoSo = model.SoHoSo,
                DiaChi = model.DiaChi,
                GioiTinh = model.GioiTinh,
                NgheNghiep = model.NgheNghiep,
                SoDienThoai = model.SoDienThoai,
                Email = model.Email,
                NgayCapNhat = DateTime.Now
            };
            await conn.ExecuteAsync(sql, parameters);
        }
        public async Task<int> DeletePatient(int id)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = @"DELETE FROM benhnhan WHERE id = @Id";
            var parameters = new
            {
                Id = id
            };
            var result = await conn.ExecuteAsync(sql, parameters);
            return result;
        }

        public async Task<int> GetPatientById(int id)
        {
            using var conn = _databaseHelper.GetConnection();
            var count = await conn.ExecuteScalarAsync<int>(
                @"SELECT COUNT (*) FROM benhnhan WHERE id = @Id", new {Id = id}
            );
            return count;
        }
    }
}
