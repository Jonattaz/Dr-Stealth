using UnityEngine;

namespace Mz.App.UI
{
    /// <summary>
    /// Contains all parameters of a rounded image
    /// </summary>
    public struct ImageRoundedOptions
    {
        public readonly float Width;
        public readonly float Height;
        public readonly float FallOffDistance;
        public readonly Vector4 Radius;
        public readonly float BorderWidth;
        public readonly float PixelSize;

        public ImageRoundedOptions(float width, float height, float fallOffDistance, float pixelSize, Vector4 radius, float borderWidth)
        {
            Width = Mathf.Abs(width);
            Height = Mathf.Abs(height);
            FallOffDistance = Mathf.Max(0, fallOffDistance);
            Radius = radius;
            BorderWidth = Mathf.Max(borderWidth, 0);
            PixelSize = Mathf.Max(0, pixelSize);
        }
    }
}