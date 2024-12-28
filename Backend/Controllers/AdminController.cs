using Backend.Data;
using Backend.Models;
using Backend.Services.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{

    public class AdminController(UserManager<AppUser> userManager,IUnitOfWork unitOfWork) : BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await userManager.Users
                .OrderBy(x => x.UserName) // Force client-side evaluation
                .ToListAsync();

            var usersWithRoles = new List<object>();
            foreach (var user in users)
            {
                usersWithRoles.Add(new
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Roles = await userManager.GetRolesAsync(user)
                });
            }
            return Ok(usersWithRoles);
        }
        
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("you must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("User not found");

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModerationAsync()
        {
            var photos = await unitOfWork.UsersService.GetPhotosToApprouve();

            return Ok(photos);
        }
        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPut("approuve/{id:int}")]
        public async Task<ActionResult> ApprouvePhoto(int id)
        {
            var result =await unitOfWork.UsersService.ApprouvePhoto(id);
            if(!result) return BadRequest("can not approuve photo");
            await unitOfWork.Complete();
            return NoContent();
        }
    }
}
