using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using OOP_Final_Project.ViewModels;
using OOP_Final_Project.ViewModels.Shared;
using OOP_Final_Project.Controllers.ApiResponses;

namespace OOP_Final_Project.Pages.Employees
{
    public class TestPharmaModel : PageModel
    {
        // -- Fields ------------------------------------
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<TestPharmaModel> _logger;
        private readonly HttpClient _client;

        // -- Constructor --------------------------------
        public TestPharmaModel(IHttpClientFactory clientFactory, ILogger<TestPharmaModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _client = _clientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5298/");

            // -- Initialize ViewModel ---------------------
            DoctorData = new DoctorViewModel();
            Employee = new EmployeeViewModel();
            Medicines = new List<MedicineViewModel>();
        }

        // -- Properties --------------------------------
        public DoctorViewModel DoctorData { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public List<MedicineViewModel> Medicines { get; set; } = new List<MedicineViewModel>();

        // -- Methods -----------------------------------
        public async Task OnGetAsync()
        {
            await FetchAllDataAsync();
            await FetchMedicinesAsync();
        }

        // -- Helper Methods -----------------------------
        private async Task FetchAllDataAsync()
        {
            await FetchMedicineCountsAsync();
        }

        // -- Partial Methods ----------------------------
        public async Task<IActionResult> OnGetLoadPartial(string section)
        {
            switch (section)
            {
                case "Dashboard":
                    return Partial("~/Pages/Employees/Pharmacists/_Dashboard.cshtml", DoctorData);
                case "Prescribe":
                    return Partial("~/Pages/Employees/Pharmacists/_Prescribe.cshtml", DoctorData);
                case "Warehouse":
                    await FetchMedicinesAsync();
                    await FetchMedicineCountsAsync();
                    return Partial("~/Pages/Employees/Pharmacists/_Warehouse.cshtml", DoctorData);
                case "Schedule":
                    return Partial("~/Pages/Employees/Shared/_Schedule.cshtml", DoctorData);
                case "Logout":
                    return Partial("~/Pages/Employees/Shared/_Logout.cshtml", DoctorData);
                default:
                    return Partial("~/Pages/Employees/Pharmacists/_Dashboard.cshtml", DoctorData);
            }
        }


        // -- API Calls ---------------------------------

        // ! -- Medicine Counts ---------------------------
        private async Task FetchMedicineCountsAsync()
        {
            DoctorData.TotalMedicineCount = await FetchMedicineCountAsync("api/medicines/total");
            DoctorData.TotalExpiredMedicineCount = await FetchMedicineCountAsync("api/medicines/total/expired");
            DoctorData.TotalExpiredSoonMedicineCount = await FetchMedicineCountAsync("api/medicines/total/expiredSoon");
            DoctorData.TotalLowStockMedicineCount = await FetchMedicineCountAsync("api/medicines/total/lowStock");

            _logger.LogInformation("Medicine counts fetched successfully");
            _logger.LogInformation($"Total: {DoctorData.TotalMedicineCount}, Expired: {DoctorData.TotalExpiredMedicineCount}, Expired Soon: {DoctorData.TotalExpiredSoonMedicineCount}, Low Stock: {DoctorData.TotalLowStockMedicineCount}");
        }

        private async Task<int> FetchMedicineCountAsync(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<int>(content);
                }
                else
                {
                    _logger.LogError($"Failed to fetch medicine count from {url}. StatusCode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching medicine count from {url}");
            }
            return 0;
        }

        // ! -- Medicine List -----------------------------
        //  -- [GET] api/medicines ------------------------
        //  -- [GET] api/medicines/expired -----------------
        //  -- [GET] api/medicines/expiredSoon -------------
        //  -- [GET] api/medicines/lowStock ----------------

        private async Task FetchMedicinesAsync()
        {
            try
            {
                // Fetch All Medicines
                var responseAll = await _client.GetAsync("api/medicines");
                if (responseAll.IsSuccessStatusCode)
                {
                    var content = await responseAll.Content.ReadAsStringAsync();
                    _logger.LogInformation($"API Response Content for All Medicines: {content}");

                    var medicinesResponse = JsonSerializer.Deserialize<MedicinesResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (medicinesResponse != null)
                    {
                        DoctorData.Medicines = medicinesResponse.Medicines;
                        _logger.LogInformation($"Total Medicines: {DoctorData.Medicines.Count}");
                    }
                    else
                    {
                        _logger.LogWarning("No medicines found in response for all medicines.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch all medicines. Status code: {responseAll.StatusCode}");
                }

                // Fetch Expired Medicines
                var responseExpired = await _client.GetAsync("api/medicines/expired");
                if (responseExpired.IsSuccessStatusCode)
                {
                    var content = await responseExpired.Content.ReadAsStringAsync();
                    _logger.LogInformation($"API Response Content for Expired Medicines: {content}");

                    var expiredResponse = JsonSerializer.Deserialize<MedicinesResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (expiredResponse != null)
                    {
                        DoctorData.ExpiredMedicines = expiredResponse.ExpiredMedicines;
                        _logger.LogInformation($"Expired Medicines: {DoctorData.ExpiredMedicines.Count}");
                    }
                    else
                    {
                        _logger.LogWarning("No expired medicines found in response.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch expired medicines. Status code: {responseExpired.StatusCode}");
                }

                // Fetch Low Stock Medicines
                var responseLowStock = await _client.GetAsync("api/medicines/lowStock");
                if (responseLowStock.IsSuccessStatusCode)
                {
                    var content = await responseLowStock.Content.ReadAsStringAsync();
                    _logger.LogInformation($"API Response Content for Low Stock Medicines: {content}");

                    var lowStockResponse = JsonSerializer.Deserialize<MedicinesResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (lowStockResponse != null)
                    {
                        DoctorData.LowStockMedicines = lowStockResponse.LowStockMedicines;
                        _logger.LogInformation($"Low Stock Medicines: {DoctorData.LowStockMedicines.Count}");
                    }
                    else
                    {
                        _logger.LogWarning("No low stock medicines found in response.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch low stock medicines. Status code: {responseLowStock.StatusCode}");
                }

                // Log Final Counts
                _logger.LogInformation($"Final counts - Total Medicines: {DoctorData.Medicines.Count}, Expired: {DoctorData.ExpiredMedicines.Count}, Low Stock: {DoctorData.LowStockMedicines.Count}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching medicines");
            }
        }



    }
}
