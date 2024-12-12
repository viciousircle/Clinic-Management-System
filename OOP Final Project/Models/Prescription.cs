using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        // Navigation property: A prescription is associated with an appointment
        public virtual required Appointment Appointment { get; set; }

        public virtual required DocumentPrescribe DocumentPrescribe { get; set; }

        // Navigation property: A prescription can have many prescription medicines
        public virtual ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; } = new List<PrescriptionMedicine>();

    }
}
