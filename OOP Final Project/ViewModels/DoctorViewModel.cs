using System;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.ViewModels;

public class DoctorViewModel
{
    public Employee Employee { get; set; }
    public Appointment Appointment { get; set; }
    public Patient Patient { get; set; }
    public Schedule Schedule { get; set; }

    public int AppointmentCount { get; set; }

    public int FutureAppointmentCount { get; set; }

    public int CompletedAppointmentCount { get; set; }

}
