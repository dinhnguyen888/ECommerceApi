using System;
using System.Linq;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BCrypt.Net;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))

        {
            context.Database.Migrate(); // Ð?m b?o database dã du?c c?p nh?t

            // Ki?m tra n?u Role dã t?n t?i
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Id = 1, RoleName = "Admin" },
                    new Role { Id = 2, RoleName = "User" }
                );
                context.SaveChanges();
            }

            // Ki?m tra n?u tài kho?n Admin dã t?n t?i
            if (!context.Accounts.Any(a => a.Email == "admin@admin.com"))
            {
                var admin = new Account
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Email = "admin@admin.com",
                    RoleId = 1,
                    PictureUrl = null,
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123") // Dùng BCrypt d? hash m?t kh?u
                };

                context.Accounts.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
