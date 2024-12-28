using System;
using Backend.Services.LikesService;
using Backend.Services.MessageService;
using Backend.Services.UnitOfWork;
using Backend.Services.UsersService;

namespace Backend.Data;

public class UnitOfWork (DataContext context,IUsersService usersService,IMessageService messageService,ILikesService likeService): IUnitOfWork
{
    public IUsersService UsersService => usersService;

    public IMessageService MessageService => messageService;

    public ILikesService LikeService => likeService;

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync()>0;
    }

    public bool HasChangers()
    {
        return context.ChangeTracker.HasChanges();
    }
}
