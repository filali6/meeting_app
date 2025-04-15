using Backend.Models;

namespace Backend.Services.LikesService;

public interface ILikeWriter
{
    void AddLike(UserLike like);
    void DeleteLike(UserLike like);
}
