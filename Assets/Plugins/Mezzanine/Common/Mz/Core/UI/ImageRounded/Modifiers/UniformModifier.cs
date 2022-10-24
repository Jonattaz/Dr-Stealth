using UnityEngine;

namespace Mz.App.UI
{
    public class UniformModifier : ImageRoundedModifier {
        public UniformModifier(float radius)
        {
            Radius = radius;
        }
    
        public float Radius { get; set; }
        
        public override Vector4 CalculateRadius (Rect imageRect){
            var r = Mathf.Min (imageRect.width,imageRect.height) * 0.5f;
            if (Radius < r) r = Radius;
            return new Vector4 (r,r,r,r);
        }
    }
}