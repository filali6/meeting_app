using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class DataContext(DbContextOptions options) : DbContext(options) 
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<UserLike> Likes {get;set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserLike>()
            .HasKey(k=>new{k.SourceUserId,k.TargetUserId});
        modelBuilder.Entity<UserLike>()
            .HasOne(s=>s.SourceUser)
            .WithMany(l=>l.Likes)
            .HasForeignKey(s=>s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserLike>()
            .HasOne(s=>s.TargetUser)
            .WithMany(l=>l.LikedBy)
            .HasForeignKey(s=>s.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
