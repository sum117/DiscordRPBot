using Discord.WebSocket;
using DiscordRpBot.Commands;

namespace DiscordRpBot.Managers;
public class SlashCommandManager(DiscordSocketClient client) : BaseManager<BaseSlashCommand>(client)
{
    public override async Task HandleItemAsync(object item)
    {
        if (item is not SocketSlashCommand command)
            return;

        string commandName = command.Data.Name;
        BaseSlashCommand? commandToRun = Items.FirstOrDefault(cmd => cmd.Name == commandName);
        if (commandToRun == null)
        {
            await command.RespondAsync("Command not found");
            return;
        }

        try
        {
            await commandToRun.ExecuteAsync(command, Client);
        }
        catch (Exception exception)
        {
            await command.RespondAsync("An error occurred while executing the command");
            Console.WriteLine("An error occurred while executing the command: " + exception.Message);
        }
    }
}
