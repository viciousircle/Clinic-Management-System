using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OOP_Final_Project.Models;

public class Patient
{
    /* 
    ! This model is used to define the patients in the system.
    - Id: The unique identifier of the patient.
    - FirstName: The first name of the patient.
    - LastName: The last name of the patient.
    - Email: The email address of the patient.
    - Phone: The phone number of the patient.
    - Address: The address of the patient.
    - UserName: The username of the patient for login. (6 months for registration)
    - Password: The password for authentication.

    ? A patient can have appointments and prescriptions.
    */

    [Key]
    public int Id { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Phone { get; set; }

    [Required]
    public string Address { get; set; }


    // Foreign key to Account
    [ForeignKey("Account")]
    public int AccountId { get; set; }

    // Navigation property: A patient can have many appointments
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Account Account { get; set; }


}
