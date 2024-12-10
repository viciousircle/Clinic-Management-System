using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{
    public class DocumentBill
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DocumentType")]
        public int DocumentTypeId { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        [ForeignKey("Employee")]
        public int ReceptionistId { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        public decimal TotalCost { get; set; }

        // Navigation properties
        public virtual required DocumentType DocumentType { get; set; }
        public virtual required Appointment Appointment { get; set; }
        public virtual required Employee Receptionist { get; set; }
    }
}
