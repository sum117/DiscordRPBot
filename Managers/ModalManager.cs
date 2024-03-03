using Discord.WebSocket;
using DiscordRpBot.Interactions;

namespace DiscordRpBot.Managers;
public class ModalManager(DiscordSocketClient client) : BaseManager<BaseModalInteraction>(client)
{

    public override async Task HandleItemAsync(object item)
    {
        if (item is not SocketModal modalSubmitInteraction)
            return;

        BaseModalInteraction? modalInteractionInstance = Items.FirstOrDefault(instance => instance.Id == modalSubmitInteraction.Data.CustomId);
        if (modalInteractionInstance != null)
        {
            await modalInteractionInstance.HandleInteractionAsync(modalSubmitInteraction, Client);
        }
    }
}