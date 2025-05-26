using DoctorSystem.Data;
using DoctorSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DoctorSystem.Services.Interfaces;
using DoctorSystem.Services.Implementations;
using DoctorSystem.DTOs.Appointments;
using DoctorSystem.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// DB Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register our services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IValidator<CreateAppointmentDto>, CreateAppointmentDtoValidator>();

var app = builder.Build();

// Ensure Roles and Seed Default Users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Admin", "Patient", "Lab Technician", "Doctor" };

    // Create roles if they do not exist
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed default Admin user
    var adminEmail = "admin@gmail.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "System Admin"
        };
        var result = await userManager.CreateAsync(admin, "Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }

    // Seed default Patient user
    var patientEmail = "njomane@gmail.com";
    if (await userManager.FindByEmailAsync(patientEmail) == null)
    {
        var patient = new ApplicationUser
        {
            UserName = patientEmail,
            Email = patientEmail,
            FullName = "Njomane"
        };
        var result = await userManager.CreateAsync(patient, "Njomane@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(patient, "Patient");
        }
    }

    // Seed default Lab Technician user
    var labTechEmail = "nongalo@gmail.com";
    if (await userManager.FindByEmailAsync(labTechEmail) == null)
    {
        var labTech = new ApplicationUser
        {
            UserName = labTechEmail,
            Email = labTechEmail,
            FullName = "Nongalo"
        };
        var result = await userManager.CreateAsync(labTech, "Ngongalo@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(labTech, "Lab Technician");
        }
    }

    // Seed default Doctor user
    var doctorEmail = "manyoni1@gmail.com";
    if (await userManager.FindByEmailAsync(doctorEmail) == null)
    {
        var doctor = new ApplicationUser
        {
            UserName = doctorEmail,
            Email = doctorEmail,
            FullName = "Manyoni"
        };
        var result = await userManager.CreateAsync(doctor, "Manyoni@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(doctor, "Doctor");
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();

app.Run();
