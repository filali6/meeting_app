using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Data;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.MessageService;

public class MessageService(DataContext context,IMapper mapper): IMessageService
{
    private readonly DataContext _context=context;

    public void AddGroup(Group group)
    {
        _context.Groups.Add(group);
    }

    public async void AddMessage(Message message)
    {
       await  _context.Messages.AddAsync(message);
    }

    public void DeleteMessage(Message message)
    {
         _context.Messages.Remove(message);
    }

    public async Task<Connection?> GetConnection(string connectionId)
    {
        return await _context.Connections.FindAsync(connectionId);
    }

    public async Task<Group?> GetGroup(string groupName)
    {
        return await _context.Groups.Include(x=>x.Connections)
                .FirstOrDefaultAsync(x=>x.Name==groupName);
    }

    public async Task<Group?> GetGroupFromConnection(string connectionId)
    {
        return await _context.Groups.Include(g=>g.Connections).Where(x=>x.Connections.Any(c=>c.ConnectionId==connectionId)).FirstOrDefaultAsync();
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await _context.Messages.FirstOrDefaultAsync(m=>m.Id==id);
    }

    public async Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
    {
        var query=_context.Messages
                    .OrderByDescending(m=>m.SentDate)
                    .AsQueryable();
        query = messageParams.Container switch
        {
            "inbox" => query.Where(x=>x.TargetUser.UserName==messageParams.Username && !x.TargetDeleted),
            "outbox"=> query.Where(x=>x.SourceUser.UserName==messageParams.Username && !x.SourceDeleted),
            _ => query.Where(x=>x.TargetUser.UserName==messageParams.Username && x.ReadDate==null && !x.TargetDeleted )
        };
        var message = query.ProjectTo<MessageDTO>(mapper.ConfigurationProvider);
        return await PagedList<MessageDTO>.CreatAsync(message,messageParams.pageNumber,messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string targetUsername)
    {
        var res= await  _context.Messages.Where(m=>((m.SourceUser.UserName==currentUsername && m.TargetUser.UserName==targetUsername)
                                               ||(m.SourceUser.UserName==targetUsername && m.TargetUser.UserName==currentUsername))
                                               && !m.SourceDeleted)
                                        .OrderBy(m=>m.SentDate)
                                        .ProjectTo<MessageDTO>(mapper.ConfigurationProvider)
                                        .ToListAsync();    
       await _context.Messages.Where(m=>m.SourceUser.UserName==targetUsername
                                        && m.TargetUser.UserName==currentUsername
                                        && m.ReadDate==null)
                         .ForEachAsync(e=>e.ReadDate=DateTime.Now);
        
            
    await Task.Run(SaveChangeAsync);
    return res;
    }

    public void RemoveConnection(Connection connection)
    {
        _context.Connections.Remove(connection);
    }

    public async Task<bool> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync()>0;
    }
}
