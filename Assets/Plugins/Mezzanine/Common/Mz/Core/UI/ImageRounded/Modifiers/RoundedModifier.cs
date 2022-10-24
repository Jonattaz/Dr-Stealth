using UnityEngine;

namespace Mz.App.UI
{
    public class RoundModifier : ImageRoundedModifier {
        public override Vector4 CalculateRadius (Rect imageRect){
            var r = Mathf.Min (imageRect.width,imageRect.height) * 0.5f;
            return new Vector4 (r,r,r,r);
        }
    }
}
