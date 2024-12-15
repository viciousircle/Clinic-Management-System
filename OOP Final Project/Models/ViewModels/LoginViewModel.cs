using System;

namespace OOP_Final_Project.Models.ViewModels;

public class LoginViewModel
{

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public bool RememberMe { get; set; }

}
