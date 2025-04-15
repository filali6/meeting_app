using Backend.Models;

namespace Backend.Services.LikesService;

public interface ILikeReader
{
    Task<UserLike?> GetUserLike(string sourceUser, string targetUser);
    Task<IEnumerable<string>> GetCurrentUserLikeIds(string userId);
}
