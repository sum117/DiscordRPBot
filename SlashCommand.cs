using Discord;
using Discord.WebSocket;

namespace DiscordRpBot.Commands
{
    public class SlashCommand
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public GuildPermission? RequiredPermission { get; set; }


        public virtual Task ExecuteAsync(SocketSlashCommand command, DiscordSocketClient client)
        {
            return Task.CompletedTask;
        }

        public SlashCommandProperties GetCommandData()
        {
            SlashCommandBuilder builder = new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription(Description);

            if (RequiredPermission != null)
            {
                builder.WithDefaultMemberPermissions(RequiredPermission);
            }


            return builder.Build();
        }
    }
}