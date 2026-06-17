using Joive.BurglinGnomes.SwimmingRing.Config;
using Joive.BurglinGnomes.SwimmingRing.Items;
using UnityEngine;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class SwimmingRingWorldVisual
    {
        private const string VisualRootName = "SwimmingRingVisual";

        internal static void Apply(ItemInstance itemInstance)
        {
            if (!GameApi.IsEnabled || itemInstance == null || !IsSwimmingRingInstance(itemInstance))
            {
                return;
            }

            GameApi.Debug("Found ItemInstance for " + SwimmingRingDefinition.ItemName + ": " + itemInstance.name + ".");

            Transform root = itemInstance.transform;
            Transform existingVisual = FindExistingVisual(root);
            HideDefaultRenderers(root, existingVisual);

            if (existingVisual != null)
            {
                ApplyWorldVisualTransform(existingVisual);
                SwimmingRingVisualUtility.PrepareVisual(existingVisual, VisualRootName);
                return;
            }

            GameObject visualPrefab = SwimmingRingAssets.GetVisualPrefab();
            if (visualPrefab == null)
            {
                return;
            }

            GameObject visual = Object.Instantiate(visualPrefab, root, false);
            visual.name = VisualRootName;
            ApplyWorldVisualTransform(visual.transform);
            SwimmingRingVisualUtility.PrepareVisual(visual.transform, VisualRootName);
            GameApi.Debug("Instantiated " + VisualRootName + " for " + SwimmingRingDefinition.ItemName + ": " + itemInstance.name + ".");
        }

        private static bool IsSwimmingRingInstance(ItemInstance itemInstance)
        {
            ItemData.InstanceData data = itemInstance.InstanceData;
            return ItemRegistry.IsSwimmingRingIndex(data.itemIndex);
        }

        private static void HideDefaultRenderers(Transform root, Transform existingVisual)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                if (renderer == null || IsInsideVisual(renderer.transform, existingVisual))
                {
                    continue;
                }

                renderer.enabled = false;
            }
        }

        private static bool IsInsideVisual(Transform candidate, Transform existingVisual)
        {
            if (existingVisual == null)
            {
                return false;
            }

            return candidate == existingVisual || candidate.IsChildOf(existingVisual);
        }

        private static Transform FindExistingVisual(Transform root)
        {
            Transform[] transforms = root.GetComponentsInChildren<Transform>(true);
            foreach (Transform transform in transforms)
            {
                if (transform != root && transform.name == VisualRootName)
                {
                    return transform;
                }
            }

            return null;
        }

        private static void ApplyWorldVisualTransform(Transform visual)
        {
            visual.localPosition = Vector3.up * PluginConfig.VisualHeightOffset.Value;
            visual.localRotation = Quaternion.identity;
            visual.localScale = Vector3.one * PluginConfig.VisualScale.Value;
        }
    }
}
