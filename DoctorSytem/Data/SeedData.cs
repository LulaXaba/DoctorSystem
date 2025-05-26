using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using DoctorSystem.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Create roles if they don't exist
        string[] roles = new[] { "Doctor", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create a doctor user if not exists
        string doctorEmail = "doctor1@example.com";
        var doctorUser = await userManager.FindByEmailAsync(doctorEmail);
        if (doctorUser == null)
        {
            doctorUser = new ApplicationUser { UserName = doctorEmail, Email = doctorEmail };
            await userManager.CreateAsync(doctorUser, "Doctor@123");  // Set password
            await userManager.AddToRoleAsync(doctorUser, "Doctor");
        }

        // Create a customer user if not exists
        string customerEmail = "customer1@example.com";
        var customerUser = await userManager.FindByEmailAsync(customerEmail);
        if (customerUser == null)
        {
            customerUser = new ApplicationUser { UserName = customerEmail, Email = customerEmail };
            await userManager.CreateAsync(customerUser, "Customer@123");  // Set password
            await userManager.AddToRoleAsync(customerUser, "Customer");
        }
    }
}
