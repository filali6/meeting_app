using System;
using System.Text;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Extensions;

public static class IdentityServices
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration conf)
    {
        services.AddIdentityCore<AppUser>(opt=>{
            opt.Password.RequireNonAlphanumeric=false;

        })
        .AddRoles<AppRole>()
        .AddRoleManager<RoleManager<AppRole>>()
        .AddEntityFrameworkStores<DataContext>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        var tokenKey = conf["TokenKey"] ?? throw new Exception("Token key not Found");
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
    services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminRole",policy=>policy.RequireRole("admin"))
    .AddPolicy("ModratePhotoRole",policy=>policy.RequireRole("admine","moderator"));
        return services;
    }
}
