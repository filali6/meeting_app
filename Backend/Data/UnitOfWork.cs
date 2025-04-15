using System;
using System.Threading.Tasks;
using Backend.Services.LikesService;
using Backend.Services.MessageService;
using Backend.Services.UsersService;
using Backend.Services.UnitOfWork;

namespace Backend.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext context; 
    private readonly IUsersService usersService; 
    private readonly IMessageService messageService;
  
    private readonly ILikeWriter likeWriter;
    private readonly ILikeReader likeReader;
    private readonly ILikeBrowser likeBrowser;
    public UnitOfWork(
        DataContext context,
        IUsersService usersService,
        IMessageService messageService,
       ILikeWriter likeWriter,
    ILikeReader likeReader,
    ILikeBrowser likeBrowser)
    {
        this.context = context;
        this.usersService = usersService;
        this.messageService = messageService;
        this.likeWriter = likeWriter;
        this.likeReader = likeReader;
        this.likeBrowser = likeBrowser;
    }

    public IUsersService UsersService => usersService;
    public IMessageService MessageService => messageService;

    // Interface Segregation Principle appliqué ici :
    public ILikeWriter LikeWriter => likeWriter;
    public ILikeReader LikeReader => likeReader;
    public ILikeBrowser LikeBrowser => likeBrowser;

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChangers()
    {
        return context.ChangeTracker.HasChanges();
    }
}