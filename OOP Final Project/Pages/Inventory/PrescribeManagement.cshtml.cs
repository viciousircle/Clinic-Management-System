using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OOP_Final_Project.Pages.Inventory
{
    public class PrescribeManagementModel : PageModel
    {
        public List<PrescriptionGroup> PrescriptionGroups { get; set; }

        public void OnGet()
        {
            var prescriptions = new List<Prescription>
    {
        new Prescription { IDAppointment = 1, Medicine = "Aspirin", Dosage = "10mg", Quantity = 30, PrescribedBy = "Dr. John Doe", Date = "2024-12-06" },
        new Prescription { IDAppointment = 1, Medicine = "Paracetamol", Dosage = "500mg", Quantity = 20, PrescribedBy = "Dr. John Doe", Date = "2024-12-06" },
        new Prescription { IDAppointment = 2, Medicine = "Ibuprofen", Dosage = "200mg", Quantity = 50, PrescribedBy = "Dr. Jane Smith", Date = "2024-12-07" }
    };

            // Grouping prescriptions by IDAppointment
            var groupedPrescriptions = prescriptions
                .GroupBy(p => p.IDAppointment)
                .Select(g => new PrescriptionGroup
                {
                    IDAppointment = g.Key,
                    Prescriptions = g.ToList()
                })
                .ToList();

            // Assign the grouped prescriptions to the model's property
            PrescriptionGroups = groupedPrescriptions;
        }

    }

    // Prescription model
    public class Prescription
    {
        public int IDAppointment { get; set; }
        public string Medicine { get; set; }
        public string Dosage { get; set; }
        public int Quantity { get; set; }
        public string PrescribedBy { get; set; }
        public string Date { get; set; }
    }

    // Grouped prescription data
    public class PrescriptionGroup
    {
        public int IDAppointment { get; set; }
        public List<Prescription> Prescriptions { get; set; }
    }
}
