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
    public class UserController(ILogger<UserController> logger, IUnitOfWork _unitOfWork, IPhotoService photo, IMapper mapper) : BaseApiController
    {
        private readonly IMapper mapper = mapper;
        private readonly IPhotoService _photo = photo;
        private readonly ILogger<UserController> _logger = logger;

        [Authorize(Roles = "member")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.CurrentUser = User.getUsernameFromToken();
            var users = await _unitOfWork.UsersService.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }

        [Authorize(Roles = "admin,member")]
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var currentUsername = User.getUsernameFromToken();
            if (currentUsername == null) return BadRequest("Cannot find user! Please login.");
            var user = await _unitOfWork.UsersService.GetMemberAsync(username, username.ToUpper() == currentUsername.ToUpper());

            if (user == null) return NotFound();

            return user;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(EditMemberDTO member)
        {
            string? username = User.FindFirstValue(ClaimTypes.Name);
            if (username == null) return Unauthorized("Login to update your profile.");
            await _unitOfWork.UsersService.UpdateMember(username, member);
            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> UploadPhoto(IFormFile file)
        {
            var userId = User.getUserIdFromToken();
            var user = await _unitOfWork.UsersService.GetUserByIdAsync(userId!);

            var result = await _photo.PhotoUploadAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            Photo photo = new()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                // ✅ Contrainte OCL : garantir une photo principale
                IsMain = !user.Photos.Any(p => p.IsMain)
            };

            user!.Photos.Add(photo);
            await _unitOfWork.Complete();

            return CreatedAtAction(
                nameof(GetUser),
                new { username = user.UserName },
                mapper.Map<PhotoDTO>(photo)
            );
        }

        [HttpPut("{idPhoto}")]
        public async Task<ActionResult> toMain(int idPhoto)
        {
            string? username = User.FindFirstValue(ClaimTypes.Name);
            if (username == null) return Unauthorized("Login to update your profile.");
            await _unitOfWork.UsersService.toMainPhoto(username, idPhoto);
            return NoContent();
        }

        [HttpDelete("{idPhoto}")]
        public async Task<ActionResult> DeletePhoto(int idPhoto)
        {
            string? username = User.FindFirstValue(ClaimTypes.Name);
            if (username == null) return Unauthorized("Login to update your profile");

            var user = await _unitOfWork.UsersService.GetUserByUsernameAsync(username);
            if (user == null) return NotFound("User not found");

            var photo = user.Photos.FirstOrDefault(p => p.Id == idPhoto);
            if (photo == null) return NotFound("Photo not found");

            // ❌ Empêcher suppression si c'est la SEULE photo principale
            if (photo.IsMain && user.Photos.Count(p => p.IsMain) == 1)
            {
                return BadRequest("Vous ne pouvez pas supprimer votre seule photo principale.");
            }

            // ✅ Suppression de la photo
            if (!string.IsNullOrEmpty(photo.PublicId))
            {
                var deleteResult = await _photo.DeletePhotoAsync(photo.PublicId);
                if (deleteResult.Error != null)
                    return BadRequest(deleteResult.Error.Message);
            }

            user.Photos.Remove(photo);

            // ✅ Si on supprime la photo principale, désigner une autre comme principale
            if (photo.IsMain)
            {
                var firstRemaining = user.Photos.FirstOrDefault();
                if (firstRemaining != null)
                    firstRemaining.IsMain = true;
            }

            await _unitOfWork.Complete();

            return Ok("Photo supprimée avec succès.");
        }
    }
}
