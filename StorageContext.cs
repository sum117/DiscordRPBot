using DiscordRpBot.Services;
using Microsoft.EntityFrameworkCore;

namespace DiscordRpBot.Storage
{
    public class StorageContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        private readonly string DATABASE_NAME = "database.sqlite3";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={PathService.GetAppPath(true)}{PathService.SEPARATOR}Storage{PathService.SEPARATOR}{DATABASE_NAME}")
            .UseLazyLoadingProxies(true);
        }

    }
}