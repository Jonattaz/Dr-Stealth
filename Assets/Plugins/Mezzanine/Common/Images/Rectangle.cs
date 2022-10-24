using Mz.Numerics;

namespace Mz.Images
{
    public partial class MzImage
    {
        public MzRectangle ValidateRectangle(MzRectangle rectangle)
        {
            if (rectangle.XInt + rectangle.WidthInt > Width)
            {
                rectangle.Width = Width - rectangle.X;
            }

            if (rectangle.YInt + rectangle.HeightInt > Height)
            {
                rectangle.Height = Height - rectangle.Y;
            }

            if (rectangle.XInt < 0)
            {
                rectangle.Width = rectangle.WidthInt + rectangle.XInt;
                rectangle.X = 0;
            }

            if (rectangle.Y < 0)
            {
                rectangle.Height = rectangle.HeightInt + rectangle.YInt;
                rectangle.Y = 0;
            }

            if (rectangle.XInt + rectangle.WidthInt > Width) rectangle.Width = Width - rectangle.XInt;
            if (rectangle.YInt + rectangle.HeightInt > Height) rectangle.Height = Height - rectangle.YInt;

            return rectangle;
        }
    }
}