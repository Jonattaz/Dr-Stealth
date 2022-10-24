using Mz.Numerics;

namespace Mz.Images
{
    public static partial class Effects
    {
        public static MzImage FastBlur(MzImage image, int radius)
        {
            if (radius < 1)
            {
                return image;
            }
        
            var w = image.Width;
            var h = image.Height;
            var wm = w - 1;
            var hm = h - 1;
            var wh = w * h;
            var div = radius + radius + 1;
            var r = new int[wh];
            var g = new int[wh];
            var b = new int[wh];
            int rsum, gsum, bsum, x, y, i, p1, p2, yp, yi, yw;
            var vmin = new int[Numbers.Max(w, h)];
            var vmax = new int[Numbers.Max(w, h)];
        
            var bytes = image.GetBytes();
        
            var dv = new int[256 * div];
            for (i = 0; i < 256 * div; i++)
            {
                dv[i] = (i / div);
            }
        
            yw = yi = 0;
        
            for (y = 0; y < h; y++)
            {
                rsum = gsum = bsum = 0;
                for (i = -radius; i <= radius; i++)
                {
                    var index = yi + Numbers.Min(wm, Numbers.Max(i, 0));
                    rsum += bytes[index * 4 + 0];
                    gsum += bytes[index * 4 + 1];
                    bsum += bytes[index * 4 + 2];
                }
        
                for (x = 0; x < w; x++)
                {
                    r[yi] = dv[rsum];
                    g[yi] = dv[gsum];
                    b[yi] = dv[bsum];
        
                    if (y == 0)
                    {
                        vmin[x] = Numbers.Min(x + radius + 1, wm);
                        vmax[x] = Numbers.Max(x - radius, 0);
                    }
        
                    var index1 = (yw + vmin[x]) * 4;
                    var index2 = (yw + vmax[x]) * 4;
        
                    rsum += bytes[index1 + 0] - bytes[index2 + 0];
                    gsum += bytes[index1 + 1] - bytes[index2 + 1];
                    bsum += bytes[index1 + 2] - bytes[index2 + 2];
        
                    yi++;
                }
        
                yw += w;
            }
        
            for (x = 0; x < w; x++)
            {
                rsum = gsum = bsum = 0;
                yp = -radius * w;
                for (i = -radius; i <= radius; i++)
                {
                    yi = Numbers.Max(0, yp) + x;
                    rsum += r[yi];
                    gsum += g[yi];
                    bsum += b[yi];
                    yp += w;
                }
        
                yi = x;
        
                for (y = 0; y < h; y++)
                {
                    bytes[yi * 4 + 0] = (byte) dv[rsum];
                    bytes[yi * 4 + 1] = (byte) dv[gsum];
                    bytes[yi * 4 + 2] = (byte) dv[bsum];
                    bytes[yi * 4 + 3] = 0xff;
        
                    if (x == 0)
                    {
                        vmin[y] = Numbers.Min(y + radius + 1, hm) * w;
                        vmax[y] = Numbers.Max(y - radius, 0) * w;
                    }
        
                    p1 = x + vmin[y];
                    p2 = x + vmax[y];
        
                    rsum += r[p1] - r[p2];
                    gsum += g[p1] - g[p2];
                    bsum += b[p1] - b[p2];
        
                    yi += w;
                }
            }

            image.SetBytes(bytes);
        
            return image;
        }
    }
}