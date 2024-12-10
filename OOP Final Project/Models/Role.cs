using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models;

public class Role
{
    /* 
    ! This model is used to define the roles of the employees in the system.
    - Id: The unique identifier of the role.
    - Name: The name of the role.

    ? Have 5 roles:
    - Admin: The admin has full access to the system.
    
    - Manager: The manager can CRUD the employees' information and reschedule appointments.
    - Doctor: The doctor can view the patients' information and add new patients.
    - Receptionist: The receptionist can view the patients' information and make appointments.
    - Pharmacist: The pharmacist can view some patients' information and prescribe medications.
    */

    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    // Navigation property: A role can have many employees
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
