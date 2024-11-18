using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class LoginDTO
{
    [Required]
    [MaxLength(50)]
    public  string username { get; set; }="";
    [Required]
    [System.ComponentModel.PasswordPropertyText]
    [MinLength(3)]
    public  string password { get; set; }="";
}
