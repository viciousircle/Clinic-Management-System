using System;
using OOP_Final_Project.ViewModels;

namespace OOP_Final_Project.Controllers.ApiResponses;

public class ScheduleResponse
{
    public List<ScheduleViewModel> Schedule { get; set; } = new List<ScheduleViewModel>();
}
