using System;
using System.ComponentModel.DataAnnotations;


namespace OOP_Final_Project.Models;

public class Document
{
    [Key] // Đánh dấu thuộc tính này là khóa chính
    public int DocumentInternalId { get; set; }
    public string DocumentName { get; set; }
    public int DocumentTypeId { get; set; }
    public DateTime TimeCreated { get; set; }
    public string DocumentUrl { get; set; }
    public string Details { get; set; }
    public int? PatientId { get; set; }
    public int? PatientCaseId { get; set; }
    public int? AppointmentId { get; set; }
    public int? InDepartmentId { get; set; }
}

