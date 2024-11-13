using System.ComponentModel.DataAnnotations;

namespace Backend.Models;
public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required Byte[] PasswordHash { get; set; }
    public required Byte[] PasswordSalt { get; set; }

}