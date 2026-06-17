using HarmonyLib;
using Joive.BurglinGnomes.SwimmingRing.Game;
using Joive.BurglinGnomes.SwimmingRing.Gameplay;

namespace Joive.BurglinGnomes.SwimmingRing.Patches
{
    [HarmonyPatch(typeof(NormalMovement), "HandleVelocity")]
    internal static class NormalMovementHandleVelocityPatch
    {
        private static void Postfix(NormalMovement __instance, float dt)
        {
            SwimmingRingMovement.Apply(__instance, dt);
        }
    }

    [HarmonyPatch(typeof(Unpinned), nameof(Unpinned.CheckEnterTransition))]
    internal static class UnpinnedCheckEnterTransitionPatch
    {
        private static void Postfix(Unpinned __instance, object fromState, ref bool __result)
        {
            if (!__result || !(fromState is NormalMovement) || !GameApi.TryGetPlayer(__instance, out PlayerNetworking player))
            {
                return;
            }

            if (GameApi.CanStandFromWater(player) && GameApi.IsPlayerInWater(__instance))
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(Unpinned), nameof(Unpinned.CheckExitTransition))]
    internal static class UnpinnedCheckExitTransitionPatch
    {
        private static void Prefix(Unpinned __instance)
        {
            if (!GameApi.TryGetPlayer(__instance, out PlayerNetworking player))
            {
                return;
            }

            if (GameApi.CanStandFromWater(player) && GameApi.IsPlayerInWater(__instance))
            {
                player.StateController.EnqueueTransition<NormalMovement>();
            }
        }
    }
}
