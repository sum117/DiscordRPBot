using Discord;
using Discord.WebSocket;
using DiscordRpBot.Services;
using DiscordRpBot.Constants;


namespace DiscordRpBot.Commands;
public class SpawnRegisterFormCommand : BaseSlashCommand
{
    public SpawnRegisterFormCommand()
    {
        Name = "spawn_register_form";
        Description = "Spawns the register form.";
        RequiredPermission = GuildPermission.Administrator;
    }

    public override async Task ExecuteAsync(SocketSlashCommand command, DiscordSocketClient client)
    {
        SocketGuild? guild = client.Guilds.FirstOrDefault(guild => guild.Id == command.GuildId);
        if (guild == null)
        {
            await command.RespondAsync("There was an error while trying to get the guild.");
            return;
        }

        (Embed Embed, MessageComponent ActionRow) messageProperties = GetRegisterPanel(guild.Name, guild.IconUrl);
        await command.RespondAsync(embeds: [messageProperties.Embed], components: messageProperties.ActionRow);
    }

    private static (Embed Embed, MessageComponent ActionRow) GetRegisterPanel(string serverName, string serverIconUrl)
    {
        Embed embed = new EmbedBuilder()
            .WithTitle($"Formulário de criação de personagem para {serverName}")
            .WithDescription("Para criar um personagem, preencha o formulário clicando no botão \"Criar personagem\" e clique em enviar quando finalizar.")
            .WithColor(CommonService.GetRandomColor())
            .WithThumbnailUrl(serverIconUrl)
            .WithCurrentTimestamp()
            .Build();

        ButtonBuilder button = new ButtonBuilder()
            .WithLabel("Criar personagem")
            .WithStyle(ButtonStyle.Primary)
            .WithCustomId(ButtonId.CreateCharacter)
            .WithEmote(new Emoji("📝"));

        MessageComponent actionRow = new ComponentBuilder()
            .WithButton(button)
            .Build();

        return (embed, actionRow);
    }
}
