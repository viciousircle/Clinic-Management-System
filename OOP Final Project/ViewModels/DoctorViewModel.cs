using System;
using System.Collections.Generic;
using OOP_Final_Project.Models;
using OOP_Final_Project.ViewModels.Shared;

namespace OOP_Final_Project.ViewModels;

public class DoctorViewModel
{
    // --- Doctor Information ---------------
    public EmployeeViewModel Doctor { get; set; } = new EmployeeViewModel();
    public List<EmployeeViewModel> DoctorList { get; set; } = new List<EmployeeViewModel>();

    // --- Appointment Count ----------------
    public int AppointmentCount { get; set; }
    public int FutureAppointmentCount { get; set; }
    public int CompletedAppointmentCount { get; set; }
    public int CancelledAppointmentCount { get; set; }

    // --- Appointment Lists ----------------
    public List<AppointmentViewModel> Appointments { get; set; } = new List<AppointmentViewModel>();
    public List<AppointmentViewModel> TodayAppointments { get; set; } = new List<AppointmentViewModel>();
    public List<AppointmentViewModel> AppointmentsOnDate { get; set; } = new List<AppointmentViewModel>();
    public List<AppointmentViewModel> PastAppointments { get; set; } = new List<AppointmentViewModel>();

    // --- Patient Count --------------------
    public int PatientCount { get; set; }
    public int ObservedPatientCount { get; set; }
    // --- Patient Lists --------------------
    public List<PatientViewModel> Patients { get; set; } = new List<PatientViewModel>();

}