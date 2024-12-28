using System.Security.Claims;
using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
using Backend.Services.PhotoService;
using Backend.Services.UnitOfWork;
using Backend.Services.UsersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
[Authorize]
    public class UserController(ILogger<UserController> logger, IUnitOfWork _unitOfWork,IPhotoService  photo,IMapper mapper) : BaseApiController
    {
        private readonly IMapper mapper=mapper;
        private readonly IPhotoService  _photo = photo;
        private readonly ILogger<UserController> _logger = logger;

        [Authorize(Roles ="member")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            userParams.CurrentUser=User.getUsernameFromToken();
            var users = await  _unitOfWork.UsersService.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }

        [Authorize( Roles ="admin")]

        
        [HttpGet("{username}")]  
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var currentUsername = User.getUsernameFromToken();
            if(currentUsername==null)return BadRequest("can not find user! please login !");
            var user = await  _unitOfWork.UsersService.GetMemberAsync(username,username.ToUpper()==currentUsername.ToUpper());

            if (user == null) return NotFound();

            return user;
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(EditMemberDTO member)
        {
            
            string? username=User.FindFirstValue(ClaimTypes.Name);
            if(username == null)return Unauthorized("login to update your profile");
            await  _unitOfWork.UsersService.UpdateMember(username,member);
            return NoContent();
        }
         
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> UploadPhoto(IFormFile file)
        {
            
            var userId= User.getUserIdFromToken();
            var user=await _unitOfWork.UsersService.GetUserByIdAsync(userId!);
            var result = await _photo.PhotoUploadAsync(file);
            if(result.Error!=null) return BadRequest(result.Error.Message);
            Photo photo= new()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId=result.PublicId
            };
            user!.Photos.Add(photo);
            await  _unitOfWork.Complete();
            return CreatedAtAction(nameof(GetUser),new {username=user.UserName},mapper.Map<PhotoDTO>(photo));
        }
        [HttpPut("{idPhoto}")]
        public async Task<ActionResult> toMain(int idPhoto)
        {
           string? username=User.FindFirstValue(ClaimTypes.Name);
            if(username == null)return Unauthorized("login to update your profile");
            await  _unitOfWork.UsersService.toMainPhoto(username,idPhoto);
            return NoContent(); 
        }
        [HttpDelete("{idPhoto}")]
        public async Task<ActionResult> DeletePhoto(int idPhoto)
        {
           string? username=User.FindFirstValue(ClaimTypes.Name);
            if(username == null)return Unauthorized("login to update your profile");
            await  _unitOfWork.UsersService.DeletePhoto(username,idPhoto);
            return Ok(); 
        }
         
    }
}
