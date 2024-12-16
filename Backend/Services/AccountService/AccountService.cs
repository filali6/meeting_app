using System;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services.AccountService;

public class AccountService(UserManager<AppUser> userManager, IConfiguration conf, IMapper mapper , RoleManager<AppRole> roleManager) : IAccountService
{
    private IMapper _mapper = mapper;
    private IConfiguration _conf = conf;
    public async Task<GetUserLoginDTO?> LoginAsync(LoginDTO loginUser)
    {
        AppUser? user = await userManager.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(x => x.NormalizedUserName == loginUser.username.ToUpper());
            
        if (user == null || user.UserName==null) return null;
        var result = await userManager.CheckPasswordAsync(user,loginUser.password);
        if(!result)return null;
        var photos = user.Photos.FirstOrDefault(a => a.IsMain);
        return new GetUserLoginDTO()
        {
            token =await CreateToken(user),
            username = user.UserName,
            url = photos != null ? photos!.Url : _conf["NoPhoto"]
        };
    }

    public async Task<GetUserLoginDTO?> RegisterAsync(RegisterDTO registerUser)
    {
        if (await UserExist(registerUser.username)) return null;
        AppUser user = _mapper.Map<AppUser>(registerUser);
       
        var result = await userManager.CreateAsync(user,registerUser.password);
        if(!result.Succeeded)  throw new Exception("error with data");
        var token =await CreateToken(user);
        var photos = user.Photos.FirstOrDefault(a => a.IsMain);
        if(user.UserName==null) throw new Exception("no username for user");
        return new GetUserLoginDTO()
        {
            token = token,
            username = user.UserName,
            url = photos != null ? photos!.Url : _conf["NoPhoto"]
        };
    }
    private async Task<string> CreateToken(AppUser user)
    {
        var tokenKey = _conf["TokenKey"] ?? throw new Exception("TokenKey not found");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        if(user.UserName==null) throw new Exception("no username for user");
        var claims = new List<Claim>
            {
                
                new(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new(ClaimTypes.Name,user.UserName)
            };
        var roles=await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r=>new Claim(ClaimTypes.Role,r)));
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
        return await userManager.Users.AnyAsync(u => u.NormalizedUserName== username.ToUpper());
    }

}
