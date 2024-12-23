using System;
using System.Collections.Generic;
using Bogus.DataSets;
using OOP_Final_Project.Models;
using OOP_Final_Project.ViewModels;

namespace OOP_Final_Project.ViewModels;

public class DoctorViewModel
{
    public EmployeeViewModel Doctor { get; set; } = new EmployeeViewModel();

    public List<EmployeeViewModel> DoctorList { get; set; } = new List<EmployeeViewModel>();

    public int AppointmentCount { get; set; }

    public int FutureAppointmentCount { get; set; }

    public int CompletedAppointmentCount { get; set; }

    public int CancelledAppointmentCount { get; set; }

    public List<PatientViewModel> Patients { get; set; } = new List<PatientViewModel>();

    public List<AppointmentViewModel> Appointments { get; set; } = new List<AppointmentViewModel>();

    public List<AppointmentViewModel> TodayAppointments { get; set; } = new List<AppointmentViewModel>();    // For today's appointments

    public List<AppointmentViewModel> AppointmentsOnDate { get; set; } = new List<AppointmentViewModel>();    // For appointments on a specific date

    public List<AppointmentViewModel> PastAppointments { get; set; } = new List<AppointmentViewModel>();     // For past appointments
}

public class EmployeeViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int AccountId { get; set; }
    public bool IsActive { get; set; }
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
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string LatestVisit { get; set; }
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
    public bool IsSick { get; set; }
    public string PatientStatus { get; set; }
}