using Discord;

namespace DiscordRpBot.Services
{
    public class CommonService
    {
        public static Color GetRandomColor()
        {
            Random random = new();
            return new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }
    }
}