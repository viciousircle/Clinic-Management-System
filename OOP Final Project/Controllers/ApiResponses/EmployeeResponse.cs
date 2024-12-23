using System;
using OOP_Final_Project.ViewModels;

namespace OOP_Final_Project.Controllers.ApiResponses;

public class EmployeeResponse
{
    public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();

}
