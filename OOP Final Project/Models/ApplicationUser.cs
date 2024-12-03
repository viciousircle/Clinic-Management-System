using System;
using Microsoft.AspNetCore.Identity;


namespace OOP_Final_Project.Models;

public class ApplicationUser : IdentityUser
{
    // Thêm các thuộc tính bổ sung cho người dùng, ví dụ:
    public string FullName { get; set; }
    public string Address { get; set; }
}