using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OOP_Final_Project.Data;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using OOP_Final_Project.Models;


var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorPages();

// Cấu hình Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Cấu hình phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("DoctorOnly", policy => policy.RequireRole("Doctor"));
    options.AddPolicy("ReceptionistOnly", policy => policy.RequireRole("Receptionist"));
});

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

app.UseAuthentication();  // Cần thêm dòng này để xác thực người dùng
// app.UseAuthorization();   // Cần thêm dòng này để kiểm tra phân quyền

// Seed dữ liệu mẫu khi ứng dụng khởi động
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//     // Xóa dữ liệu cũ
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM Documents");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM StatusHistories");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM Appointments");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM Schedules");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM PatientCases");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM InDepartments");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM HasRoles");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM Employees");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM DocumentTypes");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM AppointmentStatuses");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM Patients");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM Roles");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM Departments");
//     dbContext.Database.ExecuteSqlRaw("DELETE FROM Clinics");

//     dbContext.SaveChanges();

//     // Kiểm tra nếu database chưa có dữ liệu, ta sẽ seed dữ liệu mẫu
//     if (!dbContext.Clinics.Any())
//     {
//         // Seed Clinics
//         var clinics = DataSeeder.SeedClinics(10);
//         dbContext.Clinics.AddRange(clinics);
//         dbContext.SaveChanges();

//         // Seed Departments
//         var departments = DataSeeder.SeedDepartments(50, clinics);
//         dbContext.Departments.AddRange(departments);
//         dbContext.SaveChanges();

//         // Seed Roles
//         var roles = DataSeeder.SeedRoles();
//         dbContext.Roles.AddRange(roles);
//         dbContext.SaveChanges();

//         // Seed Patients
//         var patients = DataSeeder.SeedPatients(5000);
//         dbContext.Patients.AddRange(patients);
//         dbContext.SaveChanges();

//         // Seed AppointmentStatuses
//         var appointmentStatuses = DataSeeder.SeedAppointmentStatuses();
//         dbContext.AppointmentStatuses.AddRange(appointmentStatuses);
//         dbContext.SaveChanges();

//         // Seed DocumentTypes
//         var documentTypes = DataSeeder.SeedDocumentTypes(5);
//         dbContext.DocumentTypes.AddRange(documentTypes);
//         dbContext.SaveChanges();




//         // Seed Employees
//         var employees = DataSeeder.SeedEmployees(100, departments);
//         dbContext.Employees.AddRange(employees);
//         dbContext.SaveChanges();

//         // Seed HasRoles
//         var hasRoles = DataSeeder.SeedHasRoles(employees, roles);
//         dbContext.HasRoles.AddRange(hasRoles);
//         dbContext.SaveChanges();

//         // Seed InDepartments
//         var inDepartments = DataSeeder.SeedInDepartments(100, employees, departments);
//         dbContext.InDepartments.AddRange(inDepartments);
//         dbContext.SaveChanges();

//         // Seed PatientCases
//         var patientCases = DataSeeder.SeedPatientCases(5000, patients);
//         dbContext.PatientCases.AddRange(patientCases);
//         dbContext.SaveChanges();



//         // Seed Schedules
//         var schedules = DataSeeder.SeedSchedules(7000, inDepartments);
//         dbContext.Schedules.AddRange(schedules);
//         dbContext.SaveChanges();

//         // Seed Appointments
//         var appointments = DataSeeder.SeedAppointments(7000, patientCases, departments);
//         dbContext.Appointments.AddRange(appointments);
//         dbContext.SaveChanges();

//         // Seed StatusHistories
//         var statusHistories = DataSeeder.SeedStatusHistories(7000, appointments, appointmentStatuses);
//         dbContext.StatusHistories.AddRange(statusHistories);
//         dbContext.SaveChanges();

//         // Seed Documents
//         var documents = DataSeeder.SeedDocuments(7000, patients, patientCases, appointments);
//         dbContext.Documents.AddRange(documents);
//         dbContext.SaveChanges();


//     }
// }


app.Run();
