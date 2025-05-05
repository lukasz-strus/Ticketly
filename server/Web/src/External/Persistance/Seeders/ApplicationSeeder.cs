using Domain.UserAggregate;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistance.Seeders;

public sealed class ApplicationSeeder(
    ApplicationDbContext dbContext,
    IServiceProvider serviceProvider)
{
    public async Task Seed()
    {
        if (!await dbContext.Database.CanConnectAsync()) return;

        await MigratePendingChanges();

        await SeedData();
    }

    private async Task MigratePendingChanges()
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }

    private async Task SeedData()
    {
        if (!await dbContext.Roles.AnyAsync())
            await SeedRoles();

        if (!await dbContext.Users.AnyAsync())
            await SeedUsers();
    }

    private async Task SeedRoles()
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[]
        {
            new IdentityRole(UserRole.User.Name),
            new IdentityRole(UserRole.Admin.Name)
        };

        foreach (var role in roles)
        {
            var roleName = role.Name;

            if (roleName is null)
                continue;

            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(role);
        }
    }

    private async Task SeedUsers()
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var admin = new User
        {
            UserName = "admin",
            Email = "admin@localhost.com",
            EmailConfirmed = true,
            FirstName = FirstName.Create("Admin").Value(),
            LastName = LastName.Create("Admin").Value(),
            Address = Address.Create("Admin Street", "10", "1", "00001", "Admin Post").Value()
        };

        if (await userManager.FindByNameAsync(admin.UserName) == null)
        {
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, UserRole.Admin.Name);
        }

        var user = new User
        {
            UserName = "user",
            Email = "user@localhost.com",
            EmailConfirmed = true,
            FirstName = FirstName.Create("User").Value(),
            LastName = LastName.Create("User").Value(),
            Address = Address.Create("User Street", "10", "1", "00001", "User Post").Value()
        };

        if (await userManager.FindByNameAsync(user.UserName) == null)
        {
            await userManager.CreateAsync(user, "User123!");
            await userManager.AddToRoleAsync(user, UserRole.User.Name);
        }
    }
}