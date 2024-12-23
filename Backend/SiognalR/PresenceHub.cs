using System;
using Backend.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend.SiognalR;

[Authorize]
public class PresenceHub(PresenceTracker tracker):Hub
{

    public override async Task OnConnectedAsync()
    {
        if(Context.User==null)throw new HubException("can not ghet user claim");
        var isOnline=await tracker.userConnected(Context.User.getUsernameFromToken()!,Context.ConnectionId);
        if(isOnline)await Clients.Others.SendAsync("UserIsOnline",Context.User?.getUsernameFromToken());
        var currentUsers = await tracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("GetOnlineUsers",currentUsers);

    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {   
        if(Context.User==null)throw new HubException("can not ghet user claim");
        var isOffline=await tracker.UserDisconncected(Context.User.getUsernameFromToken()!,Context.ConnectionId);
        if(isOffline)await Clients.Others.SendAsync("UserIsOffline",Context.User?.getUsernameFromToken());
        await base.OnDisconnectedAsync(exception);

    }
}
