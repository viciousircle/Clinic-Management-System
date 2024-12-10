using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{
    public class DocumentCancel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DocumentType")]
        public int DocumentTypeId { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        [Required]
        public required string Reason { get; set; }

        [Required]
        public DateTime TimeCancel { get; set; }

        // Navigation properties
        public virtual required DocumentType DocumentType { get; set; }
        public virtual required Appointment Appointment { get; set; }
    }
}
