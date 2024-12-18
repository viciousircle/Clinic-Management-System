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
                    // Handle logout
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
                // break;
                default:
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
            }

            return Page();
        }

        private async Task FetchEmployeeData()
        {
            try
            {
                _logger.LogInformation("Fetching employee details from API...");

                //! Fetch employee details
                //! [GET] /api/employees/6

                var responseEmployee = await _client.GetAsync("api/employees/6");

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

                //! Fetch total appointments for the employee
                //! [GET] /api/employees/6/appointments/count

                var responseAppointments = await _client.GetAsync("api/employees/6/appointments/count");

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

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employee details.");
                DoctorData = new DoctorViewModel();
            }
        }
    }
}