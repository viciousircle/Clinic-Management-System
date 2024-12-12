using System.ComponentModel.DataAnnotations;

namespace OOP_Final_Project.Models;

public class Schedule
{
    /* 
    ! This model is used to define the schedule for employees.
    - Id: The unique identifier of the schedule.
    - Date: The day of the week (e.g., Monday, Tuesday).
    - TimeStart: The starting time of the schedule.
    - TimeEnd: The ending time of the schedule.

    ? Schedules help assign time slots to employees for their shifts or duties.
    */

    [Key]
    public int Id { get; set; }

    [Required]
    public DayOfWeek Date { get; set; }

    [Required]
    public required TimeSpan TimeStart { get; set; }

    [Required]
    public required TimeSpan TimeEnd { get; set; }

    // Navigation property: A schedule can have many employee schedules
    public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; } = new List<EmployeeSchedule>();
}
