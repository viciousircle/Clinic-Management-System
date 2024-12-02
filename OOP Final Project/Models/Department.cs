using System;

namespace OOP_Final_Project.Models;

public class Department
{
    public int Id { get; set; }
    public int ClinicId { get; set; }
    public string DepartmentName { get; set; }
    public Clinic Clinic { get; set; }
}

