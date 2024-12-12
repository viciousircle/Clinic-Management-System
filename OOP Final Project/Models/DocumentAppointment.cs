using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{
    public class DocumentAppointment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DocumentType")]
        public int DocumentTypeId { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        [Required]
        public DateTime TimeBook { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan TimeStart { get; set; }

        [Required]
        public TimeSpan TimeEnd { get; set; }

        [Required]
        public required string Location { get; set; }

        // Navigation properties
        public virtual required DocumentType DocumentType { get; set; }
        public virtual required Appointment Appointment { get; set; }
    }
}
