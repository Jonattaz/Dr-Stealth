using Mz.Numerics;

namespace Mz.Images
{
    public static partial class Effects
    {
        public static MzImage Multipy(
            MzImage image,
            byte[] bytes
        )
        {
            var bytesImage = image.GetBytes();
            
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var indexStart = x * image.BytesPerPixel + y * image.Width * image.BytesPerPixel;
                    
                    bytesImage[indexStart] = (byte) (bytesImage[indexStart] * (bytes[indexStart] / 255f));
                    bytesImage[indexStart + 1] = (byte) (bytesImage[indexStart + 1] * (bytes[indexStart + 1] / 255f));
                    bytesImage[indexStart + 2] = (byte) (bytesImage[indexStart + 2] * (bytes[indexStart + 2] / 255f));
                    bytesImage[indexStart + 3] = (byte) (bytesImage[indexStart + 3] * (bytes[indexStart + 3] / 255f));
                }
            }

            image.SetBytes(bytesImage);

            return image;
        }
        
        public static MzImage MultipyColors(MzImage image, float[] rgbaFactors)
        {
            return MultiplyColors(image, rgbaFactors, new MzRectangle(0, 0,image. Width, image.Height));
        }
        
        public static MzImage MultiplyColors(MzImage image, float[] rgbaFactors, MzRectangle rectangle)
        {
            var bytesImage = image.GetBytes();
            
            for (var y = rectangle.YInt; y < rectangle.Y + rectangle.Height; y++)
            {
                for (var x = rectangle.XInt; x < rectangle.X + rectangle.Width; x++)
                {
                    var indexStart = x * image.BytesPerPixel + y * image.Width * image.BytesPerPixel;

                    bytesImage[indexStart] = (byte) (bytesImage[indexStart] * rgbaFactors[0]);
                    bytesImage[indexStart + 1] = (byte) (bytesImage[indexStart + 1] * rgbaFactors[1]);
                    bytesImage[indexStart + 2] = (byte) (bytesImage[indexStart + 2] * rgbaFactors[2]);
                    bytesImage[indexStart + 3] = (byte) (bytesImage[indexStart + 3] * rgbaFactors[3]);
                }
            }

            image.SetBytes(bytesImage);
            
            return image;
        }
        
        public static MzImage Offset(MzImage image, byte[] bytes)
        {
            var bytesImage = image.GetBytes();
            
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var indexStart = x * image.BytesPerPixel + y * image.Width * image.BytesPerPixel;
                    
                    bytesImage[indexStart] = (byte) (bytesImage[indexStart] + bytes[indexStart]);
                    bytesImage[indexStart + 1] = (byte) (bytesImage[indexStart + 1] + bytes[indexStart + 1]);
                    bytesImage[indexStart + 2] = (byte) (bytesImage[indexStart + 2] + bytes[indexStart + 2]);
                    bytesImage[indexStart + 3] = (byte) (bytesImage[indexStart + 3] + bytes[indexStart + 3]);
                }
            }

            image.SetBytes(bytesImage);

            return image;
        }

        public static MzImage OffsetColors(MzImage image, byte[] rgbaOffsets)
        {
            return OffsetColors(image, rgbaOffsets, new MzRectangle(0, 0, image.Width, image.Height));
        }
        
        public static MzImage OffsetColors(MzImage image, byte[] rgbaOffsets, MzRectangle rectangle)
        {
            var bytesImage = image.GetBytes();
            
            for (var y = rectangle.YInt; y < rectangle.Y + rectangle.Height; y++)
            {
                for (var x = rectangle.XInt; x < rectangle.X + rectangle.Width; x++)
                {
                    var indexStart = x * image.BytesPerPixel + y * image.Width * image.BytesPerPixel;

                    bytesImage[indexStart] = (byte) (bytesImage[indexStart] * rgbaOffsets[0]);
                    bytesImage[indexStart + 1] = (byte) (bytesImage[indexStart + 1] + rgbaOffsets[1]);
                    bytesImage[indexStart + 2] = (byte) (bytesImage[indexStart + 2] + rgbaOffsets[2]);
                    bytesImage[indexStart + 3] = (byte) (bytesImage[indexStart + 3] + rgbaOffsets[3]);
                }
            }
            
            image.SetBytes(bytesImage);

            return image;
        }
    }
}