using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class RegisterDTO
{
    [Required]
    [MaxLength(50)]
    public string username { get; set; } = "";
    [Required]
    public string password { get; set; } = "";
    [Required]
    public DateOnly DateBirth { get; set; }
    [Required] 
    public required string KnownAs { get; set; }
    [Required] 
    public required bool IsMale { get; set; }
    [Required] 
    public string? Introduction { get; set; }
    [Required] 
    public string? Interests { get; set; }
    [Required] 
    public string? LookingFor { get; set; }
    [Required] 
    public string? City { get; set; }
    [Required] 
    public string? Country { get; set; }
}
