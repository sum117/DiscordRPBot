using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordRpBot.Commands;
using DiscordRpBot.Interactions;
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
    private static ButtonManager? buttonManager;
    private static ModalManager? modalManager;
    public static JObject? BotConfig { get; private set; }

    public static async Task Main()
    {
        db = new StorageContext();
        await db.Database.EnsureCreatedAsync();
        await db.Database.MigrateAsync();

        client = new DiscordSocketClient();
        slashCommandManager = new SlashCommandManager(client);
        buttonManager = new ButtonManager(client);
        modalManager = new ModalManager(client);

        client.SlashCommandExecuted += slashCommandManager.HandleItemAsync;
        client.ButtonExecuted += buttonManager.HandleItemAsync;
        client.ModalSubmitted += modalManager.HandleItemAsync;
        client.Ready += SetupManagersAsync;
        client.Log += Log;

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

    private static async Task SetupManagersAsync()
    {

        SocketGuild? guild = client?.Guilds.First();
        if (guild == null)
        {
            Console.WriteLine("No guild found to register slash commands.");
            return;
        }

        try
        {
            List<BaseSlashCommand>? commands = slashCommandManager != null ? await slashCommandManager.RegisterItemsAsync() : null;
            List<BaseButtonInteraction>? buttonInteractions = buttonManager != null ? await buttonManager.RegisterItemsAsync() : null;
            List<BaseModalInteraction>? modalInteractions = modalManager != null ? await modalManager.RegisterItemsAsync() : null;
        }
        catch (HttpException exception)
        {
            string json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine($"Failed to register bot manager items: {exception.Message}\n{json}");
        }

        Console.WriteLine("Bot managers registered successfully");
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

