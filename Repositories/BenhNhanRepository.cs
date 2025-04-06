using ClinicApplication.Helpers;
using Dapper;
using ClinicApplication.Models;
using ClinicApplication.ViewModels;
using System.Threading.Tasks;
namespace ClinicApplication.Repositories
{
    public class BenhNhanRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public BenhNhanRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<IEnumerable<BenhNhan>> GetAllBenhNhan()
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                var benhNhans = await conn.QueryAsync<BenhNhan>(@"SELECT id, hovaten, socon, namsinh, sohoso, diachi, 
                                                            gioitinh, sodienthoai, email, ngaytao, ngaycapnhat
                                                            FROM benhnhan ORDER BY id");
                return benhNhans;
            }
        }
        public async Task AddBenhNhan(BenhNhanViewModel model)
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
                NgayTao = DateTime.UtcNow, 
                NgayCapNhat = DateTime.UtcNow
            };
            await conn.ExecuteAsync(sql, parameters);
        }
        public async Task<int> CountBenhNhanById(int id)
        {
            using var conn = _databaseHelper.GetConnection();
            var count = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT (*) FROM benhnhan WHERE id = @Id",
                new {Id = id}
            );
            return count;
        }
        public async Task UpdateBenhNhan (int id, BenhNhanViewModel model)
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
                NgayCapNhat = DateTime.UtcNow
            };

            await conn.ExecuteAsync(sql, parameters);
        }
        public async Task<BenhNhan?> GetBenhNhanById(int id)
        {
            using var conn = _databaseHelper.GetConnection();
            var existingBenhNhan = await conn.QueryFirstOrDefaultAsync<BenhNhan>(
                "SELECT * FROM benhnhan WHERE id = @Id", new {Id = id}
            );
            return existingBenhNhan;
        }
        public async Task<int> DeleteBenhNhan(int id)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = "DELETE FROM benhnhan WHERE id = @Id";
            int rowAffected = await conn.ExecuteAsync(sql, new {Id = id});

            return rowAffected;
        }
    }
}
