using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.Pages;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IHttpClientFactory clientFactory, ILogger<IndexModel> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public List<Patient> Patients { get; set; } = new();
    public Patient? PatientWithId1 { get; set; }

    public async Task OnGetAsync()
    {
        var client = _clientFactory.CreateClient();

        try
        {
            _logger.LogInformation("Fetching patients from API...");
            var response = await client.GetAsync("http://localhost:5298/api/patients");

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
                Patients = JsonSerializer.Deserialize<List<Patient>>(json, options) ?? new List<Patient>();
                _logger.LogInformation($"Deserialized {Patients.Count} patients.");

                // Find the patient with ID 1
                PatientWithId1 = Patients.FirstOrDefault(p => p.Id == 4750);
            }
            else
            {
                _logger.LogError($"Failed to fetch patients. Status code: {response.StatusCode}");
                Patients = new List<Patient>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching patients.");
            Patients = new List<Patient>();
        }
    }
}
