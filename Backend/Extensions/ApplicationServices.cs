using System;
using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Backend.Services.AccountService;
using Backend.Services.LikesService;
using Backend.Services.MessageService;
using Backend.Services.PhotoService;
using Backend.Services.UnitOfWork;
using Backend.Services.UsersService;
using Backend.SiognalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Backend.Extensions;

public static class ApplicationServices
{
    public static IServiceCollection AddServices(this IServiceCollection collection, IConfiguration conf)
    {
        collection.AddControllers();
        collection.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(conf.GetConnectionString("DefaultConnection"));
        });
        
        collection.AddCors();
        collection.AddScoped<IUsersService,UsersService>();
        collection.AddScoped<IAccountService,AccountService>();
        collection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        collection.Configure<CloudinarySettings>(conf.GetSection("CloudinarySettings"));
        collection.AddScoped<IPhotoService,PhotoService>();
        collection.AddScoped<ILikesService,LikesService>();
        collection.AddScoped<IMessageService,MessageService>();
        collection.AddScoped<IUnitOfWork,UnitOfWork>();
        collection.AddScoped<LastActive>();
        collection.AddSignalR();
        collection.AddSingleton<PresenceTracker>();
        return collection;

    }
}
