using System;
using System.Security.Claims;
using Backend.Extensions;
using Backend.Services.UsersService;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Helpers;

public class LastActive : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var res = await next();
        if(context.HttpContext.User.Identity?.IsAuthenticated !=true)return;
        var repo = res.HttpContext.RequestServices.GetRequiredService<IUsersService>();
        var user=await context.HttpContext.User.getUserFromIdToken(repo);
        if(user==null) return;
        user.LastActive=DateTime.Now;
        await repo.SaveAllAsync();

    }
}
