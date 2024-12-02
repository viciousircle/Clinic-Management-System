using System;

namespace OOP_Final_Project.Models;

public class HasRole
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int RoleId { get; set; }
    public DateTime TimeFrom { get; set; }
    public DateTime TimeTo { get; set; }
    public bool IsActive { get; set; }
    public Employee Employee { get; set; }
    public Role Role { get; set; }
}

