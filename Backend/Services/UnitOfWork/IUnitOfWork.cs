using System;
using Backend.Services.LikesService;
using Backend.Services.MessageService;
using Backend.Services.UsersService;

namespace Backend.Services.UnitOfWork;

public interface IUnitOfWork
{
    IUsersService UsersService{get;}
    IMessageService MessageService{get;}
    ILikeWriter LikeWriter { get; }
    ILikeReader LikeReader { get; }
    ILikeBrowser LikeBrowser { get; }
    Task<bool> Complete();
    bool HasChangers();
}
