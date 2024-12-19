using System;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.ViewModels;

public class DoctorViewModel
{
    public Employee Employee { get; set; }

    public int AppointmentCount { get; set; }

    public int FutureAppointmentCount { get; set; }

    public int CompletedAppointmentCount { get; set; }

    public int CancelledAppointmentCount { get; set; }

    public List<Appointment> Appointments { get; set; } = new List<Appointment>();

}
