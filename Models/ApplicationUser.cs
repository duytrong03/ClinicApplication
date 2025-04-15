using Microsoft.AspNetCore.Identity;
using System;

public class ApplicationUser : IdentityUser
{
    public string HoTen { get; set; }   // Họ tên người dùng
    public DateTime NgayTao { get; set; } = DateTime.UtcNow; // Thời gian tạo tài khoản
}