using System;
using Microsoft.AspNetCore.Mvc;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;


namespace OOP_Final_Project.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{

    // - GET /api/patients: List all patients.
    // - GET /api/patients/{id}: Get a specific patient by ID.
    // - POST /api/patients: Add a new patient.

    private readonly ApplicationDbContext _context;

    public PatientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var patients = _context.Patients.ToList();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var patient = _context.Patients.Find(id);
        if (patient == null)
            return NotFound();

        return Ok(patient);
    }

    [HttpPost]
    public IActionResult Create(Patient patient)
    {
        _context.Patients.Add(patient);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }


}
