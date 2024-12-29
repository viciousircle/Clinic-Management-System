using System;
using OOP_Final_Project.ViewModels;
using OOP_Final_Project.ViewModels.Shared;

namespace OOP_Final_Project.Controllers.ApiResponses;

/*
    - This class is used to represent the response from the API when requesting a single employee.
    
*/

public class EmployeeResponse
{
    public EmployeeViewModel Employee { get; set; } = new();

}

public class EmployeesResponse
{
    public List<EmployeeViewModel> Employees { get; set; } = new();
}

public class LoginResponse
{
    public int AccountId { get; set; }
    public int AccountTypeId { get; set; }
}
