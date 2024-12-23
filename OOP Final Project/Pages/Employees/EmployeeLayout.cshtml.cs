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
        }

        // -- Helper Methods -----------------------------
        private async Task FetchAllDataAsync()
        {
            await FetchEmployeeDetailsAsync();
        }


        // -- Get Partial View ---------------------------
        public async Task<IActionResult> OnGetLoadPartialAsync(string section, string view)
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
            }

            if (section == "AppointmentTableRows")
            {
                return Partial("~/Pages/Employees/Doctors/_AppointmentTableRows.cshtml", DoctorData);
            }

            switch (section)
            {
                case "Dashboard":
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
                case "Appointment":

                    await FetchAppointmentCountsAsync();

                    return Partial("~/Pages/Employees/Doctors/_Appointment.cshtml", DoctorData);

                case "Patient":
                    await FetchPatientsAsync();


                    return Partial("~/Pages/Employees/Doctors/_Patient.cshtml", DoctorData);
                case "Schedule":
                    return Partial("~/Pages/Employees/Shared/_Schedule.cshtml", DoctorData);
                case "Logout":
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);


                default:
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
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
        // -- [GET] api/employees/96 -----------------------
        private async Task FetchEmployeeDetailsAsync()
        {
            try
            {
                // _logger.LogInformation("Fetching employee details from API..."); //For logging purposes
                var response = await _client.GetAsync("api/employees/96");

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
        // -- [GET] api/employees/96/appointments/count ---
        // -- [GET] api/employees/96/appointments/future/count ---
        // -- [GET] api/employees/96/appointments/completed/count ---
        // -- [GET] api/employees/96/appointments/cancelled/count ---
        private async Task FetchAppointmentCountsAsync()
        {
            DoctorData.AppointmentCount = await FetchAppointmentCountAsync("api/employees/96/appointments/count", "totalAppointments");
            DoctorData.FutureAppointmentCount = await FetchAppointmentCountAsync("api/employees/96/appointments/future/count", "totalFutureAppointments");
            DoctorData.CompletedAppointmentCount = await FetchAppointmentCountAsync("api/employees/96/appointments/completed/count", "totalCompletedAppointments");
            DoctorData.CancelledAppointmentCount = await FetchAppointmentCountAsync("api/employees/96/appointments/cancelled/count", "totalCancelledAppointments");
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
        // -- [GET] api/employees/96/appointments ---------
        // -- [GET] api/employees/96/appointments/today ---
        // -- [GET] api/employees/96/appointments/{date} --
        // -- [GET] api/employees/96/appointments/past ----
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
            return FetchAppointmentsAsync("api/employees/96/appointments");
        }

        // Method to fetch today's appointments
        private Task FetchAppointmentsTodayAsync()
        {
            return FetchAppointmentsAsync("api/employees/96/appointments/today", "today");

        }

        // Method to fetch appointments for a specific date
        private Task FetchAppointmentsByDateAsync(DateTime date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            return FetchAppointmentsAsync($"api/employees/96/appointments/on/{formattedDate}", $"Successfully fetched appointments for the specified date: {formattedDate}");
        }

        // Method to fetch past appointments
        private Task FetchPastAppointmentsAsync()
        {
            return FetchAppointmentsAsync("api/employees/96/appointments/past", "in the past");

        }

        // ! ------------------------------------------------------------------------------------------------

        // --- Fetch Patients -----------------------------
        // -- [GET] api/employees/96/patients -------------
        private async Task FetchPatientsAsync()
        {
            try
            {
                var response = await _client.GetAsync("api/employees/96/patients");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var patientsResponse = JsonSerializer.Deserialize<PatientsResponse>(json, options);

                    if (patientsResponse?.Patients != null)
                    {
                        // Manually parse the LatestVisit field to DateTime if needed
                        foreach (var patient in patientsResponse.Patients)
                        {
                            if (DateTime.TryParseExact(patient.LatestVisit, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                            {
                                // _logger.LogInformation($"Parsed LatestVisit: {parsedDate:yyyy-MM-dd}"); //For logging purposes
                            }
                            else
                            {
                                _logger.LogWarning($"Failed to parse LatestVisit for patient ID {patient.Id}: {patient.LatestVisit}");
                            }
                        }

                        DoctorData.Patients = patientsResponse.Patients;
                        _logger.LogInformation($"Successfully fetched {DoctorData.Patients.Count} patients.");
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
                _logger.LogError(ex, "An error occurred while fetching patients.");
            }
        }





    }
}