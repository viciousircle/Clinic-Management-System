using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models
{

    // ! Patient and Employee 
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
    }
}
