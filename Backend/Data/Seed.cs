using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if (await context.Users.AnyAsync()) return;
        var userData = await File.ReadAllTextAsync("Data/UsersSeed.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        if (users != null)
        {
            users.ForEach(async u =>
            {
                using (var hmac = new HMACSHA256())
                {
                    
                    
                    u.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("PassWord"));
                    u.PasswordSalt = hmac.Key;
                };
                await context.Users.AddAsync(u);
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
