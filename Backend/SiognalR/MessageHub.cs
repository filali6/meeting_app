using System;
using AutoMapper;
using Backend.DTOs;
using Backend.Extensions;
using Backend.Models;
using Backend.Services.MessageService;
using Backend.Services.UnitOfWork;
using Backend.Services.UsersService;
using Microsoft.AspNetCore.SignalR;
using Backend.Services.MessageCommand;

namespace Backend.SiognalR;

public class MessageHub(IUnitOfWork unitOfWork,
  IMapper mapper,
  IHubContext<PresenceHub> presenceHub) : Hub
{

    public override async Task OnConnectedAsync()
    {
        var context = Context.GetHttpContext();
        var otherUser = context!.Request.Query["user"];
        if (context.User == null || string.IsNullOrEmpty(otherUser)) throw new HubException("can not create group");
        var groupName = GetGroupName(otherUser!, Context.User!.getUsernameFromToken()!);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var  group = await AddToGroup(groupName);
        await Clients.Group(groupName).SendAsync("updatedGroup",group);
        var messages = await unitOfWork.MessageService.GetMessageThread(context.User.getUsernameFromToken()!, otherUser!);
        if(unitOfWork.HasChangers())await unitOfWork.Complete();
        await Clients.Caller.SendAsync("RecieveMessageThread", messages);
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var group=await RemoveFromGroup();
        await Clients.Group(group.Name).SendAsync("updatedGroup",group);
        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendMessage(CreateMessageDTO creatMessageDTO)
    {
        var username = Context.User?.getUsernameFromToken() ?? throw new HubException("can not get user");
        if (username == null) throw new HubException("user not identified");
        if (username.ToLower() == creatMessageDTO.TargetUsername.ToLower()) throw new HubException("do not message yourself");
        var sender = await  unitOfWork.UsersService.GetUserByUsernameAsync(username);
        var receiver = await  unitOfWork.UsersService.GetUserByUsernameAsync(creatMessageDTO.TargetUsername);
        if (sender == null || receiver == null) throw new HubException("can not send message");
        Message newMessage = new()
        {
            SourceUser = sender,
            SourceUserId = sender.Id,
            TargetUser = receiver,
            TargetUserId = receiver.Id,
            Content = creatMessageDTO.Content
        };
        var groupName = GetGroupName(receiver.UserName!, sender.UserName!);
        var group = await unitOfWork.MessageService.GetGroup(groupName);
        if (group != null && group.Connections.Any(x => x.Username == receiver.UserName))
        {
            newMessage.ReadDate = DateTime.UtcNow;
        }
        else
        {
            var connection = await PresenceTracker.GetConnectionsForUser(receiver.UserName!);
            if (connection != null && connection.Count != 0)
            {
                await presenceHub.Clients.Clients(connection).SendAsync("NewMessageRecieved", new
                {
                    Username = sender.UserName,
                    KnownAs = sender.KnownAs
                });
            }

        }

        //unitOfWork.MessageService.AddMessage(newMessage);
        var command = new SendMessageCommand(unitOfWork.MessageService, newMessage);
        await new MessageCommandDispatcher().DispatchAsync(command);
        Console.WriteLine("🧠 Commande créée dans le Hub");
        
        if (await unitOfWork.Complete())
        {
            await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDTO>(newMessage));
        }
        else throw new HubException("can not send message");
    }

    private string GetGroupName(string coller, string other)
    {
        var compare = string.CompareOrdinal(coller, other) < 0;
        return compare ? $"{coller}-{other}" : $"{other}-{coller}";
    }

    private async Task<Group> AddToGroup(string groupName)
    {
        var username = Context.User?.getUsernameFromToken() ?? throw new HubException("can not get user");
        var group = await unitOfWork.MessageService.GetGroup(groupName);
        var connection = new Connection
        {
            ConnectionId = Context.ConnectionId,
            Username = username
        };
        if (group == null)
        {
            group = new Group { Name = groupName };
            unitOfWork.MessageService.AddGroup(group);
        }
        group.Connections.Add(connection);
        if (await unitOfWork.Complete()) return group;
        else throw new HubException("failed to join group");
    }
    private async Task<Group> RemoveFromGroup()
    {
        var group=await unitOfWork.MessageService.GetGroupFromConnection(Context.ConnectionId);
        var connection = group?.Connections.FirstOrDefault(x=>x.ConnectionId==Context.ConnectionId);
        if (connection != null && group !=null)
        {
            unitOfWork.MessageService.RemoveConnection(connection);
            if(await unitOfWork.Complete()) return group;
        }
        throw new HubException("faziled to remove from group");
    }
}
