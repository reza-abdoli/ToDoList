using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Entity;

namespace Presentation.Extension;

public static class SeedAdmin
{
    public static async Task SeedData(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (!db.Users.Any(u => u.Role == "Admin"))
        {
            db.Users.Add(new User
            {
                Name = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

    }

}