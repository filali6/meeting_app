using Backend.DTOs;
using Backend.Helpers;

namespace Backend.Services.LikesService;

public interface ILikeBrowser
{
    Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
}
