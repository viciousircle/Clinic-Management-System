using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using OOP_Final_Project.ViewModels;
using System.Collections.Generic;
using OOP_Final_Project.Controllers.ApiResponses;


namespace OOP_Final_Project.Pages.Employees
{
    public class EmployeeLayoutModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<EmployeeLayoutModel> _logger;
        private readonly HttpClient _client;

        public EmployeeLayoutModel(IHttpClientFactory clientFactory, ILogger<EmployeeLayoutModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _client = _clientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5298/"); // Replace with your actual base URL
            DoctorData = new DoctorViewModel();
        }

        public DoctorViewModel DoctorData { get; set; }

        public async Task OnGetAsync()
        {
            await FetchEmployeeData();
        }

        public async Task<IActionResult> OnGetLoadPartialAsync(string section)
        {
            await FetchEmployeeData();

            switch (section)
            {
                case "Dashboard":
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
                case "Appointment":
                    return Partial("~/Pages/Employees/Doctors/_Appointment.cshtml", DoctorData);
                case "Patient":
                    return Partial("~/Pages/Employees/Doctors/_Patient.cshtml", DoctorData);
                case "Schedule":
                    return Partial("~/Pages/Employees/Shared/_Schedule.cshtml", DoctorData);
                case "Logout":
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
                default:
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
            }
        }

        private async Task FetchEmployeeData()
        {
            try
            {
                _logger.LogInformation("Fetching employee details from API...");

                // - Fetch employee details
                // - GET /api/employees/96

                var responseEmployee = await _client.GetAsync("api/employees/96");

                if (responseEmployee.IsSuccessStatusCode)
                {
                    var employeeJson = await responseEmployee.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    DoctorData.Employee = JsonSerializer.Deserialize<Employee>(employeeJson, options) ?? new Employee();
                }
                else
                {
                    _logger.LogError($"Failed to fetch employee details. Status code: {responseEmployee.StatusCode}");
                    DoctorData = new DoctorViewModel();
                }

                // - Fetch appointment details
                // - GET /api/employees/96/appointments/count

                var responseAppointments = await _client.GetAsync("api/employees/96/appointments/count");

                if (responseAppointments != null && responseAppointments.IsSuccessStatusCode)
                {
                    var appointmentsJson = await responseAppointments.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(appointmentsJson))
                    {
                        var appointmentsData = JsonSerializer.Deserialize<JsonElement>(appointmentsJson);
                        if (appointmentsData.TryGetProperty("totalAppointments", out JsonElement totalAppointmentsElement))
                        {
                            DoctorData.AppointmentCount = totalAppointmentsElement.GetInt32();
                        }
                        else
                        {
                            _logger.LogError($"totalAppointments property not found in the response. Response: {appointmentsJson}");
                        }
                    }
                    else
                    {
                        _logger.LogError("Appointments JSON is null or empty.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch appointments count. Status code: {responseAppointments?.StatusCode}");
                }


                // - Fetch future, completed, and cancelled appointments count
                // - GET /api/employees/96/appointments/future/count
                // - GET /api/employees/96/appointments/completed/count
                // - GET /api/employees/96/appointments/cancelled/count

                var responseFutureAppointments = await _client.GetAsync("api/employees/96/appointments/future/count");

                if (responseFutureAppointments != null && responseFutureAppointments.IsSuccessStatusCode)
                {
                    var futureAppointmentsJson = await responseFutureAppointments.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(futureAppointmentsJson))
                    {
                        var futureAppointmentsData = JsonSerializer.Deserialize<JsonElement>(futureAppointmentsJson);

                        if (futureAppointmentsData.TryGetProperty("totalFutureAppointments", out JsonElement totalFutureAppointmentsElementy))
                        {
                            DoctorData.FutureAppointmentCount = totalFutureAppointmentsElementy.GetInt32();
                        }
                        else
                        {
                            _logger.LogError($"totalFutureAppointments property not found in the response. Response: {futureAppointmentsJson}");
                        }
                    }
                    else
                    {
                        _logger.LogError("Future Appointments JSON is null or empty.");
                    }
                }

                var responseCompletedAppointments = await _client.GetAsync("api/employees/96/appointments/completed/count");

                if (responseCompletedAppointments != null && responseCompletedAppointments.IsSuccessStatusCode)
                {
                    var completedAppointmentsJson = await responseCompletedAppointments.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(completedAppointmentsJson))
                    {
                        var completedAppointmentsData = JsonSerializer.Deserialize<JsonElement>(completedAppointmentsJson);

                        if (completedAppointmentsData.TryGetProperty("totalCompletedAppointments", out JsonElement totalCompletedAppointmentsElement))
                        {
                            DoctorData.CompletedAppointmentCount = totalCompletedAppointmentsElement.GetInt32();
                        }
                        else
                        {
                            _logger.LogError($"totalCompletedAppointments property not found in the response. Response: {completedAppointmentsJson}");
                        }
                    }
                    else
                    {
                        _logger.LogError("Completed Appointments JSON is null or empty.");
                    }
                }

                var responseCancelledAppointments = await _client.GetAsync("api/employees/96/appointments/cancelled/count");

                if (responseCancelledAppointments != null && responseCancelledAppointments.IsSuccessStatusCode)
                {
                    var cancelledAppointmentsJson = await responseCancelledAppointments.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(cancelledAppointmentsJson))
                    {
                        var cancelledAppointmentsData = JsonSerializer.Deserialize<JsonElement>(cancelledAppointmentsJson);

                        if (cancelledAppointmentsData.TryGetProperty("totalCancelledAppointments", out JsonElement totalCancelledAppointmentsElement))
                        {
                            DoctorData.CancelledAppointmentCount = totalCancelledAppointmentsElement.GetInt32();
                        }
                        else
                        {
                            _logger.LogError($"totalCancelledAppointments property not found in the response. Response: {cancelledAppointmentsJson}");
                        }
                    }
                    else
                    {
                        _logger.LogError("Cancelled Appointments JSON is null or empty.");
                    }
                }


                // - Fetch patients
                // - GET /api/employees/96/patients

                var responsePatients = await _client.GetAsync("api/employees/96/patients");

                if (responsePatients != null && responsePatients.IsSuccessStatusCode)
                {
                    var patientsJson = await responsePatients.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Patients JSON: {patientsJson}"); // Log the JSON response
                    if (!string.IsNullOrEmpty(patientsJson))
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var patientResponse = JsonSerializer.Deserialize<PatientResponse>(patientsJson, options);

                        if (patientResponse != null && patientResponse.Patients != null)
                        {
                            DoctorData.Patients = patientResponse.Patients;
                        }
                        else
                        {
                            _logger.LogError("Failed to deserialize patients JSON into PatientResponse.");
                        }
                    }
                    else
                    {
                        _logger.LogError("Patients JSON is null or empty.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch patients. Status code: {responsePatients?.StatusCode}");
                }

                // - Fetch appointments
                // - GET /api/employees/96/appointments

                var responseAppointmentsList = await _client.GetAsync("api/employees/96/appointments"); // Replace with actual employee ID
                if (responseAppointmentsList.IsSuccessStatusCode)
                {
                    var appointmentsJson = await responseAppointmentsList.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Appointments JSON: {appointmentsJson}"); // Log the JSON response
                    if (!string.IsNullOrEmpty(appointmentsJson))
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        try
                        {
                            var appointmentResponse = JsonSerializer.Deserialize<AppointmentResponse>(appointmentsJson, options);
                            if (appointmentResponse != null && appointmentResponse.Appointments != null)
                            {
                                DoctorData.Appointments = appointmentResponse.Appointments.Select(a => new AppointmentViewModel
                                {
                                    Id = a.Id,
                                    DoctorId = a.DoctorId,
                                    Patient = new PatientViewModel
                                    {
                                        Id = a.Patient.Id,
                                        FirstName = a.Patient.FirstName,
                                        LastName = a.Patient.LastName,
                                        IsSick = a.DocumentDiagnose.IsSick,
                                        PatientStatus = a.DocumentDiagnose.PatientStatus
                                    },
                                    AppointmentRecord = new AppointmentRecordViewModel
                                    {
                                        TimeBook = a.DocumentAppointment.TimeBook,
                                        Date = a.DocumentAppointment.Date,
                                        TimeStart = a.DocumentAppointment.TimeStart,
                                        TimeEnd = a.DocumentAppointment.TimeEnd,
                                        Location = a.DocumentAppointment.Location
                                    },
                                    Diagnose = new DiagnoseViewModel
                                    {
                                        DiagnoseDetails = a.DocumentDiagnose.DiagnoseDetails
                                    }
                                    // Map other properties as needed
                                }).ToList();
                            }
                            else
                            {
                                _logger.LogError("Failed to deserialize appointments JSON into AppointmentResponse.");
                            }
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError($"JSON deserialization error: {ex.Message}");
                        }
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch appointments. Status code: {responseAppointmentsList.StatusCode}");
                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employee details.");
                DoctorData = new DoctorViewModel();
            }
        }
    }
}