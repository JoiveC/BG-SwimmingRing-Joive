using System.Collections.Generic;
using Joive.BurglinGnomes.SwimmingRing.Config;
using UnityEngine;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class SwimmingRingPlayerVisual
    {
        private const string PlayerVisualRootName = "SwimmingRingPlayerVisual";

        private static readonly Dictionary<int, Transform> playerVisuals = new Dictionary<int, Transform>();

        internal static void Apply(PlayerNetworking player)
        {
            if (!GameApi.IsEnabled || player == null)
            {
                return;
            }

            Transform existingVisual = GetCachedPlayerVisual(player);
            if (!GameApi.HasEquippedSwimmingRing(player))
            {
                if (existingVisual != null && existingVisual.gameObject.activeSelf)
                {
                    existingVisual.gameObject.SetActive(false);
                }

                return;
            }

            bool createdVisual = existingVisual == null;
            if (createdVisual)
            {
                GameObject visualPrefab = SwimmingRingAssets.GetVisualPrefab();
                if (visualPrefab == null)
                {
                    return;
                }

                GameObject visual = Object.Instantiate(visualPrefab, player.transform, false);
                visual.name = PlayerVisualRootName;
                existingVisual = visual.transform;
                playerVisuals[player.GetInstanceID()] = existingVisual;
                SwimmingRingVisualUtility.StripGameplayComponents(existingVisual);
                GameApi.Debug("Instantiated " + PlayerVisualRootName + " for player: " + player.name + ".");
            }

            existingVisual.gameObject.SetActive(true);
            ApplyPlayerVisualTransform(existingVisual);
            if (createdVisual)
            {
                SwimmingRingVisualUtility.PrepareVisual(existingVisual, PlayerVisualRootName);
            }
        }

        private static Transform GetCachedPlayerVisual(PlayerNetworking player)
        {
            int key = player.GetInstanceID();
            if (!playerVisuals.TryGetValue(key, out Transform visual))
            {
                return null;
            }

            if (visual != null)
            {
                return visual;
            }

            playerVisuals.Remove(key);
            return null;
        }

        private static void ApplyPlayerVisualTransform(Transform visual)
        {
            visual.localPosition = new Vector3(0f, PluginConfig.PlayerVisualHeightOffset.Value, PluginConfig.PlayerVisualForwardOffset.Value);
            visual.localRotation = Quaternion.identity;
            visual.localScale = Vector3.one * PluginConfig.PlayerVisualScale.Value;
        }
    }
}
