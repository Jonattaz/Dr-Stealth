using UnityEngine;

namespace Mz.Unity
{
    public static class Materials
    {
        public class MaterialState
        {
            public string RenderType { get; set; }
            public int SrcBlend { get; set; }
            public int DstBlend { get; set; }
            public int ZWrite { get; set; }
            public bool IsAlphaTestOn { get; set; }
            public bool IsAlphaBlendOn { get; set; }
            public bool IsAlphaPremultiplyOn { get; set; }
            public int RenderQueue { get; set; }
        }

        public static MaterialState GetMaterialState(this Material material)
        {
            var materialState = new MaterialState();

            materialState.RenderType = material.GetTag("RenderType", false);
            materialState.SrcBlend = material.GetInt("_SrcBlend");
            materialState.DstBlend = material.GetInt("_DstBlend");
            materialState.ZWrite = material.GetInt("_ZWrite");
            materialState.IsAlphaTestOn = material.IsKeywordEnabled("_ALPHATEST_ON");
            materialState.IsAlphaBlendOn = material.IsKeywordEnabled("_ALPHABLEND_ON");
            materialState.IsAlphaPremultiplyOn = material.IsKeywordEnabled("_ALPHAPREMULTIPLY_ON");
            materialState.RenderQueue = material.renderQueue;

            return materialState;
        }
        
        public static void SetMaterialState(this Material material, MaterialState materialState)
        {
            material.SetOverrideTag("RenderType", materialState.RenderType);
            material.SetInt("_SrcBlend", materialState.SrcBlend);
            material.SetInt("_DstBlend", materialState.DstBlend);
            material.SetInt("_ZWrite", materialState.ZWrite);
            
            if (materialState.IsAlphaTestOn) material.EnableKeyword("_ALPHATEST_ON");
            else material.DisableKeyword("_ALPHATEST_ON");
            
            if (materialState.IsAlphaBlendOn) material.EnableKeyword("_ALPHABLEND_ON");
            else material.DisableKeyword("_ALPHABLEND_ON");
            
            if (materialState.IsAlphaPremultiplyOn) material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            else material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            
            material.renderQueue = materialState.RenderQueue;
        }
        
        public static void ToOpaqueMode(this Material material)
        {
            material.SetOverrideTag("RenderType", "");
            material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }
   
        // See SetupMaterialWithBlendMode()
        // at https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Inspector/StandardShaderGUI.cs
        public static void ToFadeMode(this Material material)
        {
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
        }
    }
}