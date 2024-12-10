using System.ComponentModel.DataAnnotations;

namespace OOP_Final_Project.Models;

public class AccountType
{
    /* 
    ! This model is used to define the types of accounts in the system.
    - Id: The unique identifier of the account type.
    - Name: There are 2 types of accounts in the system: Limited and Indefinitely.

    ? Different account types determine the permissions of the users.
    - Employees account is indefinitely account type.
    - Patients account is a limited account type. (6 months)
    */

    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    //! Navigation property: An account type can have many accounts
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}

