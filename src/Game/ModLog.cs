using BepInEx.Logging;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class ModLog
    {
        private static ManualLogSource logger;

        internal static void Bind(ManualLogSource source)
        {
            logger = source;
        }

        internal static void Info(string message)
        {
            logger?.LogInfo(message);
        }

        internal static void Warning(string message)
        {
            logger?.LogWarning(message);
        }

        internal static void Error(string message)
        {
            logger?.LogError(message);
        }
    }
}
