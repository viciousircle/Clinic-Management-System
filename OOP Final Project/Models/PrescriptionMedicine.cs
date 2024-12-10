using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{
    public class PrescriptionMedicine
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }

        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        [Required]
        public required string DosageAmount { get; set; }

        [Required]
        public required string Frequency { get; set; }

        [Required]
        public required string FrequencyUnit { get; set; }

        [Required]
        public required string Route { get; set; }

        public required string Instructions { get; set; }

        // Navigation properties
        public virtual required Prescription Prescription { get; set; }
        public virtual required Medicine Medicine { get; set; }
    }
}
