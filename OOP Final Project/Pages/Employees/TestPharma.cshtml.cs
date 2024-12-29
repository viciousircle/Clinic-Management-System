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
        public int EmployeeId { get; set; }

        // -- Methods -----------------------------------
        public async Task OnGetAsync()
        {
            EmployeeId = HttpContext.Session.GetInt32("EmployeeId") ?? 0;
            await FetchAllDataAsync();

            await FetchEmployeeDetailsAsync();
            await FetchPrescriptionsByDateAsync(EmployeeId, "08-10-2025");
            await FetchPrescriptionsByDatePrepareAsync(EmployeeId, "08-10-2025");
            await FetchPrescriptionByDatePickupAsync(EmployeeId, "08-10-2025");
            await FetchPrescriptionByDateDoneAsync(EmployeeId, "08-10-2025");
            await FetchMedicineCountsAsync();
            // await FetchMedicinesAsync();
        }

        // -- Helper Methods -----------------------------
        private async Task FetchAllDataAsync()
        {
        }

        // -- Partial Methods ----------------------------
        public async Task<IActionResult> OnGetLoadPartial(string section, string filter, string view)
        {
            switch (section)
            {
                case "PatientCards":
                    // Ensure prescriptions are loaded
                    if (DoctorData.OnDatePickupPrescriptions == null || !DoctorData.OnDatePickupPrescriptions.Any())
                    {
                        _logger.LogError("No prescriptions found for OnDatePickupPrescriptions.");
                    }
                    await LoadPatientCardsAsync(view);
                    return Partial("~/Pages/Employees/Pharmacists/_PatientCards.cshtml", DoctorData);
                case "Dashboard":
                    EmployeeId = HttpContext.Session.GetInt32("EmployeeId") ?? 0;
                    await FetchPrescriptionsByDateAsync(EmployeeId, "08-10-2025");
                    await FetchPrescriptionsByDatePrepareAsync(EmployeeId, "08-10-2025");
                    await FetchPrescriptionByDatePickupAsync(EmployeeId, "08-10-2025");
                    await FetchPrescriptionByDateDoneAsync(EmployeeId, "08-10-2025");
                    await FetchMedicinesAsync(filter);
                    await FetchMedicineCountsAsync();
                    return Partial("~/Pages/Employees/Pharmacists/_Dashboard.cshtml", DoctorData);
                case "Prescribe":
                    EmployeeId = HttpContext.Session.GetInt32("EmployeeId") ?? 0;
                    await FetchPrescriptionsByDateAsync(EmployeeId, "08-10-2025");
                    await FetchPrescriptionsByDatePrepareAsync(EmployeeId, "08-10-2025");
                    await FetchPrescriptionByDatePickupAsync(EmployeeId, "08-10-2025");
                    await FetchPrescriptionByDateDoneAsync(EmployeeId, "08-10-2025");
                    return Partial("~/Pages/Employees/Pharmacists/_Prescribe.cshtml", DoctorData);
                case "Warehouse":
                    EmployeeId = HttpContext.Session.GetInt32("EmployeeId") ?? 0;
                    await FetchMedicinesAsync(filter);
                    await FetchMedicineCountsAsync();
                    return Partial("~/Pages/Employees/Pharmacists/_Warehouse.cshtml", DoctorData);
                case "WarehouseTableRows":
                    await FetchMedicineCountsAsync();

                    await FetchMedicinesAsync(filter);
                    return Partial("~/Pages/Employees/Pharmacists/_WarehouseTableRows.cshtml", DoctorData);
                case "Schedule":
                    return Partial("~/Pages/Employees/Shared/_Schedule.cshtml", DoctorData);
                case "Logout":
                    return Partial("~/Pages/Employees/Shared/_Logout.cshtml", DoctorData);
                default:
                    return Partial("~/Pages/Employees/Pharmacists/_Dashboard.cshtml", DoctorData);
            }
        }

        // -- API Calls ---------------------------------

        private async Task LoadPatientCardsAsync(string view)
        {
            EmployeeId = HttpContext.Session.GetInt32("EmployeeId") ?? 0;

            switch (view)
            {
                case "prepare":
                    await FetchPrescriptionsByDatePrepareAsync(EmployeeId, "08-10-2025");
                    break;

                case "pickup":
                    await FetchPrescriptionByDatePickupAsync(EmployeeId, "08-10-2025");
                    break;
                case "done":
                    await FetchPrescriptionByDateDoneAsync(EmployeeId, "08-10-2025");
                    break;

                default:
                    throw new ArgumentException("Invalid view parameter");
            }

            // Log DoctorData for debugging
            _logger.LogInformation("DoctorData: {@DoctorData}", DoctorData);
        }



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

        // ! --- Employee  ----------------------------
        private async Task FetchEmployeeDetailsAsync()
        {
            try
            {
                // _logger.LogInformation("Fetching employee details from API..."); //For logging purposes
                var response = await _client.GetAsync($"api/employees/{EmployeeId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
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



        // ! -- Medicine List -----------------------------
        //  -- [GET] api/medicines ------------------------
        //  -- [GET] api/medicines/expired -----------------
        //  -- [GET] api/medicines/expiredSoon -------------
        //  -- [GET] api/medicines/lowStock ----------------

        private async Task FetchMedicinesAsync(string filter = "all")
        {
            try
            {
                string url = filter switch
                {
                    "expired" => "api/medicines/expired",
                    "low-stock" => "api/medicines/lowStock",
                    _ => "api/medicines"
                };

                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var medicinesResponse = JsonSerializer.Deserialize<MedicinesResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (medicinesResponse != null)
                    {
                        if (filter == "all")
                        {
                            DoctorData.Medicines = medicinesResponse.Medicines;
                            _logger.LogInformation($"All Medicines: {DoctorData.Medicines.Count}");
                        }
                        else if (filter == "expired")
                        {
                            DoctorData.ExpiredMedicines = medicinesResponse.ExpiredMedicines;
                            _logger.LogInformation($"{filter} Medicines: {DoctorData.Medicines.Count}");
                        }

                        else if (filter == "low-stock")
                        {
                            DoctorData.LowStockMedicines = medicinesResponse.LowStockMedicines;
                            _logger.LogInformation($"{filter} Medicines: {DoctorData.Medicines.Count}");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"No {filter} medicines found in response.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch {filter} medicines. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching {filter} medicines");
            }
        }

        // ! -- Prescribe -------------------------------
        //  -- [GET] api/emloyees/pharmacist/{id}/prescrptions/today
        //  -- [GET] api/emloyees/pharmacist/{id}/prescriptions/on/{date}

        private async Task FetchPrescriptionsByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var response = await _client.GetAsync($"api/employees/pharmacist/{employeeId}/prescriptions");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var prescriptionsResponse = JsonSerializer.Deserialize<PrescriptionsResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (prescriptionsResponse != null)
                    {
                        DoctorData.Prescriptions = prescriptionsResponse.Prescriptions;
                        _logger.LogInformation($"Prescriptions for Employee {employeeId}: {DoctorData.Prescriptions.Count}");
                    }
                    else
                    {
                        _logger.LogWarning($"No prescriptions found for Employee {employeeId}.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch prescriptions for Employee {employeeId}. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching prescriptions for Employee {employeeId}");
            }
        }


        private async Task FetchPrescriptionsByDateAsync(int employeeId, string date)
        {
            try
            {
                // Define the expected date format
                string format = "dd-MM-yyyy";
                DateTime parsedDate;

                // Try parsing the provided date with the specified format
                bool isValidDate = DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);

                if (!isValidDate)
                {
                    _logger.LogError($"Invalid date format: {date}");
                    return; // Exit if the date is invalid
                }

                string normalizedDate = parsedDate.ToString("dd-MM-yyyy"); // Ensure the date is in the correct format

                var response = await _client.GetAsync($"api/employees/pharmacist/{employeeId}/prescriptions");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var prescriptionsResponse = JsonSerializer.Deserialize<PrescriptionsResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (prescriptionsResponse != null)
                    {
                        // Filter prescriptions that match the provided date (ignoring time)
                        DoctorData.OnDatePrescriptions = prescriptionsResponse.Prescriptions
                            .Where(p => p.AppointmentTime.Contains(normalizedDate)) // Check if the appointment time contains the normalized date
                            .ToList();

                        _logger.LogInformation($"Prescriptions for Employee {employeeId} on {normalizedDate}: {DoctorData.OnDatePrescriptions.Count}");
                    }
                    else
                    {
                        _logger.LogWarning($"No prescriptions found for Employee {employeeId} on {normalizedDate}.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch prescriptions for Employee {employeeId}. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching prescriptions for Employee {employeeId} on {date}");
            }
        }



        private async Task FetchPrescriptionsTodayAsync(int employeeId)
        {
            try
            {
                // Get today's date and format it as "dd-MM-yyyy"
                string todayDate = DateTime.Today.ToString("dd-MM-yyyy");

                // Use FetchPrescriptionsByDateAsync to fetch today's prescriptions
                await FetchPrescriptionsByDateAsync(employeeId, todayDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching prescriptions for Employee {employeeId} today.");
            }
        }

        private async Task FetchPrescriptionsByDatePrepareAsync(int employeeId, string date)
        {
            try
            {
                // Define the expected date format
                string format = "dd-MM-yyyy";
                DateTime parsedDate;

                // Try parsing the provided date with the specified format
                bool isValidDate = DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);

                if (!isValidDate)
                {
                    _logger.LogError($"Invalid date format: {date}");
                    return; // Exit if the date is invalid
                }

                string normalizedDate = parsedDate.ToString("dd-MM-yyyy"); // Ensure the date is in the correct format

                var response = await _client.GetAsync($"api/employees/pharmacist/{employeeId}/prescriptions");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var prescriptionsResponse = JsonSerializer.Deserialize<PrescriptionsResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (prescriptionsResponse != null)
                    {
                        // Filter prescriptions that match the provided date and have status "Prepare"
                        DoctorData.OnDatePreparePrescriptions = prescriptionsResponse.Prescriptions
                            .Where(p => p.AppointmentTime.Contains(normalizedDate) && p.PrescriptionStatus == "Prepare") // Assuming PrescriptionStatus is a string
                            .ToList();

                        _logger.LogInformation($"Prescriptions for Employee {employeeId} on {normalizedDate} with 'Prepare' status: {DoctorData.OnDatePreparePrescriptions.Count}");
                    }
                    else
                    {
                        _logger.LogWarning($"No prescriptions found for Employee {employeeId} on {normalizedDate}.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch prescriptions for Employee {employeeId}. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching prescriptions for Employee {employeeId} on {date} with 'Prepare' status.");
            }
        }

        private async Task FetchPrescriptionByDatePickupAsync(int employeeId, string date)
        {
            try
            {
                // Define the expected date format
                string format = "dd-MM-yyyy";
                DateTime parsedDate;

                // Try parsing the provided date with the specified format
                bool isValidDate = DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);

                if (!isValidDate)
                {
                    _logger.LogError($"Invalid date format: {date}");
                    return; // Exit if the date is invalid
                }

                string normalizedDate = parsedDate.ToString("dd-MM-yyyy"); // Ensure the date is in the correct format

                var response = await _client.GetAsync($"api/employees/pharmacist/{employeeId}/prescriptions");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var prescriptionsResponse = JsonSerializer.Deserialize<PrescriptionsResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (prescriptionsResponse != null)
                    {
                        // Filter prescriptions that match the provided date and have status "Pickup"
                        DoctorData.OnDatePickupPrescriptions = prescriptionsResponse.Prescriptions
                            .Where(p => p.AppointmentTime.Contains(normalizedDate) && p.PrescriptionStatus == "Pick up") // Assuming PrescriptionStatus is a string
                            .ToList();

                        _logger.LogInformation($"Prescriptions for Employee {employeeId} on {normalizedDate} with 'Pickup' status: {DoctorData.OnDatePickupPrescriptions.Count}");
                    }
                    else
                    {
                        _logger.LogWarning($"No prescriptions found for Employee {employeeId} on {normalizedDate}.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch prescriptions for Employee {employeeId}. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching prescriptions for Employee {employeeId} on {date} with 'Pickup' status.");
            }
        }


        private async Task FetchPrescriptionByDateDoneAsync(int employeeId, string date)
        {
            try
            {
                // Define the expected date format
                string format = "dd-MM-yyyy";
                DateTime parsedDate;

                // Try parsing the provided date with the specified format
                bool isValidDate = DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);

                if (!isValidDate)
                {
                    _logger.LogError($"Invalid date format: {date}");
                    return; // Exit if the date is invalid
                }

                string normalizedDate = parsedDate.ToString("dd-MM-yyyy"); // Ensure the date is in the correct format

                var response = await _client.GetAsync($"api/employees/pharmacist/{employeeId}/prescriptions");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var prescriptionsResponse = JsonSerializer.Deserialize<PrescriptionsResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (prescriptionsResponse != null)
                    {
                        // Filter prescriptions that match the provided date and have status "Done"
                        DoctorData.OnDateDonePrescriptions = prescriptionsResponse.Prescriptions
                            .Where(p => p.AppointmentTime.Contains(normalizedDate) && p.PrescriptionStatus == "Done") // Assuming PrescriptionStatus is a string
                            .ToList();

                        _logger.LogInformation($"Prescriptions for Employee {employeeId} on {normalizedDate} with 'Done' status: {DoctorData.OnDateDonePrescriptions.Count}");
                    }
                    else
                    {
                        _logger.LogWarning($"No prescriptions found for Employee {employeeId} on {normalizedDate}.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch prescriptions for Employee {employeeId}. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching prescriptions for Employee {employeeId} on {date} with 'Done' status.");
            }
        }
    }
}