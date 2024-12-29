using System;
using System.Collections.Generic;
using Bogus.DataSets;
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

    public List<PatientViewModel> ObservedPatients { get; set; } = new List<PatientViewModel>();

    public List<MedicineViewModel> Medicines { get; set; } = new List<MedicineViewModel>();

    public List<MedicineViewModel> ExpiredMedicines { get; set; } = new List<MedicineViewModel>();

    public List<MedicineViewModel> ExpiredSoonMedicines { get; set; } = new List<MedicineViewModel>();

    public List<MedicineViewModel> LowStockMedicines { get; set; } = new List<MedicineViewModel>();

    public MedicineViewModel Medicine { get; set; } = new MedicineViewModel();

    public int TotalMedicineCount { get; set; }
    public int TotalExpiredMedicineCount { get; set; }
    public int TotalExpiredSoonMedicineCount { get; set; }
    public int TotalLowStockMedicineCount { get; set; }

    public List<PrescriptionViewModel> Prescriptions { get; set; } = new List<PrescriptionViewModel>();
    public List<PrescriptionViewModel> OnDatePrescriptions { get; set; } = new List<PrescriptionViewModel>();

    public PrescriptionViewModel Prescription { get; set; } = new PrescriptionViewModel();

    public List<PrescriptionViewModel> OnDatePreparePrescriptions { get; set; } = new List<PrescriptionViewModel>();
    public List<PrescriptionViewModel> OnDatePickupPrescriptions { get; set; } = new List<PrescriptionViewModel>();
    public List<PrescriptionViewModel> OnDateDonePrescriptions { get; set; } = new List<PrescriptionViewModel>();


    public ScheduleViewModel Schedule { get; set; } = new ScheduleViewModel();



}


public class MedicineViewModel
{
    public int Id { get; set; }
    public string MedicineTypeName { get; set; }
    public string Name { get; set; }
    public string ExpiredDate { get; set; }
    public string ImportDate { get; set; }
    public int ImporterId { get; set; }
    public int Quantity { get; set; }

}


public class PrescriptionViewModel
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public int PharmacistId { get; set; }
    public string PatientName { get; set; }
    public string DoctorName { get; set; }
    public string AppointmentTime { get; set; }
    public string PrescriptionStatus { get; set; }

    public List<MedicinePrescriptionViewModel> Medicines { get; set; } = new List<MedicinePrescriptionViewModel>();

}

public class MedicinePrescriptionViewModel
{
    public int MedicineId { get; set; }
    public string MedicineName { get; set; }
    public string DosageAmount { get; set; }
    public string Frequency { get; set; }
    public string FrequencyUnit { get; set; }
    public string Route { get; set; }
    public string Instruction { get; set; }
}

public class ScheduleViewModel
{
    public int DoctorId { get; set; }
    public List<SectionViewModel> Sections { get; set; } = new List<SectionViewModel>();  // Make sure 'Sections' is defined
}

public class SectionViewModel
{
    public string Time { get; set; }
}
