using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.UsersRepository;

public class UsersRepository(DataContext context,IMapper mapper) : IUsersRepository
{
    private readonly DataContext context = context;
    private readonly IMapper mapper = mapper;

     public async Task<MemberDto?> GetMemberAsync(string username)
     {
         return await context.Users
             .Where(x => x.UserName.ToLower() == username.ToLower())
             .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
             .SingleOrDefaultAsync();
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
        return await context.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
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
    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
}
