using System;
using OOP_Final_Project.ViewModels;

namespace OOP_Final_Project.Controllers.ApiResponses;

public class MedicineResponse
{
    public MedicineViewModel Medicine { get; set; } = new();

}

public class MedicinesResponse
{
    public List<MedicineViewModel> Medicines { get; set; } = new();

    public List<MedicineViewModel> ExpiredMedicines { get; set; } = new();

    public List<MedicineViewModel> ExpiredSoonMedicines { get; set; } = new();

    public List<MedicineViewModel> LowStockMedicines { get; set; } = new();
}