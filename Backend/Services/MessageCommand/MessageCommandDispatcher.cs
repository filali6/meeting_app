namespace Backend.Services.MessageCommand;

public class MessageCommandDispatcher
{
    public async Task DispatchAsync(IMessageCommand command)
    {
        await command.ExecuteAsync();
    }
}
