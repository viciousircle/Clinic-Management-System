using System;
using OOP_Final_Project.ViewModels;


namespace OOP_Final_Project.Controllers.ApiResponses;

public class AppointmentResponse
{
    public List<AppointmentViewModel> Appointments { get; set; } = new();

}


