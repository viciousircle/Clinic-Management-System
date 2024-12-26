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
            MedicineTypeName = m.MedicineType.Name,
            Name = m.Name,
            ExpiredDate = m.ExpiredDate.ToString("yyyy-MM-dd"),
            ImportDate = m.ImportDate.ToString("yyyy-MM-dd"),
            ImporterId = m.ImporterId,
            Quantity = m.Quantity
        }).ToList();

        return Ok(medicines);
    }

    [HttpGet("total")]
    public IActionResult GetTotal()
    {
        var total = _context.Medicines.Count();
        return Ok(total);
    }

    [HttpGet("total/expired")]
    public IActionResult GetTotalExpired()
    {
        var total = _context.Medicines.Count(m => m.ExpiredDate < DateTime.Now);
        return Ok(total);
    }

    [HttpGet("total/available")]
    public IActionResult GetTotalAvailable()
    {
        var total = _context.Medicines.Count(m => m.ExpiredDate > DateTime.Now);
        return Ok(total);
    }

    [HttpGet("total/expiredSoon")]
    public IActionResult GetTotalExpiredSoon()
    {
        var total = _context.Medicines.Count(m => m.ExpiredDate < DateTime.Now.AddDays(30));
        return Ok(total);
    }

    [HttpGet("total/lowStock")]
    public IActionResult GetTotalLowStock()
    {
        var total = _context.Medicines.Count(m => m.Quantity < 10);
        return Ok(total);
    }

    [HttpGet("expired")]
    public IActionResult GetExpired()
    {
        var medicines = _context.Medicines.Include(m => m.MedicineType).Include(m => m.Importer)
        .Where(m => m.ExpiredDate < DateTime.Now)
        .Select(m => new MedicineViewModel
        {
            Id = m.Id,
            MedicineTypeName = m.MedicineType.Name,
            Name = m.Name,
            ExpiredDate = m.ExpiredDate.ToString("yyyy-MM-dd"),
            ImportDate = m.ImportDate.ToString("yyyy-MM-dd"),
            ImporterId = m.ImporterId,
            Quantity = m.Quantity
        }).ToList();

        return Ok(medicines);
    }

    [HttpGet("lowStock")]
    public IActionResult GetLowStock()
    {
        var medicines = _context.Medicines.Include(m => m.MedicineType).Include(m => m.Importer)
        .Where(m => m.Quantity < 10)
        .Select(m => new MedicineViewModel
        {
            Id = m.Id,
            MedicineTypeName = m.MedicineType.Name,
            Name = m.Name,
            ExpiredDate = m.ExpiredDate.ToString("yyyy-MM-dd"),
            ImportDate = m.ImportDate.ToString("yyyy-MM-dd"),
            ImporterId = m.ImporterId,
            Quantity = m.Quantity
        }).ToList();

        return Ok(medicines);
    }

}
