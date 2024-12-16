using System;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class AppRole:IdentityRole<int>
{
    public ICollection<AppUserRole> UserRoles {get;set;}= [];
}
