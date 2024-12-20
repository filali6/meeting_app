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
        await roleManager.CreateAsync(new IdentityRole(Roles.Member) );
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin) );
        await roleManager.CreateAsync(new IdentityRole(Roles.Moderator) );
        var userData = await File.ReadAllTextAsync("Data/UsersSeed.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        if (users == null)return;
        var roles = new List<AppRole>{
            new() {Name="member"},
            new() {Name="admin"},
            new() {Name="moderator"},
        };
        roles.ForEach(async r=>{
            await roleManager.CreateAsync(r);
        });
        users.ForEach(async u =>
            {

                await userManager.CreateAsync(u,"Pa$$w0rd");
                await userManager.AddToRoleAsync(u,Roles.Member);
                }
            );
        }
        var admin = new AppUser{
            KnownAs = "admin",
            UserName = "admin",
            City="",
            IsMale = true
        };
        await userManager.CreateAsync(admin,"Pa$$w0rd");
        await userManager.AddToRolesAsync(admin,[Roles.Admin,Roles.Moderator]);
    }
}
