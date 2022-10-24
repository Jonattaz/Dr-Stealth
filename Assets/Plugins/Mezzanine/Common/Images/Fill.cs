using Mz.Numerics;
using SkiaSharp;

namespace Mz.Images
{
    public partial class MzImage
    {
        public MzImage Clear(byte[] rgba)
        {
            var color = new SKColor(rgba[0], rgba[1], rgba[2], rgba[3]);
            _canvasSk.Clear(color);
            
            // push to the pixel buffer
            _canvasSk.Flush();

            return this;
        }
        
        public MzImage Fill(byte[] rgba, int xStart = 0, int yStart = 0, int width = -1, int height = -1)
        {
            var rectangle = new MzRectangle(xStart, yStart, width < 0 ? Width : width, height < 0 ? Height : height);
            rectangle = ValidateRectangle(rectangle);
            
            for (var y = 0; y < rectangle.HeightInt; y++)
            {
                for (var x = 0; x < rectangle.WidthInt; x++)
                {
                    SetPixel(x + rectangle.XInt, y + rectangle.YInt, rgba);
                }
            }

            return this;
        }
    }
}