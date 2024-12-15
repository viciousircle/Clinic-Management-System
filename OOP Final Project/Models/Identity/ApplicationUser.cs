using System;
using Microsoft.AspNetCore.Identity;

namespace OOP_Final_Project.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public int AccountTypeId { get; set; }
    public required AccountType AccountType { get; set; }
}
