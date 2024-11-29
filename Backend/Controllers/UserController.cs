using System.Security.Claims;
using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Extensions;
using Backend.Models;
using Backend.Services.PhotoService;
using Backend.Services.UsersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{

[Authorize]
    public class UserController(ILogger<WeatherForecastController> logger, IUsersService userRepository,IPhotoService  photo,IMapper mapper) : BaseApiController
    {
        private readonly IMapper mapper=mapper;
        private readonly IPhotoService  _photo = photo;
        private readonly ILogger<WeatherForecastController> _logger = logger;
        private readonly IUsersService userRepository = userRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await userRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{username}")]  
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await userRepository.GetMemberAsync(username);

            if (user == null) return NotFound();

            return user;
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(EditMemberDTO member)
        {
            
            string? username=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(username == null)return Unauthorized("login to update your profile");
            await userRepository.UpdateMember(username,member);
            return NoContent();
        }
         
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> UploadPhoto(IFormFile file)
        {
            
            var user=await User.getUserToken(userRepository);
            var result = await _photo.PhotoUploadAsync(file);
            if(result.Error!=null) return BadRequest(result.Error.Message);
            Photo photo= new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId=result.PublicId
            };
            user!.Photos.Add(photo);
            await userRepository.SaveAllAsync();
            return CreatedAtAction(nameof(GetUser),new {username=user.UserName},mapper.Map<PhotoDTO>(photo));
        }
        [HttpPut("{idPhoto}")]
        public async Task<ActionResult> toMain(int idPhoto)
        {
           string? username=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(username == null)return Unauthorized("login to update your profile");
            await userRepository.toMainPhoto(username,idPhoto);
            return NoContent(); 
        }
        [HttpDelete("{idPhoto}")]
        public async Task<ActionResult> DeletePhoto(int idPhoto)
        {
           string? username=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(username == null)return Unauthorized("login to update your profile");
            await userRepository.DeletePhoto(username,idPhoto);
            return Ok(); 
        }
         
    }
}
