using System;

namespace OOP_Final_Project.Models;

public class Schedule
{
    public int Id { get; set; }
    public int InDepartmentId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan TimeStart { get; set; }
    public TimeSpan TimeEnd { get; set; }
    public InDepartment InDepartment { get; set; }
}

