using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Backend.Models;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
    {
         if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UsersSeed.json");

        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;

        var roles = new List<IdentityRole>
        {
            new() {Name = "member"},
            new() {Name = "admin"},
            new() {Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.Photos.First().Approuved = true;
            user.UserName = user.UserName!.ToLower();
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "member");
        }

        var admin = new AppUser
        {
            UserName = "admin",
            KnownAs = "Admin",
            IsMale=true
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["admin", "Moderator"]);
    }
}
