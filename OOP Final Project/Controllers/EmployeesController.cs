using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;


namespace OOP_Final_Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{


    // - GET /api/employees: List all employees.
    // - GET /api/employees/{id}: Get a specific employee by ID.
    // - GET /api/employees/byFirstName/{firstName}: Get an employee by first name.
    // - GET /api/employees/{id}/appointments/count: Get the total number of appointments for an employee.
    // - GET /api/employees/{id}/appointments/future/count: Get the total number of future appointments for an employee in the next 30 days.
    // - GET /api/employees/{id}/appointments/completed/count: Get the total number of completed appointments for an employee.

    // - POST /api/employees: Add a new employee.

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



    // TODO: Fix this Total Cancelled Appointments method
    // [HttpGet("{id}/appointments/cancelled/count")]
    // public IActionResult GetTotalCancelledAppointmentsByEmployeeId(int id)
    // {
    //     // Retrieve the employee's schedules
    //     var employeeSchedules = _context.EmployeeSchedules
    //         .Include(es => es.Schedule)
    //         .Where(es => es.EmployeeId == id && es.IsActive) // Ensure the schedule is active
    //         .ToList();

    //     if (employeeSchedules == null || !employeeSchedules.Any())
    //         return NotFound();

    //     // Get all appointments related to the employee that have a 'cancelled' status
    //     var cancelledAppointments = employeeSchedules
    //         .SelectMany(es => es.Schedule.Appointments)
    //         .Where(a => a.DocumentTypeId == < cancelled_document_type_id >) // Replace with the actual document type ID for cancelled
    //         .ToList();

    //     var totalCancelledAppointments = cancelledAppointments.Count;

    //     return Ok(new { EmployeeId = id, TotalCancelledAppointments = totalCancelledAppointments });
    // }




    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }




}
