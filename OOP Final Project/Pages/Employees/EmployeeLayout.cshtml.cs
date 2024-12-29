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
using OOP_Final_Project.ViewModels.Shared;
using System.Collections.Generic;
using OOP_Final_Project.Controllers.ApiResponses;


namespace OOP_Final_Project.Pages.Employees
{
    public class EmployeeLayoutModel : PageModel
    {

        // -- Fields ------------------------------------
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<EmployeeLayoutModel> _logger;
        private readonly HttpClient _client;

        // -- Constructor --------------------------------
        public EmployeeLayoutModel(IHttpClientFactory clientFactory, ILogger<EmployeeLayoutModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _client = _clientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5298/");

            // -- Initialize ViewModel ---------------------
            DoctorData = new DoctorViewModel();
            Employee = new EmployeeViewModel();
        }

        // -- Properties --------------------------------
        public DoctorViewModel DoctorData { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();


        // -- Methods -----------------------------------
        public async Task OnGetAsync()
        {
            await FetchAllDataAsync();

            // Load appointments for "today" by default
            await LoadAppointmentsAsync("today");
            await LoadPatientCardsAsync("observedPatients");
        }

        // -- Helper Methods -----------------------------
        private async Task FetchAllDataAsync()
        {
            await FetchEmployeeDetailsAsync();
            await FetchAppointmentCountsAsync();
            await FetchAllPatientsAsync();
            await FetchEmployeeDetailsAsync();
            await FetchScheduleAsync();
            await FetchPatientCountAsync();
            await FetchAllPatientsAsync();
            await FetchScheduleAsync();
            await FetchEmployeeDetailsAsync();

            await LoadAppointmentsAsync("today");


        }


        // -- Get Partial View ---------------------------
        public async Task<IActionResult> OnGetLoadPartialAsync(string section, string view)
        {
            switch (section)
            {
                case "AppointmentTableRows":
                    await LoadAppointmentsAsync(view);
                    return Partial("~/Pages/Employees/Doctors/_AppointmentTableRows.cshtml", DoctorData);

                case "PatientCards":
                    await LoadPatientCardsAsync(view);
                    return Partial("~/Pages/Employees/Doctors/_PatientCards.cshtml", DoctorData);

                case "Dashboard":
                    await FetchAppointmentCountsAsync();
                    await FetchAllPatientsAsync();
                    await FetchEmployeeDetailsAsync();
                    await FetchScheduleAsync();
                    await FetchPatientCountAsync();
                    await FetchAllPatientsAsync();
                    await FetchScheduleAsync();
                    await FetchEmployeeDetailsAsync();
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);

                case "Appointment":
                    await FetchAppointmentCountsAsync();
                    await FetchAllPatientsAsync();
                    await FetchEmployeeDetailsAsync();
                    await FetchScheduleAsync();


                    return Partial("~/Pages/Employees/Doctors/_Appointment.cshtml", DoctorData);

                case "Patient":
                    await FetchPatientCountAsync();
                    await FetchAllPatientsAsync();
                    await FetchScheduleAsync();
                    await FetchEmployeeDetailsAsync();
                    return Partial("~/Pages/Employees/Doctors/_Patient.cshtml", DoctorData);

                case "Schedule":
                    return Partial("~/Pages/Employees/Shared/_Schedule.cshtml", DoctorData);

                case "Logout":
                default:
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
            }
        }

        private async Task LoadAppointmentsAsync(string view)
        {
            switch (view)
            {
                case "today":
                    await FetchAppointmentsTodayAsync();
                    break;
                case "all":
                    await FetchAppointmentsAsync();
                    break;
                case "previous":
                    await FetchPastAppointmentsAsync();
                    break;
                default:
                    throw new ArgumentException("Invalid view parameter");
            }
        }

        private async Task LoadPatientCardsAsync(string view)
        {
            switch (view)
            {
                case "observedPatients":
                    await FetchObservedPatientsAsync();
                    break;

                case "allPatients":
                    await FetchAllPatientsAsync();
                    break;
                default:
                    throw new ArgumentException("Invalid view parameter");
            }
        }




        // ! ------------------------------------------------------------------------------------------------


        // --- Fetch All Employees -----------------------
        // -- [GET] api/employees -------------------------

        private async Task FetchAllEmployeesAsync()
        {
            try
            {
                // _logger.LogInformation("Fetching all employee details from API..."); //For logging purposes
                var response = await _client.GetAsync("api/employees");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    var responseWrapper = JsonSerializer.Deserialize<EmployeesResponse>(json, options);

                    if (responseWrapper?.Employees != null)
                    {
                        _logger.LogInformation($"Successfully fetched {responseWrapper.Employees.Count} employees.");
                        DoctorData.DoctorList = responseWrapper.Employees;
                    }
                    else
                    {
                        _logger.LogError("Failed to deserialize employee data.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch employees. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all employee details.");
            }
        }


        // ! ------------------------------------------------------------------------------------------------

        // --- Fetch Employee Details By ID ---------------------
        // -- [GET] api/employees/6 -----------------------
        private async Task FetchEmployeeDetailsAsync()
        {
            try
            {
                // _logger.LogInformation("Fetching employee details from API..."); //For logging purposes
                var response = await _client.GetAsync("api/employees/6");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var employeeResponse = JsonSerializer.Deserialize<EmployeeResponse>(json, options);

                    if (employeeResponse?.Employee != null)
                    {
                        DoctorData.Doctor = employeeResponse.Employee;
                        _logger.LogInformation("Employee details fetched successfully.");
                        _logger.LogInformation($"Employee: {DoctorData.Doctor.FirstName} {DoctorData.Doctor.LastName}");
                    }
                    else
                    {
                        _logger.LogError("Failed to deserialize employee data.");
                        DoctorData.Doctor = new EmployeeViewModel();
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch employee details. Status code: {response.StatusCode}");
                    DoctorData.Doctor = new EmployeeViewModel();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employee details.");
                DoctorData.Doctor = new EmployeeViewModel();

            }

        }


        // ! ------------------------------------------------------------------------------------------------

        // --- Fetch Appointment Counts -------------------
        // -- [GET] api/employees/6/appointments/count ---
        // -- [GET] api/employees/6/appointments/future/count ---
        // -- [GET] api/employees/6/appointments/completed/count ---
        // -- [GET] api/employees/6/appointments/cancelled/count ---
        private async Task FetchAppointmentCountsAsync()
        {
            DoctorData.AppointmentCount = await FetchAppointmentCountAsync("api/employees/6/appointments/count", "totalAppointments");
            DoctorData.FutureAppointmentCount = await FetchAppointmentCountAsync("api/employees/6/appointments/future/count", "totalFutureAppointments");
            DoctorData.CompletedAppointmentCount = await FetchAppointmentCountAsync("api/employees/6/appointments/completed/count", "totalCompletedAppointments");
            DoctorData.CancelledAppointmentCount = await FetchAppointmentCountAsync("api/employees/6/appointments/cancelled/count", "totalCancelledAppointments");
        }

        private async Task<int> FetchAppointmentCountAsync(string url, string jsonProperty)
        {
            try
            {
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(json))
                    {
                        var jsonData = JsonSerializer.Deserialize<JsonElement>(json);
                        if (jsonData.TryGetProperty(jsonProperty, out var propertyValue))
                        {
                            return propertyValue.GetInt32();
                        }
                        else
                        {
                            _logger.LogError($"{jsonProperty} not found in response. Response: {json}");
                        }
                    }
                    else
                    {
                        _logger.LogError($"Response for {url} is empty.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch appointment count from {url}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching appointment count from {url}");
            }

            return 0;
        }


        // ! ------------------------------------------------------------------------------------------------

        // --- Fetch Appointments -------------------------
        // -- [GET] api/employees/6/appointments ---------
        // -- [GET] api/employees/6/appointments/today ---
        // -- [GET] api/employees/6/appointments/{date} --
        // -- [GET] api/employees/6/appointments/past ----
        // This method will handle the common logic for fetching appointments
        private async Task FetchAppointmentsAsync(string url, string dateDescription = null)
        {
            try
            {

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var appointmentsResponse = JsonSerializer.Deserialize<AppointmentsResponse>(json, options);

                    if (appointmentsResponse != null)
                    {

                        // Check each possible appointment list and assign it to DoctorData.Appointments
                        if (appointmentsResponse.Appointments != null && appointmentsResponse.Appointments.Count > 0)
                        {
                            DoctorData.Appointments = appointmentsResponse.Appointments;
                            _logger.LogInformation($"Successfully fetched {DoctorData.Appointments.Count} appointments.");
                        }
                        else if (appointmentsResponse.TodayAppointments != null && appointmentsResponse.TodayAppointments.Count > 0)
                        {
                            DoctorData.Appointments = appointmentsResponse.TodayAppointments;
                            _logger.LogInformation($"Successfully fetched {DoctorData.Appointments.Count} appointments today.");
                        }
                        else if (appointmentsResponse.PastAppointments != null && appointmentsResponse.PastAppointments.Count > 0)
                        {
                            DoctorData.Appointments = appointmentsResponse.PastAppointments;
                            _logger.LogInformation($"Successfully fetched {DoctorData.Appointments.Count} past appointments.");
                        }
                        else if (appointmentsResponse.AppointmentsOnDate != null && appointmentsResponse.AppointmentsOnDate.Count > 0)
                        {
                            DoctorData.Appointments = appointmentsResponse.AppointmentsOnDate;
                            if (string.IsNullOrEmpty(dateDescription))
                            {
                                dateDescription = "for the specified date";
                            }
                            _logger.LogInformation($"Successfully fetched {DoctorData.Appointments.Count} appointments {dateDescription}.");
                        }
                        else
                        {
                            _logger.LogWarning("No appointments found for the specified date or criteria.");
                            DoctorData.Appointments = new List<AppointmentViewModel>();
                        }
                    }
                    else
                    {
                        _logger.LogError("Failed to deserialize appointment data.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch appointments. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching appointments.");
            }
        }


        // Method to fetch all appointments
        private Task FetchAppointmentsAsync()
        {
            return FetchAppointmentsAsync("api/employees/6/appointments");
        }

        // Method to fetch today's appointments
        private Task FetchAppointmentsTodayAsync()
        {
            return FetchAppointmentsAsync("api/employees/6/appointments/today", "today");

        }

        // Method to fetch appointments for a specific date
        private Task FetchAppointmentsByDateAsync(DateTime date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            return FetchAppointmentsAsync($"api/employees/6/appointments/on/{formattedDate}", $"Successfully fetched appointments for the specified date: {formattedDate}");
        }

        // Method to fetch past appointments
        private Task FetchPastAppointmentsAsync()
        {
            return FetchAppointmentsAsync("api/employees/6/appointments/past", "in the past");

        }

        // ! ------------------------------------------------------------------------------------------------

        // --- Fetch Patients -----------------------------
        // -- [GET] api/employees/6/patients -------------
        // -- [GET] api/employees/6/patients/observed ----
        // -- [GET] api/employees/6/patients/{status} ----

        private async Task FetchPatientsAsync(string url, string description = null)
        {
            try
            {
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var patientsResponse = JsonSerializer.Deserialize<PatientsResponse>(json, options);

                    if (patientsResponse != null)
                    {
                        if (patientsResponse.Patients != null && patientsResponse.Patients.Count > 0)
                        {
                            DoctorData.Patients = patientsResponse.Patients;
                            _logger.LogInformation($"Successfully fetched {DoctorData.Patients.Count} patients.");
                        }
                        else if (patientsResponse.ObservedPatients != null && patientsResponse.ObservedPatients.Count > 0)
                        {
                            DoctorData.Patients = patientsResponse.ObservedPatients;
                            _logger.LogInformation($"Successfully fetched {DoctorData.Patients.Count} observed patients.");
                        }
                        else
                        {
                            _logger.LogError("Failed to deserialize patient data.");
                        }
                    }
                    else
                    {
                        _logger.LogError("Failed to deserialize patient data.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch patients. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching patients {description ?? ""}.");
            }
        }

        // Fetch all patients
        private Task FetchAllPatientsAsync()
        {
            return FetchPatientsAsync("api/employees/6/patients", "all patients");
        }

        // Fetch observed patients
        private Task FetchObservedPatientsAsync()
        {
            return FetchPatientsAsync("api/employees/6/patients/observed", "observed patients");
        }



        // ! ------------------------------------------------------------------------------------------------

        // --- Fetch Appointment Counts -------------------
        // -- [GET] api/employees/6/appointments/count ---

        private async Task FetchPatientCountAsync()
        {
            DoctorData.PatientCount = await FetchPatientCountAsync("api/employees/6/patients/count", "totalPatients");
            DoctorData.ObservedPatientCount = await FetchPatientCountAsync("api/employees/6/patients/observed/count", "totalObservedPatients");
        }

        private async Task<int> FetchPatientCountAsync(string url, string jsonProperty)
        {
            try
            {
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(json))
                    {
                        var jsonData = JsonSerializer.Deserialize<JsonElement>(json);
                        if (jsonData.TryGetProperty(jsonProperty, out var propertyValue))
                        {
                            _logger.LogInformation($"Fetched {jsonProperty}: {propertyValue.GetInt32()}");
                            return propertyValue.GetInt32();
                        }
                        else
                        {
                            _logger.LogError($"{jsonProperty} not found in response. Response: {json}");
                        }
                    }
                    else
                    {
                        _logger.LogError($"Response for {url} is empty.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch appointment count from {url}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching appointment count from {url}");
            }

            return 0;
        }

        // ! ------------------------------------------------------------------------------------------------

        // --- Fetch Schedule -----------------------------
        // -- [GET] api/employees/6/schedule --------------

        private async Task FetchScheduleAsync()
        {
            try
            {
                var response = await _client.GetAsync("api/employees/6/schedule");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var scheduleResponse = JsonSerializer.Deserialize<ScheduleResponse>(json, options);

                    if (scheduleResponse?.Schedule != null)
                    {
                        DoctorData.Schedule = scheduleResponse.Schedule.FirstOrDefault();
                        _logger.LogInformation("Successfully fetched schedule.");
                        _logger.LogInformation($"Schedule: {DoctorData.Schedule.Sections.Count} sections");
                    }
                    else
                    {
                        _logger.LogError("Failed to deserialize schedule data.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch schedule. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching schedule.");
            }
        }


    }
}