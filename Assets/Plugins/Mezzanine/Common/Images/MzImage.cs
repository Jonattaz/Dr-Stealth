using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mz.Numerics;
using SkiaSharp;

namespace Mz.Images
{
    public partial class MzImage : ICloneable, IDisposable
    {
        public MzImage(int width, int height) : this(width, height, new byte[] {0, 0, 0, 255}) {}

        public MzImage(int width, int height, byte[] rgbaFill)
        {
            var info = new SKImageInfo(width, height)
            {
                ColorType = SKColorType.Rgba8888
            };
            
            ByteSize = info.BytesSize;
            _imageSk = new SKBitmap(info);
            Width = info.Width;
            Height = info.Height;
            
            // canvas
            _canvasSk = new SKCanvas(_imageSk);

            // flip the texture due to coordinate differences
            _canvasSk.Scale(1, -1, 0, _imageSk.Height / 2f);
            
            Fill(rgbaFill);
        }
        
        public MzImage(byte[] bytes, int width, int height)
        {
            var info = new SKImageInfo(width, height)
            {
                ColorType = SKColorType.Rgba8888
            };
            
            ByteSize = info.BytesSize;
            _imageSk = new SKBitmap(info);
            Width = info.Width;
            Height = info.Height;
            
            // canvas
            _canvasSk = new SKCanvas(_imageSk);

            // flip the texture due to coordinate differences
            _canvasSk.Scale(1, -1, 0, _imageSk.Height / 2f);
            
            _canvasSk.Clear(SKColors.Black);

            SetBytes(bytes);
        }
  
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ByteSize { get; private set; }
        public int BitsPerPixel { get; } = 32;
        public int BytesPerPixel => BitsPerPixel / 8;
        public int Stride => Width * BytesPerPixel;

        private SKBitmap _imageSk;
        private SKCanvas _canvasSk;
        public SKCanvas Canvas => _canvasSk;
        
        /// <summary>
        /// Gets or sets the color of a pixel by its coordinates.
        /// </summary>
        /// <param name="x">The X location of the pixel.</param>
        /// <param name="y">The Y location of the pixel.</param>
        public byte[] this[int x, int y]
        {
            get => GetPixel(x, y);
            set => SetPixel(x, y, value);
        }

        object ICloneable.Clone() => Clone();
        public MzImage Clone()
        {
            var image = new MzImage(GetBytes(), Width, Height);

            return image;
        }

        public Byte[] GetPixel(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return new [] { (byte)0xff, (byte)0xff, (byte)0xff, (byte)0 };
            }

            var color = _imageSk.GetPixel(x, y);
            return new [] { color.Red, color.Green, color.Blue, color.Alpha };
        }
        
        public void SetPixel(int x, int y, byte[] rgba)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height || rgba == null || rgba.Length < 1) return;
            _imageSk.SetPixel(x, y, new SKColor(rgba[0], rgba[1], rgba[2], rgba[3]));
        }
        
        /// <summary>
        /// The returned array is a flattened 2D array, where pixels
        /// are laid out left to right, top to bottom.
        /// NOTE: Unity Color32 arrays are laid out bottom to top.
        /// </summary>
        public MzColor[] GetPixels(int xStart = 0, int yStart = 0, int width = -1, int height = -1)
        {
            if (width < 0) width = Width;
            if (height < 0) height = Height;
            var pixels = new MzColor[width * height];

            var index = 0;
            for (var y = yStart; y < yStart + height; y++)
            {
                for (var x = xStart; x < xStart + width; x++)
                {
                    var rgba = GetPixel(x, y);
                    pixels[index] = new MzColor(rgba);
                    index++;
                }
            }

            return pixels;
        }
        
        public void SetPixels(MzColor[] pixels, int xStart = 0, int yStart = 0, int width = -1, int height = -1)
        {
            var index = 0;
            if (width < 0) width = Width;
            if (height < 0) height = Height;
            for (var y = yStart; y < yStart + height; y++)
            {
                for (var x = xStart; x < xStart + width; x++)
                {
                    var pixel = pixels[index];
                    var rgba = new [] { pixel.RByte, pixel.GByte, pixel.BByte, pixel.AByte};
                    SetPixel(x, y, rgba);
                    index++;
                }
            }
        }
        
        /// <summary>
        /// A flattened 2D array, where pixel data is represented
        /// by bytes for red, green, blue, and alpha respectively.
        /// The byte sequences are laid out left-to-right, bottom-to-top,
        /// so as to match Unity's expected format for texture byte data.
        /// </summary>
        public byte[] GetBytes(int xStart = 0, int yStart = 0, int width = -1, int height = -1)
        {
            var intPtr = _imageSk.GetPixels();
            var bytesAll = new byte[ByteSize];
            Marshal.Copy(intPtr, bytesAll, 0, ByteSize);

            if (xStart == 0 && yStart == 0 && width < 0 && height < 0) return bytesAll;

            var bytes = new List<byte>();

            for (var y = yStart; y < yStart + height; y++)
            {
                for (var x = xStart; x < xStart + width; x++)
                {
                    var indexBytesStart = x * BytesPerPixel + y * Stride;
                    var r = bytesAll[indexBytesStart];
                    var g = bytesAll[indexBytesStart + 1];
                    var b = bytesAll[indexBytesStart + 2];
                    var a = bytesAll[indexBytesStart + 3];
                    
                    bytes.Add(r);
                    bytes.Add(g);
                    bytes.Add(b);
                    bytes.Add(a);
                }
            }

            return bytes.ToArray();
        }
        
        public MzImage SetBytes(byte[] bytes)
        {
            // Bytes will be arranged by row from bottom-to-top
            var yPixel = 0;
            for (var y = Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < Width; x++)
                {
                    var indexBytesStart = x * BytesPerPixel + y * Stride;
                    var r = bytes[indexBytesStart];
                    var g = bytes[indexBytesStart + 1];
                    var b = bytes[indexBytesStart + 2];
                    var a = bytes[indexBytesStart + 3];
       
                    SetPixel(x, yPixel, new byte[] { r, g, b, a });
                }

                yPixel++;
            }

            return this;
        }

        public void Dispose()
        {
            _imageSk = null;
        }
    }
}