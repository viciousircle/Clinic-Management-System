using System;
using Bogus;
using OOP_Final_Project.Models;
using System.Collections.Generic;

namespace OOP_Final_Project.Data;

public class DataSeeder
{
    public static List<Clinic> SeedClinics(int count)
    {
        var faker = new Faker<Clinic>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1) // Tự động tăng ID
            .RuleFor(c => c.ClinicName, f => f.Company.CompanyName()) // Tên phòng khám
            .RuleFor(c => c.Address, f => f.Address.FullAddress()) // Địa chỉ
            .RuleFor(c => c.Details, f => f.Lorem.Sentence()); // Thông tin thêm

        return faker.Generate(count);
    }

    public static List<Patient> SeedPatients(int count)
    {
        var faker = new Faker<Patient>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName());

        return faker.Generate(count);
    }

    public static List<Department> SeedDepartments(int count, List<Clinic> clinics)
    {
        var faker = new Faker<Department>()
            .RuleFor(d => d.Id, f => f.IndexFaker + 1)
            .RuleFor(d => d.ClinicId, f => f.PickRandom(clinics).Id) // Associate with random clinic
            .RuleFor(d => d.DepartmentName, f => f.Commerce.Department());

        return faker.Generate(count);
    }

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

    public static List<Appointment> SeedAppointments(int count, List<PatientCase> patientCases, List<Department> departments)
    {
        var faker = new Faker<Appointment>()
            .RuleFor(a => a.Id, f => f.IndexFaker + 1)
            .RuleFor(a => a.PatientCaseId, f => f.PickRandom(patientCases).Id)
            .RuleFor(a => a.InDepartmentId, f => f.PickRandom(departments).Id)
            .RuleFor(a => a.TimeCreated, f => f.Date.Recent())
            .RuleFor(a => a.AppointmentStartTime, f => f.Date.Future())
            .RuleFor(a => a.AppointmentEndTime, f => f.Date.Future())
            .RuleFor(a => a.AppointmentStatusId, f => f.Random.Int(1, 5)); // For example, 5 different statuses

        return faker.Generate(count);
    }





}
