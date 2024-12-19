using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;


namespace OOP_Final_Project.Controllers;

[ApiController]
[Route("api/[controller]")]

// - GET /api/employees: List all employees.
// - GET /api/employees/{id}: Get a specific employee by ID.
// - GET /api/employees/byFirstName/{firstName}: Get an employee by first name.

// - GET /api/employees/{id}/appointments: Get all appointments for an employee.
// - GET /api/employees/{id}/appointments/count: Get the total number of appointments for an employee.

// - GET /api/employees/{id}/appointments/future: Get all future appointments for an employee in the next 30 days.
// - GET /api/employees/{id}/appointments/future/count: Get the total number of future appointments for an employee in the next 30 days.

// - GET /api/employees/{id}/appointments/completed: Get all completed appointments for an employee.
// - GET /api/employees/{id}/appointments/completed/count: Get the total number of completed appointments for an employee.

// - GET /api/employees/{id}/appointments/cancelled: Get all cancelled appointments for an employee.
// - GET /api/employees/{id}/appointments/cancelled/count: Get the total number of cancelled appointments for an employee.

// - GET /api/employees/{id}/patients: Get all patients for an employee.


// - POST /api/employees: Add a new employee.

public class EmployeesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var employees = _context.Employees.ToList();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var employee = _context.Employees.Find(id);
        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpGet("byFirstName/{firstName}")]
    public IActionResult GetByFirstName(string firstName)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.FirstName == firstName);
        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpGet("{id}/appointments/count")]
    public IActionResult GetTotalAppointmentsByEmployeeId(int id)
    {
        var employee = _context.Employees
            .Include(e => e.Appointments) // Ensure Appointments are included
            .FirstOrDefault(e => e.Id == id);

        if (employee == null)
            return NotFound();

        var totalAppointments = employee.Appointments.Count;
        return Ok(new { EmployeeId = id, TotalAppointments = totalAppointments });
    }

    [HttpGet("{id}/appointments/future/count")]
    public IActionResult GetTotalFutureAppointmentsByEmployeeId(int id)
    {
        // Calculate the date 30 days from today
        var currentDate = DateTime.Now;
        var futureDate = currentDate.AddDays(30);

        // Query to fetch the total future appointments
        var totalFutureAppointments = _context.DocumentAppointments
            .Join(_context.Appointments, doc => doc.AppointmentId, appt => appt.Id, (doc, appt) => new { doc, appt })
            .Where(joined => joined.appt.DoctorId == id
                          && joined.doc.Date >= currentDate
                          && joined.doc.Date <= futureDate)
            .Count();

        return Ok(new { EmployeeId = id, TotalFutureAppointments = totalFutureAppointments });
    }

    [HttpGet("{id}/appointments/completed/count")]
    public IActionResult GetTotalCompletedAppointmentsByEmployeeId(int id)
    {
        // Query to fetch total completed appointments
        var totalCompletedAppointments = _context.DocumentAppointments
            .Join(_context.Appointments, doc => doc.AppointmentId, appt => appt.Id, (doc, appt) => new { doc, appt })
            .Where(joined => joined.appt.DoctorId == id
                          && joined.doc.Date <= DateTime.Now.Date)
            .Count();

        // Return the result
        return Ok(new { EmployeeId = id, TotalCompletedAppointments = totalCompletedAppointments });
    }

    [HttpGet("{id}/appointments/cancelled/count")]
    public IActionResult GetTotalCancelledAppointmentsByEmployeeId(int id)
    {
        // Query to fetch total canceled appointments
        var totalCancelledAppointments = _context.DocumentCancels
            .Join(_context.Appointments, cancel => cancel.AppointmentId, appt => appt.Id, (cancel, appt) => new { cancel, appt })
            .Where(joined => joined.appt.DoctorId == id)
            .Count();

        // Return the result
        return Ok(new { EmployeeId = id, TotalCancelledAppointments = totalCancelledAppointments });
    }

    [HttpGet("{id}/patients")]
    public IActionResult GetAllPatientsByEmployeeId(int id)
    {
        var patients = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Join(_context.Patients, appt => appt.PatientId, patient => patient.Id, (appt, patient) => patient)
            .Distinct()
            .Select(patient => new
            {
                patient.Id,
                patient.FirstName,
                patient.LastName,
                patient.Email,
                patient.Phone,

            })
            .ToList();

        return Ok(new { EmployeeId = id, Patients = patients });
    }


    [HttpGet("{id}/appointments")]
    public IActionResult GetAllAppointmentsByEmployeeId(int id)
    {
        var appointments = _context.Appointments
            .Where(appt => appt.DoctorId == id)
             .Include(appt => appt.Doctor)  // Include Doctor data
            .Include(appt => appt.Patient) // Include Patient data
            .Join(_context.DocumentAppointments, appt => appt.Id, doc => doc.AppointmentId, (appt, doc) => new { appt, doc })
            .Join(_context.DocumentDiagnoses, appt => appt.appt.Id, diag => diag.AppointmentId, (appt, diag) => new { appt.appt, appt.doc, diag })
            .Select(appt => new
            {
                appt.appt.Id,
                appt.appt.PatientId,
                appt.appt.DoctorId,
                appt.doc.TimeBook,
                appt.doc.Date,
                appt.doc.TimeStart,
                appt.doc.TimeEnd,
                appt.doc.Location,
                appt.diag.IsSick,
                appt.diag.PatientStatus,
                appt.diag.DiagnoseDetails,
                Doctor = new
                {
                    appt.appt.Doctor.Id, // Add Doctor's ID
                    appt.appt.Doctor.FirstName, // Add other Doctor properties you need

                },
                Patient = new
                {
                    appt.appt.Patient.Id, // Add Patient's ID
                    appt.appt.Patient.FirstName, // Add other Patient properties you need
                    appt.appt.Patient.LastName,
                },
                DocumentAppointment = new
                {
                    appt.doc.Date,
                    appt.doc.TimeStart
                },
                DocumentDiagnose = new
                {
                    appt.diag.PatientStatus
                }
            })
            .ToList();

        return Ok(new { Appointments = appointments });

    }



    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }




}

