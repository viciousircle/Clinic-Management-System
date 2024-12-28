using System;
using OOP_Final_Project.ViewModels;

namespace OOP_Final_Project.Controllers.ApiResponses;

public class PrescriptionResponse
{
    public PrescriptionViewModel Prescription { get; set; } = new();
}

public class PrescriptionsResponse
{
    public List<PrescriptionViewModel> Prescriptions { get; set; } = new();
}
