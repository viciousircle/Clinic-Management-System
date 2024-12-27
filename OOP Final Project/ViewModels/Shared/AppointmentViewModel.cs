using System;

namespace OOP_Final_Project.ViewModels.Shared;

public class AppointmentViewModel
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public PatientViewModel Patient { get; set; }
    public AppointmentRecordViewModel AppointmentRecord { get; set; }
    public DiagnoseViewModel Diagnose { get; set; }
}

public class AppointmentRecordViewModel
{
    public DateTime TimeBook { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan TimeStart { get; set; }
    public TimeSpan TimeEnd { get; set; }
}

public class DiagnoseViewModel
{
    public string DiagnoseDetails { get; set; }
    public bool IsSick { get; set; }
    public string PatientStatus { get; set; }
}