using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    public class AccountController(IAccountService account) : BaseApiController
    {
        private readonly IAccountService _account=account;
        [HttpPost("register")]
        public async Task<ActionResult<GetUserLoginDTO>> Register(RegisterDTO registerUser)
        {
            
            var login = await _account.RegisterAsync(registerUser);
            if(login == null)return Unauthorized("user can not register");
            return login;
                
       
        }

        [HttpPost("login")]
        public async Task<ActionResult<GetUserLoginDTO>> Login(LoginDTO loginUser)
        {
            var login = await _account.LoginAsync(loginUser);
            if(login == null)return Unauthorized("user can not login");
            return login;
        }



    }
}
