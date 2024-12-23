using System;
using OOP_Final_Project.ViewModels;
using OOP_Final_Project.ViewModels.Shared;

namespace OOP_Final_Project.Controllers.ApiResponses;

public class PatientsResponse
{
    public int EmployeeId { get; set; }
    public List<PatientViewModel> Patients { get; set; } = new();

}
