using Discord.WebSocket;
using DiscordRpBot.Interactions;

namespace DiscordRpBot.Managers;
public class ButtonManager(DiscordSocketClient client) : BaseManager<BaseButtonInteraction>(client)
{
    private readonly Dictionary<string, BaseButtonInteraction> _buttonInteractions = new();

    public override async Task HandleItemAsync(object item)
    {
        if (item is not SocketMessageComponent interaction)
            return;

        if (!_buttonInteractions.TryGetValue(interaction.Data.CustomId, out BaseButtonInteraction? buttonInteraction))
            return;

        await buttonInteraction.HandleInteractionAsync(interaction, Client);
    }
}
