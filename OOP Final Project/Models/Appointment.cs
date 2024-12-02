using System;

namespace OOP_Final_Project.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientCaseId { get; set; }
    public int InDepartmentId { get; set; }
    public DateTime TimeCreated { get; set; }
    public DateTime AppointmentStartTime { get; set; }
    public DateTime AppointmentEndTime { get; set; }
    public int AppointmentStatusId { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
}

