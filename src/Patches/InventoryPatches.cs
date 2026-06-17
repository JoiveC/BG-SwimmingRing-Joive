using HarmonyLib;
using Joive.BurglinGnomes.SwimmingRing.Game;

namespace Joive.BurglinGnomes.SwimmingRing.Patches
{
    [HarmonyPatch(typeof(InventoryBase), nameof(InventoryBase.OnNetworkSpawn))]
    internal static class InventoryOnNetworkSpawnPatch
    {
        private static void Postfix(InventoryBase __instance)
        {
            ItemRegistry.EnsureRegistered(__instance.BoundItems);
        }
    }
}
