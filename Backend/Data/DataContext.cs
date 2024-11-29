using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class DataContext(DbContextOptions options) : DbContext(options) 
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Photo> Photos { get; set; }
}
