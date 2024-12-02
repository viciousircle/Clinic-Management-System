using System;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Models;


namespace OOP_Final_Project.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }

    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<HasRole> HasRoles { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<InDepartment> InDepartments { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<PatientCase> PatientCases { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<StatusHistory> StatusHistories { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<AppointmentStatus> AppointmentStatuses { get; set; }

}
