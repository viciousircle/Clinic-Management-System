using System;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Models;
using OOP_Final_Project.Models.Identity;

namespace OOP_Final_Project.Data;

public class ApplicationDbContext : DbContext
{

        //! DbSets for the models
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public DbSet<MedicineType> MedicineTypes { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; }
        public DbSet<DocumentBill> DocumentBills { get; set; }
        public DbSet<DocumentDiagnose> DocumentDiagnoses { get; set; }
        public DbSet<DocumentAppointment> DocumentAppointments { get; set; }
        public DbSet<DocumentCancel> DocumentCancels { get; set; }
        public DbSet<DocumentPrescribe> DocumentPrescribes { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                AccountTypes = Set<AccountType>();
                DocumentTypes = Set<DocumentType>();
                MedicineTypes = Set<MedicineType>();
                Schedules = Set<Schedule>();
                Clinics = Set<Clinic>();
                Departments = Set<Department>();
                Accounts = Set<Account>();
                Patients = Set<Patient>();
                Employees = Set<Employee>();
                Appointments = Set<Appointment>();
                EmployeeSchedules = Set<EmployeeSchedule>();
                Medicines = Set<Medicine>();
                Prescriptions = Set<Prescription>();
                PrescriptionMedicines = Set<PrescriptionMedicine>();
                DocumentBills = Set<DocumentBill>();
                DocumentDiagnoses = Set<DocumentDiagnose>();
                DocumentAppointments = Set<DocumentAppointment>();
                DocumentCancels = Set<DocumentCancel>();
                DocumentPrescribes = Set<DocumentPrescribe>();
        }



        //! Fluent API configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);

                //! Define identity for ApplicationUser
                modelBuilder.Entity<ApplicationUser>()
                        .HasOne(u => u.AccountType)
                        .WithMany()
                        .HasForeignKey(u => u.AccountTypeId)
                        .OnDelete(DeleteBehavior.Restrict);

                //- 1 - N relationship between AccountType and Account
                modelBuilder.Entity<Account>()
                        .HasOne(a => a.AccountType)
                        .WithMany(at => at.Accounts)
                        .HasForeignKey(a => a.AccountTypeId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- Define a unique constraint for the combination of UserName and Password
                modelBuilder.Entity<Account>()
                    .HasIndex(a => new { a.UserName, a.Password })
                    .IsUnique();

                // //- 1 - N relationship between AccountType and Department
                modelBuilder.Entity<Department>()
                        .HasOne(d => d.AccountType)
                        .WithMany(e => e.Departments)
                        .HasForeignKey(d => d.AccountTypeId)
                        .OnDelete(DeleteBehavior.SetNull);

                //- 1 - N relationship between Employee and EmployeeSchedule
                modelBuilder.Entity<EmployeeSchedule>()
                        .HasOne(es => es.Employee)
                        .WithMany(e => e.EmployeeSchedules)
                        .HasForeignKey(es => es.EmployeeId)
                        .OnDelete(DeleteBehavior.SetNull);

                //- 1 - N relationship between Schedule and EmployeeSchedule
                modelBuilder.Entity<EmployeeSchedule>()
                        .HasOne(es => es.Schedule)
                        .WithMany(s => s.EmployeeSchedules)
                        .HasForeignKey(es => es.ScheduleId)
                        .OnDelete(DeleteBehavior.SetNull);


                //- 1 - N relationship between Clinic and Department
                modelBuilder.Entity<Department>()
                        .HasOne(d => d.Clinic)
                        .WithMany(c => c.Departments)
                        .HasForeignKey(d => d.ClinicId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between MedicineType and Medicine
                modelBuilder.Entity<Medicine>()
                        .HasOne(m => m.MedicineType)
                        .WithMany(mt => mt.Medicines)
                        .HasForeignKey(m => m.MedicineTypeId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between Importer and Medicine
                modelBuilder.Entity<Medicine>()
                        .HasOne(m => m.Importer)
                        .WithMany(i => i.Medicines)
                        .HasForeignKey(m => m.ImporterId)
                        .OnDelete(DeleteBehavior.SetNull);

                //- 1 - N relationship between Patient and Appointment
                modelBuilder.Entity<Appointment>()
                        .HasOne(a => a.Patient)
                        .WithMany(p => p.Appointments)
                        .HasForeignKey(a => a.PatientId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between Employee and Appointment
                modelBuilder.Entity<Appointment>()
                        .HasOne(a => a.Doctor)
                        .WithMany(e => e.Appointments)
                        .HasForeignKey(a => a.DoctorId)
                        .OnDelete(DeleteBehavior.SetNull);

                //- 1 - 1 relationship between Appointment and Prescription
                modelBuilder.Entity<Prescription>()
                        .HasOne(p => p.Appointment)
                        .WithOne(a => a.Prescription)
                        .HasForeignKey<Prescription>(p => p.AppointmentId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between Prescription and PrescriptionMedicine
                modelBuilder.Entity<PrescriptionMedicine>()
                        .HasOne(pm => pm.Prescription)
                        .WithMany(p => p.PrescriptionMedicines)
                        .HasForeignKey(pm => pm.PrescriptionId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between Medicine and PrescriptionMedicine
                modelBuilder.Entity<PrescriptionMedicine>()
                        .HasOne(pm => pm.Medicine)
                        .WithMany(m => m.PrescriptionMedicines)
                        .HasForeignKey(pm => pm.MedicineId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - 1 relationship between Appointment and DocumentAppointment
                modelBuilder.Entity<DocumentAppointment>()
                        .HasOne(da => da.Appointment)
                        .WithOne(a => a.DocumentAppointment)
                        .HasForeignKey<DocumentAppointment>(da => da.AppointmentId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - 1 relationship between Appointment and DocumentCancel
                modelBuilder.Entity<DocumentCancel>()
                        .HasOne(dc => dc.Appointment)
                        .WithOne(a => a.DocumentCancel)
                        .HasForeignKey<DocumentCancel>(dc => dc.AppointmentId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - 1 relationship between Appointment and DocumentDiagnose
                modelBuilder.Entity<DocumentDiagnose>()
                        .HasOne(dd => dd.Appointment)
                        .WithOne(a => a.DocumentDiagnose)
                        .HasForeignKey<DocumentDiagnose>(dd => dd.AppointmentId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - 1 relationship between Appointment and DocumentBill
                modelBuilder.Entity<DocumentBill>()
                        .HasOne(db => db.Appointment)
                        .WithOne(a => a.DocumentBill)
                        .HasForeignKey<DocumentBill>(db => db.AppointmentId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - 1 relationship between Prescription and DocumentPrescribe
                modelBuilder.Entity<DocumentPrescribe>()
                        .HasOne(dp => dp.Prescription)
                        .WithOne(p => p.DocumentPrescribe)
                        .HasForeignKey<DocumentPrescribe>(dp => dp.PrescriptionId)
                        .OnDelete(DeleteBehavior.Cascade);


                //- 1 - N relationship between DocumentType and DocumentBill
                modelBuilder.Entity<DocumentBill>()
                        .HasOne(db => db.DocumentType)
                        .WithMany(dt => dt.DocumentBills)
                        .HasForeignKey(db => db.DocumentTypeId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between DocumentType and DocumentDiagnose
                modelBuilder.Entity<DocumentDiagnose>()
                        .HasOne(dd => dd.DocumentType)
                        .WithMany(dt => dt.DocumentDiagnoses)
                        .HasForeignKey(dd => dd.DocumentTypeId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between DocumentType and DocumentAppointment
                modelBuilder.Entity<DocumentAppointment>()
                        .HasOne(da => da.DocumentType)
                        .WithMany(dt => dt.DocumentAppointments)
                        .HasForeignKey(da => da.DocumentTypeId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between DocumentType and DocumentCancel
                modelBuilder.Entity<DocumentCancel>()
                        .HasOne(dc => dc.DocumentType)
                        .WithMany(dt => dt.DocumentCancels)
                        .HasForeignKey(dc => dc.DocumentTypeId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between DocumentType and DocumentPrescribe
                modelBuilder.Entity<DocumentPrescribe>()
                        .HasOne(dp => dp.DocumentType)
                        .WithMany(dt => dt.DocumentPrescribes)
                        .HasForeignKey(dp => dp.DocumentTypeId)
                        .OnDelete(DeleteBehavior.Cascade);

                //- 1 - N relationship between Pharmacist and DocumentPrescribe
                modelBuilder.Entity<DocumentPrescribe>()
                        .HasOne(dp => dp.Pharmacist)
                        .WithMany(p => p.DocumentPrescribes)
                        .HasForeignKey(dp => dp.PharmacistId)
                        .OnDelete(DeleteBehavior.SetNull);

                //- 1 - N relationship between Receptionist and DocumentBill
                modelBuilder.Entity<DocumentBill>()
                        .HasOne(db => db.Receptionist)
                        .WithMany(r => r.DocumentBills)
                        .HasForeignKey(db => db.ReceptionistId)
                        .OnDelete(DeleteBehavior.SetNull);

                //- 1 - 1 relationship between Patient and Account
                modelBuilder.Entity<Patient>()
                    .HasOne(p => p.Account)
                    .WithOne(a => a.Patient)
                    .HasForeignKey<Patient>(p => p.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                //- 1 - 1 relationship between Employee and Account
                modelBuilder.Entity<Employee>()
                    .HasOne(e => e.Account)
                    .WithOne(a => a.Employee)
                    .HasForeignKey<Employee>(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

        }


}
