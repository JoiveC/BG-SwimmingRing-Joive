using BepInEx.Configuration;

namespace Joive.BurglinGnomes.SwimmingRing.Config
{
    internal static class PluginConfig
    {
        private const int DefaultPlasticCost = 4;
        private const float DefaultSwimSpeedMultiplier = 0.85f;
        private const float DefaultSwimSpeed = 4.8f;
        private const float DefaultSwimAcceleration = 18f;
        private const float DefaultSwimAnimationSpeed = 1.15f;
        private const float DefaultSwimAnimationLerp = 8f;
        private const float DefaultBuoyancyForce = 18f;
        private const float DefaultActiveRiseForce = 16f;
        private const float DefaultMaxRiseSpeed = 5.5f;
        private const float DefaultMaxSinkSpeed = 0.4f;
        private const float DefaultWorldVisualScale = 6f;
        private const float DefaultWorldVisualHeightOffset = 0.25f;
        private const float DefaultPlayerVisualScale = 9f;
        private const float DefaultPlayerVisualHeightOffset = 0.35f;
        private const float DefaultPlayerVisualForwardOffset = -0.08f;

        internal static ConfigEntry<bool> EnableMod { get; private set; }
        internal static ConfigEntry<int> PlasticCost { get; private set; }
        internal static ConfigEntry<float> SwimSpeedMultiplier { get; private set; }
        internal static ConfigEntry<float> SwimSpeed { get; private set; }
        internal static ConfigEntry<float> SwimAcceleration { get; private set; }
        internal static ConfigEntry<float> SwimAnimationSpeed { get; private set; }
        internal static ConfigEntry<float> SwimAnimationLerp { get; private set; }
        internal static ConfigEntry<float> BuoyancyForce { get; private set; }
        internal static ConfigEntry<float> ActiveRiseForce { get; private set; }
        internal static ConfigEntry<float> MaxRiseSpeed { get; private set; }
        internal static ConfigEntry<float> MaxSinkSpeed { get; private set; }
        internal static ConfigEntry<float> VisualScale { get; private set; }
        internal static ConfigEntry<float> VisualHeightOffset { get; private set; }
        internal static ConfigEntry<float> PlayerVisualScale { get; private set; }
        internal static ConfigEntry<float> PlayerVisualHeightOffset { get; private set; }
        internal static ConfigEntry<float> PlayerVisualForwardOffset { get; private set; }
        internal static ConfigEntry<bool> ForceRuntimeOrangeMaterial { get; private set; }
        internal static ConfigEntry<bool> DebugLogs { get; private set; }

        internal static void Bind(ConfigFile config)
        {
            EnableMod = config.Bind("General", "EnableMod", true, "Turns the mod on.");
            PlasticCost = config.Bind("Crafting", "PlasticCost", DefaultPlasticCost, "Plastic cost for the recipe.");
            SwimSpeedMultiplier = config.Bind("Swimming", "SwimSpeedMultiplier", DefaultSwimSpeedMultiplier, "Speed kept when there is no swim input.");
            SwimSpeed = config.Bind("Swimming", "SwimSpeed", DefaultSwimSpeed, "Swim speed with the ring equipped.");
            SwimAcceleration = config.Bind("Swimming", "SwimAcceleration", DefaultSwimAcceleration, "How fast swim movement catches up.");
            SwimAnimationSpeed = config.Bind("Swimming", "SwimAnimationSpeed", DefaultSwimAnimationSpeed, "Swim animation value while moving.");
            SwimAnimationLerp = config.Bind("Swimming", "SwimAnimationLerp", DefaultSwimAnimationLerp, "Swim animation blend speed.");
            BuoyancyForce = config.Bind("Swimming", "BuoyancyForce", DefaultBuoyancyForce, "Upward force in water.");
            ActiveRiseForce = config.Bind("Swimming", "ActiveRiseForce", DefaultActiveRiseForce, "Extra lift from jump or forward input.");
            MaxRiseSpeed = config.Bind("Swimming", "MaxRiseSpeed", DefaultMaxRiseSpeed, "Top upward speed from the ring.");
            MaxSinkSpeed = config.Bind("Swimming", "MaxSinkSpeed", DefaultMaxSinkSpeed, "Sink speed limit while floating.");
            VisualScale = config.Bind("Visual", "VisualScale", DefaultWorldVisualScale, "World item visual size.");
            VisualHeightOffset = config.Bind("Visual", "VisualHeightOffset", DefaultWorldVisualHeightOffset, "World item visual height.");
            PlayerVisualScale = config.Bind("Visual", "PlayerVisualScale", DefaultPlayerVisualScale, "Player visual size.");
            PlayerVisualHeightOffset = config.Bind("Visual", "PlayerVisualHeightOffset", DefaultPlayerVisualHeightOffset, "Player visual height.");
            PlayerVisualForwardOffset = config.Bind("Visual", "PlayerVisualForwardOffset", DefaultPlayerVisualForwardOffset, "Player visual forward offset.");
            ForceRuntimeOrangeMaterial = config.Bind("Visual", "ForceRuntimeOrangeMaterial", true, "Use the runtime orange material.");
            DebugLogs = config.Bind("General", "DebugLogs", false, "Write extra setup logs.");

            if (PlayerVisualScale.Value < DefaultPlayerVisualScale)
            {
                PlayerVisualScale.Value = DefaultPlayerVisualScale;
            }
        }
    }
}
