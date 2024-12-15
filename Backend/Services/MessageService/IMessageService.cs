using System;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Services.MessageService;

public interface IMessageService
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message?> GetMessage(int id);
    Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams);
    Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername,string targetUsername);
    Task<bool> SaveChangeAsync();
}
