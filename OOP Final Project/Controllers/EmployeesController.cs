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
        var employees = _context.Employees
        .Select(employee => new
        {
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.Phone,
            employee.AccountId,
            employee.IsActive,
        })
        .ToList();
        return Ok(new { Employees = employees });
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var employee = _context.Employees.Where(e => e.Id == id).Select(e => new
        {
            e.Id,
            e.FirstName,
            e.LastName,
            e.Email,
            e.Phone,
            e.AccountId,
            e.IsActive,
        }).ToList().FirstOrDefault();

        if (employee == null)
            return NotFound();

        return Ok(new { Employee = employee });
    }

    // ------------------- Appointments -------------------

    [HttpGet("{id}/appointments")]
    public IActionResult GetAllAppointmentsByEmployeeId(int id)
    {
        var appointments = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Include(appt => appt.Doctor)
            .Include(appt => appt.Patient)
            .Join(_context.DocumentAppointments, appt => appt.Id, doc => doc.AppointmentId, (appt, doc) => new { appt, doc })
            .Join(_context.DocumentDiagnoses, appt => appt.appt.Id, diag => diag.AppointmentId, (appt, diag) => new { appt.appt, appt.doc, diag })
            .Select(appt => new
            {
                appt.appt.Id,
                appt.appt.DoctorId,
                Patient = new
                {
                    appt.appt.Patient.Id,
                    appt.appt.Patient.FirstName,
                    appt.appt.Patient.LastName,
                    appt.diag.IsSick,
                    appt.diag.PatientStatus,
                },
                AppointmentRecord = new
                {
                    appt.doc.TimeBook,
                    appt.doc.Date,
                    appt.doc.TimeStart,
                    appt.doc.TimeEnd,
                    appt.doc.Location,
                },
                Diagnose = new
                {
                    appt.diag.DiagnoseDetails,
                }
            })
            .ToList();

        return Ok(new { Appointments = appointments });
    }

    [HttpGet("{id}/appointments/today")]
    public IActionResult GetAppointmentsToday(int id)
    {
        var today = DateTime.Today;

        var appointments = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Include(appt => appt.Doctor)
            .Include(appt => appt.Patient)
            .Join(_context.DocumentAppointments, appt => appt.Id, doc => doc.AppointmentId, (appt, doc) => new { appt, doc })
            .Join(_context.DocumentDiagnoses, appt => appt.appt.Id, diag => diag.AppointmentId, (appt, diag) => new { appt.appt, appt.doc, diag })
            .Where(joined => joined.doc.Date.Date == today)
            .Select(joined => new
            {
                joined.appt.Id,
                joined.appt.DoctorId,
                Patient = new
                {
                    joined.appt.Patient.Id,
                    joined.appt.Patient.FirstName,
                    joined.appt.Patient.LastName,
                    joined.diag.IsSick,
                    joined.diag.PatientStatus,
                },
                AppointmentRecord = new
                {
                    joined.doc.TimeBook,
                    Date = joined.doc.Date.Date, // Ensure only the date part is returned
                    joined.doc.TimeStart,
                    joined.doc.TimeEnd,
                    joined.doc.Location,
                },
                Diagnose = new
                {
                    joined.diag.DiagnoseDetails,
                }
            })
            .ToList();

        return Ok(new { Date = today, Appointments = appointments });
    }

    [HttpGet("{id}/appointments/on/{date}")]
    public IActionResult GetAppointmentsOnSpecificDay(int id, DateTime date)
    {
        var appointments = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Include(appt => appt.Doctor)
            .Include(appt => appt.Patient)
            .Join(_context.DocumentAppointments, appt => appt.Id, doc => doc.AppointmentId, (appt, doc) => new { appt, doc })
            .Join(_context.DocumentDiagnoses, appt => appt.appt.Id, diag => diag.AppointmentId, (appt, diag) => new { appt.appt, appt.doc, diag })
            .Where(joined => joined.doc.Date.Date == date.Date)
            .Select(joined => new
            {
                joined.appt.Id,
                joined.appt.DoctorId,
                Patient = new
                {
                    joined.appt.Patient.Id,
                    joined.appt.Patient.FirstName,
                    joined.appt.Patient.LastName,
                    joined.diag.IsSick,
                    joined.diag.PatientStatus,
                },
                AppointmentRecord = new
                {
                    joined.doc.TimeBook,
                    Date = joined.doc.Date.Date, // Ensure only the date part is returned
                    joined.doc.TimeStart,
                    joined.doc.TimeEnd,
                    joined.doc.Location,
                },
                Diagnose = new
                {
                    joined.diag.DiagnoseDetails,
                }
            })
            .ToList();

        return Ok(new { Date = date.Date, Appointments = appointments });
    }

    [HttpGet("{id}/appointments/past")]
    public IActionResult GetPastAppointments(int id)
    {
        var today = DateTime.Today;

        var appointments = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Include(appt => appt.Doctor)
            .Include(appt => appt.Patient)
            .Join(_context.DocumentAppointments, appt => appt.Id, doc => doc.AppointmentId, (appt, doc) => new { appt, doc })
            .Join(_context.DocumentDiagnoses, appt => appt.appt.Id, diag => diag.AppointmentId, (appt, diag) => new { appt.appt, appt.doc, diag })
            .Where(joined => joined.doc.Date.Date < today)
            .Select(joined => new
            {
                joined.appt.Id,
                joined.appt.DoctorId,
                Patient = new
                {
                    joined.appt.Patient.Id,
                    joined.appt.Patient.FirstName,
                    joined.appt.Patient.LastName,
                    joined.diag.IsSick,
                    joined.diag.PatientStatus,
                },
                AppointmentRecord = new
                {
                    joined.doc.TimeBook,
                    joined.doc.Date,
                    joined.doc.TimeStart,
                    joined.doc.TimeEnd,
                    joined.doc.Location,
                },
                Diagnose = new
                {
                    joined.diag.DiagnoseDetails,
                }
            })
            .ToList();

        return Ok(new { Date = today, PastAppointments = appointments });
    }

    // ------------------- Appointments Counts -------------------

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

    // ------------------- Patients -------------------

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
                Phone = System.Text.RegularExpressions.Regex.Replace(patient.Phone, @"\s*x\d+$", "") // Remove extension
            })
            .ToList();

        return Ok(new { EmployeeId = id, Patients = patients });
    }



    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }





}

