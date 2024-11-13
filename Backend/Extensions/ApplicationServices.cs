using System;
using Backend.Data;
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
        return collection;
    }
}
