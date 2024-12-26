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
    public class TestPharmaModel : PageModel
    {
        // -- Fields ------------------------------------
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<EmployeeLayoutModel> _logger;
        private readonly HttpClient _client;


        // -- Constructor --------------------------------
        public TestPharmaModel(IHttpClientFactory clientFactory, ILogger<EmployeeLayoutModel> logger)
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
        public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();

        // -- Methods -----------------------------------
        public async Task OnGetAsync()
        {
            await FetchAllDataAsync();

        }

        // -- Helper Methods -----------------------------
        private async Task FetchAllDataAsync()
        {
        }


        // -- Get Partial View ---------------------------
        public async Task<IActionResult> OnGetLoadPartial(string section)
        {
            switch (section)
            {
                case "Dashboard":
                    return Partial("~/Pages/Employees/Pharmacists/_Dashboard.cshtml"); // Ensure the correct path
                case "Prescribe":
                    return Partial("~/Pages/Employees/Pharmacists/_Prescribe.cshtml"); // Ensure the correct path
                case "Warehouse":

                    await FetchMedicineCountsAsync();

                    return Partial("~/Pages/Employees/Pharmacists/_Warehouse.cshtml"); // Ensure the correct path
                case "Schedule":
                    return Partial("~/Pages/Employees/Shared/_Schedule.cshtml"); // Ensure the correct path
                case "Logout":
                    return Partial("~/Pages/Employees/Shared/_Logout.cshtml"); // Ensure the correct path
                default:
                    return Partial("~/Pages/Employees/Pharmacists/_Dashboard.cshtml"); // Ensure the correct path
            }
        }

        // ! ------ API Calls ----------------------------

        // ! Fetch count of medicines --------------------
        // -- [GET] /api/medicines/total -----------------
        // -- [GET] /api/medicines/total/expired ----------
        // -- [GET] /api/medicines/total/expiredSoon ------
        // -- [GET] /api/medicines/total/lowStock ---------

        private async Task FetchMedicineCountsAsync()
        {
            DoctorData.TotalMedicineCount = await FetchMedicineCountAsync("api/medicines/total");

            DoctorData.TotalExpiredMedicineCount = await FetchMedicineCountAsync("api/medicines/total/expired");

            DoctorData.TotalExpiredSoonMedicineCount = await FetchMedicineCountAsync("api/medicines/total/expiredSoon");

            DoctorData.TotalLowStockMedicineCount = await FetchMedicineCountAsync("api/medicines/total/lowStock");

            _logger.LogInformation("Medicine counts fetched successfully");
            _logger.LogInformation($"Total: {DoctorData.TotalMedicineCount}");
            _logger.LogInformation($"Expired: {DoctorData.TotalExpiredMedicineCount}");
            _logger.LogInformation($"Expired Soon: {DoctorData.TotalExpiredSoonMedicineCount}");
            _logger.LogInformation($"Low Stock: {DoctorData.TotalLowStockMedicineCount}");


        }

        private async Task<int> FetchMedicineCountAsync(string url)
        {

            try
            {
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var count = JsonSerializer.Deserialize<int>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return count;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching medicine count");
            }

            return 0;
        }


    }

}


