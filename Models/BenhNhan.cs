using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
namespace ClinicApplication.Models
{
    [Table("benhnhan")]
    public class BenhNhan{
        [Key]
        [Column("id")]
        public int Id {get; set;}
        [Column("hovaten")]
        [Required(ErrorMessage = "Họ và tên không được để trống!")]
        [MaxLength(50,ErrorMessage = "Họ và tên chỉ 50 ký tự!")]
        public string? HoVaTen {get; set;}
        [Column("socon")]
        public int SoCon {get; set;}
        [Column("namsinh")]
        public int NamSinh {get; set;}
        [Column("sohoso")]
        public string? SoHoSo {get; set;}
        [Column("diachi")]
        public string? DiaChi {get; set;}
        [Column("gioitinh")]

        [RegularExpression("Nam|Nữ", ErrorMessage = "Giới tính phải Nam hoặc Nữ!")]
        public string? GioiTinh {get; set;}

        [Column("nghenghiep")]
        public string? NgheNghiep {get; set;}

        [Column("ngaytao")]
        public DateTime NgayTao {get; set;}= DateTime.Now;

        [Column("ngaycapnhat")]
        public DateTime? NgayCapNhat {get; set;} = DateTime.Now;

        [Column("sodienthoai")]
        [RegularExpression(@"^0[1-9][0-9]{8}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có 10 số!")]
        public string? SoDienThoai {get; set;}
        
        [Column("email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email {get; set;}

    }
}