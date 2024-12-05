using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OOP_Final_Project.Pages.Documents
{
    public class HealthCheckFormModel : PageModel
    {

        [BindProperty]
        public string PatientName { get; set; }

        [BindProperty]
        public int Age { get; set; }

        [BindProperty]
        public string Symptoms { get; set; }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                // Handle form submission (e.g., save data to database, redirect, etc.)
            }
        }

        public void OnGet()
        {
        }
    }
}
