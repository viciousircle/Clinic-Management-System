using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

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
            Employee = new Employee();
            _client = _clientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5298/"); // Replace with your actual base URL
        }

        public Employee Employee { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                _logger.LogInformation("Fetching employee details from API...");
                var response = await _client.GetAsync("api/employees/6"); // The URI is now relative to the BaseAddress

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"API response: {json}");
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    };
                    Employee = JsonSerializer.Deserialize<Employee>(json, options) ?? new Employee();
                    _logger.LogInformation($"Deserialized employee details.");
                }
                else
                {
                    _logger.LogError($"Failed to fetch employee details. Status code: {response.StatusCode}");
                    Employee = new Employee();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employee details.");
                Employee = new Employee();
            }
        }

        public async Task<IActionResult> OnGetLoadPartialAsync(string section)
        {
            try
            {
                // Fetch the latest Employee data from the API
                var response = await _client.GetAsync("api/employees/6"); // The URI is now relative to the BaseAddress

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Employee = JsonSerializer.Deserialize<Employee>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new Employee();
                }
                else
                {
                    _logger.LogError("Failed to fetch employee data. Status code: {StatusCode}", response.StatusCode);
                    Employee = new Employee(); // Fallback to a new Employee instance
                }

                switch (section)
                {
                    case "Dashboard":
                        return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", Employee);
                    case "Appointment":
                        return Partial("~/Pages/Employees/Doctors/_Appointment.cshtml", Employee);
                    case "Patient":
                        return Partial("~/Pages/Employees/Doctors/_Patient.cshtml", Employee);
                    case "Schedule":
                        return Partial("~/Pages/Employees/Shared/_Schedule.cshtml", Employee);
                    case "Logout":
                        return Partial("~/Pages/Employees/Shared/_Logout.cshtml", Employee);
                    default:
                        return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", Employee);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the partial view.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
