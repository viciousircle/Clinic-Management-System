using System;
using Bogus;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.Data;

// Helper method to truncate fractional seconds
public static class DateTimeExtensions
{
    public static DateTime TruncateSeconds(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
    }
}

public class DataSeeder
{

    // ? Level 1

    //! Create a Faker instance for AccountType
    public static List<AccountType> SeedAccountTypes()
    {
        // Explicitly defining the account types
        var accountTypes = new List<AccountType>
        {
            new AccountType { Id = 1, Name = "Manager" },
            new AccountType { Id = 2, Name = "Receptionist" },
            new AccountType { Id = 3, Name = "Pharmacist" },
            new AccountType { Id = 4, Name = "Doctor" },
            new AccountType { Id = 5, Name = "Patient" },
        };

        return accountTypes;
    }

    //! Create a Faker instance for Clinic
    public static List<Clinic> SeedClinics(int count)
    {
        var faker = new Faker<Clinic>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Name, (f, c) => $"Clinic {c.Id}")
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.Details, f => f.Lorem.Paragraph());

        return faker.Generate(count);
    }

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

    // ? Level 2

    //! Create a Faker instance for Department
    public static List<Department> SeedDepartments(List<Clinic> clinics, List<AccountType> accountTypes)
    {
        var departments = new List<Department>();
        int idCounter = 1;

        foreach (var clinic in clinics)
        {
            // Create a department for each of the first 4 account types
            foreach (var accountType in accountTypes.Where(at => at.Id >= 1 && at.Id <= 4))
            {
                departments.Add(new Department
                {
                    Id = idCounter++,
                    ClinicId = clinic.Id,
                    AccountTypeId = accountType.Id,
                    Clinic = clinic,
                    AccountType = accountType
                });
            }
        }

        return departments;
    }


    //! Create a Faker instance for Account

    public static List<Account> SeedAccounts(List<AccountType> accountTypes, List<Clinic> clinics, int patientCount)
    {
        var accounts = new List<Account>();
        int idCounter = 1;

        // - Clinic Accounts
        foreach (var clinic in clinics)
        {
            accounts.Add(new Account
            {
                Id = idCounter++,
                AccountTypeId = 1,
                AccountType = accountTypes.First(at => at.Id == 1),
                UserName = $"{clinic.Name}_manager",
                Password = "Manager123",
                CreateDate = DateTime.Now.AddDays(-new Random().Next(1, 365)).TruncateSeconds() // Helper function to truncate seconds
            });

            accounts.AddRange(Enumerable.Range(0, 2).Select(_ => new Account
            {
                Id = idCounter++,
                AccountTypeId = 2,
                AccountType = accountTypes.First(at => at.Id == 2),
                UserName = $"{clinic.Name}_receptionist{_ + 1}",
                Password = $"Receptionist{_ + 1}",
                CreateDate = DateTime.Now.AddDays(-new Random().Next(1, 365)).TruncateSeconds()
            }));

            accounts.AddRange(Enumerable.Range(0, 2).Select(_ => new Account
            {
                Id = idCounter++,
                AccountTypeId = 3,
                AccountType = accountTypes.First(at => at.Id == 3),
                UserName = $"{clinic.Name}_pharmacist{_ + 1}",
                Password = $"Pharmacist{_ + 1}",
                CreateDate = DateTime.Now.AddDays(-new Random().Next(1, 365)).TruncateSeconds()
            }));

            accounts.AddRange(Enumerable.Range(0, 5).Select(_ => new Account
            {
                Id = idCounter++,
                AccountTypeId = 4,
                AccountType = accountTypes.First(at => at.Id == 4),
                UserName = $"{clinic.Name}_doctor{_ + 1}",
                Password = $"Doctor{_ + 1}",
                CreateDate = DateTime.Now.AddDays(-new Random().Next(1, 365)).TruncateSeconds()
            }));
        }

        // - Patient Accounts
        var faker = new Faker<Account>()
            .RuleFor(a => a.Id, f => idCounter++)
            .RuleFor(a => a.AccountTypeId, f => 5) // 5 represents Patient account type
            .RuleFor(a => a.UserName, f => f.Internet.UserName())
            .RuleFor(a => a.Password, f => f.Internet.Password(8))
            .RuleFor(a => a.CreateDate, f => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now).TruncateSeconds());

        accounts.AddRange(faker.Generate(patientCount)); // Add patient accounts

        return accounts;
    }


    // //! Create a Faker instance for Patient
    public static List<Patient> SeedPatients(List<Clinic> clinics, List<Account> accounts)
    {
        // Filter accounts with AccountTypeId == 5
        var validAccounts = accounts.Where(a => a.AccountTypeId == 5).ToList();

        if (!validAccounts.Any())
        {
            Console.WriteLine("No valid accounts available for patient assignment.");
            return new List<Patient>();
        }

        var faker = new Faker<Patient>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.Email, (f, p) => f.Internet.Email(p.FirstName, p.LastName))
            .RuleFor(p => p.Phone, f => f.Random.ReplaceNumbers("###-###-####")) // Custom phone number format
            .RuleFor(p => p.Address, f => f.Address.FullAddress());

        // Generate one patient for each valid account
        var patients = validAccounts.Select(account =>
            faker
                .RuleFor(p => p.AccountId, _ => account.Id) // Assign the AccountId directly
                .Generate()
        ).ToList();

        Console.WriteLine($"{patients.Count} patients generated, matching accounts with AccountTypeId == 5.");
        return patients;
    }



    //! Create a Faker instance for Employee
    public static List<Employee> SeedEmployees(
        List<Account> accounts,
        List<Clinic> clinics,
        List<Department> departments)
    {
        var employees = new List<Employee>();
        var faker = new Faker();

        int idCounter = 1;

        foreach (var clinic in clinics)
        {
            var clinicDepartments = departments.Where(d => d.ClinicId == clinic.Id).ToList();

            // Manager
            var managerAccount = accounts.FirstOrDefault(a => a.UserName == $"{clinic.Name}_manager");
            var managementDepartment = clinicDepartments.FirstOrDefault(d => d.AccountTypeId == 1);

            if (managerAccount != null && managementDepartment != null)
            {
                employees.Add(new Employee
                {
                    Id = idCounter++,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    AccountId = managerAccount.Id,
                    Account = managerAccount,
                    Email = $"{clinic.Name.ToLower()}_manager@clinic.com",
                    Phone = faker.Random.ReplaceNumbers("###-###-####"), // Custom phone format
                    IsActive = true
                });
            }

            // Receptionists
            for (int i = 1; i <= 2; i++)
            {
                var receptionistAccount = accounts.FirstOrDefault(a => a.UserName == $"{clinic.Name}_receptionist{i}");
                var receptionistDepartment = clinicDepartments.FirstOrDefault(d => d.AccountTypeId == 2);

                if (receptionistAccount != null && receptionistDepartment != null)
                {
                    employees.Add(new Employee
                    {
                        Id = idCounter++,
                        FirstName = faker.Name.FirstName(),
                        LastName = faker.Name.LastName(),
                        AccountId = receptionistAccount.Id,
                        Account = receptionistAccount,
                        Email = $"{clinic.Name.ToLower()}_receptionist{i}@clinic.com",
                        Phone = faker.Random.ReplaceNumbers("###-###-####"), // Custom phone format
                        IsActive = true
                    });
                }
            }

            // Pharmacists
            for (int i = 1; i <= 2; i++)
            {
                var pharmacistAccount = accounts.FirstOrDefault(a => a.UserName == $"{clinic.Name}_pharmacist{i}");
                var pharmacyDepartment = clinicDepartments.FirstOrDefault(d => d.AccountTypeId == 3);

                if (pharmacistAccount != null && pharmacyDepartment != null)
                {
                    employees.Add(new Employee
                    {
                        Id = idCounter++,
                        FirstName = faker.Name.FirstName(),
                        LastName = faker.Name.LastName(),
                        AccountId = pharmacistAccount.Id,
                        Account = pharmacistAccount,
                        Email = $"{clinic.Name.ToLower()}_pharmacist{i}@clinic.com",
                        Phone = faker.Random.ReplaceNumbers("###-###-####"), // Custom phone format
                        IsActive = true
                    });
                }
            }

            // Doctors
            for (int i = 1; i <= 5; i++)
            {
                var doctorAccount = accounts.FirstOrDefault(a => a.UserName == $"{clinic.Name}_doctor{i}");
                var medicalDepartment = clinicDepartments.FirstOrDefault(d => d.AccountTypeId == 4);

                if (doctorAccount != null && medicalDepartment != null)
                {
                    employees.Add(new Employee
                    {
                        Id = idCounter++,
                        FirstName = faker.Name.FirstName(),
                        LastName = faker.Name.LastName(),
                        AccountId = doctorAccount.Id,
                        Account = doctorAccount,
                        Email = $"{clinic.Name.ToLower()}_doctor{i}@clinic.com",
                        Phone = faker.Random.ReplaceNumbers("###-###-####"), // Custom phone format
                        IsActive = true
                    });
                }
            }
        }

        return employees;
    }


    //! Create a Faker instance for Medicine
    public static List<Medicine> SeedMedicines(List<MedicineType> medicineTypes, List<Employee> employees, List<Account> accounts, int medicineCount)
    {
        // Filter the list of employees to only include pharmacists with valid Account and Department
        var pharmacistAccounts = accounts
            .Where(a => a.AccountTypeId == 3) // AccountTypeId 3 corresponds to "Pharmacist"
            .ToList();

        // Filter employees to only include those with a pharmacist account
        var pharmacists = employees
            .Where(e => pharmacistAccounts.Any(a => a.Id == e.AccountId))
            .ToList();

        // Check if any pharmacists are found
        if (!pharmacists.Any())
        {
            Console.WriteLine("No pharmacists found.");
            return new List<Medicine>(); // Return an empty list if no pharmacists are found
        }

        // Generate medicines using Faker
        var faker = new Faker<Medicine>()
            .RuleFor(m => m.Id, f => f.IndexFaker + 1) // Auto-increment Id
            .RuleFor(m => m.Name, f => f.Commerce.ProductName().Split(' ').First() + "ine") // Medicine-like name
            .RuleFor(m => m.MedicineTypeId, f => f.PickRandom(medicineTypes).Id) // Random medicine type
            .RuleFor(m => m.ImporterId, f => f.PickRandom(pharmacists).AccountId) // Random pharmacist as importer
            .RuleFor(m => m.ExpiredDate, f => f.Date.Future(2).Date) // Future expiration date (yyyy-MM-dd format)
            .RuleFor(m => m.ImportDate, f => f.Date.Past(1).Date) // Past import date (yyyy-MM-dd format)
            .RuleFor(m => m.Quantity, f => f.Random.Int(1, 500)); // Random quantity between 1 and 500

        // Generate the specified number of medicines
        return faker.Generate(medicineCount);
    }

    //! Create a Faker instance for Appointment
    public static List<Appointment> SeedAppointments(List<Employee> employees, List<Patient> patients, List<Account> accounts)
    {
        // Debug: Log all doctors
        foreach (var employee in employees)
        {
            var accountType = accounts.FirstOrDefault(a => a.Id == employee.AccountId)?.UserName;
            Console.WriteLine($"Employee ID: {employee.Id}, Account Type: {accountType ?? "Unknown"}");
        }

        // Filter employees to only include doctors based on AccountTypeId
        var doctorAccounts = accounts
            .Where(a => a.AccountTypeId == 4) // Assuming AccountTypeId 4 corresponds to "Doctor"
            .ToList();

        var validDoctors = employees
            .Where(e => doctorAccounts.Any(a => a.Id == e.AccountId))
            .ToList();

        if (!validDoctors.Any())
        {
            Console.WriteLine("No valid doctors found for appointments.");
            return new List<Appointment>(); // Return an empty list if no valid doctors
        }

        if (!patients.Any())
        {
            Console.WriteLine("No patients found for appointments.");
            return new List<Appointment>(); // Return an empty list if no patients
        }

        var faker = new Faker<Appointment>()
            .RuleFor(a => a.Id, f => f.IndexFaker + 1)
            .RuleFor(a => a.DoctorId, f => f.PickRandom(validDoctors).Id) // Random doctor from valid doctors
            .RuleFor(a => a.PatientId, f => f.PickRandom(patients).Id);   // Random patient

        return faker.Generate(7000); // Generate 1000 appointments
    }


    //! Create a Faker instance for EmployeeSchedule
    public static List<EmployeeSchedule> SeedEmployeeSchedules(List<Schedule> schedules, List<Employee> employees, List<Account> accounts)
    {
        var faker = new Faker<EmployeeSchedule>();
        var employeeSchedules = new List<EmployeeSchedule>();
        var roomNumber = 201;

        foreach (var employee in employees)
        {
            var account = accounts.FirstOrDefault(a => a.Id == employee.AccountId);

            // Assign location
            var location = account.AccountTypeId == 2 ? "Lobby" : $"Room {roomNumber++}";

            // Randomly select one schedule for the employee to be active
            var activeSchedule = faker.RuleFor(es => es.ScheduleId, f => f.PickRandom(schedules).Id)
                                      .RuleFor(es => es.EmployeeId, f => employee.Id)
                                      .RuleFor(es => es.TimeFrom, (f, es) => f.Date.Past(1))  // Generate the date
                                      .RuleFor(es => es.TimeTo, (f, es) => f.Date.Future(1))  // Generate the date
                                      .RuleFor(es => es.IsActive, f => true) // Active schedule
                                      .RuleFor(es => es.WorkLocation, f => location)
                                      .Generate(1)
                                      .First();

            // Format the dates to yyyy-MM-dd after they are generated
            activeSchedule.TimeFrom = DateTime.Parse(activeSchedule.TimeFrom.ToString("yyyy-MM-dd"));
            activeSchedule.TimeTo = DateTime.Parse(activeSchedule.TimeTo.ToString("yyyy-MM-dd"));

            employeeSchedules.Add(activeSchedule);

            // For the remaining schedules, set them as inactive
            var inactiveSchedules = faker.RuleFor(es => es.ScheduleId, f => f.PickRandom(schedules.Where(s => s.Id != activeSchedule.ScheduleId)).Id)
                                          .RuleFor(es => es.EmployeeId, f => employee.Id)
                                          .RuleFor(es => es.TimeFrom, (f, es) => f.Date.Past(1))  // Generate the date
                                          .RuleFor(es => es.TimeTo, (f, es) => f.Date.Future(1))  // Generate the date
                                          .RuleFor(es => es.IsActive, f => false) // Inactive schedule
                                          .RuleFor(es => es.WorkLocation, f => location)
                                          .Generate(schedules.Count - 1);

            // Format the dates to yyyy-MM-dd after they are generated
            foreach (var schedule in inactiveSchedules)
            {
                schedule.TimeFrom = DateTime.Parse(schedule.TimeFrom.ToString("yyyy-MM-dd"));
                schedule.TimeTo = DateTime.Parse(schedule.TimeTo.ToString("yyyy-MM-dd"));
            }

            employeeSchedules.AddRange(inactiveSchedules);
        }

        return employeeSchedules;
    }


    //! Create a Faker instance for DocumentCancel
    public static List<DocumentCancel> SeedDocumentCancels(List<Appointment> appointments)
    {
        var faker = new Faker<DocumentCancel>()
            .RuleFor(dc => dc.Id, f => f.IndexFaker + 1)
            .RuleFor(dc => dc.DocumentTypeId, f => 5)
            .RuleFor(dc => dc.AppointmentId, f => f.PickRandom(appointments).Id)
            .RuleFor(dc => dc.Reason, f => GenerateReason(f))
            .RuleFor(dc => dc.TimeCancel, f => f.Date.Recent().TruncateSeconds()); // Truncate the time part

        return faker.Generate(50);
    }

    private static string GenerateReason(Faker f)
    {
        // Create a sentence with random words
        var sentence = f.Lorem.Sentence(f.Random.Int(1, 200)); // Generate a sentence with up to 200 words
        var words = sentence.Split(' ');

        // If the sentence has more than 200 words, limit it
        if (words.Length > 200)
        {
            sentence = string.Join(" ", words.Take(200));
        }

        return sentence;
    }


    //! Create a Faker instance for Prescription
    public static List<Prescription> SeedPrescriptions(List<Appointment> appointments, List<DocumentCancel> documentCancels)
    {
        // Filter out appointments that have a corresponding cancellation
        var nonCancelledAppointments = appointments
            .Where(a => !documentCancels.Any(dc => dc.AppointmentId == a.Id)) // Exclude cancelled appointments
            .ToList();

        Console.WriteLine($"Non-cancelled appointments: {nonCancelledAppointments.Count}");

        // Check if there are any non-cancelled appointments
        if (!nonCancelledAppointments.Any())
        {
            Console.WriteLine("No valid appointments found for prescriptions.");
            return new List<Prescription>(); // Return an empty list if no valid appointments
        }

        // Generate exactly 6000 prescriptions using Faker
        var faker = new Faker<Prescription>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.AppointmentId, f => f.PickRandom(nonCancelledAppointments).Id);

        var prescriptions = faker.Generate(6000);

        return prescriptions; // Return the generated prescriptions
    }



    // ? Level 4


    //! Create a Faker instance for PrescriptionMedicine
    public static List<PrescriptionMedicine> SeedPrescriptionMedicines(List<Prescription> prescriptions, List<Medicine> medicines)
    {
        var faker = new Faker<PrescriptionMedicine>()
            .RuleFor(pm => pm.PrescriptionId, f => f.PickRandom(prescriptions).Id)
            .RuleFor(pm => pm.MedicineId, f => f.PickRandom(medicines).Id)
            .RuleFor(pm => pm.DosageAmount, f => f.Random.Int(1, 5).ToString())
            .RuleFor(pm => pm.Frequency, f => f.Random.Int(1, 4).ToString())
            .RuleFor(pm => pm.FrequencyUnit, f => f.PickRandom(new[] { "day", "week", "hour" }))
            .RuleFor(pm => pm.Route, f => f.PickRandom(new[] { "oral", "injection", "topical" }))
            .RuleFor(pm => pm.Instructions, f => f.Lorem.Sentence());
        return faker.Generate(2000);
    }

    // ? Level 5

    //! Create a Faker instance for DocumentAppointment
    public static List<DocumentAppointment> SeedDocumentAppointments(List<Appointment> appointments, List<EmployeeSchedule> employeeSchedules)
    {
        var faker = new Faker<DocumentAppointment>()
            .RuleFor(da => da.Id, f => f.IndexFaker + 1)
            .RuleFor(da => da.DocumentTypeId, f => 1) // 1 represents Appointment document type
            .RuleFor(da => da.AppointmentId, f => f.PickRandom(appointments).Id)
            .RuleFor(da => da.Date, f => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1))) // Random date
            .RuleFor(da => da.TimeBook, (f, da) => f.Date.Between(da.Date.AddDays(-7), da.Date.AddMinutes(30))) // Random TimeBook
            .RuleFor(da => da.Location, f => f.Address.FullAddress());

        var documentAppointments = faker.Generate(1300);

        foreach (var docAppointment in documentAppointments)
        {
            // Safely retrieve the doctor associated with the appointment
            var appointment = appointments.FirstOrDefault(a => a.Id == docAppointment.AppointmentId);
            if (appointment == null || appointment.DoctorId == 0)
            {
                // Handle missing appointment or doctor gracefully
                Console.WriteLine($"No valid appointment or doctor found for DocumentAppointment Id {docAppointment.Id}");
                continue;
            }

            // Get the doctor's schedule for the day
            var doctorSchedule = employeeSchedules
                .FirstOrDefault(es => es.EmployeeId == appointment.DoctorId && es.Schedule?.Date == docAppointment.Date.DayOfWeek);

            if (doctorSchedule != null && doctorSchedule.Schedule != null)
            {
                // Generate TimeStart and TimeEnd within the doctor's working hours
                var startTime = doctorSchedule.Schedule.TimeStart;
                var endTime = doctorSchedule.Schedule.TimeEnd;

                if (startTime < endTime) // Ensure the time range is valid
                {
                    var timeSpan = new Faker().Date.Between(
                        DateTime.Today.Add(startTime),
                        DateTime.Today.Add(endTime));
                    docAppointment.TimeStart = timeSpan.TimeOfDay;
                    docAppointment.TimeEnd = docAppointment.TimeStart.Add(TimeSpan.FromHours(1)); // 1-hour duration
                }
            }
            else
            {
                // Default TimeStart and TimeEnd when no schedule is found
                Console.WriteLine($"No schedule found for DoctorId {appointment.DoctorId} on {docAppointment.Date.DayOfWeek}");
                var dateFaker = new Faker();
                var randomTime = dateFaker.Date.Soon().TimeOfDay;
                docAppointment.TimeStart = randomTime;
                docAppointment.TimeEnd = randomTime.Add(TimeSpan.FromHours(1));
            }
        }

        return documentAppointments;
    }



    //! Create a Faker instance for DocumentDiagnosis
    public static List<DocumentDiagnose> SeedDocumentDiagnoses(List<Appointment> appointments)
    {
        var faker = new Faker<DocumentDiagnose>()
            .RuleFor(dd => dd.Id, f => f.IndexFaker + 1)
            .RuleFor(dd => dd.DocumentTypeId, f => 2)
            .RuleFor(dd => dd.AppointmentId, f => f.PickRandom(appointments).Id)
            .RuleFor(dd => dd.PatientStatus, f => f.PickRandom(new[] { "stable", "critical", "recovering" }))
            .RuleFor(dd => dd.IsSick, f => f.Random.Bool())
            .RuleFor(dd => dd.DiagnoseDetails, f => f.Lorem.Paragraph());
        return faker.Generate(400);
    }


    //! Create a Faker instance for DocumentPrescribe
    public static List<DocumentPrescribe> SeedDocumentPrescribes(List<Prescription> prescriptions, List<Employee> pharmacists)
    {
        var faker = new Faker<DocumentPrescribe>()
            .RuleFor(dp => dp.Id, f => f.IndexFaker + 1)
            .RuleFor(dp => dp.DocumentTypeId, f => 3)
            .RuleFor(dp => dp.PrescriptionId, f => f.PickRandom(prescriptions).Id)
            .RuleFor(dp => dp.PharmacistId, f => f.PickRandom(pharmacists).Id);
        return faker.Generate(800);
    }

    //! Create a Faker instance for DocumentBill
    public static List<DocumentBill> SeedDocumentBills(List<Appointment> appointments, List<Employee> employees, List<Account> accounts)
    {
        // Filter employees to include only those with AccountTypeId corresponding to "Receptionist"
        var receptionistAccounts = accounts
            .Where(a => a.AccountTypeId == 2) // Assuming AccountTypeId 2 corresponds to "Receptionist"
            .ToList();

        var validReceptionists = employees
            .Where(e => receptionistAccounts.Any(a => a.Id == e.AccountId))
            .ToList();

        if (!validReceptionists.Any())
        {
            Console.WriteLine("No valid receptionists found for Document Bills.");
            return new List<DocumentBill>(); // Return an empty list if no valid receptionists
        }

        if (!appointments.Any())
        {
            Console.WriteLine("No appointments found for Document Bills.");
            return new List<DocumentBill>(); // Return an empty list if no appointments
        }

        // Generate DocumentBills using Faker
        var faker = new Faker<DocumentBill>()
            .RuleFor(db => db.Id, f => f.IndexFaker + 1) // Auto-increment Id
            .RuleFor(db => db.DocumentTypeId, f => 4) // Assuming DocumentTypeId is always 4
            .RuleFor(db => db.AppointmentId, f => f.PickRandom(appointments).Id) // Random appointment
            .RuleFor(db => db.ReceptionistId, f => f.PickRandom(validReceptionists).Id) // Random valid receptionist
            .RuleFor(db => db.TotalCost, f => f.Finance.Amount(100, 1000)) // Random Total Cost between 100 and 1000
            .RuleFor(db => db.AmountPaid, (f, db) => f.Finance.Amount(0, db.TotalCost)); // AmountPaid less than or equal to TotalCost

        return faker.Generate(1000); // Generate 1000 Document Bills
    }




}
