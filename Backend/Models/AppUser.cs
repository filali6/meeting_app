using System.ComponentModel.DataAnnotations;
using Backend.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;
public class AppUser:IdentityUser
{
    public DateOnly DateBirth { get; set; }
    public required string KnownAs {get;set;}="tunisia";
    public DateTime Created {get;set;}=DateTime.UtcNow;
    public DateTime LastActive {get;set;}=DateTime.UtcNow;
    public required bool IsMale {get;set;}
    public string? Introduction {get;set;}
    public string? Interests {get;set;}
    public string? LookingFor {get;set;}
    public string? City {get;set;}
    public string? Country {get;set;}
    public List<Photo> Photos{get;set;}=[];
    public List<UserLike> Likes{get;set;}=[];
    public List<UserLike> LikedBy{get;set;}=[];
   public List<Message> MessagesSent{get;set;}=[];
    public List<Message> MessagesReceived{get;set;}=[];
}