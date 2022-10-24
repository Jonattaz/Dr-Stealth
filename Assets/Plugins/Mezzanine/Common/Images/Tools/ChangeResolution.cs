using Mz.Numerics;

namespace Mz.Images
{
    public static partial class Tools
    {
        public static void ChangeResolution(MzImage imageSource, MzImage imageDestination, int width, int height)
        {
            var u = imageSource.Width / width; // Number of horizontal pixels per block
            var v = imageSource.Height / height; // Number of vertical pixels per block

            var n_u = imageSource.Width % width;
            var n_v = imageSource.Height % height;

            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    var rect = new MzRectangle(i * u, j * v, u, v);
                    var colorAverage = Tools.ColorAverage(imageSource, rect);
                    imageDestination.Fill(colorAverage, i * u, j * v, u, v);
                }
            }
            
            MzRectangle t_rect;
            byte[] t_rgb;
            
            if (n_v > 0)
            {
                for (var i = 0; i < width; i++)
                {
                    t_rect = new MzRectangle(i * u, height * v, u, n_v);
                    t_rgb = Tools.ColorAverage(imageSource, t_rect);
                    imageDestination.Fill(t_rgb, t_rect.XInt, t_rect.YInt, t_rect.WidthInt, t_rect.HeightInt);
                }
            }

            if (n_u > 0)
            {
                for (var j = 0; j < height; j++)
                {
                    t_rect = new MzRectangle(width * u, j * v, n_u, v);
                    t_rgb = Tools.ColorAverage(imageSource, t_rect);
                    imageDestination.Fill(t_rgb, t_rect.XInt, t_rect.YInt, t_rect.WidthInt, t_rect.HeightInt);
                }
            }

            if (n_v <= 0 || n_u <= 0) return;
            
            t_rect = new MzRectangle(width * u, height * v, n_u, n_v);
            t_rgb = Tools.ColorAverage(imageSource, t_rect);
            imageDestination.Fill(t_rgb, t_rect.XInt, t_rect.YInt, t_rect.WidthInt, t_rect.HeightInt);
        }
    }
}