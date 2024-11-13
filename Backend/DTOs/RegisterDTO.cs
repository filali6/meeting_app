using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class RegisterDTO
{
    [Required]
    [MaxLength(50)]
    public required string username { get; set; }
    [Required]
    [System.ComponentModel.PasswordPropertyText]
    public required string password { get; set; }
}
