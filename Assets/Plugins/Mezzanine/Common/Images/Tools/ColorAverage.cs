using Mz.Numerics;

namespace Mz.Images
{
    public static partial class Tools
    {
        public static byte[] ColorAverage(MzImage image, MzRectangle rectangle)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;
            byte a = 0;

            var bytes = image.GetBytes();
     
            for (var yRect = 0; yRect < rectangle.Height; yRect++)
            {
                for (var xRect = 0; xRect < rectangle.Width; xRect++)
                {
                    var x = rectangle.XInt + xRect;
                    var y = rectangle.YInt + yRect;
                    var indexStart = x * image.BytesPerPixel + y * image.Stride;
                    
                    if (indexStart >= bytes.Length) continue;
                    
                    r += bytes[indexStart];
                    g += bytes[indexStart + 1];
                    b += bytes[indexStart + 2];
                    a += bytes[indexStart + 3];
                }
            }

            var pixelCount = rectangle.WidthInt * rectangle.HeightInt;
            return new[]
            {
                (byte) (r / pixelCount),
                (byte) (g / pixelCount),
                (byte) (b / pixelCount),
                (byte) (a / pixelCount)
            };
        }
    }
}