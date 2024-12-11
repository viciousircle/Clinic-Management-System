using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{

    // ! Patient and Employee 

    // ? Have 5 roles:
    // - Admin: The admin has full access to the system.

    // - Manager: The manager can CRUD the employees' information and reschedule appointments.
    // - Doctor: The doctor can view the patients' information and add new patients.
    // - Receptionist: The receptionist can view the patients' information and make appointments.
    // - Pharmacist: The pharmacist can view some patients' information and prescribe medications.
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AccountType")]
        public int AccountTypeId { get; set; }

        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }

        public DateTime CreateDate { get; set; }

        // Navigation property
        public virtual required AccountType AccountType { get; set; }

        // !Account will be associated with either an Employee or a Patient
        // !Polymorphic relationship between Employee/Patient// Relationship: 
        // - If the AccountType is Employee, then Employee will be associated.
        // - If the AccountType is Patient, then Patient will be associated.
        public virtual Employee? Employee { get; set; }  // Nullable for Patient accounts
        public virtual Patient? Patient { get; set; }    // Nullable for Employee accounts
    }
}
