using System;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

        //! DbSets for the models
        // public required DbSet<Role> Roles { get; set; }
        public required DbSet<AccountType> AccountTypes { get; set; }
        public required DbSet<Clinic> Clinics { get; set; }
        public required DbSet<Department> Departments { get; set; }
        public required DbSet<Employee> Employees { get; set; }
        public required DbSet<Patient> Patients { get; set; }
        public required DbSet<Account> Accounts { get; set; }
        public required DbSet<Schedule> Schedules { get; set; }
        public required DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public required DbSet<MedicineType> MedicineTypes { get; set; }
        public required DbSet<Medicine> Medicines { get; set; }
        public required DbSet<Prescription> Prescriptions { get; set; }
        public required DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; }
        public required DbSet<DocumentBill> DocumentBills { get; set; }
        public required DbSet<DocumentDiagnose> DocumentDiagnoses { get; set; }
        public required DbSet<DocumentAppointment> DocumentAppointments { get; set; }
        public required DbSet<DocumentCancel> DocumentCancels { get; set; }
        public required DbSet<DocumentPrescribe> DocumentPrescribes { get; set; }
        public required DbSet<Appointment> Appointments { get; set; }


        //! Fluent API configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);

                //- 1 - N relationship between AccountType and Account
                modelBuilder.Entity<Account>()
                        .HasOne(a => a.AccountType)
                        .WithMany(at => at.Accounts)
                        .HasForeignKey(a => a.AccountTypeId)
                        .OnDelete(DeleteBehavior.Cascade);

                // Define a unique constraint for the combination of UserName and Password
                modelBuilder.Entity<Account>()
                    .HasIndex(a => new { a.UserName, a.Password })
                    .IsUnique();

                //- 1 - N relationship between Department and Employee
                modelBuilder.Entity<Employee>()
                        .HasOne(e => e.Department)
                        .WithMany(d => d.Employees)
                        .HasForeignKey(e => e.DepartmentId)
                        .OnDelete(DeleteBehavior.Cascade);

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



                // //? Level 1

                // //! Seeding the clinics
                // var clinics = DataSeeder.SeedClinics(10); // Generate 10 fake clinics
                // modelBuilder.Entity<Clinic>().HasData(clinics);

                // //! Seeding the schedules
                // var schedules = DataSeeder.SeedSchedules();
                // modelBuilder.Entity<Schedule>().HasData(schedules);

                // //! Seeding the account types
                // var accountTypes = DataSeeder.SeedAccountTypes();
                // modelBuilder.Entity<AccountType>().HasData(accountTypes);

                // //! Seeding the medicine types
                // var medicineTypes = DataSeeder.SeedMedicineTypes();
                // modelBuilder.Entity<MedicineType>().HasData(medicineTypes);

                // //! Seeding the document types
                // var documentTypes = DataSeeder.SeedDocumentTypes();
                // modelBuilder.Entity<DocumentType>().HasData(documentTypes);

                // //! Seeding the patients



                // // ? Level 2

                // //! Seeding the departments
                // var departments = DataSeeder.SeedDepartments(clinics); // Generate departments for the seeded clinics
                // modelBuilder.Entity<Department>().HasData(departments);

                // //! Seeding the accounts
                // // var accounts = DataSeeder.SeedAccounts(accountTypes);
                // // modelBuilder.Entity<Account>().HasData(accounts);




        }


}
