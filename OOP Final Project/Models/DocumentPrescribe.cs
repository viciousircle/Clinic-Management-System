using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{
    public class DocumentPrescribe
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DocumentType")]
        public int DocumentTypeId { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }

        [ForeignKey("Pharmacist")]
        public int PharmacistId { get; set; }

        // Navigation properties
        public virtual required DocumentType DocumentType { get; set; }
        public virtual required Prescription Prescription { get; set; }
        public virtual required Employee Pharmacist { get; set; }
    }
}
