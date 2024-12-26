using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        // Navigation properties
        public virtual Employee Doctor { get; set; }
        public virtual Patient Patient { get; set; }

        // 1-to-1 relationship with Prescription
        public virtual Prescription? Prescription { get; set; }
        public virtual DocumentAppointment? DocumentAppointment { get; set; }
        public virtual DocumentCancel? DocumentCancel { get; set; }
        public virtual DocumentDiagnose? DocumentDiagnose { get; set; }
        public virtual DocumentBill? DocumentBill { get; set; }


        // public virtual ICollection<DocumentAppointment> DocumentAppointments { get; set; } = new List<DocumentAppointment>();
    }
}
