using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP_Final_Project.Pages.Appointments
{
    public class MakeAnAppointmentModel : PageModel
    {
        [BindProperty]
        public string PatientName { get; set; }

        [BindProperty]
        public string Doctor { get; set; }

        [BindProperty]
        public string AppointmentDate { get; set; }

        [BindProperty]
        public string AppointmentTime { get; set; }

        [BindProperty]
        public string Reason { get; set; }

        public List<string> AvailableTimeSlots { get; set; }

        private Dictionary<string, List<(TimeSpan start, TimeSpan end)>> DoctorSchedules = new()
        {
            { "Dr. Lee", new List<(TimeSpan, TimeSpan)> { (new TimeSpan(7, 0, 0), new TimeSpan(10, 0, 0)) } },
            { "Dr. Smith", new List<(TimeSpan, TimeSpan)> { (new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0)) } },
            // Add more doctors and their schedules here
        };

        private Dictionary<string, List<(DateTime date, TimeSpan start, TimeSpan end)>> Appointments = new()
        {
            { "Dr. Lee", new List<(DateTime, TimeSpan, TimeSpan)> { (DateTime.Parse("2024-12-08"), new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0)) } },
            // Add other appointments here
        };

        public void OnGet()
        {
            AvailableTimeSlots = new List<string>();
        }

        public void OnPost()
        {
            if (Doctor != null && AppointmentDate != null)
            {
                var date = DateTime.Parse(AppointmentDate);

                // Fetch the doctor's schedule for the selected day
                if (DoctorSchedules.ContainsKey(Doctor))
                {
                    var doctorSchedule = DoctorSchedules[Doctor];

                    // Loop through the doctor's schedule and check for available slots
                    foreach (var (start, end) in doctorSchedule)
                    {
                        var availableSlots = GetAvailableSlots(date, start, end);
                        AvailableTimeSlots = AvailableTimeSlots.Concat(availableSlots).ToList();
                    }
                }
            }
        }

        // Function to calculate available time slots for the given day
        private List<string> GetAvailableSlots(DateTime date, TimeSpan start, TimeSpan end)
        {
            var availableSlots = new List<string>();

            // Check if there are any existing appointments on the selected date
            if (Appointments.ContainsKey(Doctor))
            {
                var doctorAppointments = Appointments[Doctor]
                    .Where(a => a.date.Date == date.Date)
                    .ToList();

                // Generate the list of available time slots, excluding already booked times
                for (var slotStart = start; slotStart < end; slotStart = slotStart.Add(TimeSpan.FromHours(1)))
                {
                    var slotEnd = slotStart.Add(TimeSpan.FromHours(1));
                    bool isAvailable = true;

                    foreach (var appointment in doctorAppointments)
                    {
                        // Check if the slot overlaps with an existing appointment
                        if (slotStart < appointment.end && slotEnd > appointment.start)
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        availableSlots.Add($"{slotStart:hh\\:mm} - {slotEnd:hh\\:mm}");
                    }
                }
            }

            return availableSlots;
        }
    }
}
