using System;
using System.Linq;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BCrypt.Net;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))

        {
            context.Database.Migrate(); // Đảm bảo database đã được cập nhật

            // Kiểm tra nếu Role đã tồn tại
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Id = 1, RoleName = "Admin" },
                    new Role { Id = 2, RoleName = "User" }
                );
                context.SaveChanges();
            }

            // Kiểm tra nếu tài khoản Admin đã tồn tại
            if (!context.Accounts.Any(a => a.Email == "admin@admin.com"))
            {
                var admin = new Account
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Email = "admin@admin.com",
                    RoleId = 1,
                    PictureUrl = null,
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123") // Dùng BCrypt để hash mật khẩu
                };

                context.Accounts.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
