using UnityEngine;

namespace Mz.App.UI
{
    public abstract class ImageRoundedModifier {
        /// <summary>
        /// Calculates the border-radius for ImageRounded.
        /// </summary>
        /// <returns>The radius as Vector4. (start top-left, clockwise)</returns>
        /// <param name="imageRect">ImageRounded RectTransform</param>
        public abstract Vector4 CalculateRadius (Rect imageRect);
    }
}