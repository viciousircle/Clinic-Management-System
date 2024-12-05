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
        public List<string> BookedSlots { get; set; }  // List of booked time slots

        private Dictionary<string, List<(TimeSpan start, TimeSpan end)>> DoctorSchedules = new()
    {
        { "Dr. Lee", new List<(TimeSpan, TimeSpan)> { (new TimeSpan(7, 0, 0), new TimeSpan(10, 0, 0)) } },
        { "Dr. Smith", new List<(TimeSpan, TimeSpan)> { (new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0)) } },
        { "Dr. Johnson", new List<(TimeSpan, TimeSpan)> { (new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0)) } }
    };

        private Dictionary<string, List<(DateTime date, TimeSpan start, TimeSpan end)>> Appointments = new()
    {
        { "Dr. Lee", new List<(DateTime, TimeSpan, TimeSpan)> { (DateTime.Parse("2024-12-08"), new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0)) } },
        { "Dr. Smith", new List<(DateTime, TimeSpan, TimeSpan)> { (DateTime.Parse("2024-12-08"), new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0)) } },
        { "Dr. Johnson", new List<(DateTime, TimeSpan, TimeSpan)> { (DateTime.Parse("2024-12-08"), new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)) } }
    };

        public void OnGet()
        {
            AvailableTimeSlots = new List<string>();
            BookedSlots = new List<string>();

            // Check if the doctor and appointment date are selected
            if (Doctor != null && AppointmentDate != null)
            {
                var date = DateTime.Parse(AppointmentDate);

                if (DoctorSchedules.ContainsKey(Doctor))
                {
                    var doctorSchedule = DoctorSchedules[Doctor];

                    foreach (var (start, end) in doctorSchedule)
                    {
                        var availableSlots = GetAvailableSlots(date, start, end);
                        AvailableTimeSlots = AvailableTimeSlots.Concat(availableSlots).ToList();
                    }

                    // Collect booked slots for the selected doctor on the selected date
                    if (Appointments.ContainsKey(Doctor))
                    {
                        var doctorAppointments = Appointments[Doctor]
                            .Where(a => a.date.Date == date.Date)
                            .ToList();

                        foreach (var appointment in doctorAppointments)
                        {
                            // Add booked slots to the list
                            for (var slotStart = appointment.start; slotStart < appointment.end; slotStart = slotStart.Add(TimeSpan.FromHours(1)))
                            {
                                var slotEnd = slotStart.Add(TimeSpan.FromHours(1));
                                BookedSlots.Add($"{slotStart:hh\\:mm} - {slotEnd:hh\\:mm}");
                            }
                        }
                    }
                }
            }
        }

        private List<string> GetAvailableSlots(DateTime date, TimeSpan start, TimeSpan end)
        {
            var availableSlots = new List<string>();

            if (Appointments.ContainsKey(Doctor))
            {
                var doctorAppointments = Appointments[Doctor]
                    .Where(a => a.date.Date == date.Date)
                    .ToList();

                for (var slotStart = start; slotStart < end; slotStart = slotStart.Add(TimeSpan.FromHours(1)))
                {
                    var slotEnd = slotStart.Add(TimeSpan.FromHours(1));
                    bool isAvailable = true;

                    foreach (var appointment in doctorAppointments)
                    {
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
