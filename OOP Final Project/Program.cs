using OOP_Final_Project.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Code thêm vào
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}





// Seed dữ liệu mẫu
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Apply migrations

    // Seed data if the database is empty
    if (!context.Patients.Any())
    {
        // Seed clinics, departments, employees, etc.
        var clinics = DataSeeder.SeedClinics(10); // 10 clinics
        context.Clinics.AddRange(clinics);
        context.SaveChanges();

        var departments = DataSeeder.SeedDepartments(10, clinics); // 10 departments
        context.Departments.AddRange(departments);
        context.SaveChanges();

        var employees = DataSeeder.SeedEmployees(50, departments); // 50 employees
        context.Employees.AddRange(employees);
        context.SaveChanges();

        var patients = DataSeeder.SeedPatients(5000); // 500 patients
        context.Patients.AddRange(patients);
        context.SaveChanges();

        var patientCases = DataSeeder.SeedPatientCases(5000, patients); // 500 patient cases
        context.PatientCases.AddRange(patientCases);
        context.SaveChanges();

        var appointments = DataSeeder.SeedAppointments(7000, patientCases, departments); // 500 appointments
        context.Appointments.AddRange(appointments);
        context.SaveChanges();
    }
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
