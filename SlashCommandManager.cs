using System.Reflection;
using Discord.WebSocket;
using DiscordRpBot.Commands;

namespace DiscordRpBot.Managers
{
    public class SlashCommandManager
    {
        private readonly List<SlashCommand> slashCommands = [];
        private readonly DiscordSocketClient client;

        public SlashCommandManager(DiscordSocketClient client)
        {
            this.client = client;
        }


        public async Task ExecuteAsync(SocketSlashCommand command)
        {
            string commandName = command.Data.Name;
            SlashCommand? commandToRun = slashCommands.FirstOrDefault(command => command.Name == commandName);
            if (commandToRun == null)
            {
                await command.RespondAsync("Command not found");
                return;
            }
            try
            {
                await commandToRun.ExecuteAsync(command, client);
            }
            catch (Exception exception)
            {
                await command.RespondAsync("An error occurred while executing the command");
                Console.WriteLine("An error occurred while executing the command: " + exception.Message);
            }
        }

        public void RegisterCommand(SlashCommand command)
        {
            slashCommands.Add(command);
        }

        public async Task<List<SlashCommand>> RegisterCommandsAsync()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            IEnumerable<SlashCommand?> commandInstances = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(SlashCommand)))
                .Select(type => (SlashCommand?)Activator.CreateInstance(type));


            foreach (SlashCommand? commandInstance in commandInstances)
            {
                if (commandInstance == null) continue;

                RegisterCommand(commandInstance);
                SocketGuild guild = client.Guilds.First();
                await guild.CreateApplicationCommandAsync(commandInstance.GetCommandData());

            }

            return slashCommands;
        }

    }

}