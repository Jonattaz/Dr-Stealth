using Mz.Numerics;

namespace Mz.Images
{
    public static partial class Effects
    {
        // See: http://d.hatena.ne.jp/flashrod/20061015
        public static void Threshold(
            MzImage image,
            MzRectangle rectangle,
            float threshold,
            byte[] rgbaHue,
            byte[] rgbaMask
        )
        {
            threshold *= 255;
        
            for (var y = rectangle.YInt; y < rectangle.YInt + rectangle.HeightInt; y++)
            {
                for (var x = rectangle.XInt; x < rectangle.XInt + rectangle.WidthInt; x++)
                {
                    var rgba = image.GetPixel(x, y);
                    var sum = (rgba[0] + rgba[1] + rgba[2]) / 3.0f;
                    var rgbaNew = sum <= threshold ? rgbaHue : rgbaMask;
                    image.SetPixel(x, y, rgbaNew);
                }
            }
        }
    }
}