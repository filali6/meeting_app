using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    public class AccountController(DataContext context,IConfiguration configuration) : BaseApiController
    {
        private readonly IConfiguration _conf=configuration;
        private readonly DataContext _db = context;
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO regiterUser)
        {
            try
            {
                if (await UserExist(regiterUser.username)) throw new Exception("username exists");
                AppUser user;
                using (var hmac = new HMACSHA256())
                {
                    user = new AppUser
                    {
                        UserName = regiterUser.username,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(regiterUser.password)),
                        PasswordSalt = hmac.Key
                    };
                }
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<GetUserLoginDTO>> Login(LoginDTO loginUser)
        {
            AppUser? user =await _db.Users.FirstOrDefaultAsync(x=>x.UserName==loginUser.username);
            if(user==null)return Unauthorized("user does not exist");
            using(var hmac=new HMACSHA256(user.PasswordSalt)){
                var compHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUser.password));
                for(int i=0 ; i<compHash.Length;i++){
                    if(compHash[i]!=user.PasswordHash[i])return Unauthorized("wrong password");
                }
            }
            var token = CreateToken(user);
            return new GetUserLoginDTO(){token=token,username=user.UserName};
        }

        private string CreateToken(AppUser user)
        {
            var tokenKey=_conf["TokenKey"]?? throw new Exception("TokenKey not found");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier,user.UserName)
            };
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature); 
            var tokenDescriptor= new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };
            var TokenHandler=new JsonWebTokenHandler();
            return TokenHandler.CreateToken(tokenDescriptor);

        }


        private async Task<bool> UserExist(string username)
        {
            return await _db.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
        }


    }
}
