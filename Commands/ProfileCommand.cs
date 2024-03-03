using Discord.WebSocket;

namespace DiscordRpBot.Commands;

public class ProfileCommand : BaseSlashCommand
{
    public ProfileCommand()
    {
        Name = "profile";
        Description = "Displays the profile of a user";
    }
    public override async Task ExecuteAsync(SocketSlashCommand command, DiscordSocketClient client)
    {
        await command.RespondAsync("This command is not implemented yet.");
    }
}
