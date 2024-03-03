using Discord;
using Discord.WebSocket;
using DiscordRpBot.Commands;
using DiscordRpBot.Services;
using DiscordRpBot.Constants;



public class SpawnRegisterFormCommand : SlashCommand
{
    public SpawnRegisterFormCommand()
    {
        Name = "spawnregisterform";
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

        (Embed Embed, MessageComponent ActionRow) messageProperties = GetRegisterForm(guild.Name, guild.IconUrl);
        await command.RespondAsync(embeds: [messageProperties.Embed], components: messageProperties.ActionRow);
    }

    private static (Embed Embed, MessageComponent ActionRow) GetRegisterForm(string serverName, string serverIconUrl)
    {
        var embed = new EmbedBuilder()
            .WithTitle($"Formulário de criação de personagem para {serverName}")
            .WithDescription("Para criar um personagem, preencha o formulário clicando no botão \"Criar personagem\" e clique em enviar quando finalizar.")
            .WithColor(CommonService.GetRandomColor())
            .WithThumbnailUrl(serverIconUrl)
            .WithCurrentTimestamp()
            .Build();

        var button = new ButtonBuilder()
            .WithLabel("Criar personagem")
            .WithStyle(ButtonStyle.Primary)
            .WithCustomId(ButtonId.CreateCharacter)
            .WithEmote(new Emoji("📝"));

        var actionRow = new ComponentBuilder()
            .WithButton(button)
            .Build();

        return (embed, actionRow);
    }
}
