using System;
using AutoMapper;
using Backend.DTOs;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
using Backend.Services.MessageService;
using Backend.Services.UnitOfWork;
using Backend.Services.UsersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
public class MessageController(IUnitOfWork _unitOfWork , IMapper mapper ):BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO messagecreate)
    {
        var username=User.getUsernameFromToken();
        if(username==null) throw new Exception("user not identified");
        if(username.ToLower() == messagecreate.TargetUsername.ToLower())return BadRequest("do not message yourself");
        var sender =await  _unitOfWork.UsersService.GetUserByUsernameAsync(username);
        var receiver =  await  _unitOfWork.UsersService.GetUserByUsernameAsync(messagecreate.TargetUsername);
        if(sender == null || receiver==null)return BadRequest("can not send message");
         Console.WriteLine("📨 Avant appel à CreateMessage dans MessageController");
        var newMessage = _unitOfWork.MessageService.CreateMessage(sender, receiver, messagecreate.Content);
        Console.WriteLine("📨 Après appel à CreateMessage dans MessageController");
         _unitOfWork.MessageService.AddMessage(newMessage);
        if(await  _unitOfWork.Complete())return Ok(mapper.Map<MessageDTO>(newMessage));
        else return BadRequest("can not send message");

    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesUser([FromQuery]MessageParams messageParams){
        messageParams.Username = User.getUsernameFromToken();
        var messages = await  _unitOfWork.MessageService.GetMessagesForUser(messageParams);
        Response.AddPaginationHeader(messages);
        return messages;
    }
    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesUser(string username){
        var currentUsername=User.getUsernameFromToken();
        if(currentUsername==null)return Unauthorized("need authorization");
        if(await  _unitOfWork.UsersService.GetUserByUsernameAsync(username)==null)return BadRequest("can not find conversation");
        var messages= await  _unitOfWork.MessageService.GetMessageThread(currentUsername!,username);
        return Ok(messages);
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMessage(int id){
        var username =User.getUsernameFromToken();
        var userId = User.getUserIdFromToken();
        var message =await  _unitOfWork.MessageService.GetMessage(id);
        if(message==null)return BadRequest("message not found");
        if(message.SourceUserId !=userId && message.TargetUserId !=userId) return Forbid("you can not touch that message");
        if(message.SourceUserId==userId)message.SourceDeleted=true;
        if (message.TargetUserId==userId)message.TargetDeleted=true;
        if(message is {TargetDeleted : true,SourceDeleted:true}) _unitOfWork.MessageService.DeleteMessage(message);
        if(await  _unitOfWork.Complete()) return NoContent();
        return BadRequest("problem deletingthe message");

    }
}
