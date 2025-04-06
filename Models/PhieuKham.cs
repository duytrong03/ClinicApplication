namespace ClinicApplication.Models
{
    public class PhieuKham
    {
        public int Id { get; set; } 
        public string? MaPhieu { get; set; } 
        public int BenhNhanId { get; set; } 

        public string? CanNang { get; set; }
        public string? ChieuCao { get; set; }
        public string? TienSu { get; set; }
        public string? LamSang { get; set; }
        public string? Mach { get; set; }
        public string? NhietDo { get; set; }
        public string? HuyetApCao { get; set; }
        public string? HuyetApThap { get; set; }
        public string? TeBao { get; set; }
        public string? MauChay { get; set; }
        public string? MoTa { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? NgayCapNhat { get; set; } = DateTime.Now;

        public string? ChuanDoan { get; set; }
        public string? DieuTri { get; set; }

        public string? HinhAnh1 { get; set; }
        public string? HinhAnh2 { get; set; }
    }
}