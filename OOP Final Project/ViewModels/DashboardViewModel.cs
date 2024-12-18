using System;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.ViewModels;

public class DashboardViewModel
{
    public Employee Employee { get; set; }
    public Appointment Appointment { get; set; }
    public Patient Patient { get; set; }
    public Schedule Schedule { get; set; }

    public int AppointmentCount { get; set; } // Add this property

}
