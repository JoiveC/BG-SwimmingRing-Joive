using System;
using System.IO;
using System.Reflection;
using Joive.BurglinGnomes.SwimmingRing.Items;
using UnityEngine;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class SwimmingRingAssets
    {
        private const string BundleFileName = "swimmingring.bundle";
        private const string VisualPrefabPath = "assets/prefabs/swimmingringvisual.prefab";

        private static bool loadAttempted;
        private static AssetBundle bundle;
        private static GameObject visualPrefab;

        internal static GameObject GetVisualPrefab()
        {
            return LoadVisualPrefab();
        }

        private static GameObject LoadVisualPrefab()
        {
            if (loadAttempted)
            {
                return visualPrefab;
            }

            loadAttempted = true;

            try
            {
                byte[] bytes = ReadBundleBytes();
                GameApi.Debug("Loading embedded AssetBundle bytes: " + bytes.Length + ".");

                bundle = AssetBundle.LoadFromMemory(bytes);
                if (bundle == null)
                {
                    ModLog.Warning("Embedded swimming ring bundle could not be loaded.");
                    return null;
                }

                GameApi.Debug("AssetBundle.LoadFromMemory succeeded for " + BundleFileName + ".");

                visualPrefab = bundle.LoadAsset<GameObject>(VisualPrefabPath);

                if (visualPrefab == null)
                {
                    ModLog.Warning("Swimming ring visual prefab was not found at " + VisualPrefabPath + ".");
                    return null;
                }

                visualPrefab.name = SwimmingRingDefinition.ItemName;
                visualPrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
                GameApi.Debug("Loaded prefab SwimmingRingVisual from embedded " + BundleFileName + ": " + visualPrefab.name + ".");
                return visualPrefab;
            }
            catch (Exception ex)
            {
                ModLog.Warning("Swimming ring bundle failed to load: " + ex.Message);
                return null;
            }
        }

        private static byte[] ReadBundleBytes()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = FindBundleResourceName(assembly);
            if (resourceName == null)
            {
                throw new InvalidOperationException("Missing embedded resource ending with " + BundleFileName + ".");
            }

            GameApi.Debug("Using embedded bundle resource: " + resourceName);
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not open embedded resource: " + resourceName);
                }

                using (MemoryStream memory = new MemoryStream())
                {
                    stream.CopyTo(memory);
                    return memory.ToArray();
                }
            }
        }

        private static string FindBundleResourceName(Assembly assembly)
        {
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith(BundleFileName, StringComparison.OrdinalIgnoreCase))
                {
                    return resourceName;
                }
            }

            return null;
        }
    }
}
