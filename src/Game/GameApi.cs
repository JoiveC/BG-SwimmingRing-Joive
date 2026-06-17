using System.Reflection;
using HarmonyLib;
using Joive.BurglinGnomes.SwimmingRing.Config;
using Joive.BurglinGnomes.SwimmingRing.Items;
using UnityEngine;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class GameApi
    {
        private static readonly FieldInfo RecipeItemField = AccessTools.Field(typeof(CraftingRecipes.Recipe), "item");
        private static readonly MethodInfo CheckIsPlayerInWaterMethod = AccessTools.Method(typeof(Unpinned), "CheckIsPlayerInWater");
        private static readonly MethodInfo LoadImageMethod = FindLoadImageMethod();

        internal static bool Ready { get; } = Validate();

        internal static bool IsEnabled => PluginConfig.EnableMod != null && PluginConfig.EnableMod.Value && Ready;

        internal static void SetRecipeItem(CraftingRecipes.Recipe recipe, ItemData item)
        {
            RecipeItemField.SetValue(recipe, item);
        }

        internal static bool LoadImage(Texture2D texture, byte[] bytes)
        {
            if (LoadImageMethod == null)
            {
                return false;
            }

            object[] args = LoadImageMethod.GetParameters().Length == 2
                ? new object[] { texture, bytes }
                : new object[] { texture, bytes, false };

            return (bool)LoadImageMethod.Invoke(null, args);
        }

        internal static bool IsPlayerInWater(Unpinned state)
        {
            return (bool)CheckIsPlayerInWaterMethod.Invoke(state, null);
        }

        internal static bool HasSwimmingRing(PlayerNetworking player)
        {
            return HasSwimmingRing(player, false);
        }

        internal static bool HasEquippedSwimmingRing(PlayerNetworking player)
        {
            return HasSwimmingRing(player, true);
        }

        private static bool HasSwimmingRing(PlayerNetworking player, bool equippedOnly)
        {
            if (player == null || player.Inventory == null)
            {
                return false;
            }

            foreach (InventoryBase.InventoryItem item in player.Inventory.Contents)
            {
                if (!item.IsValidItem)
                {
                    continue;
                }

                if (equippedOnly && item.slotIndex > 1)
                {
                    continue;
                }

                ItemData itemData = player.Inventory.GetItemData(item.ItemIndex);
                if (itemData != null && IsSwimmingRing(itemData))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool IsSwimmingRing(ItemData itemData)
        {
            return itemData != null && IsSwimmingRingName(itemData.Name);
        }

        internal static bool IsSwimmingRingName(string itemName)
        {
            return itemName == SwimmingRingDefinition.ItemName ||
                itemName == SwimmingRingDefinition.LegacyItemName;
        }

        internal static bool CanStandFromWater(PlayerNetworking player)
        {
            return HasEquippedSwimmingRing(player) &&
                !player.Health.Dead &&
                !player.Tied &&
                player.StatusEffects.StunnedDuration <= 0f &&
                !player.Dismemberment.IsMissingLeg;
        }

        internal static bool TryGetPlayer(Component component, out PlayerNetworking player)
        {
            player = component.GetComponentInParent<PlayerNetworking>();
            return player != null;
        }

        internal static void Debug(string message)
        {
            if (PluginConfig.DebugLogs != null && PluginConfig.DebugLogs.Value)
            {
                ModLog.Info(message);
            }
        }

        private static bool Validate()
        {
            bool ok = true;
            ok &= Require(RecipeItemField, "CraftingRecipes.Recipe.item");
            ok &= Require(CheckIsPlayerInWaterMethod, "Unpinned.CheckIsPlayerInWater");
            ok &= Require(LoadImageMethod, "UnityEngine.ImageConversion.LoadImage");
            return ok;
        }

        private static MethodInfo FindLoadImageMethod()
        {
            System.Type type = System.Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule");
            if (type == null)
            {
                return null;
            }

            return type.GetMethod(
                "LoadImage",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(Texture2D), typeof(byte[]) },
                null) ??
                type.GetMethod(
                    "LoadImage",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new[] { typeof(Texture2D), typeof(byte[]), typeof(bool) },
                    null);
        }

        private static bool Require(MemberInfo member, string name)
        {
            if (member != null)
            {
                return true;
            }

            ModLog.Warning("Missing game member: " + name);
            return false;
        }
    }
}
