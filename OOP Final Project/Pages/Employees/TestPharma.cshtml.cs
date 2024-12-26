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
            DoctorData.Medicines = await FetchMedicinesAsync("api/medicines");
            // DoctorData.ExpiredMedicines = await FetchMedicinesAsync("api/medicines/expired");
            // DoctorData.ExpiredSoonMedicines = await FetchMedicinesAsync("api/medicines/expiredSoon");
            // DoctorData.LowStockMedicines = await FetchMedicinesAsync("api/medicines/lowStock");

            _logger.LogInformation("Medicines fetched successfull blahblah");
            _logger.LogInformation($"Total: {DoctorData.Medicines.Count}");
        }

        private async Task<List<MedicineViewModel>> FetchMedicinesAsync(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Raw JSON Response: " + content);  // Log the raw JSON content for debugging

                    // Deserialize into the MedicinesResponse object
                    var medicinesResponse = JsonSerializer.Deserialize<MedicinesResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return medicinesResponse?.Medicines ?? new List<MedicineViewModel>();
                }
                else
                {
                    _logger.LogError($"Failed to fetch medicines from {url}. StatusCode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching medicines from {url}");
            }
            return new List<MedicineViewModel>();
        }


    }
}
