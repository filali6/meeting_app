using System;
using System.Security.Claims;
using Backend.Models;
using Backend.Services.UsersService;

namespace Backend.Extensions;

public static class GetUserFromToken
{
    public async static Task<AppUser?> getUserToken(this ClaimsPrincipal User,IUsersService userRepository ){
        string? username=User.FindFirstValue(ClaimTypes.Name);
        if(username == null) throw new Exception("log in first");
        else return  await userRepository.GetUserByUsernameAsync(username);
    }
    public async static Task<AppUser?> getUserFromIdToken(this ClaimsPrincipal User,IUsersService userRepository ){
        string? id=User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(id == null) throw new Exception("log in first");
        else return  await userRepository.GetUserByIdAsync(int.Parse(id));
    }
}
