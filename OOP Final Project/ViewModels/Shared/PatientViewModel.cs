using System;

namespace OOP_Final_Project.ViewModels.Shared;

public class PatientViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string LatestVisit { get; set; }
}
