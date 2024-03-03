using System.Runtime.CompilerServices;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordRpBot.Commands;
using DiscordRpBot.Managers;
using DiscordRpBot.Services;
using DiscordRpBot.Storage;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class Program
{
    private static readonly string TOKEN_NAME = "DISCORD_BOT_TOKEN";

    private static StorageContext? db;
    private static DiscordSocketClient? client;
    private static SlashCommandManager? slashCommandManager;

    public static JObject? BotConfig { get; private set; }

    public static async Task Main()
    {
        db = new StorageContext();
        await db.Database.EnsureCreatedAsync();
        await db.Database.MigrateAsync();

        client = new DiscordSocketClient();
        client.Log += Log;
        client.Ready += SetupApplicationCommandsAsync;

        slashCommandManager = new SlashCommandManager(client);
        client.SlashCommandExecuted += slashCommandManager.ExecuteAsync;

        JObject config = TryGetConfigFile(isDevelopment: true);
        string? token = config.Value<string>(TOKEN_NAME);
        if (token == null)
        {
            Console.WriteLine($"Token {TOKEN_NAME} not found");
            return;
        }

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();
        await Task.Delay(-1);
    }

    private static async Task SetupApplicationCommandsAsync()
    {

        SocketGuild? guild = client?.Guilds.First();
        if (guild == null)
        {
            Console.WriteLine("No guild found to register slash commands.");
            return;
        }

        try
        {
            if (slashCommandManager == null)
            {
                Console.WriteLine("Slash command manager not found");
                return;
            }

            List<SlashCommand> commands = await slashCommandManager.RegisterCommandsAsync();
        }
        catch (HttpException exception)
        {
            string json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine($"Failed to create application command: {json}");
        }

        Console.WriteLine("Application commands registered");
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    public static JObject TryGetConfigFile(bool isDevelopment = false)
    {
        try
        {
            using StreamReader streamReader = new(PathService.GetAppPath(isDevelopment) + "config.json");
            using JsonReader reader = new JsonTextReader(streamReader);

            return JObject.Load(reader);
        }
        catch (FileNotFoundException exception)
        {
            Console.WriteLine($"Config file not found: {exception.Message}");
            return [];
        }
    }
}
