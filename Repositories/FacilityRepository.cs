using ClinicApplication.Helpers;
using ClinicApplication.Models;
using ClinicApplication.ViewModels;
using Dapper;
using Npgsql;

namespace ClinicApplication.Repositories
{
    public class FacilityRepository
    {
        private readonly DatabaseHelper _databaseHelper;
        public FacilityRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<IEnumerable<Facility>> GetAllFacility()
        {
            using var conn = _databaseHelper.GetConnection();
            var facilities = await conn.QueryAsync<Facility>(
                @"SELECT id, tencoso, macoso, diachi, tinhthanhpho, quanhuyen, xaphuong, 
                        kinhdo, vido, ngaytao, ngaycapnhat
                FROM coso
                ORDER BY id"
            );
            return facilities;
        }
        public async Task AddFacility(FacilityViewModel model)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = @"INSERT INTO coso (
                            tencoso, macoso, diachi, tinhthanhpho, quanhuyen,
                            xaphuong, kinhdo, vido, ngaytao, ngaycapnhat, geom
                        )
                        VALUES (
                            @TenCoSo, @MaCoSo, @DiaChi, @TinhThanhPho, @QuanHuyen,
                            @XaPhuong, @KinhDo, @ViDo, @NgayTao, @NgayCapNhat,
                            ST_SetSRID(ST_MakePoint(@ViDo, @KinhDo), 4326)
                        )";
            var parameters = new
            {
                TenCoSo = model.TenCoSo,
                MaCoSo = model.MaCoSo,
                DiaChi = model.DiaChi,
                TinhThanhPho = model.TinhThanhPho,
                QuanHuyen = model.QuanHuyen,
                XaPhuong = model.XaPhuong,
                KinhDo = model.KinhDo,
                ViDo = model.ViDo,
                NgayTao = DateTime.UtcNow,
                NgayCapNhat = DateTime.UtcNow
            };
            await conn.ExecuteAsync(sql, parameters);
        }
        public async Task UpdateFacility(int id,FacilityViewModel model)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = @"UPDATE coso
                        SET tencoso = @TenCoSo, macoso = @MaCoSo, diachi = @DiaChi, 
                            tinhthanhpho = @TinhThanhPho, quanhuyen = @QuanHuyen, xaphuong = @XaPhuong
                            kinhdo = @KinhDo, vido = @ViDo, ngaycapnhat = @NgayCapNhat
                        WHERE id = @Id";
            var parameters = new
            {
                Id = id,
                TenCoSo = model.TenCoSo,
                MaCoSo = model.MaCoSo,
                DiaChi = model.DiaChi,
                TinhThanhPho = model.TinhThanhPho,
                QuanHuyen = model.QuanHuyen,
                XaPhuong = model.XaPhuong,
                KinhDo = model.KinhDo,
                ViDo = model.ViDo,
                NgayCapNhat = DateTime.UtcNow
            };
            await conn.ExecuteAsync(sql, parameters);
        }
        public async Task<int> IsFacilityExists(int id)
        {
            using var conn = _databaseHelper.GetConnection();
            var count = await conn.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*) FROM coso WHERE id = @Id", new {Id = id}
            );
            return count;
        }
        public async Task DeleteFacility(int id)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = "DELETE FROM coso WHERE id = @Id";
            await conn.ExecuteAsync(sql, new {Id = id});
        }

        public async Task<List<Facility>> SearchNearbyFacilities(double kinhDo, double viDo, double radius)
        {
            using var conn = _databaseHelper.GetConnection() as NpgsqlConnection;
            await conn.OpenAsync();
            var sql = @"SELECT id, tencoso, macoso, diachi, tinhthanhpho, quanhuyen, xaphuong,
                        kinhdo, vido
                        FROM coso
                        WHERE ST_Within(
                            geom,
                            ST_Buffer(
                                ST_SetSRID(ST_MakePoint(@KinhDo, @ViDo), 4326),
                                @radius 
                            )
                        );";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@KinhDo",kinhDo);
            cmd.Parameters.AddWithValue("@ViDo", viDo);
            cmd.Parameters.AddWithValue("@radius", radius);

            var result = new List<Facility>();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var facilities = new Facility
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    TenCoSo = reader.GetString(reader.GetOrdinal("tencoso")),
                    MaCoSo = reader.GetString(reader.GetOrdinal("macoso")),
                    DiaChi = reader.GetString(reader.GetOrdinal("diachi")),
                    TinhThanhPho = reader.GetString(reader.GetOrdinal("tinhthanhpho")),
                    QuanHuyen = reader.GetString(reader.GetOrdinal("quanhuyen")),
                    XaPhuong = reader.GetString(reader.GetOrdinal("xaphuong")),
                    KinhDo = reader.GetDouble(reader.GetOrdinal("kinhdo")),
                    ViDo = reader.GetDouble(reader.GetOrdinal("vido"))
                };
                result.Add(facilities);
            }
            return result;
        }
    }
}