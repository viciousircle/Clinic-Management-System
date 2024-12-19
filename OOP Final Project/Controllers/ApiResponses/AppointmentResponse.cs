using System;
using OOP_Final_Project.Models;


namespace OOP_Final_Project.Controllers.ApiResponses;

public class AppointmentResponse
{
    public List<Appointment> Appointments { get; set; } = new();

}
