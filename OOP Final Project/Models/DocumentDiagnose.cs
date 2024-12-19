using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{
    public class DocumentDiagnose
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DocumentType")]
        public int DocumentTypeId { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        [Required]
        public required string PatientStatus { get; set; }

        [Required]
        public bool IsSick { get; set; }

        [Required]
        public string DiagnoseDetails { get; set; }

        // Navigation properties
        public virtual DocumentType DocumentType { get; set; }
        public virtual Appointment Appointment { get; set; }
    }
}
