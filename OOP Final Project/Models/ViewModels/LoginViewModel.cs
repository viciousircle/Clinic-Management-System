using System;
using System.ComponentModel.DataAnnotations;

namespace OOP_Final_Project.Models.ViewModels;

public class LoginViewModel
{

    [Required(ErrorMessage = "Username is required.")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    public bool RememberMe { get; set; }

}
