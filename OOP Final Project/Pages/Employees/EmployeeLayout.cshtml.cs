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
using System.Collections.Generic;
using OOP_Final_Project.Controllers.ApiResponses;


namespace OOP_Final_Project.Pages.Employees
{
    public class EmployeeLayoutModel : PageModel
    {

        // -- Fields ------------------------------------
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<EmployeeLayoutModel> _logger;
        private readonly HttpClient _client;

        // -- Constructor --------------------------------
        public EmployeeLayoutModel(IHttpClientFactory clientFactory, ILogger<EmployeeLayoutModel> logger)
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


        public async Task OnGetAsync()
        {
            await FetchAllDataAsync();
        }

        private async Task FetchAllDataAsync()
        {
            await FetchEmployeeDetailsAsync();
            await FetchAppointmentCountsAsync();
        }


        public async Task<IActionResult> OnGetLoadPartialAsync(string section)
        {

            await FetchAllDataAsync();

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
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
                default:
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml", DoctorData);
            }
        }

        private async Task FetchEmployeeDetailsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching employee details from API...");
                var response = await _client.GetAsync("api/employees/96");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Raw API response: {json}");
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var employeeResponse = JsonSerializer.Deserialize<EmployeeResponse>(json, options);

                    if (employeeResponse?.Employee != null)
                    {
                        DoctorData.Doctor = employeeResponse.Employee;
                        _logger.LogInformation("Employee details fetched successfully.");
                        _logger.LogInformation($"Employee: {DoctorData.Doctor.FirstName} {DoctorData.Doctor.LastName}");
                    }
                    else
                    {
                        _logger.LogError("Failed to deserialize employee data.");
                        DoctorData.Doctor = new EmployeeViewModel();
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch employee details. Status code: {response.StatusCode}");
                    DoctorData.Doctor = new EmployeeViewModel();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employee details.");
                DoctorData.Doctor = new EmployeeViewModel();

            }

        }



        private async Task FetchAppointmentCountsAsync()
        {
            DoctorData.AppointmentCount = await FetchAppointmentCountAsync("api/employees/96/appointments/count", "totalAppointments");
            DoctorData.FutureAppointmentCount = await FetchAppointmentCountAsync("api/employees/96/appointments/future/count", "totalFutureAppointments");
            DoctorData.CompletedAppointmentCount = await FetchAppointmentCountAsync("api/employees/96/appointments/completed/count", "totalCompletedAppointments");
            DoctorData.CancelledAppointmentCount = await FetchAppointmentCountAsync("api/employees/96/appointments/cancelled/count", "totalCancelledAppointments");
        }

        private async Task<int> FetchAppointmentCountAsync(string url, string jsonProperty)
        {
            try
            {
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(json))
                    {
                        var jsonData = JsonSerializer.Deserialize<JsonElement>(json);
                        if (jsonData.TryGetProperty(jsonProperty, out var propertyValue))
                        {
                            return propertyValue.GetInt32();
                        }
                        else
                        {
                            _logger.LogError($"{jsonProperty} not found in response. Response: {json}");
                        }
                    }
                    else
                    {
                        _logger.LogError($"Response for {url} is empty.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch appointment count from {url}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching appointment count from {url}");
            }

            return 0; // Default to 0 if fetching fails
        }

        // private async Task FetchEmployeeData()
        // {
        //     try
        //     {
        //         _logger.LogInformation("Fetching employee details from API...");

        //         // - Fetch employee details
        //         // - GET /api/employees/96

        //         // var responseEmployee = await _client.GetAsync("api/employees/96");

        //         // if (responseEmployee.IsSuccessStatusCode)
        //         // {
        //         //     var employeeJson = await responseEmployee.Content.ReadAsStringAsync();
        //         //     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        //         //     var employeeData = JsonSerializer.Deserialize<EmployeeResponse>(employeeJson, options);
        //         //     if (employeeData != null)
        //         //     {
        //         //         DoctorData.Doctor = employeeData.Employees.FirstOrDefault();
        //         //         _logger.LogInformation("Employee data fetched successfully.");
        //         //         _logger.LogInformation($"Employee: {DoctorData.Doctor.FirstName} {DoctorData.Doctor.LastName}");
        //         //     }
        //         //     else
        //         //     {
        //         //         _logger.LogError("Failed to deserialize employee data.");
        //         //         DoctorData.Doctor = new EmployeeViewModel();
        //         //     }
        //         // }
        //         // else
        //         // {
        //         //     _logger.LogError($"Failed to fetch employee details. Status code: {responseEmployee.StatusCode}");
        //         //     DoctorData.Doctor = new EmployeeViewModel();
        //         // }

        //         // - Fetch appointment details
        //         // - GET /api/employees/96/appointments/count

        //         var responseAppointments = await _client.GetAsync("api/employees/96/appointments/count");

        //         if (responseAppointments != null && responseAppointments.IsSuccessStatusCode)
        //         {
        //             var appointmentsJson = await responseAppointments.Content.ReadAsStringAsync();
        //             if (!string.IsNullOrEmpty(appointmentsJson))
        //             {
        //                 var appointmentsData = JsonSerializer.Deserialize<JsonElement>(appointmentsJson);
        //                 if (appointmentsData.TryGetProperty("totalAppointments", out JsonElement totalAppointmentsElement))
        //                 {
        //                     DoctorData.AppointmentCount = totalAppointmentsElement.GetInt32();
        //                 }
        //                 else
        //                 {
        //                     _logger.LogError($"totalAppointments property not found in the response. Response: {appointmentsJson}");
        //                 }
        //             }
        //             else
        //             {
        //                 _logger.LogError("Appointments JSON is null or empty.");
        //             }
        //         }
        //         else
        //         {
        //             _logger.LogError($"Failed to fetch appointments count. Status code: {responseAppointments?.StatusCode}");
        //         }


        //         // - Fetch future, completed, and cancelled appointments count
        //         // - GET /api/employees/96/appointments/future/count
        //         // - GET /api/employees/96/appointments/completed/count
        //         // - GET /api/employees/96/appointments/cancelled/count

        //         var responseFutureAppointments = await _client.GetAsync("api/employees/96/appointments/future/count");

        //         if (responseFutureAppointments != null && responseFutureAppointments.IsSuccessStatusCode)
        //         {
        //             var futureAppointmentsJson = await responseFutureAppointments.Content.ReadAsStringAsync();
        //             if (!string.IsNullOrEmpty(futureAppointmentsJson))
        //             {
        //                 var futureAppointmentsData = JsonSerializer.Deserialize<JsonElement>(futureAppointmentsJson);

        //                 if (futureAppointmentsData.TryGetProperty("totalFutureAppointments", out JsonElement totalFutureAppointmentsElementy))
        //                 {
        //                     DoctorData.FutureAppointmentCount = totalFutureAppointmentsElementy.GetInt32();
        //                 }
        //                 else
        //                 {
        //                     _logger.LogError($"totalFutureAppointments property not found in the response. Response: {futureAppointmentsJson}");
        //                 }
        //             }
        //             else
        //             {
        //                 _logger.LogError("Future Appointments JSON is null or empty.");
        //             }
        //         }

        //         var responseCompletedAppointments = await _client.GetAsync("api/employees/96/appointments/completed/count");

        //         if (responseCompletedAppointments != null && responseCompletedAppointments.IsSuccessStatusCode)
        //         {
        //             var completedAppointmentsJson = await responseCompletedAppointments.Content.ReadAsStringAsync();
        //             if (!string.IsNullOrEmpty(completedAppointmentsJson))
        //             {
        //                 var completedAppointmentsData = JsonSerializer.Deserialize<JsonElement>(completedAppointmentsJson);

        //                 if (completedAppointmentsData.TryGetProperty("totalCompletedAppointments", out JsonElement totalCompletedAppointmentsElement))
        //                 {
        //                     DoctorData.CompletedAppointmentCount = totalCompletedAppointmentsElement.GetInt32();
        //                 }
        //                 else
        //                 {
        //                     _logger.LogError($"totalCompletedAppointments property not found in the response. Response: {completedAppointmentsJson}");
        //                 }
        //             }
        //             else
        //             {
        //                 _logger.LogError("Completed Appointments JSON is null or empty.");
        //             }
        //         }

        //         var responseCancelledAppointments = await _client.GetAsync("api/employees/96/appointments/cancelled/count");

        //         if (responseCancelledAppointments != null && responseCancelledAppointments.IsSuccessStatusCode)
        //         {
        //             var cancelledAppointmentsJson = await responseCancelledAppointments.Content.ReadAsStringAsync();
        //             if (!string.IsNullOrEmpty(cancelledAppointmentsJson))
        //             {
        //                 var cancelledAppointmentsData = JsonSerializer.Deserialize<JsonElement>(cancelledAppointmentsJson);

        //                 if (cancelledAppointmentsData.TryGetProperty("totalCancelledAppointments", out JsonElement totalCancelledAppointmentsElement))
        //                 {
        //                     DoctorData.CancelledAppointmentCount = totalCancelledAppointmentsElement.GetInt32();
        //                 }
        //                 else
        //                 {
        //                     _logger.LogError($"totalCancelledAppointments property not found in the response. Response: {cancelledAppointmentsJson}");
        //                 }
        //             }
        //             else
        //             {
        //                 _logger.LogError("Cancelled Appointments JSON is null or empty.");
        //             }
        //         }


        //         // - Fetch patients
        //         // - GET /api/employees/96/patients


        //         // - Fetch appointments
        //         // - GET /api/employees/96/appointments




        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "An error occurred while fetching employee details.");
        //         DoctorData = new DoctorViewModel();
        //     }
        // }


    }
}