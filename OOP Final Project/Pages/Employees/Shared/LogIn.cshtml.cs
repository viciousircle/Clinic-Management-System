using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OOP_Final_Project.Controllers.ApiResponses;
using OOP_Final_Project.ViewModels;
using OOP_Final_Project.ViewModels.Shared;

namespace OOP_Final_Project.Pages.Employees.Shared
{
    public class LogInModel : PageModel
    {

        // -- Fields ------------------------------------
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<LogInModel> _logger;
        private readonly HttpClient _client;

        // -- Constructor --------------------------------
        public LogInModel(IHttpClientFactory clientFactory, ILogger<LogInModel> logger)
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
        public async Task<IActionResult> OnPostLogInAsync(string username, string password)
        {
            _logger.LogInformation("Logging in...");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Username and password are required.");
                return Page();
            }

            try
            {
                var response = await _client.GetAsync($"api/employees/username/{username}/password/{password}");
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize into the strongly-typed LoginResponse object
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    if (result != null)
                    {
                        var accountTypeId = result.AccountTypeId;

                        // Store EmployeeId in session
                        HttpContext.Session.SetInt32("EmployeeId", result.EmployeeId);

                        // Redirect based on AccountTypeId
                        if (accountTypeId == 4)
                        {
                            _logger.LogInformation("Employee logged in.");
                            return RedirectToPage("/Employees/EmployeeLayout");
                        }
                        else
                        {
                            return RedirectToPage("/Employees/Shared/LogIn");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login.");
                ModelState.AddModelError(string.Empty, "An error occurred. Please try again later.");
            }

            return Page();
        }

    }
}
