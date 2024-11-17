using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class RegisterDTO
{
    [Required]
    [MaxLength(50)]
    public  string username { get; set; }="";
    [Required]
    [System.ComponentModel.PasswordPropertyText]
    public  string password { get; set; }="";
}
