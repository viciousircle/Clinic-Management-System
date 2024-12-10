using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models;

public class Employee
{
    /* 
    ! This model is used to define the employees in the system.
    - Id: The unique identifier of the employee.
    - FirstName: The first name of the employee.
    - LastName: The last name of the employee.
    - UserName: The username of the employee for login.
    - Password: The password for authentication.
    - DepartmentId: The foreign key linking to the department the employee belongs to.
    - RoleId: The foreign key linking to the role of the employee.
    - Email: The email address of the employee.
    - Phone: The phone number of the employee.
    - IsActive: Whether the employee is working for the company or not.

    ? An employee has a role and belongs to a department.
    */

    [Key]
    public int Id { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string Password { get; set; }

    [ForeignKey("Department")]
    public int DepartmentId { get; set; }

    [ForeignKey("Role")]
    public int RoleId { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Phone { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual required Department Department { get; set; }
    public virtual required Role Role { get; set; }
    public virtual required Account Account { get; set; }


    // Navigation property: An employee can have many employee schedules
    public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; } = new List<EmployeeSchedule>();
    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<DocumentPrescribe> DocumentPrescribes { get; set; } = new List<DocumentPrescribe>();
    public virtual ICollection<DocumentBill> DocumentBills { get; set; } = new List<DocumentBill>();
}
