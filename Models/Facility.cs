namespace ClinicApplication.Models
{
    public class Facility
    {
        public int Id {get; set;}
        public string? TenCoSo {get; set;}
        public string? MaCoSo {get; set;}
        public string? DiaChi {get; set;}
        public string? TinhThanhPho {get; set;}
        public string? QuanHuyen {get; set;}
        public string? XaPhuong {get; set;}
        public double KinhDo {get; set;}
        public double ViDo {get; set;}
        public DateTime NgayTao {get; set;} = DateTime.UtcNow;
        public DateTime NgayCapNhat {get; set;} = DateTime.UtcNow;
    }
}