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

public class EmployeesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ! ------------------- Employees -------------------
    // ? [GET] /api/employees : Get all employees
    // ? [GET] /api/employees/{id} : Get employee by id

    [HttpGet]
    public IActionResult GetAll()
    {
        var employees = _context.Employees
        .Where(employee => employee.IsActive) // Optional: filter active employees
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

    // ! ----------------------------------------------------

    // ! ------------------- Appointments -------------------
    // ? [GET] /api/employees/{id}/appointments : Get all appointments by employee id
    // ? [GET] /api/employees/{id}/appointments/today : Get today's appointments by employee id
    // ? [GET] /api/employees/{id}/appointments/on/{date} : Get appointments by employee id and date
    // ? [GET] /api/employees/{id}/appointments/past : Get past appointments by employee id

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


    // ! -----------------------------------------------------------

    // ! ------------------- Appointment --------------------------------
    // ? [GET] /api/employees/{id}/appointments/{appointmentId} : Get appointment by employee id and appointment id

    [HttpGet("{id}/appointments/{appointmentId}")]
    public IActionResult GetAppointmentByEmployeeIdAndAppointmentId(int id, int appointmentId)
    {
        var appointment = _context.Appointments
            .Where(appt => appt.DoctorId == id && appt.Id == appointmentId)
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
                },
                Diagnose = new DiagnoseViewModel
                {
                    DiagnoseDetails = appt.diag.DiagnoseDetails,
                    IsSick = appt.diag.IsSick,
                    PatientStatus = appt.diag.PatientStatus
                }
            })
            .FirstOrDefault();

        if (appointment == null)
        {
            return NotFound(new { Message = "Appointment not found for the specified employee and appointment ID." });
        }

        return Ok(new { Appointment = appointment });
    }




    // ! ------------------- Appointments Counts -------------------
    // ? [GET] /api/employees/{id}/appointments/count : Get total appointments by employee id
    // ? [GET] /api/employees/{id}/appointments/future/count : Get total future appointments by employee id
    // ? [GET] /api/employees/{id}/appointments/completed/count : Get total completed appointments by employee id
    // ? [GET] /api/employees/{id}/appointments/cancelled/count : Get total cancelled appointments by employee id

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

    // ! -----------------------------------------------------------

    // ! ------------------- Patients ------------------------------
    // ? [GET] /api/employees/{id}/patients : Get all patients by employee id
    // ? [GET] /api/employees/{id}/patients/observed : Get all observed patients by employee id

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

        return Ok(new { EmployeeId = id, Patients = patients });
    }

    [HttpGet("{id}/patients/observed")]
    public IActionResult GetAllObservedPatientsByEmployeeId(int id)
    {
        var observedPatients = _context.Appointments
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
            .Join(
                _context.DocumentDiagnoses,
                result => result.LatestVisit.AppointmentId,
                diag => diag.AppointmentId,
                (result, diag) => new
                {
                    result.Patient,
                    result.LatestVisit,
                    diag
                }
            )
            .AsEnumerable()
            .Where(result => result.LatestVisit != null && result.diag != null && result.diag.IsSick)
            .Select(result => new PatientViewModel
            {
                Id = result.Patient.Id,
                FirstName = result.Patient.FirstName,
                LastName = result.Patient.LastName,
                Email = result.Patient.Email,
                Phone = System.Text.RegularExpressions.Regex.Replace(result.Patient.Phone ?? "", @"\s*x\d+$", ""),
                Address = result.Patient.Address,
                LatestVisit = result.LatestVisit.Date.ToString("dd-MM-yyyy")
            })
            .Distinct()
            .ToList();

        return Ok(new { EmployeeId = id, ObservedPatients = observedPatients });
    }

    // ! -----------------------------------------------------------

    // ! ------------------- Patients Counts ------------------------
    // ? [GET] /api/employees/{id}/patients/count : Get total patients by employee id
    // ? [GET] /api/employees/{id}/patients/observed/count : Get total observed patients by employee id

    [HttpGet("{id}/patients/count")]
    public IActionResult GetTotalPatientsByEmployeeId(int id)
    {
        var totalPatients = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Select(appt => appt.PatientId)
            .Distinct()
            .Count();

        var response = new { EmployeeId = id, TotalPatients = totalPatients };

        return Ok(response);
    }

    [HttpGet("{id}/patients/observed/count")]
    public IActionResult GetTotalObservedPatientsByEmployeeId(int id)
    {
        var totalObservedPatients = _context.Appointments
            .Where(appt => appt.DoctorId == id)
            .Join(
                _context.DocumentAppointments,
                appt => appt.Id,
                docAppt => docAppt.AppointmentId,
                (appt, docAppt) => new { appt, docAppt }
            )
            .Join(
                _context.DocumentDiagnoses,
                joined => joined.appt.Id,
                diag => diag.AppointmentId,
                (joined, diag) => new { joined.appt, joined.docAppt, diag }
            )
            .AsEnumerable() // Switch to client-side evaluation
            .GroupBy(joined => joined.appt.PatientId)
            .Select(group => new
            {
                PatientId = group.Key,
                LatestAppointment = group.OrderByDescending(g => g.docAppt.Date).FirstOrDefault()
            })
            .Where(result => result.LatestAppointment != null && result.LatestAppointment.diag.IsSick)
            .Select(result => result.PatientId)
            .Distinct()
            .Count();

        var response = new { EmployeeId = id, TotalObservedPatients = totalObservedPatients };

        return Ok(response);
    }

    // ! -----------------------------------------------------------

    // ! --- Medicines ---------------------------------------------

    // ? [GET] /api/employees/{id}/medicines : Get all medicines by employee id


    // ! -----------------------------------------------------------

    // ! --- Prescriptions -----------------------------------------

    [HttpGet("pharmacist/{id}/prescriptions")]
    public IActionResult GetAllPrescriptionsByPharmacistId(int id)
    {
        var prescriptions = _context.DocumentPrescribes
            .Where(prescription => prescription.PharmacistId == id)
            .Join(_context.Prescriptions, joined => joined.PrescriptionId, prescription => prescription.Id, (joined, prescription) => new { joined, prescription })
            .Join(_context.Appointments, joined => joined.prescription.AppointmentId, appointment => appointment.Id, (joined, appointment) => new { joined.prescription, joined.joined, appointment })
            .Select(joined => new PrescriptionViewModel
            {
                Id = joined.prescription.Id,
                AppointmentId = joined.prescription.AppointmentId,
                PharmacistId = joined.joined.Pharmacist.Id,
                PatientName = joined.appointment.Patient.FirstName + " " + joined.appointment.Patient.LastName,
                DoctorName = joined.appointment.Doctor.FirstName + " " + joined.appointment.Doctor.LastName,
                AppointmentTime = joined.appointment.DocumentAppointment.Date.ToString("dd-MM-yyyy") + " " + joined.appointment.DocumentAppointment.TimeStart.ToString(@"hh\:mm") + "-" + joined.appointment.DocumentAppointment.TimeEnd.ToString(@"hh\:mm"),
                Medicines = _context.PrescriptionMedicines
                    .Where(prescribeMedicine => prescribeMedicine.PrescriptionId == joined.prescription.Id)
                    .Join(_context.Medicines, joined => joined.MedicineId, medicine => medicine.Id, (joined, medicine) => new MedicinePrescriptionViewModel
                    {
                        MedicineId = joined.MedicineId,
                        MedicineName = medicine.Name,
                        DosageAmount = joined.DosageAmount,
                        Frequency = joined.Frequency,
                        FrequencyUnit = joined.FrequencyUnit,
                        Route = joined.Route,
                        Instruction = joined.Instructions
                    })
                    .ToList(),
                PrescriptionStatus = joined.prescription.PrescriptionStatus
            })
            .ToList();

        return Ok(new { Prescriptions = prescriptions });

    }


    [HttpGet("pharmacist/{id}/prescriptions/on/{date}")]
    public IActionResult GetPrescriptionsByPharmacistIdAndDate(int id, string date)
    {
        string[] formats = { "yyyy-MM-dd", "dd-MM-yyyy" };
        if (!DateTime.TryParseExact(date, formats, null, System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            return BadRequest("Invalid date format. Please use yyyy-MM-dd or dd-MM-yyyy.");
        }

        var prescriptions = _context.DocumentPrescribes
            .Where(prescription => prescription.PharmacistId == id)
            .Join(_context.Prescriptions, joined => joined.PrescriptionId, prescription => prescription.Id, (joined, prescription) => new { joined, prescription })
            .Join(_context.Appointments, joined => joined.prescription.AppointmentId, appointment => appointment.Id, (joined, appointment) => new { joined.prescription, joined.joined, appointment })
            .Where(joined => joined.appointment.DocumentAppointment.Date.Date == parsedDate.Date)
            .Select(joined => new PrescriptionViewModel
            {
                Id = joined.prescription.Id,
                AppointmentId = joined.prescription.AppointmentId,
                PharmacistId = joined.joined.Pharmacist.Id,
                PatientName = joined.appointment.Patient.FirstName + " " + joined.appointment.Patient.LastName,
                DoctorName = joined.appointment.Doctor.FirstName + " " + joined.appointment.Doctor.LastName,
                AppointmentTime = joined.appointment.DocumentAppointment.Date.ToString("dd-MM-yyyy") + " " + joined.appointment.DocumentAppointment.TimeStart.ToString(@"hh\:mm") + "-" + joined.appointment.DocumentAppointment.TimeEnd.ToString(@"hh\:mm"),
                Medicines = _context.PrescriptionMedicines
                    .Where(prescribeMedicine => prescribeMedicine.PrescriptionId == joined.prescription.Id)
                    .Join(_context.Medicines, joined => joined.MedicineId, medicine => medicine.Id, (joined, medicine) => new MedicinePrescriptionViewModel
                    {
                        MedicineId = joined.MedicineId,
                        MedicineName = medicine.Name,
                        DosageAmount = joined.DosageAmount,
                        Frequency = joined.Frequency,
                        FrequencyUnit = joined.FrequencyUnit,
                        Route = joined.Route,
                        Instruction = joined.Instructions
                    })
                    .ToList(),
                PrescriptionStatus = joined.prescription.PrescriptionStatus
            })
            .ToList();

        return Ok(new { PrescriptionsOnDate = prescriptions });
    }

}

