using Discord.WebSocket;


namespace DiscordRpBot.Commands;
public class PingCommand : SlashCommand
{
    public PingCommand()
    {
        Name = "ping";
        Description = "Replies with pong!";
    }

    public override async Task ExecuteAsync(SocketSlashCommand command, DiscordSocketClient client)
    {
        await command.RespondAsync("Pong!");
    }

}
