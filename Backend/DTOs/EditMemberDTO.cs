using System;

namespace Backend.DTOs;

public class EditMemberDTO
{
    public required string KnownAs {get;set;}
    public string? Introduction {get;set;}
    public string? Interests {get;set;}
    public string? LookingFor {get;set;}
    public string? City {get;set;}
    public string? Country {get;set;}
}
