using UnityEngine.Localization;

namespace Joive.BurglinGnomes.SwimmingRing.Items
{
    internal static class SwimmingRingDefinition
    {
        internal const string ItemName = "Inflatable Swimming Ring";
        internal const string LegacyItemName = "inflatable_swimming_ring";
        internal const string DisplayName = "Inflatable Swimming Ring";
        internal const string Description = "Keeps you steady in water.";
        internal const float MaxDurability = 100f;

        internal static int PlasticCost { get; private set; } = 4;

        internal static void Configure(int plasticCost)
        {
            PlasticCost = plasticCost;
        }

        internal static LocalizedString NameText()
        {
            return new LocalizedString("Items", ItemName);
        }

        internal static LocalizedString DescriptionText()
        {
            return new LocalizedString("Items Descriptions", ItemName);
        }
    }
}
