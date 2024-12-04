using System;
using Microsoft.AspNetCore.Identity;


namespace OOP_Final_Project.Models;

public class ApplicationRole : IdentityRole
{
    // Thêm các thuộc tính bổ sung cho vai trò nếu cần
    public string Description { get; set; }
}

