using System.Reflection;
using Discord.WebSocket;
using DiscordRpBot.Commands;

namespace DiscordRpBot.Managers
{
    public abstract class BaseManager<T>(DiscordSocketClient client)
    {
        protected readonly List<T> Items = [];
        protected readonly DiscordSocketClient Client = client;

        public abstract Task HandleItemAsync(object item);

        public void RegisterItem(T item)
        {
            Items.Add(item);
        }

        public async Task<List<T>> RegisterItemsAsync()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            IEnumerable<T?> instances = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(T).IsAssignableFrom(type))
                .Select(type => (T?)Activator.CreateInstance(type));

            foreach (T? instance in instances)
            {
                if (instance is BaseSlashCommand slashCommand)
                {
                    RegisterItem(instance);
                    SocketGuild guild = Client.Guilds.First();
                    await guild.CreateApplicationCommandAsync(slashCommand.GetCommandData());
                }
                else if (instance != null)
                {
                    RegisterItem(instance);

                }
            }

            return Items;
        }
    }
}


