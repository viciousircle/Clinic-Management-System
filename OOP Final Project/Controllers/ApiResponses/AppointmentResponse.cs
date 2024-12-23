using System;
using OOP_Final_Project.ViewModels;


namespace OOP_Final_Project.Controllers.ApiResponses;

public class AppointmentsResponse
{
    public List<AppointmentViewModel> Appointments { get; set; } = new();
    public List<AppointmentViewModel> TodayAppointments { get; set; } = new();
    public List<AppointmentViewModel> AppointmentsOnDate { get; set; } = new();
    public List<AppointmentViewModel> PastAppointments { get; set; } = new();

}

public class AppointmentResponse
{
    public AppointmentViewModel Appointment { get; set; } = new();
}


