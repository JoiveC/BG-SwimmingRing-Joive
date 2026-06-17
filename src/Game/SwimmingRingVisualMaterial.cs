using UnityEngine;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class SwimmingRingVisualMaterial
    {
        private const float RuntimeSmoothness = 0.45f;
        private const string RuntimeShaderName = "Standard";

        private static readonly Color FallbackOrange = new Color(1f, 0.58f, 0.22f, 1f);

        private static Material runtimeMaterial;
        private static bool missingShaderLogged;

        internal static Material GetRuntimeMaterial(Material sourceMaterial)
        {
            if (runtimeMaterial == null)
            {
                runtimeMaterial = CreateRuntimeMaterial();
                if (runtimeMaterial == null)
                {
                    return sourceMaterial;
                }
            }

            Texture sourceTexture = GetMainTexture(sourceMaterial);
            if (sourceTexture != null)
            {
                SetMaterialTexture(runtimeMaterial, sourceTexture);
                SetMaterialColor(runtimeMaterial, Color.white);
                return runtimeMaterial;
            }

            SetMaterialTexture(runtimeMaterial, null);
            SetMaterialColor(runtimeMaterial, FallbackOrange);
            return runtimeMaterial;
        }

        internal static string GetMaterialName(Renderer renderer)
        {
            Material material = renderer.sharedMaterial;
            return material != null ? material.name : "null";
        }

        internal static string GetShaderName(Renderer renderer)
        {
            Material material = renderer.sharedMaterial;
            return material != null && material.shader != null ? material.shader.name : "null";
        }

        internal static string GetTextureName(Renderer renderer)
        {
            Material material = renderer.sharedMaterial;
            Texture texture = GetMainTexture(material);
            return texture != null ? texture.name : "null";
        }

        private static Material CreateRuntimeMaterial()
        {
            Shader shader = Shader.Find(RuntimeShaderName);
            if (shader == null)
            {
                if (!missingShaderLogged)
                {
                    missingShaderLogged = true;
                    ModLog.Warning("Shader not found: " + RuntimeShaderName + ".");
                }

                return null;
            }

            Material material = new Material(shader);
            material.name = "SwimmingRingRuntimeOrangeMaterial";

            SetMaterialColor(material, FallbackOrange);
            if (material.HasProperty("_Metallic"))
            {
                material.SetFloat("_Metallic", 0f);
            }

            if (material.HasProperty("_Smoothness"))
            {
                material.SetFloat("_Smoothness", RuntimeSmoothness);
            }

            GameApi.Debug("Created runtime visual material with shader: " + shader.name + ".");
            return material;
        }

        private static void SetMaterialColor(Material material, Color color)
        {
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }
        }

        private static void SetMaterialTexture(Material material, Texture texture)
        {
            material.mainTexture = texture;
            if (material.HasProperty("_MainTex"))
            {
                material.SetTexture("_MainTex", texture);
            }

            if (material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", texture);
            }
        }

        private static Texture GetMainTexture(Material material)
        {
            if (material == null)
            {
                return null;
            }

            if (material.mainTexture != null)
            {
                return material.mainTexture;
            }

            if (material.HasProperty("_BaseMap"))
            {
                return material.GetTexture("_BaseMap");
            }

            if (material.HasProperty("_MainTex"))
            {
                return material.GetTexture("_MainTex");
            }

            return null;
        }
    }
}
