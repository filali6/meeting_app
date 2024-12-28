using System;
using Backend.Services.LikesService;
using Backend.Services.MessageService;
using Backend.Services.UsersService;

namespace Backend.Services.UnitOfWork;

public interface IUnitOfWork
{
    IUsersService UsersService{get;}
    IMessageService MessageService{get;}
    ILikesService LikeService{get;}
    Task<bool> Complete();
    bool HasChangers();
}
