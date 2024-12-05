using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OOP_Final_Project.Pages.Appointments
{
    public class InfoAppointmentModel : PageModel
    {
        public string AppointmentDate { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }

        public void OnGet()
        {
            // Example data, replace with actual data fetching logic
            AppointmentDate = "20th December 2024, 10:30 AM";
            DoctorName = "Dr. Emily Carter";
            Specialization = "Cardiology";
            Location = "Viciousircle Clinic, Room 204";
            Notes = "Please arrive 15 minutes early for check-in.";
        }
    }
}
