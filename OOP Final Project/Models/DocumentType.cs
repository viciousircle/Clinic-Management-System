using System.ComponentModel.DataAnnotations;

namespace OOP_Final_Project.Models;

public class DocumentType
{
    /* 
    ! This model is used to define the types of documents in the system.
    - Id: The unique identifier of the document type.
    - Name: The name of the document type. There are 4 main different types of documents in the system.
        - 1: Appointment Record
        - 2: Diagnosis
        - 3: Prescription
        - 4: Bill

        - Additional type is Cancelled Appointment document.

    ? Document types categorize the different documents in the system.
    
    ? Why is this important?
    - This is important because it allows the system to categorize the documents and apply different rules to each type.
    */

    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }


}
