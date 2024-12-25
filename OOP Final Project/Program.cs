using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Data;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using OOP_Final_Project.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;



Console.WriteLine("Application is starting...");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddRazorPages();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();




// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers(); // Map API controllers

app.UseSwagger();
app.UseSwaggerUI();



app.UseAuthentication();  // Enable authentication


// ! For set up database fake data ----------------------------

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    //- Drop the database (this will drop all tables) ----------------

    if (dbContext.Database.EnsureDeleted())
    {
        Console.WriteLine("Database dropped successfully.");
    }


    // ----------------------------------------------------------------

    // Apply any pending migrations
    dbContext.Database.Migrate();

    Console.WriteLine("Starting data seeding...");

    //- Seed the data after migrations are applied -------------------

    SeedDataLevel1(scope.ServiceProvider);
    SeedDataLevel2(scope.ServiceProvider);
    SeedDataLevel3(scope.ServiceProvider);
    SeedDataLevel4(scope.ServiceProvider);
    SeedDataLevel5(scope.ServiceProvider);
    SeedDataLevel6(scope.ServiceProvider);

    // ----------------------------------------------------------------

    Console.WriteLine("Data seeding completed.");

}
Console.WriteLine("Application setup completed.");

// ! -----------------------------------------------------------

app.Run();

// ! Seed data method --------------------------------------------

static void SeedDataLevel1(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    Console.WriteLine("Starting data seeding level 1...");

    // ? Level 1

    if (!dbContext.AccountTypes.Any())
    {
        var accountTypes = DataSeeder.SeedAccountTypes();
        dbContext.AccountTypes.AddRange(accountTypes);
    }

    if (!dbContext.Clinics.Any())
    {
        var clinics = DataSeeder.SeedClinics(2);
        dbContext.Clinics.AddRange(clinics);
    }

    if (!dbContext.Schedules.Any())
    {
        var schedules = DataSeeder.SeedSchedules();
        dbContext.Schedules.AddRange(schedules);
    }

    if (!dbContext.MedicineTypes.Any())
    {
        var medicineTypes = DataSeeder.SeedMedicineTypes();
        dbContext.MedicineTypes.AddRange(medicineTypes);
    }

    if (!dbContext.DocumentTypes.Any())
    {
        var documentTypes = DataSeeder.SeedDocumentTypes();
        dbContext.DocumentTypes.AddRange(documentTypes);
    }

    Console.WriteLine("Data seeding complete.");
    // Save changes once all data is added
    dbContext.SaveChanges();

}

static void SeedDataLevel2(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    Console.WriteLine("Starting data seeding level 2...");

    // ? Level 2

    if (!dbContext.Departments.Any())
    {
        var departments = DataSeeder.SeedDepartments(dbContext.Clinics.ToList(), dbContext.AccountTypes.ToList());
        dbContext.Departments.AddRange(departments);
    }


    if (!dbContext.Accounts.Any())
    {
        var accounts = DataSeeder.SeedAccounts(dbContext.AccountTypes.ToList(), dbContext.Clinics.ToList(), 4000);
        dbContext.Accounts.AddRange(accounts);
    }

    Console.WriteLine("Data seeding complete.");
    // Save changes once all data is added
    dbContext.SaveChanges();


}

static void SeedDataLevel3(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    Console.WriteLine("Starting data seeding level 3...");


    if (!dbContext.Patients.Any())
    {
        // Fetch clinics and accounts from the database
        var clinics = dbContext.Clinics.ToList();
        var accounts = dbContext.Accounts.ToList();

        // Ensure valid accounts (AccountTypeId == 5) exist
        var patientAccounts = accounts.Where(a => a.AccountTypeId == 5).ToList();

        if (!patientAccounts.Any())
        {
            Console.WriteLine("No accounts with AccountTypeId == 5 found. No patients will be created.");
        }
        else
        {
            // Seed patients
            var patients = DataSeeder.SeedPatients(clinics, accounts);

            // Add patients to the database
            dbContext.Patients.AddRange(patients);
            dbContext.SaveChanges();

            Console.WriteLine($"{patients.Count} patients seeded successfully.");
        }
    }
    else
    {
        Console.WriteLine("Patients already exist in the database. Seeding skipped.");
    }


    if (!dbContext.Employees.Any())
    {
        var employees = DataSeeder.SeedEmployees(dbContext.Accounts.ToList(), dbContext.Clinics.ToList(), dbContext.Departments.ToList());
        dbContext.Employees.AddRange(employees);
    }




    Console.WriteLine("Data seeding complete.");
    // Save changes once all data is added
    dbContext.SaveChanges();
}

static void SeedDataLevel4(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    Console.WriteLine("Starting data seeding level 4...");

    if (!dbContext.Medicines.Any())
    {
        var medicines = DataSeeder.SeedMedicines(dbContext.MedicineTypes.ToList(), dbContext.Employees.ToList(), dbContext.Accounts.ToList(), 1000);
        dbContext.Medicines.AddRange(medicines);
    }

    if (!dbContext.Appointments.Any())
    {
        var appointments = DataSeeder.SeedAppointments(dbContext.Employees.ToList(), dbContext.Patients.ToList(), dbContext.Accounts.ToList());
        dbContext.Appointments.AddRange(appointments);
    }

    if (!dbContext.EmployeeSchedules.Any())
    {
        var employeeSchedules = DataSeeder.SeedEmployeeSchedules(dbContext.Schedules.ToList(), dbContext.Employees.ToList(), dbContext.Accounts.ToList());
        dbContext.EmployeeSchedules.AddRange(employeeSchedules);
    }

    Console.WriteLine("Data seeding complete.");
    // Save changes once all data is added
    dbContext.SaveChanges();
}

static void SeedDataLevel5(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    Console.WriteLine("Starting data seeding level 5...");


    if (!dbContext.DocumentCancels.Any())
    {
        var documentCancels = DataSeeder.SeedDocumentCancels(dbContext.Appointments.ToList());
        dbContext.DocumentCancels.AddRange(documentCancels);
    }

    Console.WriteLine("Data seeding complete.");
    // Save changes once all data is added
    dbContext.SaveChanges();
}

static void SeedDataLevel6(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    Console.WriteLine("Starting data seeding level 6...");

    // ? Level 6

    if (dbContext.Prescriptions.Count() < 6000)
    {
        var documentCancels = dbContext.DocumentCancels.ToList();  // Fetch existing document cancels
        var appointments = dbContext.Appointments.ToList(); // Fetch existing appointments
        var existingPrescriptionsCount = dbContext.Prescriptions.Count();
        var prescriptionsToGenerate = 6000 - existingPrescriptionsCount;
        var prescriptions = DataSeeder.SeedPrescriptions(appointments, documentCancels).Take(prescriptionsToGenerate).ToList();  // Seed prescriptions for non-cancelled appointments
        dbContext.Prescriptions.AddRange(prescriptions); // Add seeded prescriptions to the context
    }
    Console.WriteLine("Data seeding complete.");
    // Save changes once all data is added
    dbContext.SaveChanges();
}


static void SeedData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    Console.WriteLine("Starting data seeding...");




    //! Check if data already exists before seeding to prevent duplicates

    // ? Level 1

    if (!dbContext.AccountTypes.Any())
    {
        var accountTypes = DataSeeder.SeedAccountTypes();
        dbContext.AccountTypes.AddRange(accountTypes);
    }

    if (!dbContext.Clinics.Any())
    {
        var clinics = DataSeeder.SeedClinics(2);
        dbContext.Clinics.AddRange(clinics);
    }

    if (!dbContext.Schedules.Any())
    {
        var schedules = DataSeeder.SeedSchedules();
        dbContext.Schedules.AddRange(schedules);
    }

    if (!dbContext.MedicineTypes.Any())
    {
        var medicineTypes = DataSeeder.SeedMedicineTypes();
        dbContext.MedicineTypes.AddRange(medicineTypes);
    }

    if (!dbContext.DocumentTypes.Any())
    {
        var documentTypes = DataSeeder.SeedDocumentTypes();
        dbContext.DocumentTypes.AddRange(documentTypes);
    }



    // ? Level 2

    if (!dbContext.Departments.Any())
    {
        var departments = DataSeeder.SeedDepartments(dbContext.Clinics.ToList(), dbContext.AccountTypes.ToList());
        dbContext.Departments.AddRange(departments);
    }


    if (!dbContext.Accounts.Any())
    {
        var accounts = DataSeeder.SeedAccounts(dbContext.AccountTypes.ToList(), dbContext.Clinics.ToList(), 4000);
        dbContext.Accounts.AddRange(accounts);
    }



    // ? Level 4

    if (!dbContext.Patients.Any())
    {
        // Fetch clinics and accounts from the database
        var clinics = dbContext.Clinics.ToList();
        var accounts = dbContext.Accounts.ToList();

        // Ensure valid accounts (AccountTypeId == 5) exist
        var patientAccounts = accounts.Where(a => a.AccountTypeId == 5).ToList();

        if (!patientAccounts.Any())
        {
            Console.WriteLine("No accounts with AccountTypeId == 5 found. No patients will be created.");
        }
        else
        {
            // Seed patients
            var patients = DataSeeder.SeedPatients(clinics, accounts);

            // Add patients to the database
            dbContext.Patients.AddRange(patients);
            dbContext.SaveChanges();

            Console.WriteLine($"{patients.Count} patients seeded successfully.");
        }
    }
    else
    {
        Console.WriteLine("Patients already exist in the database. Seeding skipped.");
    }


    if (!dbContext.Employees.Any())
    {
        var employees = DataSeeder.SeedEmployees(dbContext.Accounts.ToList(), dbContext.Clinics.ToList(), dbContext.Departments.ToList());
        dbContext.Employees.AddRange(employees);
    }



    // ? Level 6

    if (!dbContext.Medicines.Any())
    {
        var medicines = DataSeeder.SeedMedicines(dbContext.MedicineTypes.ToList(), dbContext.Employees.ToList(), dbContext.Accounts.ToList(), 1000);
        dbContext.Medicines.AddRange(medicines);
    }

    if (!dbContext.Appointments.Any())
    {
        var appointments = DataSeeder.SeedAppointments(dbContext.Employees.ToList(), dbContext.Patients.ToList(), dbContext.Accounts.ToList());
        dbContext.Appointments.AddRange(appointments);
    }

    if (!dbContext.EmployeeSchedules.Any())
    {
        var employeeSchedules = DataSeeder.SeedEmployeeSchedules(dbContext.Schedules.ToList(), dbContext.Employees.ToList(), dbContext.Accounts.ToList());
        dbContext.EmployeeSchedules.AddRange(employeeSchedules);
    }

    //  ? Level 7

    if (!dbContext.DocumentCancels.Any())
    {
        var documentCancels = DataSeeder.SeedDocumentCancels(dbContext.Appointments.ToList());
        dbContext.DocumentCancels.AddRange(documentCancels);
    }

    // ? Level 8

    if (!dbContext.Prescriptions.Any())
    {
        var documentCancels = dbContext.DocumentCancels.ToList();  // Fetch existing document cancels
        var appointments = dbContext.Appointments.ToList(); // Fetch existing appointments
        var prescriptions = DataSeeder.SeedPrescriptions(appointments, documentCancels);  // Seed prescriptions for non-cancelled appointments
        dbContext.Prescriptions.AddRange(prescriptions); // Add seeded prescriptions to the context
    }



    // if (!dbContext.PrescriptionMedicines.Any())
    // {
    //     var prescriptionMedicines = DataSeeder.SeedPrescriptionMedicines(dbContext.Prescriptions.ToList(), dbContext.Medicines.ToList());
    //     dbContext.PrescriptionMedicines.AddRange(prescriptionMedicines);
    // }


    // if (!dbContext.DocumentAppointments.Any())
    // {
    //     var documentAppointments = DataSeeder.SeedDocumentAppointments(dbContext.Appointments.ToList(), dbContext.EmployeeSchedules.ToList());
    //     dbContext.DocumentAppointments.AddRange(documentAppointments);
    // }

    // if (!dbContext.DocumentDiagnoses.Any())
    // {
    //     var documentDiagnoses = DataSeeder.SeedDocumentDiagnoses(dbContext.Appointments.ToList());
    //     dbContext.DocumentDiagnoses.AddRange(documentDiagnoses);
    // }

    // if (!dbContext.DocumentPrescribes.Any())
    // {
    //     var documentPrescribes = DataSeeder.SeedDocumentPrescribes(dbContext.Prescriptions.ToList(), dbContext.Employees.ToList());
    //     dbContext.DocumentPrescribes.AddRange(documentPrescribes);
    // }

    // if (!dbContext.DocumentBills.Any())
    // {
    //     var documentBills = DataSeeder.SeedDocumentBills(dbContext.Appointments.ToList(), dbContext.Employees.ToList(), dbContext.Accounts.ToList());
    //     dbContext.DocumentBills.AddRange(documentBills);
    // }



    Console.WriteLine("Data seeding complete.");
    // Save changes once all data is added
    dbContext.SaveChanges();
}

