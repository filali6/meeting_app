using System;
using System.Security.Claims;
using Backend.Models;
using Backend.Services.UsersService;

namespace Backend.Extensions;

public static class GetUserFromToken
{
    public async static Task<AppUser?> getUserToken(this ClaimsPrincipal User,IUsersService userRepository ){
        string? username=User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(username == null) throw new Exception("log in first");
        else return  await userRepository.GetUserByUsernameAsync(username);
    }
}
