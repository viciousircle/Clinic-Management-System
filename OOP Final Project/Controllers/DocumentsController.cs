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

// - GET /api/documentsBill: List all documents of type Bill.
// - GET /api/documentsDiagnose: List all documents of type Diagnose.
// - GET /api/documentsAppointment: List all documents of type Appointment.
// - GET /api/documentsCancel: List all documents of type Cancel.
// - GET /api/documentBill/{id}: Get a document of type Bill by id.

public class DocumentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DocumentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("documentsBill")]
    public IActionResult GetAll()
    {
        var documents = _context.DocumentBills
            .Include(d => d.Appointment)
            .ToList();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        return new JsonResult(documents, options);
    }

    [HttpGet("documentsDiagnose")]
    public IActionResult GetAllDiagnose()
    {
        var documents = _context.DocumentDiagnoses
            .Include(d => d.Appointment)
            .ToList();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        return new JsonResult(documents, options);
    }

    [HttpGet("documentsAppointment")]
    public IActionResult GetAllAppointment()
    {
        var documents = _context.DocumentAppointments
            .Include(d => d.Appointment)
            .ToList();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        return new JsonResult(documents, options);
    }

    [HttpGet("documentsCancel")]
    public IActionResult GetAllCancel()
    {
        var documents = _context.DocumentCancels
            .Include(d => d.Appointment)
            .ToList();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        return new JsonResult(documents, options);
    }

    [HttpGet("documentBill/{id}")]
    public IActionResult GetBill(int id)
    {
        var document = _context.DocumentBills
            .Include(d => d.Appointment)
            .FirstOrDefault(d => d.Id == id);

        if (document == null)
        {
            return NotFound();
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        return new JsonResult(document, options);
    }

    [HttpGet("documentDiagnose/{id}")]
    public IActionResult GetDiagnose(int id)
    {
        var document = _context.DocumentDiagnoses
            .Include(d => d.Appointment)
            .FirstOrDefault(d => d.Id == id);

        if (document == null)
        {
            return NotFound();
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        return new JsonResult(document, options);
    }

    [HttpGet("documentAppointment/{id}")]
    public IActionResult GetAppointment(int id)
    {
        var document = _context.DocumentAppointments
            .Include(d => d.Appointment)
            .FirstOrDefault(d => d.Id == id);

        if (document == null)
        {
            return NotFound();
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        return new JsonResult(document, options);
    }

    [HttpGet("documentCancel/{id}")]
    public IActionResult GetCancel(int id)
    {
        var document = _context.DocumentCancels
            .Include(d => d.Appointment)
            .FirstOrDefault(d => d.Id == id);

        if (document == null)
        {
            return NotFound();
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        return new JsonResult(document, options);
    }

}
