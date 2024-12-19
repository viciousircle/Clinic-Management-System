using System;
using System.Collections.Generic;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.ViewModels;

public class DoctorViewModel
{
    public Employee Employee { get; set; } = new Employee();

    public int AppointmentCount { get; set; }

    public int FutureAppointmentCount { get; set; }

    public int CompletedAppointmentCount { get; set; }

    public int CancelledAppointmentCount { get; set; }

    public List<Patient> Patients { get; set; } = new List<Patient>();

    public List<Appointment> Appointments { get; set; } = new List<Appointment>();

}
