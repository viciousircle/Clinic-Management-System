using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OOP_Final_Project.Pages.Employees
{
    public class EmployeeLayoutModel : PageModel
    {
        // Default handler for the page (no need for OnGet(), since we're dynamically loading content)
        public void OnGet()
        {
        }

        // Dynamic handler to load partial views based on the "section" parameter
        public IActionResult OnGetLoadPartial(string section)
        {
            switch (section)
            {
                case "Dashboard":
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml"); // Ensure the correct path
                case "Appointment":
                    return Partial("~/Pages/Employees/Doctors/_Appointment.cshtml"); // Ensure the correct path
                case "Patient":
                    return Partial("~/Pages/Employees/Doctors/_Patient.cshtml"); // Ensure the correct path
                case "Schedule":
                    return Partial("~/Pages/Employees/Shared/_Schedule.cshtml"); // Ensure the correct path
                case "Logout":
                    return Partial("~/Pages/Employees/Shared/_Logout.cshtml"); // Ensure the correct path
                default:
                    return Partial("~/Pages/Employees/Doctors/_Dashboard.cshtml"); // Ensure the correct path
            }
        }
    }
}
