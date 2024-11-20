using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Repositories.UsersRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{

[Authorize]
    public class UserController(ILogger<WeatherForecastController> logger, IUsersRepository userRepository) : BaseApiController
    {
        private readonly ILogger<WeatherForecastController> _logger = logger;
        private readonly IUsersRepository userRepository = userRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await userRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{username}")]  // /api/users/2
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await userRepository.GetMemberAsync(username);

            if (user == null) return NotFound();

            return user;
        }
    }
}
