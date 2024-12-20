using System;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Services.LikesService;

public interface ILikesService
{
    Task<UserLike?> GetUserLike(string sourceUser,string TargetUser);
    Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
    Task<IEnumerable<string>> GetCurrentUserLikeIds(string userId);
    void DeleteLike(UserLike like);
    void AddLike(UserLike like);
    Task<bool> saveChangeAsync();
}
