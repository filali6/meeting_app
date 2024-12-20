using System;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services.AccountService;

public class AccountService(UserManager<AppUser> userManager, IConfiguration conf, IMapper mapper) : IAccountService
{
    private IMapper _mapper = mapper;
    private IConfiguration _conf = conf;
    public async Task<GetUserLoginDTO?> LoginAsync(LoginDTO loginUser)
    {
        AppUser? user = await userManager.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(x => x.UserName == loginUser.username);
        if (user == null) return null;
       var resultPasswordCheck=await userManager.CheckPasswordAsync(user,loginUser.password);
       if(!resultPasswordCheck) return null;
        var token =await CreateToken(user);
        var photos = user.Photos.FirstOrDefault(a => a.IsMain);
        return new GetUserLoginDTO()
        {
            token = token,
            username = user.UserName!,
            url = photos != null ? photos!.Url : _conf["NoPhoto"]
        };
    }

    public async Task<GetUserLoginDTO?> RegisterAsync(RegisterDTO registerUser)
    {
        if (await UserExist(registerUser.username)) return null;
        AppUser user = _mapper.Map<AppUser>(registerUser);
        
        await userManager.CreateAsync(user,registerUser.password);
        var token = await CreateToken(user);
        var photos = user.Photos.FirstOrDefault(a => a.IsMain);

        return new GetUserLoginDTO()
        {
            token = token,
            username = user.UserName!,
            url = photos != null ? photos!.Url : _conf["NoPhoto"]
        };
    }
    private async Task<string> CreateToken(AppUser user)
    {
        var tokenKey = _conf["TokenKey"] ?? throw new Exception("TokenKey not found");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new(ClaimTypes.Name,user.UserName!)
            };
        var userRoles = await userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };
        var TokenHandler = new JsonWebTokenHandler();
        return TokenHandler.CreateToken(tokenDescriptor);

    }


    private async Task<bool> UserExist(string username)
    {
        return await userManager.Users.AnyAsync(u => u.NormalizedUserName == username.ToUpper());
    }

}
