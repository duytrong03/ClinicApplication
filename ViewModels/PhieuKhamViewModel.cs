using System.ComponentModel.DataAnnotations;
namespace ClinicApplication.ViewModels
{
    public class MedicalFormViewModel
    {
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
        public string? ChuanDoan { get; set; }
        public string? DieuTri { get; set; }
        public string? HinhAnh1 { get; set; }
        public string? HinhAnh2 { get; set; }
    }
}