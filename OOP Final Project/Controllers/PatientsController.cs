using System;
using Microsoft.AspNetCore.Mvc;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;
using OOP_Final_Project.ViewModels.Shared;


namespace OOP_Final_Project.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PatientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ! ------------------ Patients ------------------
    // ? [GET] /api/patients : Get all patients
    // ? [GET] /api/patients/{id} : Get patient by id

    [HttpGet]
    public IActionResult GetAll()
    {
        var patients = _context.Appointments
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
            .AsEnumerable()
            .Select(result => new PatientViewModel
            {
                Id = result.Patient.Id,
                FirstName = result.Patient.FirstName,
                LastName = result.Patient.LastName,
                Email = result.Patient.Email,
                Phone = System.Text.RegularExpressions.Regex.Replace(result.Patient.Phone, @"\s*x\d+$", ""),
                Address = result.Patient.Address,
                LatestVisit = result.LatestVisit?.Date.ToString("dd-MM-yyyy") ?? "N/A"
            })
            .Distinct()
            .ToList();

        var response = new
        {
            Patients = patients
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var patient = _context.Patients.Find(id);
        if (patient == null)
            return NotFound();

        return Ok(patient);
    }

    // [HttpPost]
    // public IActionResult Create(Patient patient)
    // {
    //     _context.Patients.Add(patient);
    //     _context.SaveChanges();
    //     return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    // }


}
