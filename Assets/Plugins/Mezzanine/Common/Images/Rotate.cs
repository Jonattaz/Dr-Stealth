namespace Mz.Images
{
    public partial class MzImage
	{
		public MzImage GetRotated90DegreesClockwise() => _GetRotated(this, 270);
		public MzImage GetRotated180Degrees() => _GetRotated(this, 180);
		public MzImage GetRotated270DegreesClockwise() => _GetRotated(this, 90);

		private static MzImage _GetRotated(MzImage imageOriginal, int rotationAngleInDegreesClockwise)
		{
			MzImage imageRotated;
			
            var widthOriginal = imageOriginal.Width;
            var heightOriginal = imageOriginal.Height;
            
            var widthRotated = imageOriginal.Width;
            var heightRotated = imageOriginal.Height;
            
            int widthRotatedMinusOne;
            int heightRotatedMinusOne;

            switch (rotationAngleInDegreesClockwise)
            {
                case 90:
                    widthRotated = imageOriginal.Height;
                    heightRotated = imageOriginal.Width;
                    
                    widthRotatedMinusOne = widthRotated - 1;
                    
                    imageRotated = new MzImage(widthRotated, heightRotated);
            
                    for (var y = 0; y < heightOriginal; ++y)
                    {
                        var xRotated = widthRotatedMinusOne - y;
                        for (var x = 0; x < widthOriginal; ++x)
                        {
                            var yRotated = x;
                            var rgba = imageOriginal.GetPixel(x, y);
                            imageRotated.SetPixel(xRotated, yRotated, rgba);
                        }
                    }
            
                    return imageRotated;
                case 180:
                    widthRotated = imageOriginal.Width;
                    heightRotated = imageOriginal.Height;
                    
                    widthRotatedMinusOne = widthRotated - 1;
                    heightRotatedMinusOne = heightRotated - 1;
                    
                    imageRotated = new MzImage(widthRotated, heightRotated);
                    
                    for (var y = 0; y < heightOriginal; ++y)
                    {
                        var yRotated = (heightRotatedMinusOne - y) * widthRotated;
                        for (var x = 0; x < widthOriginal; ++x)
                        {
                            var xRotated = widthRotatedMinusOne - x;
                            var rgba = imageOriginal.GetPixel(x, y);
                            imageRotated.SetPixel(xRotated, yRotated, rgba);
                        }
                    }
            
                    return imageRotated;
                case 270:
                    widthRotated = imageOriginal.Height;
                    heightRotated = imageOriginal.Width;
                    
                    heightRotatedMinusOne = heightRotated - 1;
                    
                    imageRotated = new MzImage(widthRotated, heightRotated);
                    
                    for (var y = 0; y < heightOriginal; ++y)
                    {
                        var xRotated = y;
                        for (var x = 0; x < widthOriginal; ++x)
                        {
                            var yRotated = heightRotatedMinusOne - x;
                            var rgba = imageOriginal.GetPixel(x, y);
                            imageRotated.SetPixel(xRotated, yRotated, rgba);
                        }
                    }
            
                    return imageRotated;
            }

            return imageOriginal.Clone();
		}
	}
}