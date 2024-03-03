
using Discord.WebSocket;

namespace DiscordRpBot.Interactions;
public abstract class BaseModalInteraction
{
    public required string Id { get; set; }
    public required List<string> Fields { get; set; }

    public async Task HandleInteractionAsync(SocketModal modalSubmitInteraction, DiscordSocketClient client)
    {
        if (Id != modalSubmitInteraction.Data.CustomId)
        {
            return;
        }
        await ProcessAsync(modalSubmitInteraction, client);
    }
    protected abstract Task ProcessAsync(SocketModal modalSubmitInteraction, DiscordSocketClient client);
    public Dictionary<string, string> GetResponseDictionary(SocketModal modalSubmitInteraction)
    {
        return modalSubmitInteraction.Data.Components
            .Where(component => Fields.Contains(component.CustomId))
            .ToDictionary(component => component.CustomId, component => component.Value);
    }
}