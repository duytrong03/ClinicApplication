using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicApplication.ViewModels
{
    public class PatientViewModel
    {
        [Required(ErrorMessage = "Họ và tên không được để trống!")]
        [MaxLength(50, ErrorMessage = "Họ và tên tối đa 50 ký tự!")]
        public string? HoVaTen { get; set; }

        public int SoCon { get; set; }
        public int NamSinh { get; set; }
        public string? SoHoSo { get; set; }
        public string? DiaChi { get; set; }

        [RegularExpression("Nam|Nữ", ErrorMessage = "Giới tính phải là 'Nam' hoặc 'Nữ'!")]
        public string? GioiTinh { get; set; }

        public string? NgheNghiep { get; set; }

        [RegularExpression(@"^0[1-9][0-9]{8}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có 10 số!")]
        public string? SoDienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email { get; set; }
    }
}
