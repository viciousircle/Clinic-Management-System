using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.Pages
{
    public class TestModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<TestModel> _logger;

        public TestModel(IHttpClientFactory clientFactory, ILogger<TestModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public List<Employee> Employees { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient();
            try
            {
                _logger.LogInformation("Fetching employees from API...");
                var response = await client.GetAsync("http://localhost:5298/api/employees");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"API response: {json}");
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        // Ignore missing properties
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    };
                    Employees = JsonSerializer.Deserialize<List<Employee>>(json, options) ?? new List<Employee>();
                    _logger.LogInformation($"Deserialized {Employees.Count} employees.");
                }
                else
                {
                    _logger.LogError($"Failed to fetch employees. Status code: {response.StatusCode}");
                    Employees = new List<Employee>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employees.");
                Employees = new List<Employee>();
            }
        }

    }
}