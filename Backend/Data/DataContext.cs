using System;
using Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext(options) 
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<UserLike> Likes {get;set;}
    public DbSet<Message> Messages {get;set;}
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

        modelBuilder.Entity<Message>()
            .HasOne(s=>s.SourceUser)
            .WithMany(l=>l.MessagesSent)
            .HasForeignKey(s=>s.SourceUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(s=>s.TargetUser)
            .WithMany(l=>l.MessagesReceived)
            .HasForeignKey(s=>s.TargetUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
