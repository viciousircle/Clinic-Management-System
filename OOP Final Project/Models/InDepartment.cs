using System;

namespace OOP_Final_Project.Models;

public class InDepartment
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int DepartmentId { get; set; }
    public DateTime TimeFrom { get; set; }
    public DateTime TimeTo { get; set; }
    public bool IsActive { get; set; }
}

