using System;
using Bogus;
using OOP_Final_Project.Models;
using System.Collections.Generic;

namespace OOP_Final_Project.Data;

public class DataSeeder
{
    // Gốc
    public static List<Clinic> SeedClinics(int count)
    {
        var faker = new Faker<Clinic>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.ClinicName, f => f.Company.CompanyName())
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.Details, f => f.Lorem.Sentence());
        return faker.Generate(count);
    }

    public static List<Department> SeedDepartments(int count, List<Clinic> clinics)
    {
        var faker = new Faker<Department>()
            .RuleFor(d => d.Id, f => f.IndexFaker + 1)
            .RuleFor(d => d.ClinicId, f => f.PickRandom(clinics).Id)
            .RuleFor(d => d.DepartmentName, f => f.Commerce.Department());
        return faker.Generate(count);
    }

    // Seed Roles
    public static List<Role> SeedRoles()
    {
        return new List<Role>
        {
            new Role { Id = 1, RoleName = "Admin" },
            new Role { Id = 2, RoleName = "Doctor" },
            new Role { Id = 3, RoleName = "Nurse" },
            new Role { Id = 4, RoleName = "Receptionist" },
            new Role { Id = 5, RoleName = "Accountant" }
        };
    }

    public static List<Patient> SeedPatients(int count)
    {
        var faker = new Faker<Patient>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName());
        return faker.Generate(count);
    }

    public static List<AppointmentStatus> SeedAppointmentStatuses()
    {
        return new List<AppointmentStatus>
        {
            new AppointmentStatus { Id = 1, StatusName = "Scheduled" },
            new AppointmentStatus { Id = 2, StatusName = "In Progress" },
            new AppointmentStatus { Id = 3, StatusName = "Completed" },
            new AppointmentStatus { Id = 4, StatusName = "Cancelled" },
            new AppointmentStatus { Id = 5, StatusName = "No Show" }
        };
    }

    public static List<DocumentType> SeedDocumentTypes(int count)
    {
        var faker = new Faker<DocumentType>()
            .RuleFor(dt => dt.Id, f => f.IndexFaker + 1)
            .RuleFor(dt => dt.TypeName, f => f.Commerce.ProductName());
        return faker.Generate(count);
    }

    // Phụ thuộc trực tiếp
    public static List<Employee> SeedEmployees(int count, List<Department> departments)
    {
        var faker = new Faker<Employee>()
            .RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.UserName, f => f.Internet.UserName())
            .RuleFor(e => e.Password, f => f.Internet.Password())
            .RuleFor(e => e.Email, f => f.Internet.Email())
            .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(e => e.IsActive, f => f.Random.Bool());
        return faker.Generate(count);
    }

    // Seed HasRoles
    public static List<HasRole> SeedHasRoles(List<Employee> employees, List<Role> roles)
    {
        var hasRoles = new List<HasRole>();

        // Gán vai trò Admin cho nhân viên đầu tiên
        var admin = employees.First();
        hasRoles.Add(new HasRole
        {
            Id = 1,
            EmployeeId = admin.Id,
            RoleId = roles.First(r => r.RoleName == "Admin").Id,
            TimeFrom = DateTime.Now.AddYears(-1),
            TimeTo = DateTime.Now.AddYears(1),
            IsActive = true
        });

        // Gán Receptionist cho 10 người tiếp theo
        var receptionists = employees.Skip(1).Take(10);
        foreach (var employee in receptionists)
        {
            hasRoles.Add(new HasRole
            {
                Id = hasRoles.Count + 1,
                EmployeeId = employee.Id,
                RoleId = roles.First(r => r.RoleName == "Receptionist").Id,
                TimeFrom = DateTime.Now.AddYears(-1),
                TimeTo = DateTime.Now.AddYears(1),
                IsActive = true
            });
        }

        // Gán Doctor cho các nhân viên còn lại
        var doctors = employees.Skip(11);
        foreach (var employee in doctors)
        {
            hasRoles.Add(new HasRole
            {
                Id = hasRoles.Count + 1,
                EmployeeId = employee.Id,
                RoleId = roles.First(r => r.RoleName == "Doctor").Id,
                TimeFrom = DateTime.Now.AddYears(-1),
                TimeTo = DateTime.Now.AddYears(1),
                IsActive = true
            });
        }

        return hasRoles;
    }

    public static List<InDepartment> SeedInDepartments(int count, List<Employee> employees, List<Department> departments)
    {
        var faker = new Faker<InDepartment>()
            .RuleFor(i => i.Id, f => f.IndexFaker + 1)
            .RuleFor(i => i.EmployeeId, f => f.PickRandom(employees).Id)
            .RuleFor(i => i.DepartmentId, f => f.PickRandom(departments).Id)
            .RuleFor(i => i.TimeFrom, f => f.Date.Past())
            .RuleFor(i => i.TimeTo, f => f.Date.Future())
            .RuleFor(i => i.IsActive, f => f.Random.Bool());
        return faker.Generate(count);
    }

    public static List<PatientCase> SeedPatientCases(int count, List<Patient> patients)
    {
        var faker = new Faker<PatientCase>()
            .RuleFor(pc => pc.Id, f => f.IndexFaker + 1)
            .RuleFor(pc => pc.PatientId, f => f.PickRandom(patients).Id)
            .RuleFor(pc => pc.StartTime, f => f.Date.Past())
            .RuleFor(pc => pc.EndTime, f => f.Date.Future())
            .RuleFor(pc => pc.InProgress, f => f.Random.Bool())
            .RuleFor(pc => pc.TotalCost, f => f.Finance.Amount())
            .RuleFor(pc => pc.AmountPaid, f => f.Finance.Amount());
        return faker.Generate(count);
    }

    // Phụ thuộc gián tiếp
    public static List<Schedule> SeedSchedules(int count, List<InDepartment> inDepartments)
    {
        var faker = new Faker<Schedule>()
            .RuleFor(s => s.Id, f => f.IndexFaker + 1)
            .RuleFor(s => s.InDepartmentId, f => f.PickRandom(inDepartments).Id)
            .RuleFor(s => s.Date, f => f.Date.Future())
            .RuleFor(s => s.TimeStart, f => f.Date.Between(DateTime.Today.AddHours(8), DateTime.Today.AddHours(12)).TimeOfDay)
            .RuleFor(s => s.TimeEnd, (f, s) => s.TimeStart.Add(TimeSpan.FromHours(2)));
        return faker.Generate(count);
    }

    public static List<Appointment> SeedAppointments(int count, List<PatientCase> patientCases, List<Department> departments)
    {
        var faker = new Faker<Appointment>()
            .RuleFor(a => a.Id, f => f.IndexFaker + 1)
            .RuleFor(a => a.PatientCaseId, f => f.PickRandom(patientCases).Id)
            .RuleFor(a => a.InDepartmentId, f => f.PickRandom(departments).Id)
            .RuleFor(a => a.TimeCreated, f => f.Date.Recent())
            .RuleFor(a => a.AppointmentStartTime, f => f.Date.Future())
            .RuleFor(a => a.AppointmentEndTime, f => f.Date.Future())
            .RuleFor(a => a.AppointmentStatusId, f => f.Random.Int(1, 5));
        return faker.Generate(count);
    }

    public static List<StatusHistory> SeedStatusHistories(int count, List<Appointment> appointments, List<AppointmentStatus> appointmentStatuses)
    {
        var faker = new Faker<StatusHistory>()
            .RuleFor(sh => sh.Id, f => f.IndexFaker + 1)
            .RuleFor(sh => sh.AppointmentId, f => f.PickRandom(appointments).Id)
            .RuleFor(sh => sh.AppointmentStatusId, f => f.PickRandom(appointmentStatuses).Id)
            .RuleFor(sh => sh.StatusTime, f => f.Date.Recent())
            .RuleFor(sh => sh.Details, f => f.Lorem.Sentence());
        return faker.Generate(count);
    }

    public static List<Document> SeedDocuments(int count, List<Patient> patients, List<PatientCase> patientCases, List<Appointment> appointments)
    {
        var faker = new Faker<Document>()
            .RuleFor(d => d.DocumentInternalId, f => f.IndexFaker + 1)
            .RuleFor(d => d.DocumentName, f => f.Commerce.ProductName())
            .RuleFor(d => d.DocumentTypeId, f => f.Random.Int(1, 3))
            .RuleFor(d => d.TimeCreated, f => f.Date.Past())
            .RuleFor(d => d.DocumentUrl, f => f.Internet.Url())
            .RuleFor(d => d.Details, f => f.Lorem.Sentence())
            .RuleFor(d => d.PatientId, f => f.PickRandom(patients).Id)
            .RuleFor(d => d.PatientCaseId, f => f.PickRandom(patientCases).Id)
            .RuleFor(d => d.AppointmentId, f => f.PickRandom(appointments).Id);
        return faker.Generate(count);
    }
}
