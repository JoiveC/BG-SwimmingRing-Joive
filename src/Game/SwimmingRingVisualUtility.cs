using Joive.BurglinGnomes.SwimmingRing.Config;
using UnityEngine;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class SwimmingRingVisualUtility
    {
        internal static void PrepareVisual(Transform visual, string visualName)
        {
            SetActiveRecursive(visual);

            Renderer[] renderers = visual.GetComponentsInChildren<Renderer>(true);
            GameApi.Debug(visualName + " renderer count: " + renderers.Length + ".");

            if (renderers.Length == 0)
            {
                ModLog.Warning(visualName + " has no renderers.");
                return;
            }

            foreach (Renderer renderer in renderers)
            {
                if (renderer == null)
                {
                    continue;
                }

                renderer.enabled = true;
                if (PluginConfig.ForceRuntimeOrangeMaterial.Value)
                {
                    renderer.sharedMaterial = SwimmingRingVisualMaterial.GetRuntimeMaterial(renderer.sharedMaterial);
                }

                LogRenderer(renderer);
            }
        }

        internal static void StripGameplayComponents(Transform visual)
        {
            Collider[] colliders = visual.GetComponentsInChildren<Collider>(true);
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }

            Rigidbody[] rigidbodies = visual.GetComponentsInChildren<Rigidbody>(true);
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                Object.Destroy(rigidbody);
            }

            Camera[] cameras = visual.GetComponentsInChildren<Camera>(true);
            foreach (Camera camera in cameras)
            {
                camera.enabled = false;
            }

            Light[] lights = visual.GetComponentsInChildren<Light>(true);
            foreach (Light light in lights)
            {
                light.enabled = false;
            }

            GameApi.Debug(
                "Prepared player visual components: disabled colliders=" + colliders.Length +
                ", removed rigidbodies=" + rigidbodies.Length +
                ", disabled cameras=" + cameras.Length +
                ", disabled lights=" + lights.Length + ".");
        }

        private static void SetActiveRecursive(Transform root)
        {
            root.gameObject.SetActive(true);
            foreach (Transform child in root)
            {
                SetActiveRecursive(child);
            }
        }

        private static void LogRenderer(Renderer renderer)
        {
            Bounds bounds = renderer.bounds;
            GameApi.Debug(
                "Prepared visual renderer: " + renderer.name +
                ", enabled=" + renderer.enabled +
                ", activeInHierarchy=" + renderer.gameObject.activeInHierarchy +
                ", material=" + SwimmingRingVisualMaterial.GetMaterialName(renderer) +
                ", shader=" + SwimmingRingVisualMaterial.GetShaderName(renderer) +
                ", texture=" + SwimmingRingVisualMaterial.GetTextureName(renderer) +
                ", boundsCenter=" + bounds.center +
                ", boundsSize=" + bounds.size + ".");
        }
    }
}
