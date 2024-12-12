using System.ComponentModel.DataAnnotations;

namespace OOP_Final_Project.Models;

public class Clinic
{
    /* 
    ! This model is used to define the clinics in the system.
    - Id: The unique identifier of the clinic.
    - Name: The name of the clinic.
    - Address: The location of the clinic.
    - Details: Any additional information about the clinic.

    ? A clinic could have multiple departments and employees.
    */

    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Address { get; set; }

    [Required]
    public required string Details { get; set; }

    // Navigation property: A clinic can have many departments
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
