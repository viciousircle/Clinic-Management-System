using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OOP_Final_Project.Pages.Appointments
{
    public class DoctorAppointmentModel : PageModel
    {
        public List<Appointment> Appointments { get; set; }

        public void OnGet()
        {
            // In a real application, you would fetch this data from a database.
            Appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    PatientName = "John Doe",
                    AppointmentDate = DateTime.Parse("2024-12-08"),
                    AppointmentTime = "09:00 AM",
                    Reason = "Routine Checkup",
                    Status = "Pending"
                },
                new Appointment
                {
                    Id = 2,
                    PatientName = "Jane Smith",
                    AppointmentDate = DateTime.Parse("2024-12-08"),
                    AppointmentTime = "10:00 AM",
                    Reason = "Consultation",
                    Status = "Pending"
                }
            };
        }

        // Handle marking appointment as completed
        public IActionResult OnPostMarkCompleted(int id)
        {
            // Find the appointment by id and update its status to "Completed"
            var appointment = Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment != null)
            {
                appointment.Status = "Completed";
            }

            // Redirect back to the doctor dashboard
            return RedirectToPage();
        }
    }

    public class Appointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
