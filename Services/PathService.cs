
namespace DiscordRpBot.Services
{
    public static class PathService
    {
        public static readonly string SEPARATOR = $"{Path.DirectorySeparatorChar}";

        public static readonly string EXE_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory;

        public static string GetAppPath(bool isDevelopment = false)
        {
            string path;
            if (isDevelopment) path = $"{Directory.GetCurrentDirectory()}{SEPARATOR}";
            else path = $"{PathService.EXE_DIRECTORY}{SEPARATOR}";

            return path;
        }
    }
}