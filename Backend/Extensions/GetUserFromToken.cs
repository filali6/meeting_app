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
        else return  await userRepository.GetUserByIdAsync(id);
    }
    public static string? getUsernameFromToken(this ClaimsPrincipal User ){
        return User.FindFirstValue(ClaimTypes.Name);
    }
        public static string? getUserIdFromToken(this ClaimsPrincipal User ){
        var userId =    User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId==null) throw new Exception("problem with token");
        return userId;
    }
    
}
