using HarmonyLib;
using Joive.BurglinGnomes.SwimmingRing.Game;

namespace Joive.BurglinGnomes.SwimmingRing.Patches
{
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.OnNetworkSpawn))]
    internal static class ItemInstanceOnNetworkSpawnPatch
    {
        private static void Postfix(ItemInstance __instance)
        {
            SwimmingRingVisualApplier.Apply(__instance);
        }
    }

    [HarmonyPatch(typeof(ItemInstance), "OnInstanceDataChange")]
    internal static class ItemInstanceOnInstanceDataChangePatch
    {
        private static void Postfix(ItemInstance __instance)
        {
            SwimmingRingVisualApplier.Apply(__instance);
        }
    }
}
