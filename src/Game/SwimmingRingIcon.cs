using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class SwimmingRingIcon
    {
        private const string IconResourceFileName = "SwimmingRingIcon.png";
        private const int Size = 128;

        private static Sprite icon;

        internal static Sprite GetIcon()
        {
            if (icon != null)
            {
                return icon;
            }

            icon = LoadEmbeddedIcon();
            if (icon != null)
            {
                return icon;
            }

            Texture2D texture = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
            Color clear = new Color(0f, 0f, 0f, 0f);
            Color body = new Color(0.08f, 0.34f, 0.95f, 1f);
            Color highlight = new Color(0.78f, 0.92f, 1f, 1f);
            Color shadow = new Color(0.02f, 0.08f, 0.28f, 1f);

            Vector2 center = new Vector2((Size - 1) * 0.5f, (Size - 1) * 0.5f);
            float outerRadius = Size * 0.42f;
            float innerRadius = Size * 0.22f;
            float outlineRadius = Size * 0.45f;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    Color pixel = clear;

                    if (distance <= outlineRadius && distance >= innerRadius * 0.92f)
                    {
                        pixel = shadow;
                    }

                    if (distance <= outerRadius && distance >= innerRadius)
                    {
                        float angle = Mathf.Atan2(y - center.y, x - center.x);
                        bool stripe = Mathf.Sin(angle * 6f) > 0.35f;
                        pixel = stripe ? highlight : body;
                    }

                    texture.SetPixel(x, y, pixel);
                }
            }

            texture.Apply();
            texture.name = "SwimmingRingIcon";
            texture.hideFlags = HideFlags.DontUnloadUnusedAsset;

            icon = Sprite.Create(texture, new Rect(0f, 0f, Size, Size), new Vector2(0.5f, 0.5f), Size);
            icon.name = "SwimmingRingIcon";
            icon.hideFlags = HideFlags.DontUnloadUnusedAsset;
            return icon;
        }

        private static Sprite LoadEmbeddedIcon()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = FindIconResourceName(assembly);
                if (resourceName == null)
                {
                    ModLog.Warning("Swimming ring icon resource was not found.");
                    return null;
                }

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        ModLog.Warning("Swimming ring icon resource could not be opened.");
                        return null;
                    }

                    using (MemoryStream memory = new MemoryStream())
                    {
                        stream.CopyTo(memory);

                        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                        if (!GameApi.LoadImage(texture, memory.ToArray()))
                        {
                            ModLog.Warning("Swimming ring icon PNG could not be loaded.");
                            return null;
                        }

                        texture.name = "SwimmingRingIcon";
                        texture.hideFlags = HideFlags.DontUnloadUnusedAsset;

                        Sprite sprite = Sprite.Create(
                            texture,
                            new Rect(0f, 0f, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f),
                            Mathf.Max(texture.width, texture.height));

                        sprite.name = "SwimmingRingIcon";
                        sprite.hideFlags = HideFlags.DontUnloadUnusedAsset;
                        return sprite;
                    }
                }
            }
            catch (Exception ex)
            {
                ModLog.Warning("Swimming ring icon failed to load: " + ex.Message);
                return null;
            }
        }

        private static string FindIconResourceName(Assembly assembly)
        {
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith(IconResourceFileName, StringComparison.OrdinalIgnoreCase))
                {
                    return resourceName;
                }
            }

            return null;
        }

    }
}
