namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class SwimmingRingVisualApplier
    {
        internal static void Apply(ItemInstance itemInstance)
        {
            SwimmingRingWorldVisual.Apply(itemInstance);
        }

        internal static void ApplyToPlayer(PlayerNetworking player)
        {
            SwimmingRingPlayerVisual.Apply(player);
        }
    }
}
