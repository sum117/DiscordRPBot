using Discord.WebSocket;
using DiscordRpBot.Interactions;

namespace DiscordRpBot.Managers;
public class ButtonManager(DiscordSocketClient client) : BaseManager<BaseButtonInteraction>(client)
{

    public override async Task HandleItemAsync(object item)
    {
        if (item is not SocketMessageComponent interaction)
            return;

        BaseButtonInteraction? buttonInstance = Items.FirstOrDefault(instance => instance.Id == interaction.Data.CustomId);
        if (buttonInstance != null)
        {
            await buttonInstance.HandleInteractionAsync(interaction, Client);
        }
    }
}
