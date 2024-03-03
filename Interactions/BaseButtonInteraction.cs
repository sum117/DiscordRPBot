using Discord.WebSocket;

namespace DiscordRpBot.Interactions;
public abstract class BaseButtonInteraction
{
    public required string Id { get; set; }
    public async Task HandleInteractionAsync(SocketMessageComponent interaction, DiscordSocketClient client)
    {
        if (Id != interaction.Data.CustomId)
        {
            return;
        }

        await ProcessAsync(interaction, client);
    }
    protected abstract Task ProcessAsync(SocketMessageComponent interaction, DiscordSocketClient client);
}