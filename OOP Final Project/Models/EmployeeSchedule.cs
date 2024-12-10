using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_Final_Project.Models;

public class EmployeeSchedule
{
    /* 
    ! This model is used to assign schedules to employees.
    - ScheduleId: The foreign key linking to the schedule.
    - EmployeeId: The foreign key linking to the employee.
    - TimeFrom: The starting time of the employee's shift.
    - TimeTo: The ending time of the employee's shift.
    - IsActive: Indicates whether the schedule is active or not.
    - WorkLocation: The location where the employee will work.

    ? An employee can have one or more schedules.
    */

    [Key]
    public int Id { get; set; }

    [ForeignKey("Schedule")]
    public int ScheduleId { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Required]
    public DateTime TimeFrom { get; set; }
    [Required]
    public DateTime TimeTo { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public required string WorkLocation { get; set; }

    public required virtual Schedule Schedule { get; set; }
    public required virtual Employee Employee { get; set; }


}
