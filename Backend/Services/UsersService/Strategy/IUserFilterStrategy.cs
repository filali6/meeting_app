using System.Linq;
using Backend.Models;
using Backend.Helpers;

namespace Backend.Services.UsersService.Strategy;

public interface IUserFilterStrategy
{
    IQueryable<AppUser> ApplyFilter(IQueryable<AppUser> query, UserParams userParams);
}