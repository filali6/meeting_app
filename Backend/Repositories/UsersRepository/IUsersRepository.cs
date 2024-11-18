using System;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Repositories.UsersRepository;

public interface IUsersRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<MemberDto>> GetMembersAsync();
   Task<MemberDto?> GetMemberAsync(string username);
}
