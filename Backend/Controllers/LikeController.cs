using System.Security.Claims;
using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
using Backend.Services.LikesService;
using Backend.Services.PhotoService;
using Backend.Services.UnitOfWork;
using Backend.Services.UsersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Backend.Controllers
{

    [Authorize]
    public class LikeController(IUnitOfWork _UnitOfWork) : BaseApiController
    {
        
        [HttpPost("{targetId}")]
        public async Task<ActionResult> ToggleLike(string targetId)
        {
           
            var userId =User.getUserIdFromToken();
            if (userId==null || userId == targetId) return BadRequest("do not like yourself");
            var likeExist = await  _UnitOfWork.LikeService.GetUserLike(userId, targetId);
            if (likeExist == null)
            {
                UserLike newLike = new UserLike
                {
                    SourceUserId = userId,
                    TargetUserId = targetId
                };
                 _UnitOfWork.LikeService.AddLike(newLike);
            }
            else
            {
                 _UnitOfWork.LikeService.DeleteLike(likeExist);
            }
            await  _UnitOfWork.Complete();

            return Ok();
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<string>>> GetCurrentUserLikeIds(){
            var user =  User.getUserIdFromToken();
            if(user==null) return BadRequest();
            return Ok(await  _UnitOfWork.LikeService.GetCurrentUserLikeIds(user));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParams likesParams){
            
            var userId = User.getUserIdFromToken();
            likesParams.userId=userId;
            var users = await  _UnitOfWork.LikeService.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }
    }
}
