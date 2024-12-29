using System;
using OOP_Final_Project.ViewModels;

namespace OOP_Final_Project.ViewModels.Shared;

public class AppointmentViewModel
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public PatientViewModel Patient { get; set; }
    public AppointmentRecordViewModel AppointmentRecord { get; set; }
    public DiagnoseViewModel Diagnose { get; set; }

    public List<MedicinePrescriptionViewModel> Medicines { get; set; } = new List<MedicinePrescriptionViewModel>();
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