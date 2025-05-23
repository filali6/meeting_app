using System;

namespace Backend.DTOs;

public class MemberDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public int Age { get; set; }
    public string? PhotoUrl { get; set; }
    public required string KnownAs {get;set;}
    public DateTime Created {get;set;}
    public DateTime LastActive {get;set;}
    public  bool IsMale {get;set;}
    public string? Introduction {get;set;}
    public string? Interests {get;set;}
    public string? LookingFor {get;set;}
    public string? City {get;set;}
    public string? Country {get;set;}
    public List<PhotoDTO>? Photos{get;set;}
}
