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
        public virtual required Employee Doctor { get; set; }
        public virtual required Patient Patient { get; set; }
    }
}
