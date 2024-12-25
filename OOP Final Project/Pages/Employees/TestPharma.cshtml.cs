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

        public TestPharmaModel(IHttpClientFactory clientFactory, ILogger<EmployeeLayoutModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _client = _clientFactory.CreateClient();

            // -- Initialize ViewModel ---------------------
            DoctorData = new DoctorViewModel();
            Employee = new EmployeeViewModel();

        }

        // -- Properties --------------------------------
        public DoctorViewModel DoctorData { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();


        // Dynamic handler to load partial views based on the "section" parameter
        public IActionResult OnGetLoadPartial(string section)
        {
            switch (section)
            {
                case "Dashboard":
                    return Partial("~/Pages/Employees/Pharmacists/_Dashboard.cshtml"); // Ensure the correct path
                case "Prescribe":
                    return Partial("~/Pages/Employees/Pharmacists/_Prescribe.cshtml"); // Ensure the correct path
                case "Warehouse":
                    return Partial("~/Pages/Employees/Pharmacists/_Warehouse.cshtml"); // Ensure the correct path
                case "Schedule":
                    return Partial("~/Pages/Employees/Shared/_Schedule.cshtml"); // Ensure the correct path
                case "Logout":
                    return Partial("~/Pages/Employees/Shared/_Logout.cshtml"); // Ensure the correct path
                default:
                    return Partial("~/Pages/Employees/Pharmacists/_Dashboard.cshtml"); // Ensure the correct path
            }
        }





    }
}

