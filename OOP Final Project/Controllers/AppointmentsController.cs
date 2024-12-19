using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OOP_Final_Project.Controllers;

[ApiController]
[Route("api/[controller]")]

// - GET /api/appointments: List all appointments.
// - GET /api/appointments/{id}: Get a specific appointment by ID.
// - GET /api/appointments/byPatientId/{patientId}: Get all appointments for a patient.
// - GET /api/appointments/byDoctorId/{doctorId}: Get all appointments for a doctor.

public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppointmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var appointments = _context.Appointments
            .ToList();

        return Ok(appointments);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var appointment = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.DocumentAppointment)
            .Include(a => a.DocumentBill)
            .Include(a => a.DocumentDiagnose)
            .FirstOrDefault(a => a.Id == id);

        if (appointment == null)
            return NotFound();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return new JsonResult(appointment, options);
    }

    [HttpGet("byPatientId/{patientId}")]
    public IActionResult GetByPatientId(int patientId)
    {
        var appointments = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.PatientId == patientId)
            .ToList();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return new JsonResult(appointments, options);
    }

    [HttpGet("byDoctorId/{doctorId}")]
    public IActionResult GetByDoctorId(int doctorId)
    {
        var appointments = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.DoctorId == doctorId)
            .ToList();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return new JsonResult(appointments, options);
    }
}

