using Backend.Models;
using Backend.Helpers;
using Microsoft.Extensions.Logging;

namespace Backend.Services.UsersService.Strategy;

public class GenderFilterStrategy : IUserFilterStrategy
{
    private readonly ILogger<GenderFilterStrategy> _logger;

    public GenderFilterStrategy(ILogger<GenderFilterStrategy> logger)
    {
        _logger = logger;
    }

    public IQueryable<AppUser> ApplyFilter(IQueryable<AppUser> query, UserParams userParams)
    {
        _logger.LogInformation("✅ GenderFilterStrategy applied");

        if (userParams.Gender == "Male" || userParams.Gender == "Female")
        {
            return userParams.Gender == "Male"
                ? query.Where(u => u.IsMale)
                : query.Where(u => !u.IsMale);
        }

        return query;
    }
}
