using System;
using Joive.BurglinGnomes.SwimmingRing.Items;
using UnityEngine;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class ItemRegistry
    {
        private static ItemData swimmingRing;
        private static int swimmingRingIndex = -1;

        internal static bool IsSwimmingRingIndex(byte itemIndex)
        {
            return swimmingRingIndex >= 0 && itemIndex == swimmingRingIndex;
        }

        internal static ItemData EnsureRegistered(AllItems allItems)
        {
            if (!GameApi.IsEnabled || allItems == null)
            {
                return null;
            }

            GameText.RegisterSwimmingRingText();

            if (swimmingRing != null && allItems.IndexOf(swimmingRing) >= 0)
            {
                swimmingRingIndex = allItems.IndexOf(swimmingRing);
                return swimmingRing;
            }

            ItemData existing = FindExisting(allItems);
            if (existing != null)
            {
                NormalizeItem(existing);
                swimmingRing = existing;
                swimmingRingIndex = allItems.IndexOf(swimmingRing);
                return swimmingRing;
            }

            swimmingRing = CreateItem(allItems);
            Array.Resize(ref allItems.items, allItems.items.Length + 1);
            allItems.items[allItems.items.Length - 1] = swimmingRing;
            swimmingRingIndex = allItems.items.Length - 1;

            GameApi.Debug("Registered item: " + SwimmingRingDefinition.ItemName + " at index " + swimmingRingIndex + ".");
            return swimmingRing;
        }

        private static ItemData CreateItem(AllItems allItems)
        {
            ItemData item = ScriptableObject.CreateInstance<ItemData>();
            item.name = SwimmingRingDefinition.ItemName;
            item.localizedName = SwimmingRingDefinition.NameText();
            item.localizedDesc = SwimmingRingDefinition.DescriptionText();
            item.description = SwimmingRingDefinition.Description;
            item.icon = SwimmingRingIcon.GetIcon();
            item.equipmentType = ItemData.EquipmentType.Weared;
            item.maxStackSize = 1;
            item.durabilityLoss = ItemData.EquipmentDurabilityLossType.None;
            item.maxDurability = SwimmingRingDefinition.MaxDurability;
            item.dontAllowEquipTogetherWith = new ItemData[0];
            item.prefab = allItems.defaultPrefab;
            item.hideFlags = HideFlags.DontUnloadUnusedAsset;
            return item;
        }

        private static ItemData FindExisting(AllItems allItems)
        {
            foreach (ItemData item in allItems.items)
            {
                if (item != null && GameApi.IsSwimmingRing(item))
                {
                    return item;
                }
            }

            return null;
        }

        private static void NormalizeItem(ItemData item)
        {
            item.name = SwimmingRingDefinition.ItemName;
            item.localizedName = SwimmingRingDefinition.NameText();
            item.localizedDesc = SwimmingRingDefinition.DescriptionText();
            item.description = SwimmingRingDefinition.Description;
            item.icon = SwimmingRingIcon.GetIcon();
            item.equipmentType = ItemData.EquipmentType.Weared;
            item.dontAllowEquipTogetherWith = new ItemData[0];
        }
    }
}
