using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OOP_Final_Project.Pages.Employees
{
    public class TestManagerModel : PageModel
    {
        public void OnGet()
        {
        }

        // Dynamic handler to load partial views based on the "section" parameter
        public IActionResult OnGetLoadPartial(string section)
        {
            switch (section)
            {
                case "Dashboard":
                    return Partial("~/Pages/Employees/Managers/_Dashboard.cshtml"); // Ensure the correct path
                case "Doctor":
                    return Partial("~/Pages/Employees/Managers/_Doctor.cshtml"); // Ensure the correct path
                case "Client":
                    return Partial("~/Pages/Employees/Managers/_Client.cshtml"); // Ensure the correct path
                case "Schedule":
                    return Partial("~/Pages/Employees/Managers/_Schedule.cshtml"); // Ensure the correct path
                default:
                    return Partial("~/Pages/Employees/Managers/_Dashboard.cshtml"); // Ensure the correct path
            }
        }

    }
}
