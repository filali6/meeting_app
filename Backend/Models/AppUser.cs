using System.ComponentModel.DataAnnotations;
using Backend.Extensions;

namespace Backend.Models;
public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public  Byte[] PasswordHash { get; set; }=[];
    public  Byte[] PasswordSalt { get; set; }=[];
    public DateOnly DateBirth { get; set; }
    public required string KnownAs {get;set;}
    public DateTime Created {get;set;}=DateTime.UtcNow;
    public DateTime LastActive {get;set;}=DateTime.UtcNow;
    public required bool IsMale {get;set;}
    public string? Introduction {get;set;}
    public string? Interests {get;set;}
    public string? LookingFor {get;set;}
    public required string City {get;set;}
    public required string Country {get;set;}
    public List<Photo> Photos{get;set;}=[];
   
}
