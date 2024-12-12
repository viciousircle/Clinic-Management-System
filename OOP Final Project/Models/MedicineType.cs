using System.ComponentModel.DataAnnotations;

namespace OOP_Final_Project.Models;

public class MedicineType
{
    /* 
    ! This model is used to define the types of medicines in the system.
    - Id: The unique identifier of the medicine type.
    - Name: The name of the medicine type (e.g. Tablet, Capsule, Syrup, etc.).

    ? Medicine types help categorize the different types of medicines available in the system.
    */

    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    // Navigation property: A medicine type can have many medicines
    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
