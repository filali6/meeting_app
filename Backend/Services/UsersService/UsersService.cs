using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Data;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Backend.Services.PhotoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Backend.Services.UsersService;


public class UsersService(DataContext context, IMapper mapper, IPhotoService photoService , ILogger<IUsersService> logger) : IUsersService
{
    private readonly IPhotoService photoService = photoService;
    private readonly DataContext context = context;
    private readonly IMapper mapper = mapper;
    private readonly ILogger<IUsersService> _logger=logger;

    public async Task<MemberDto?> GetMemberAsync(string username,bool isConnected)
    {

        var m = isConnected?
            await context.Users
                .Where(x => x.NormalizedUserName == username.ToUpper())
                .Include(u=>u.Photos)
                .SingleOrDefaultAsync()
            :await context.Users
                .Where(x => x.NormalizedUserName == username.ToUpper())
                .Include(u=>u.Photos.Where(p => p.Approuved == true))
                .SingleOrDefaultAsync();
        return mapper.Map<MemberDto>(m);
    }
    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {

        var query = context.Users.AsQueryable();
        query = query.Where(x => x.UserName != userParams.CurrentUser);
        _logger.LogInformation(userParams.Gender);
        if ( userParams.Gender=="Male" || userParams.Gender=="Female")
         query = userParams.Gender == "Male" ? query.Where(u => u.IsMale) : query.Where(u => !u.IsMale);
         //var minDob=DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge-1));
        //var maxDob=DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge-1));

        //query=query.Where(u=>u.DateBirth>=minDob && u.DateBirth<=maxDob);
        query=query.Include(u=>u.Photos.Where(p => p.Approuved == true));
        return await PagedList<MemberDto>.CreatAsync(query.Select(u=>mapper.Map<MemberDto>(u))
           , userParams.pageNumber, userParams.PageSize);
    }
    public async Task<AppUser?> GetUserByIdAsync(string id)
    {
        return await context.Users.FindAsync(id);
    }
    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        var user = await context.Users
            .Include(x => x.Photos)
            .Include(x=>x.Likes)
            .SingleOrDefaultAsync(x => x.UserName == username);
        if (user == null) throw new Exception("user "+username+" not found");
        return user;
    }
    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
            .Include(x => x.Photos)
            .ToListAsync();
    }


    public async Task toMainPhoto(string username, int photoId)
    {
        var userPhoto = await context.Photos.Where(p => p.AppUser.UserName == username).ToListAsync();
        if (userPhoto.Any(p => p.Id == photoId&&p.Approuved))
        {
            userPhoto.ForEach(p =>
            {
                if (p.Id == photoId) p.IsMain = true;
                else p.IsMain = false;
            });
            await context.SaveChangesAsync();
        }
        else throw new Exception("photo not found with user");
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    public async Task UpdateMember(string username, EditMemberDTO member)
    {
        AppUser? user = await GetUserByUsernameAsync(username);
        mapper.Map(member, user);
    }
    public async Task DeletePhoto(string username, int photoId)
    {
        var userPhoto = await context.Photos.Where(p => p.AppUser.UserName == username).FirstOrDefaultAsync(p => p.Id == photoId) ?? throw new Exception("photo not found ");
        if (userPhoto.IsMain) throw new Exception("main photo can not be deleted");

        context.Entry(userPhoto).State = EntityState.Deleted;
        if (userPhoto.PublicId != null)
        {
            var result = await photoService.PhotoDeleteAsync(userPhoto.PublicId);
            if (result.Error != null) throw new Exception(result.Error.Message);
        }
        await context.SaveChangesAsync();

    }

    public async Task<bool> ApprouvePhoto(int photoId)
    {
        var photo = await context.Photos.FirstOrDefaultAsync(p=>p.Id==photoId && !p.Approuved);
        if(photo==null)return false;
        photo.Approuved=true;
        return true;
    }
    public async Task<List<PhotoDTO>> GetPhotosToApprouve()
    {
        return await context.Photos.Where(p=>!p.Approuved).ProjectTo<PhotoDTO>(mapper.ConfigurationProvider).ToListAsync();
    }

}
