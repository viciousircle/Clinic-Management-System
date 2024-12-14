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

// Use SQLite for the database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers support (for API)
builder.Services.AddControllers();


// Add services to the container.
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

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//     // dbContext.Database.ExecuteSqlRaw("DELETE FROM Employees");

//     // Apply any pending migrations
//     dbContext.Database.Migrate();

//     Console.WriteLine("Starting data seeding...");

//     // Seed the data after migrations are applied
//     SeedData(scope.ServiceProvider);

//     Console.WriteLine("Data seeding completed.");

// }
// Console.WriteLine("Application setup completed.");



app.Run();

static void SeedData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    Console.WriteLine("Starting data seeding...");


    // Check if data already exists before seeding to prevent duplicates

    if (!dbContext.AccountTypes.Any())
    {
        var accountTypes = DataSeeder.SeedAccountTypes();
        dbContext.AccountTypes.AddRange(accountTypes);
    }

    if (!dbContext.Clinics.Any())
    {
        var clinics = DataSeeder.SeedClinics(10);
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

    // Level 2
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

    if (!dbContext.Patients.Any())
    {
        var patients = DataSeeder.SeedPatients(dbContext.Clinics.ToList(), dbContext.Accounts.ToList());
        dbContext.Patients.AddRange(patients);
    }

    // Level 3

    if (!dbContext.Employees.Any())
    {
        var employees = DataSeeder.SeedEmployees(dbContext.Accounts.ToList(), dbContext.Clinics.ToList(), dbContext.Departments.ToList());
        dbContext.Employees.AddRange(employees);
    }

    // TODO - Fix this shit/
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

    if (!dbContext.Prescriptions.Any())
    {
        var prescriptions = DataSeeder.SeedPrescriptions(dbContext.Appointments.ToList());
        dbContext.Prescriptions.AddRange(prescriptions);
    }

    // Level 4

    if (!dbContext.EmployeeSchedules.Any())
    {
        var employeeSchedules = DataSeeder.SeedEmployeeSchedules(dbContext.Schedules.ToList(), dbContext.Employees.ToList());
        dbContext.EmployeeSchedules.AddRange(employeeSchedules);
    }

    if (!dbContext.PrescriptionMedicines.Any())
    {
        var prescriptionMedicines = DataSeeder.SeedPrescriptionMedicines(dbContext.Prescriptions.ToList(), dbContext.Medicines.ToList());
        dbContext.PrescriptionMedicines.AddRange(prescriptionMedicines);
    }

    // Level 5

    if (!dbContext.DocumentAppointments.Any())
    {
        var documentAppointments = DataSeeder.SeedDocumentAppointments(dbContext.Appointments.ToList(), dbContext.EmployeeSchedules.ToList());
        dbContext.DocumentAppointments.AddRange(documentAppointments);
    }

    if (!dbContext.DocumentDiagnoses.Any())
    {
        var documentDiagnoses = DataSeeder.SeedDocumentDiagnoses(dbContext.Appointments.ToList());
        dbContext.DocumentDiagnoses.AddRange(documentDiagnoses);
    }

    if (!dbContext.DocumentPrescribes.Any())
    {
        var documentPrescribes = DataSeeder.SeedDocumentPrescribes(dbContext.Prescriptions.ToList(), dbContext.Employees.ToList());
        dbContext.DocumentPrescribes.AddRange(documentPrescribes);
    }

    if (!dbContext.DocumentBills.Any())
    {
        var documentBills = DataSeeder.SeedDocumentBills(dbContext.Appointments.ToList(), dbContext.Employees.ToList(), dbContext.Accounts.ToList());
        dbContext.DocumentBills.AddRange(documentBills);
    }

    if (!dbContext.DocumentCancels.Any())
    {
        var documentCancels = DataSeeder.SeedDocumentCancels(dbContext.Appointments.ToList());
        dbContext.DocumentCancels.AddRange(documentCancels);
    }

    Console.WriteLine("Data seeding complete.");
    // Save changes once all data is added
    dbContext.SaveChanges();
}

