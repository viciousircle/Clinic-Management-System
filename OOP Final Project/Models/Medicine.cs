using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models;

public class Medicine
{
    /* 
    ! This model is used to define the medicines in the system.
    - Id: The unique identifier of the medicine.
    - Name: The name of the medicine.
    - MedicineTypeId: The foreign key linking to the medicine type.
    - ExpiredDate: The expiration date of the medicine.
    - ImporterId: The foreign key linking to the employee who imported the medicine.
    - ImportDate: The date the medicine was imported.
    - Quantity: The number of units of the medicine in stock.

    ? A medicine can be prescribed in a prescription.
    */

    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [ForeignKey("MedicineType")]
    public int MedicineTypeId { get; set; }

    public DateTime ExpiredDate { get; set; }

    [ForeignKey("Employee")]
    public int ImporterId { get; set; }

    public DateTime ImportDate { get; set; }

    public int Quantity { get; set; }
}
