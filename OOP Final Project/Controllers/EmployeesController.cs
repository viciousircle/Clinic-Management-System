using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;
using OOP_Final_Project.ViewModels;


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
        .Select(employee => new EmployeeViewModel
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            AccountId = employee.AccountId,
            IsActive = employee.IsActive
        })
        .ToList();

        var response = new { Employees = employees };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var employee = _context.Employees
        .Where(e => e.Id == id)
        .Select(e => new EmployeeViewModel
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            AccountId = e.AccountId,
            IsActive = e.IsActive
        }).ToList().FirstOrDefault();

        if (employee == null)
            return NotFound();

        var response = new { Employee = employee };

        return Ok(response);
    }


    // ------------------- Appointments -------------------
    [HttpGet("{id}/appointments")]
    public IActionResult GetAllAppointmentsByEmployeeId(int id)
    {
        var appointments = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Include(appt => appt.Doctor)
            .Include(appt => appt.Patient)
            .GroupJoin(_context.DocumentAppointments, appt => appt.Id, doc => doc.AppointmentId, (appt, docAppointments) => new { appt, docAppointments })
            .Join(_context.DocumentDiagnoses, appt => appt.appt.Id, diag => diag.AppointmentId, (appt, diag) => new { appt.appt, appt.docAppointments, diag })
            .AsEnumerable()
            .Select(appt => new
            {
                appt.appt.Id,
                appt.appt.DoctorId,
                Patient = new PatientViewModel
                {
                    Id = appt.appt.Patient.Id,
                    FirstName = appt.appt.Patient.FirstName,
                    LastName = appt.appt.Patient.LastName,
                    Email = appt.appt.Patient.Email,
                    Phone = System.Text.RegularExpressions.Regex.Replace(appt.appt.Patient.Phone ?? "", @"\s*x\d+$", ""),
                    Address = appt.appt.Patient.Address,
                    LatestVisit = appt.docAppointments
                    .OrderByDescending(doc => doc.Date)
                    .FirstOrDefault()?.Date != default(DateTime) ? appt.docAppointments
                    .OrderByDescending(doc => doc.Date)
                    .FirstOrDefault()?.Date.ToString("dd-MM-yyyy") : "N/A"
                },
                AppointmentRecord = new AppointmentRecordViewModel
                {
                    TimeBook = appt.docAppointments.FirstOrDefault()?.TimeBook ?? default(DateTime),
                    Date = appt.docAppointments.FirstOrDefault()?.Date ?? default(DateTime),
                    TimeStart = appt.docAppointments.FirstOrDefault()?.TimeStart ?? default(TimeSpan),
                    TimeEnd = appt.docAppointments.FirstOrDefault()?.TimeEnd ?? default(TimeSpan),
                    Location = appt.docAppointments.FirstOrDefault()?.Location,
                },
                Diagnose = new DiagnoseViewModel
                {
                    DiagnoseDetails = appt.diag.DiagnoseDetails,
                    IsSick = appt.diag.IsSick,
                    PatientStatus = appt.diag.PatientStatus
                }
            })
            .ToList();

        return Ok(new { Appointments = appointments });
    }


    [HttpGet("{id}/appointments/today")]
    public IActionResult GetTodayAppointmentsByEmployeeId(int id)
    {
        var today = DateTime.Today;

        var appointments = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Include(appt => appt.Doctor)
            .Include(appt => appt.Patient)
            .GroupJoin(_context.DocumentAppointments, appt => appt.Id, doc => doc.AppointmentId, (appt, docAppointments) => new { appt, docAppointments })
            .Join(_context.DocumentDiagnoses, appt => appt.appt.Id, diag => diag.AppointmentId, (appt, diag) => new { appt.appt, appt.docAppointments, diag })
            .AsEnumerable()
            .Where(appt => appt.docAppointments
            .Any(doc => doc.Date.Date == today))
            .Select(appt => new
            {
                appt.appt.Id,
                appt.appt.DoctorId,
                Patient = new PatientViewModel
                {
                    Id = appt.appt.Patient.Id,
                    FirstName = appt.appt.Patient.FirstName,
                    LastName = appt.appt.Patient.LastName,
                    Email = appt.appt.Patient.Email,
                    Phone = System.Text.RegularExpressions.Regex.Replace(appt.appt.Patient.Phone ?? "", @"\s*x\d+$", ""),
                    Address = appt.appt.Patient.Address,
                    LatestVisit = appt.docAppointments
                    .OrderByDescending(doc => doc.Date)
                    .FirstOrDefault()?.Date != default(DateTime) ? appt.docAppointments
                    .OrderByDescending(doc => doc.Date)
                    .FirstOrDefault()?.Date.ToString("dd-MM-yyyy") : "N/A"
                },
                AppointmentRecord = new AppointmentRecordViewModel
                {
                    // Safely get the first appointment record if available
                    TimeBook = appt.docAppointments.FirstOrDefault()?.TimeBook ?? default(DateTime),
                    Date = appt.docAppointments.FirstOrDefault()?.Date ?? default(DateTime),
                    TimeStart = appt.docAppointments.FirstOrDefault()?.TimeStart ?? default(TimeSpan),
                    TimeEnd = appt.docAppointments.FirstOrDefault()?.TimeEnd ?? default(TimeSpan),
                    Location = appt.docAppointments.FirstOrDefault()?.Location,
                },
                Diagnose = new DiagnoseViewModel
                {
                    DiagnoseDetails = appt.diag.DiagnoseDetails,
                    IsSick = appt.diag.IsSick,
                    PatientStatus = appt.diag.PatientStatus
                }
            })
            .ToList();

        return Ok(new { TodayAppointments = appointments });
    }


    [HttpGet("{id}/appointments/on/{date}")]
    public IActionResult GetAppointmentsByEmployeeIdAndDate(int id, string date)
    {
        string[] formats = { "yyyy-MM-dd", "dd-MM-yyyy" };
        if (!DateTime.TryParseExact(date, formats, null, System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            return BadRequest("Invalid date format. Please use yyyy-MM-dd or dd-MM-yyyy.");
        }

        var appointments = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Include(appt => appt.Doctor)
            .Include(appt => appt.Patient)
            .GroupJoin(_context.DocumentAppointments, appt => appt.Id, doc => doc.AppointmentId, (appt, docAppointments) => new { appt, docAppointments })
            .Join(_context.DocumentDiagnoses, appt => appt.appt.Id, diag => diag.AppointmentId, (appt, diag) => new { appt.appt, appt.docAppointments, diag })
            .AsEnumerable() // Move processing to in-memory for further LINQ transformations
            .Where(appt => appt.docAppointments
                .Any(doc => doc.Date.Date == parsedDate.Date)) // Filter by the given date in DocumentAppointments
            .Select(appt => new
            {
                appt.appt.Id,
                appt.appt.DoctorId,
                Patient = new PatientViewModel
                {
                    Id = appt.appt.Patient.Id,
                    FirstName = appt.appt.Patient.FirstName,
                    LastName = appt.appt.Patient.LastName,
                    Email = appt.appt.Patient.Email,
                    Phone = System.Text.RegularExpressions.Regex.Replace(appt.appt.Patient.Phone ?? "", @"\s*x\d+$", ""),
                    Address = appt.appt.Patient.Address,
                    LatestVisit = appt.docAppointments
                    .OrderByDescending(doc => doc.Date)
                    .FirstOrDefault()?.Date != default(DateTime) ? appt.docAppointments
                    .OrderByDescending(doc => doc.Date)
                    .FirstOrDefault()?.Date.ToString("dd-MM-yyyy") : "N/A"
                },
                AppointmentRecord = new AppointmentRecordViewModel
                {
                    // Safely get the first appointment record if available
                    TimeBook = appt.docAppointments.FirstOrDefault()?.TimeBook ?? default(DateTime),
                    Date = appt.docAppointments.FirstOrDefault()?.Date ?? default(DateTime),
                    TimeStart = appt.docAppointments.FirstOrDefault()?.TimeStart ?? default(TimeSpan),
                    TimeEnd = appt.docAppointments.FirstOrDefault()?.TimeEnd ?? default(TimeSpan),
                    Location = appt.docAppointments.FirstOrDefault()?.Location,
                },
                Diagnose = new DiagnoseViewModel
                {
                    DiagnoseDetails = appt.diag.DiagnoseDetails,
                    IsSick = appt.diag.IsSick,
                    PatientStatus = appt.diag.PatientStatus
                }
            })
            .ToList();

        return Ok(new { AppointmentsOnDate = appointments });
    }


    [HttpGet("{id}/appointments/past")]
    public IActionResult GetPastAppointmentsByEmployeeId(int id)
    {
        var currentDate = DateTime.Now;

        var appointments = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Include(appt => appt.Doctor)
            .Include(appt => appt.Patient)
            .GroupJoin(_context.DocumentAppointments, appt => appt.Id, doc => doc.AppointmentId, (appt, docAppointments) => new { appt, docAppointments })
            .Join(_context.DocumentDiagnoses, appt => appt.appt.Id, diag => diag.AppointmentId, (appt, diag) => new { appt.appt, appt.docAppointments, diag })
            .AsEnumerable()
            .Where(appt => appt.docAppointments
                .Any(doc => doc.Date.Date < currentDate.Date))
            .Select(appt => new
            {
                appt.appt.Id,
                appt.appt.DoctorId,
                Patient = new PatientViewModel
                {
                    Id = appt.appt.Patient.Id,
                    FirstName = appt.appt.Patient.FirstName,
                    LastName = appt.appt.Patient.LastName,
                    Email = appt.appt.Patient.Email,
                    Phone = System.Text.RegularExpressions.Regex.Replace(appt.appt.Patient.Phone ?? "", @"\s*x\d+$", ""),
                    Address = appt.appt.Patient.Address,
                    LatestVisit = appt.docAppointments
                    .OrderByDescending(doc => doc.Date)
                    .FirstOrDefault()?.Date != default(DateTime) ? appt.docAppointments
                    .OrderByDescending(doc => doc.Date)
                    .FirstOrDefault()?.Date.ToString("dd-MM-yyyy") : "N/A"
                },
                AppointmentRecord = new AppointmentRecordViewModel
                {
                    // Safely get the first appointment record if available
                    TimeBook = appt.docAppointments.FirstOrDefault()?.TimeBook ?? default(DateTime),
                    Date = appt.docAppointments.FirstOrDefault()?.Date ?? default(DateTime),
                    TimeStart = appt.docAppointments.FirstOrDefault()?.TimeStart ?? default(TimeSpan),
                    TimeEnd = appt.docAppointments.FirstOrDefault()?.TimeEnd ?? default(TimeSpan),
                    Location = appt.docAppointments.FirstOrDefault()?.Location,
                },
                Diagnose = new DiagnoseViewModel
                {
                    DiagnoseDetails = appt.diag.DiagnoseDetails,
                    IsSick = appt.diag.IsSick,
                    PatientStatus = appt.diag.PatientStatus
                }
            })
            .ToList();

        return Ok(new { PastAppointments = appointments });
    }



    // ------------------- Appointments Counts -------------------

    [HttpGet("{id}/appointments/count")]
    public IActionResult GetTotalAppointmentsByEmployeeId(int id)
    {
        var employee = _context.Employees
            .Include(e => e.Appointments)
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
            .Join(
                _context.Patients,
                appt => appt.PatientId,
                patient => patient.Id,
                (appt, patient) => new { Appointment = appt, Patient = patient }
            )
            .GroupJoin(
                _context.DocumentAppointments,
                apptPatient => apptPatient.Appointment.Id,
                docAppt => docAppt.AppointmentId,
                (apptPatient, docAppointments) => new
                {
                    apptPatient.Patient,
                    LatestVisit = docAppointments
                        .OrderByDescending(doc => doc.Date)
                        .FirstOrDefault()
                }
            )
            .AsEnumerable() // Move processing to in-memory
            .Select(result => new
            {
                Id = result.Patient.Id,
                FirstName = result.Patient.FirstName,
                LastName = result.Patient.LastName,
                Email = result.Patient.Email,
                Phone = System.Text.RegularExpressions.Regex.Replace(result.Patient.Phone, @"\s*x\d+$", ""),
                Address = result.Patient.Address,
                LatestVisit = result.LatestVisit?.Date.ToString("dd-MM-yyyy") ?? "N/A" // Format date as dd-MM-yyyy or use "N/A"
            })
            .Distinct()
            .ToList();

        return Ok(new { EmployeeId = id, Patients = patients });
    }



}

