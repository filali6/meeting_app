using System;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.UsersService;

public interface IUsersService
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<MemberDto>> GetMembersAsync();
   Task<MemberDto?> GetMemberAsync(string username);
   Task UpdateMember(string username,EditMemberDTO member);
   Task toMainPhoto(string username, int photoId);
   Task DeletePhoto(string username, int photoId);
}
