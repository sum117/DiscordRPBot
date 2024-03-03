using Discord.WebSocket;
using DiscordRpBot.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DiscordRpBot.Commands;

public class ProfileCommand : SlashCommand
{
    public ProfileCommand()
    {
        Name = "profile";
        Description = "Displays the profile of a user";
    }

    public override async Task ExecuteAsync(SocketSlashCommand command, DiscordSocketClient client)
    {
        ulong userId = command.User.Id;
        StorageContext db = new();
        EntityEntry<Character> createdCharacter = await db.Characters.AddAsync(new Character
        {
            Level = 1,
            Name = command.User.Username,
            Id = Guid.NewGuid(),
            OwnerId = userId
        });

        await command.RespondAsync($"Profile created for {command.User.Username}. Your character ID is {createdCharacter.Entity.Id} and your level is {createdCharacter.Entity.Level}");

        await db.SaveChangesAsync();
    }
}
