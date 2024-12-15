using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OOP_Final_Project.Pages.Employees.Shared
{
    public class LoadPartialModel : PageModel
    {
        public IActionResult OnGet(string section)
        {
            // Check the value of section and return the corresponding partial view
            if (string.IsNullOrEmpty(section))
            {
                return Content("No section selected.");
            }

            switch (section)
            {
                case "Dashboard":
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml"); // Correct path to the Dashboard partial view
                case "Appointment":
                    return Partial("~/Pages/Employees/Doctors/_Appointment.cshtml"); // Correct path to the Appointment partial view
                case "Patients":
                    return Partial("~/Pages/Employees/Doctors/_Patient.cshtml"); // Correct path to the Patients partial view
                case "Schedule":
                    return Partial("~/Pages/Employees/Doctors/_Schedule.cshtml"); // Correct path to the Schedule partial view
                case "Logout":
                    // Handle logout (optional)
                    return RedirectToPage("/Account/Logout");
                default:
                    return Content("Invalid section.");
            }
        }
    }
}
