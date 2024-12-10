using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models;

public class Department
{
    /* 
    ! This model is used to define the departments within a clinic.
    - Id: The unique identifier of the department.
    - ClinicId: The foreign key linking to the clinic where the department belongs.
    - Name: The name of the department.There are 4 departments in the clinic: Reception, Dental, Medical, and Management.

    ? A department can belong to a specific clinic and may contain many employees.
    */

    [Key]
    public int Id { get; set; }

    [ForeignKey("Clinic")]
    public int ClinicId { get; set; }

    [Required]
    public required string Name { get; set; }

    public required virtual Clinic Clinic { get; set; }
}
