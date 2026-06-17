using System;
using Joive.BurglinGnomes.SwimmingRing.Items;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class CraftingRegistry
    {
        private static bool missingPlasticLogged;

        internal static void EnsureRegistered(GnomiumDeposit deposit)
        {
            if (!GameApi.IsEnabled || deposit == null || deposit.Recipes == null || deposit.ResourceInventory == null)
            {
                return;
            }

            ItemData ring = ItemRegistry.EnsureRegistered(deposit.ResourceInventory.BoundItems);
            ItemData plastic = FindPlasticResource(deposit.ResourceInventory.BoundItems);
            if (ring == null || plastic == null || HasRecipe(deposit.Recipes, ring))
            {
                return;
            }

            CraftingRecipes.Recipe recipe = new CraftingRecipes.Recipe
            {
                craftResultType = CraftingRecipes.CraftResultType.EQUIPMENT,
                cost = new[]
                {
                    new CraftingRecipes.ItemCost
                    {
                        item = plastic,
                        amount = SwimmingRingDefinition.PlasticCost
                    }
                }
            };

            GameApi.SetRecipeItem(recipe, ring);
            Array.Resize(ref deposit.Recipes.recipes, deposit.Recipes.recipes.Length + 1);
            deposit.Recipes.recipes[deposit.Recipes.recipes.Length - 1] = recipe;

            GameApi.Debug("Registered recipe: " + SwimmingRingDefinition.ItemName);
        }

        private static ItemData FindPlasticResource(AllItems allItems)
        {
            string plasticName = ResourceStorage.GetResourceName((int)AllItems.ResourceTypes.Plastic);
            ItemData item = allItems.GetItem(plasticName, out _);
            if (item == null && !missingPlasticLogged)
            {
                missingPlasticLogged = true;
                ModLog.Warning("Plastic resource item was not found.");
            }

            return item;
        }

        private static bool HasRecipe(CraftingRecipes recipes, ItemData result)
        {
            foreach (CraftingRecipes.Recipe recipe in recipes.recipes)
            {
                if (recipe != null && recipe.CraftResult == result)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
