using System;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Services.LikesService;

public interface ILikesService
{
    Task<UserLike?> GetUserLike(int sourceUser,int TargetUser);
    Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
    Task<IEnumerable<int>> GetCurrentUserLikeIds(int userId);
    void DeleteLike(UserLike like);
    void AddLike(UserLike like);
    Task<bool> saveChangeAsync();
}
