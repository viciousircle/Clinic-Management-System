using System;
using OOP_Final_Project.ViewModels;

namespace OOP_Final_Project.Controllers.ApiResponses;

public class PatientResponse
{
    public int EmployeeId { get; set; }
    public List<PatientViewModel> Patients { get; set; } = new List<PatientViewModel>();

}
