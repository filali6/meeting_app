using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{

    public class UserController(ILogger<WeatherForecastController> logger, DataContext context) : BaseApiController
    {
        private readonly ILogger<WeatherForecastController> _logger = logger;
        private readonly DataContext _db = context;

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetByID(int id)
        {
            AppUser? res= await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(res == null) return NotFound();
            else return Ok(res);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> Users()
        {
            IEnumerable<AppUser> res=await  _db.Users.ToListAsync();
            if(res.Any())  return Ok(res);
            else  return NotFound();
        }
    }
}
