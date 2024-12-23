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

    public List<PatientViewModel> Patients { get; set; } = new List<PatientViewModel>();

    public List<AppointmentViewModel> Appointments { get; set; } = new List<AppointmentViewModel>();
}

public class AppointmentViewModel
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public PatientViewModel Patient { get; set; }
    public AppointmentRecordViewModel AppointmentRecord { get; set; }
    public DiagnoseViewModel Diagnose { get; set; }
}

public class PatientViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsSick { get; set; }
    public string PatientStatus { get; set; }
}

public class AppointmentRecordViewModel
{
    public DateTime TimeBook { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan TimeStart { get; set; }
    public TimeSpan TimeEnd { get; set; }
    public string Location { get; set; }
}

public class DiagnoseViewModel
{
    public string DiagnoseDetails { get; set; }
}