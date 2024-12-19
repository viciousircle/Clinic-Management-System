using System;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.Controllers.ApiResponses;

public class PatientResponse
{
    public int EmployeeId { get; set; }
    public List<Patient> Patients { get; set; } = new();

}
