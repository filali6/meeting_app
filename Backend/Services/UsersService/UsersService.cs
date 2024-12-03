using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.PhotoService;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Backend.Services.UsersService;

public class UsersService(DataContext context, IMapper mapper, IPhotoService photoService) : IUsersService
{
    private readonly IPhotoService photoService = photoService;
    private readonly DataContext context = context;
    private readonly IMapper mapper = mapper;

    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        var m=await context.Users
            .Where(x => x.UserName.ToLower() == username.ToLower())
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        return m;
    }
    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
        return await context.Users
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }
    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        var user = await context.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        if (user == null) throw new Exception("user not found");
        return user;
    }
    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
            .Include(x => x.Photos)
            .ToListAsync();
    }
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task toMainPhoto(string username, int photoId)
    {
        var userPhoto = await context.Photos.Where(p => p.AppUser.UserName == username).ToListAsync();
        if (userPhoto.Any(p => p.Id == photoId))
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
        await SaveAllAsync();
    }
    public async Task DeletePhoto(string username, int photoId)
    {
        var userPhoto = await context.Photos.Where(p => p.AppUser.UserName == username).FirstOrDefaultAsync(p => p.Id == photoId);
        if (userPhoto == null) throw new Exception("photo not found ");
        if (userPhoto.IsMain) throw new Exception("main photo can not be deleted");

        context.Entry(userPhoto).State = EntityState.Deleted;
        if (userPhoto.PublicId != null)
        {
            var result = await photoService.PhotoDeleteAsync(userPhoto.PublicId);
            if (result.Error != null) throw new Exception(result.Error.Message);
        }
        await context.SaveChangesAsync();

    }
}
