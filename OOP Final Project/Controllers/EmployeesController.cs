using System;
using Microsoft.AspNetCore.Mvc;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;


namespace OOP_Final_Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{


    // - GET /api/employees: List all employees.
    // - GET /api/employees/{id}: Get a specific employee by ID.
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

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

}
