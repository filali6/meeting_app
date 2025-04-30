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
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;

        })
        .AddRoles<IdentityRole>()
        .AddRoleManager<RoleManager<IdentityRole>>()
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
        //to make authorization for SignalR 
        option.Events=new JwtBearerEvents{
            OnMessageReceived = context =>{
                var accesToken=context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if(!string.IsNullOrEmpty(accesToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token=accesToken;
                }
                return Task.CompletedTask;
            }
        };
    });
        services.AddAuthorizationBuilder()
                .AddPolicy(Policies.RequireAdminRole, policy => policy.RequireRole(Roles.Admin))
                .AddPolicy(Policies.ModeratePhotoRole, policy => policy.RequireRole( Roles.Moderator));
        return services;
    }
}
