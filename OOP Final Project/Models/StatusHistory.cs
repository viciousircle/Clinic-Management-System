using System;

namespace OOP_Final_Project.Models;

public class StatusHistory
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public int AppointmentStatusId { get; set; }
    public DateTime StatusTime { get; set; }
    public string Details { get; set; }
}

