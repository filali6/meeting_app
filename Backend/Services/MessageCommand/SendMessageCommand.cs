using Backend.Models;
using Backend.Services.MessageService;

namespace Backend.Services.MessageCommand;

public class SendMessageCommand : IMessageCommand
{
    private readonly IMessageService _messageService;
    private readonly Message _message;

    public SendMessageCommand(IMessageService messageService, Message message)
    {
        _messageService = messageService;
        _message = message;
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("📨 SendMessageCommand exécutée !");
        _messageService.AddMessage(_message);
        await Task.CompletedTask;
    }
}
