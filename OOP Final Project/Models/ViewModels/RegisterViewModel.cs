using System;

namespace OOP_Final_Project.Models.ViewModels;

public class RegisterViewModel
{

    public required string Email { get; set; }

    public required string Password { get; set; }

    public int AccountTypeId { get; set; } // Selected role (AccountType)

}
