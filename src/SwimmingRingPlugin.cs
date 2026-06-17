using BepInEx;
using HarmonyLib;
using Joive.BurglinGnomes.SwimmingRing.Config;
using Joive.BurglinGnomes.SwimmingRing.Game;
using Joive.BurglinGnomes.SwimmingRing.Items;

namespace Joive.BurglinGnomes.SwimmingRing
{
    [BepInPlugin(PluginId, PluginName, Version)]
    public sealed class SwimmingRingPlugin : BaseUnityPlugin
    {
        public const string PluginId = "joive.bg.swimmingring";
        public const string PluginName = "BG Swimming Ring";
        public const string Version = "1.0.16";

        private Harmony harmony;

        private void Awake()
        {
            PluginConfig.Bind(Config);
            ModLog.Bind(Logger);

            SwimmingRingDefinition.Configure(PluginConfig.PlasticCost.Value);
            GameText.RegisterSwimmingRingText();
            if (PluginConfig.DebugLogs.Value)
            {
                ModLog.Info("BG-SwimmingRing-Joive.dll loaded. Version: " + Version);
            }

            harmony = new Harmony(PluginId);
            harmony.PatchAll(typeof(SwimmingRingPlugin).Assembly);

            ModLog.Info(PluginName + " " + Version + " loaded.");
        }

        private void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }
    }
}
