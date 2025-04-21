using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ClinicApplication.Helpers;
using ClinicApplication.Models;
using ClinicApplication.ViewModels;
using ClinicApplication.Services;
using Dapper;

namespace ClinicApplication.Repositories
{
    public class MedicalFormRepository
    {   
        private readonly DatabaseHelper _databaseHelper;
        public MedicalFormRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<IEnumerable<MedicalForm>> GetPhieuKham(
            string? keyword,
            DateTime? fromDate,
            DateTime? toDate,
            int? page,
            int? pageSize
        )
        {
            using var conn = _databaseHelper.GetConnection();  
            var parameters = new DynamicParameters();
            var query = new StringBuilder("SELECT * FROM phieukham WHERE 1=1");

            if (!string.IsNullOrEmpty(keyword))
            {
                query.Append(" AND (maphieu ILIKE @Keyword OR chuandoan ILIKE @Keyword)");
                parameters.Add("Keyword", $"%{keyword}%");
            }
            if(fromDate.HasValue)
            {
                query.Append(" AND (ngaytao >= @fromDate)");
                parameters.Add("FromDate", fromDate.Value);
            }
            if (fromDate.HasValue)
            {
                query.Append(" AND (ngaytao <= @ToDate)");
                parameters.Add("ToDate", toDate.Value);
            }
            query.Append(" ORDER BY id");
            query.Append(" LIMIT @PageSize OFFSET @Offset");
            parameters.Add("PageSize", pageSize);
            parameters.Add("Offset",(page - 1)*pageSize);

            return await conn.QueryAsync<MedicalForm>(query.ToString(), parameters);
        }

        public async Task<int> GetPhieuKhamCountInYear()
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = @"SELECT COUNT (*) 
                        FROM phieukham
                        WHERE EXTRACT(YEAR FROM ngaytao) = EXTRACT(YEAR FROM CURRENT_DATE)";
            int count = await conn.ExecuteScalarAsync<int>(sql);
            return count;
        }
        public async Task<int> AddPhieuKham(string maPhieu, MedicalFormViewModel model)
        {
            using var conn = _databaseHelper.GetConnection();

            var sql = @"INSERT INTO phieukham (maphieu,benhnhan_id, cannang, chieucao, tiensu, lamsang, mach, 
                        nhietdo, huyetapcao, huyetapthap,tebao,mauchay,mota,chuandoan,dieutri,hinhanh1,hinhanh2,
                        ngaytao,ngaycapnhat)
                        VALUES (@MaPhieu, @BenhNhanId, @CanNang, @ChieuCao, @TienSu, @LamSang, @Mach, @NhietDo, 
                        @HuyetApCao, @HuyetApThap, @TeBao, @MauChay, @MoTa, @ChuanDoan, @DieuTri, @HinhAnh1, @HinhAnh2, 
                        @NgayTao, @NgayCapNhat)
                        RETURNING id";
            var parameters = new
            {
                MaPhieu = maPhieu,
                BenhNhanId = model.BenhNhanId,
                CanNang = model.CanNang,
                ChieuCao = model.ChieuCao,
                TienSu = model.TienSu,
                LamSang = model.LamSang,
                Mach = model.Mach,
                NhietDo = model.NhietDo,
                HuyetApCao = model.HuyetApCao,
                HuyetApThap = model.HuyetApThap,
                TeBao = model.TeBao,
                MauChay = model.MauChay,
                MoTa = model.MoTa,
                ChuanDoan = model.ChuanDoan,
                DieuTri = model.DieuTri,
                HinhAnh1 = model.HinhAnh1,
                HinhAnh2 = model.HinhAnh2,
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };
            var id = await conn.ExecuteScalarAsync<int>(sql, parameters);
            return id;
        }
        public async Task UpdatePhieuKham(int id, MedicalFormViewModel model)
        {
            using var conn = _databaseHelper.GetConnection();
            var sql = @"UPDATE phieukham SET benhnhan_id = @BenhNhanId, cannang = @CanNang, chieucao = @ChieuCao, 
                        tiensu = @TienSu, lamsang = @LamSang, mach = @Mach, nhietdo = @NhietDo, huyetapcao = @HuyetApCao,
                        huyetapthap = @HuyetApThap, tebao = @TeBao, mauchay = @MauChay, mota = @MoTa, chuandoan = @ChuanDoan,
                        dieutri = @DieuTri, hinhanh1 = @HinhAnh1, hinhanh2 = @HinhAnh2, ngaycapnhat = @NgayCapNhat
                        WHERE id = @Id";
            var parameters = new 
            {
                Id = id,
                BenhNhanId = model.BenhNhanId,  
                CanNang = model.CanNang,
                ChieuCao = model.ChieuCao,
                TienSu = model.TienSu,
                LamSang = model.LamSang,
                Mach = model.Mach,
                NhietDo = model.NhietDo,
                HuyetApCao = model.HuyetApCao,
                HuyetApThap = model.HuyetApThap,
                TeBao = model.TeBao,
                MauChay = model.MauChay,
                MoTa = model.MoTa,
                ChuanDoan = model.ChuanDoan,
                DieuTri = model.DieuTri,
                HinhAnh1 = model.HinhAnh1,
                HinhAnh2 = model.HinhAnh2,
                NgayCapNhat = DateTime.Now
            };
            await conn.ExecuteAsync(sql, parameters);
        }
    }
}