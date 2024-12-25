using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;
using OOP_Final_Project.ViewModels;
using OOP_Final_Project.ViewModels.Shared;

namespace OOP_Final_Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MedicinesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var medicines = _context.Medicines.Include(m => m.MedicineType).Include(m => m.Importer)
        .Select(m => new MedicineViewModel
        {
            Id = m.Id,
            MedicineTypeId = m.MedicineTypeId,
            Name = m.Name,
            ExpiredDate = m.ExpiredDate.ToString("yyyy-MM-dd"),
            ImportDate = m.ImportDate.ToString("yyyy-MM-dd"),
            ImporterId = m.ImporterId,
            Quantity = m.Quantity
        }).ToList();

        return Ok(medicines);
    }

}
