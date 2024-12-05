using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OOP_Final_Project.Pages.Patients
{
    public class MedicalHistoryModel : PageModel
    {
        public string PatientName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }

        public List<MedicalRecord> MedicalRecords { get; set; }

        public void OnGet()
        {
            // Sample Data - Replace with actual data from your database or service
            PatientName = "John Doe";
            DateOfBirth = new DateTime(1990, 5, 15);
            Gender = "Male";
            ContactNumber = "123-456-7890";

            MedicalRecords = new List<MedicalRecord>
            {
                new MedicalRecord
                {
                    Date = DateTime.Now.AddMonths(-3),
                    Condition = "Hypertension",
                    Medication = "Amlodipine 5mg",
                    Notes = "Follow up in 3 months."
                },
                new MedicalRecord
                {
                    Date = DateTime.Now.AddMonths(-6),
                    Condition = "Diabetes Type 2",
                    Medication = "Metformin 500mg",
                    Notes = "Monitor blood sugar levels regularly."
                }
            };
        }
    }

    public class MedicalRecord
    {
        public DateTime Date { get; set; }
        public string Condition { get; set; }
        public string Medication { get; set; }
        public string Notes { get; set; }
    }
}
