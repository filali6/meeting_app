using System;
using System.Security.Claims;
using Backend.Extensions;
using Backend.Services.UnitOfWork;
using Backend.Services.UsersService;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Helpers;

public class LastActive : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var res = await next();
        if(context.HttpContext.User.Identity?.IsAuthenticated !=true)return;
        var unitOfWork = res.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
        var userId=context.HttpContext.User.getUserIdFromToken();
        if(userId==null) return;
        var user =await unitOfWork.UsersService.GetUserByIdAsync(userId);
        if(user==null) return;
        user.LastActive=DateTime.Now;
        await unitOfWork.Complete();

    }
}
