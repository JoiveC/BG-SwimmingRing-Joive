using HarmonyLib;
using Joive.BurglinGnomes.SwimmingRing.Game;

namespace Joive.BurglinGnomes.SwimmingRing.Patches
{
    [HarmonyPatch(typeof(GnomiumDeposit), nameof(GnomiumDeposit.OnNetworkSpawn))]
    internal static class GnomiumDepositOnNetworkSpawnPatch
    {
        private static void Postfix(GnomiumDeposit __instance)
        {
            CraftingRegistry.EnsureRegistered(__instance);
        }
    }
}
