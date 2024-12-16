using System;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class AppUserRole:IdentityUserRole<int>
{
    public AppUser User {get;set;}=null!;
    public AppRole UserRole { get; set; }=null!;
}
