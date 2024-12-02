using System;

namespace OOP_Final_Project.Models;

public class PatientCase
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool InProgress { get; set; }
    public decimal TotalCost { get; set; }
    public decimal AmountPaid { get; set; }
    public Patient Patient { get; set; }
}

