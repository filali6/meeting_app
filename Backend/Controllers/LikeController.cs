using System.Security.Claims;
using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
using Backend.Services.LikesService;
using Backend.Services.PhotoService;
using Backend.Services.UsersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Backend.Controllers
{

    [Authorize]
    public class LikeController(IUsersService userService, ILikesService likesService) : BaseApiController
    {
        private readonly ILikesService _likesService = likesService;
        private readonly IUsersService _userService = userService;

        [HttpPost("{targetId}")]
        public async Task<ActionResult> ToggleLike(string targetId)
        {
            var user = await User.getUserFromIdToken(_userService);
            if (user == null) return Unauthorized("log in please");
            if (user.Id == targetId) return BadRequest("do not like yourself");
            var likeExist = await _likesService.GetUserLike(user.Id, targetId);
            if (likeExist == null)
            {
                UserLike newLike = new UserLike
                {
                    SourceUserId = user.Id,
                    TargetUserId = targetId
                };
                _likesService.AddLike(newLike);
            }
            else
            {
                _likesService.DeleteLike(likeExist);
            }
            await _likesService.saveChangeAsync();

            return Ok();
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<string>>> GetCurrentUserLikeIds(){
            var user = await User.getUserFromIdToken(_userService);
            return Ok(await _likesService.GetCurrentUserLikeIds(user!.Id));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParams likesParams){
            
            var user = await User.getUserFromIdToken(_userService);
            likesParams.userId=user!.Id;
            var users = await _likesService.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }
    }
}
