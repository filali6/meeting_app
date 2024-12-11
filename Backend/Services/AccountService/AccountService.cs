using System;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services.AccountService;

public class AccountService(DataContext db, IConfiguration conf, IMapper mapper) : IAccountService
{
    private DataContext _db = db;
    private IMapper _mapper = mapper;
    private IConfiguration _conf = conf;
    public async Task<GetUserLoginDTO?> LoginAsync(LoginDTO loginUser)
    {
        AppUser? user = await _db.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(x => x.UserName == loginUser.username);
        if (user == null) return null;
        using (var hmac = new HMACSHA256(user.PasswordSalt))
        {
            var compHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUser.password));
            for (int i = 0; i < compHash.Length; i++)
            {
                if (compHash[i] != user.PasswordHash[i]) return null;
            }
        }
        var token = CreateToken(user);
        var photos = user.Photos.FirstOrDefault(a => a.IsMain);
        return new GetUserLoginDTO()
        {
            token = token,
            username = user.UserName,
            url = photos != null ? photos!.Url : _conf["NoPhoto"]
        };
    }

    public async Task<GetUserLoginDTO?> RegisterAsync(RegisterDTO registerUser)
    {
        if (await UserExist(registerUser.username)) return null;
        AppUser user = _mapper.Map<AppUser>(registerUser);
        using (var hmac = new HMACSHA256())
        {
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.password));
            user.PasswordSalt = hmac.Key;

        }
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        var token = CreateToken(user);
        var photos = user.Photos.FirstOrDefault(a => a.IsMain);

        return new GetUserLoginDTO()
        {
            token = token,
            username = user.UserName,
            url = photos != null ? photos!.Url : _conf["NoPhoto"]
        };
    }
    private string CreateToken(AppUser user)
    {
        var tokenKey = _conf["TokenKey"] ?? throw new Exception("TokenKey not found");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
                new(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new(ClaimTypes.Name,user.UserName)
            };
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
        return await _db.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
    }

}
