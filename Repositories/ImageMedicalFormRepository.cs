using ClinicApplication.Helpers;
using Dapper;
using ClinicApplication.Models; 
using ClinicApplication.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;

public class ImageMedicalFormRepository
{
    private readonly DatabaseHelper _databaseHelper;
    public ImageMedicalFormRepository(DatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }
    public async Task AddImageMedicalForm(int phieuKhamId, string? tenFile, string? tenFileLuuTru, long kichCo, string url)
    {
        using var conn = _databaseHelper.GetConnection();
        var sql = @"INSERT INTO phieukham_hinhanh (phieukham_id, ten_file, ten_file_luutru, kich_co, url)
                    VALUES (@PhieuKhamId, @TenFile, @TenFileLuuTru, @KichCo, @Url) RETURNING id";
        var parameters = new
        {
            PhieuKhamId = phieuKhamId,
            TenFile = tenFile,
            TenFileLuuTru = tenFileLuuTru,
            KichCo = kichCo,
            Url = url
        };
        await conn.ExecuteAsync(sql, parameters);
    }
}