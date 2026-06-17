using HarmonyLib;
using Joive.BurglinGnomes.SwimmingRing.Game;

namespace Joive.BurglinGnomes.SwimmingRing.Patches
{
    [HarmonyPatch(typeof(EquipmentHandler), nameof(EquipmentHandler.SetWearedEquipment))]
    internal static class EquipmentHandlerSetWearedEquipmentPatch
    {
        private static bool Prefix(string name)
        {
            return !GameApi.IsSwimmingRingName(name);
        }
    }

    [HarmonyPatch(typeof(EquipmentHandler), nameof(EquipmentHandler.SetOtherWearedEquipment))]
    internal static class EquipmentHandlerSetOtherWearedEquipmentPatch
    {
        private static bool Prefix(string name)
        {
            return !GameApi.IsSwimmingRingName(name);
        }
    }
}
