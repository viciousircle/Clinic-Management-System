using System;

namespace OOP_Final_Project.Models;

public class Clinic
{
    public int Id { get; set; }
    public required string ClinicName { get; set; }
    public required string Address { get; set; }
    public required string Details { get; set; }
}

