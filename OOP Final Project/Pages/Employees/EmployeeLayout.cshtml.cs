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

        public EmployeeLayoutModel(IHttpClientFactory clientFactory, ILogger<EmployeeLayoutModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            Employee = new Employee();
        }


        public Employee Employee { get; set; }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient();

            try
            {
                _logger.LogInformation("Fetching employee details from API...");
                var response = await client.GetAsync("http://localhost:5298/api/employees/6"); // Replace '1' with the actual employee ID

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



        // Dynamic handler to load partial views based on the "section" parameter
        public IActionResult OnGetLoadPartial(string section)
        {
            switch (section)
            {
                case "Dashboard":
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml"); // Ensure the correct path
                case "Appointment":
                    return Partial("~/Pages/Employees/Doctors/_Appointment.cshtml"); // Ensure the correct path
                case "Patient":
                    return Partial("~/Pages/Employees/Doctors/_Patient.cshtml"); // Ensure the correct path
                case "Schedule":
                    return Partial("~/Pages/Employees/Shared/_Schedule.cshtml"); // Ensure the correct path
                case "Logout":
                    return Partial("~/Pages/Employees/Shared/_Logout.cshtml"); // Ensure the correct path
                default:
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml"); // Ensure the correct path
            }
        }
    }
}
