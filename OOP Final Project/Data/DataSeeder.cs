using System;
using Bogus;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.Data;

public class DataSeeder
{

    // ? Level 1

    //! Create a Faker instance for Clinic
    public static List<Clinic> SeedClinics(int count)
    {
        var faker = new Faker<Clinic>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1) // Auto-increment Id
            .RuleFor(c => c.Name, f => f.Company.CompanyName()) // Random company name
            .RuleFor(c => c.Address, f => f.Address.FullAddress()) // Random address
            .RuleFor(c => c.Details, f => f.Lorem.Paragraph()); // Random details

        // Generate the specified number of clinics
        return faker.Generate(count);
    }

    //! Create a Faker instance for Role
    // public static List<Role> SeedRoles()
    // {
    //     // Explicitly defining the roles
    //     var roles = new List<Role>
    //     {
    //         new Role { Id = 1, Name = "Admin" },
    //         new Role { Id = 2, Name = "Manager" },
    //         new Role { Id = 3, Name = "Doctor" },
    //         new Role { Id = 4, Name = "Receptionist" },
    //         new Role { Id = 5, Name = "Pharmacist" }
    //     };

    //     return roles;
    // }

    //! Create a Faker instance for Schedule
    public static List<Schedule> SeedSchedules()
    {
        // Explicitly defining the schedules
        var schedules = new List<Schedule>
        {
           new Schedule
            {
                Id = 1,
                Date = DayOfWeek.Monday,
                TimeStart = new TimeSpan(8, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(11, 0, 0)   // 11:00 AM
            },
           new Schedule
            {
                Id = 2,
                Date = DayOfWeek.Monday,
                TimeStart = new TimeSpan(1, 0, 0),  // 1:00 PM
                TimeEnd = new TimeSpan(17, 0, 0)   // 5:00 PM
            },
            new Schedule
                {
                Id = 3,
                Date = DayOfWeek.Tuesday,
                TimeStart = new TimeSpan(8, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(11, 0, 0)   // 11:00 AM
                },
            new Schedule
                {
                Id = 4,
                Date = DayOfWeek.Tuesday,
                TimeStart = new TimeSpan(1, 0, 0),  // 1:00 PM
                TimeEnd = new TimeSpan(17, 0, 0)   // 5:00 PM
                },
            new Schedule
                {
                Id = 5,
                Date = DayOfWeek.Wednesday,
                TimeStart = new TimeSpan(8, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(11, 0, 0)   // 11:00 AM
                },
            new Schedule
                {
                Id = 6,
                Date = DayOfWeek.Wednesday,
                TimeStart = new TimeSpan(1, 0, 0),  // 1:00 PM
                TimeEnd = new TimeSpan(17, 0, 0)   // 5:00 PM
                },
            new Schedule
                {
                Id = 7,
                Date = DayOfWeek.Thursday,
                TimeStart = new TimeSpan(8, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(11, 0, 0)   // 11:00 AM
                },
            new Schedule
                {
                Id = 8,
                Date = DayOfWeek.Thursday,
                TimeStart = new TimeSpan(1, 0, 0),  // 1:00 PM
                TimeEnd = new TimeSpan(17, 0, 0)   // 5:00 PM
                },
            new Schedule
                {
                Id = 9,
                Date = DayOfWeek.Friday,
                TimeStart = new TimeSpan(8, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(11, 0, 0)   // 11:00 AM
                },
            new Schedule
                {
                Id = 10,
                Date = DayOfWeek.Friday,
                TimeStart = new TimeSpan(1, 0, 0),  // 1:00 PM
                TimeEnd = new TimeSpan(17, 0, 0)   // 5:00 PM
                },

            new Schedule
                {
                Id = 11,
                Date = DayOfWeek.Monday,
                TimeStart = new TimeSpan(9, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(12, 0, 0)   // 12:00 PM
                },

            new Schedule
                {
                Id = 12,
                Date = DayOfWeek.Monday,
                TimeStart = new TimeSpan(2, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(6, 0, 0)   // 12:00 PM
                },

            new Schedule
                {
                Id = 13,
                Date = DayOfWeek.Tuesday,
                TimeStart = new TimeSpan(9, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(12, 0, 0)   // 12:00 PM
                },
            new Schedule
                {
                Id = 14,
                Date = DayOfWeek.Tuesday,
                TimeStart = new TimeSpan(2, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(6, 0, 0)   // 12:00 PM
                },

            new Schedule
                {
                Id = 15,
                Date = DayOfWeek.Wednesday,
                TimeStart = new TimeSpan(9, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(12, 0, 0)   // 12:00 PM
                },
            new Schedule
                {
                Id = 16,
                Date = DayOfWeek.Wednesday,
                TimeStart = new TimeSpan(2, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(6, 0, 0)   // 12:00 PM
                },

            new Schedule
                {
                Id = 17,
                Date = DayOfWeek.Thursday,
                TimeStart = new TimeSpan(9, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(12, 0, 0)   // 12:00 PM
                },
            new Schedule
                {
                Id = 18,
                Date = DayOfWeek.Thursday,
                TimeStart = new TimeSpan(2, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(6, 0, 0)   // 12:00 PM
                },

            new Schedule
                {
                Id = 19,
                Date = DayOfWeek.Friday,
                TimeStart = new TimeSpan(9, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(12, 0, 0)   // 12:00 PM
                },

            new Schedule
                {
                Id = 20,
                Date = DayOfWeek.Friday,
                TimeStart = new TimeSpan(2, 0, 0),  // 9:00 AM
                TimeEnd = new TimeSpan(6, 0, 0)   // 12:00 PM
                },

        };

        return schedules;
    }

    //! Create a Faker instance for AccountType
    public static List<AccountType> SeedAccountTypes()
    {
        // Explicitly defining the account types
        var accountTypes = new List<AccountType>
        {
            new AccountType { Id = 1, Name = "Indefinitely" },
            new AccountType { Id = 2, Name = "Limited" }
        };

        return accountTypes;
    }

    //! Create a Faker instance for MedicineType
    public static List<MedicineType> SeedMedicineTypes()
    {
        // Explicitly defining the medicine types
        var medicineTypes = new List<MedicineType>
        {
            new MedicineType { Id = 1, Name = "Tablet" },
            new MedicineType { Id = 2, Name = "Capsule" },
            new MedicineType { Id = 3, Name = "Syrup" },
            new MedicineType { Id = 4, Name = "Injection" },
            new MedicineType { Id = 5, Name = "Cream" },
            new MedicineType { Id = 6, Name = "Ointment" }
        };

        return medicineTypes;
    }

    //! Create a Faker instance for DocumentType
    public static List<DocumentType> SeedDocumentTypes()
    {
        // Explicitly defining the document types
        var documentTypes = new List<DocumentType>
        {
            new DocumentType { Id = 1, Name = "Appointment" },
            new DocumentType { Id = 2, Name = "Diagnosis" },
            new DocumentType { Id = 3, Name = "Prescription" },
            new DocumentType { Id = 4, Name = "Bill" },
            new DocumentType { Id = 5, Name = "Cancel" }
        };

        return documentTypes;
    }

    //! Create a Faker instance for Patient

    // ? Level 2

    //! Create a Faker instance for Department
    public static List<Department> SeedDepartments(List<Clinic> clinics)
    {
        // Predefined department names
        var departmentNames = new List<string> { "Reception", "Dental", "Medical", "Management" };
        var departments = new List<Department>();

        int departmentId = 1; // Start department ID counter

        foreach (var clinic in clinics)
        {
            foreach (var name in departmentNames)
            {
                departments.Add(new Department
                {
                    Id = departmentId++,
                    ClinicId = clinic.Id,
                    Clinic = clinic,
                    Name = name
                });
            }
        }

        return departments;
    }

    //! Create a Faker instance for Account
    // public static List<Account> SeedAccounts(List<Clinic> clinics, List<Role> roles)
    // {
    //     var accounts = new List<Account>();
    //     var faker = new Faker();
    //     int accountId = 1;

    //     foreach (var clinic in clinics)
    //     {
    //         // Add 1 Manager
    //         accounts.Add(new Account
    //         {
    //             Id = accountId++,
    //             UserName = faker.Internet.UserName(),
    //             Password = faker.Internet.Password(8),
    //             AccountTypeId = faker.Random.Number(1, 2), // Random AccountTypeId (e.g., 1 or 2)
    //             RoleId = roles.First(r => r.Name == "Manager").Id,
    //             ClinicId = clinic.Id
    //         });

    //         // Add 2 Receptionists
    //         for (int i = 0; i < 2; i++)
    //         {
    //             accounts.Add(new Account
    //             {
    //                 Id = accountId++,
    //                 UserName = faker.Internet.UserName(),
    //                 Password = faker.Internet.Password(8),
    //                 AccountTypeId = faker.Random.Number(1, 2),
    //                 RoleId = roles.First(r => r.Name == "Receptionist").Id,
    //                 ClinicId = clinic.Id
    //             });
    //         }

    //         // Add 2 Pharmacists
    //         for (int i = 0; i < 2; i++)
    //         {
    //             accounts.Add(new Account
    //             {
    //                 Id = accountId++,
    //                 UserName = faker.Internet.UserName(),
    //                 Password = faker.Internet.Password(8),
    //                 AccountTypeId = faker.Random.Number(1, 2),
    //                 RoleId = roles.First(r => r.Name == "Pharmacist").Id,
    //                 ClinicId = clinic.Id
    //             });
    //         }

    //         // Add 5 Doctors
    //         for (int i = 0; i < 5; i++)
    //         {
    //             accounts.Add(new Account
    //             {
    //                 Id = accountId++,
    //                 UserName = faker.Internet.UserName(),
    //                 Password = faker.Internet.Password(8),
    //                 AccountTypeId = faker.Random.Number(1, 2),
    //                 RoleId = roles.First(r => r.Name == "Doctor").Id,
    //                 ClinicId = clinic.Id
    //             });
    //         }
    //     }

    //     return accounts;
    // }



}
