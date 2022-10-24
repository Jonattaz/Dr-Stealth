using System.Runtime.InteropServices;
using UnityEngine;
using Mz.Numerics;

namespace Mz.Images
{
    public static class UnityImageTools
    {
        // See: https://stackoverflow.com/questions/21512259/fast-copy-of-color32-array-to-byte-array
        public static byte[] Color32ArrayToByteArray(Color32[] colors)
        {
            if (colors == null || colors.Length == 0) return new byte[0];

            var lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
            var length = lengthOfColor32 * colors.Length;
            var bytes = new byte[length];

            var handle = default(GCHandle);

            try
            {
                handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
                var intPtr = handle.AddrOfPinnedObject();
                Marshal.Copy(intPtr, bytes, 0, length);
            }
            finally
            {
                if (handle != default) handle.Free();
            }

            return bytes;
        }

        public static Texture2D DuplicateTextureAsReadable(Texture2D textureSource)
        {
            // See: https://issue.life/questions/44733841/how-to-make-texture2d-readable-via-script

            var renderTexture = RenderTexture.GetTemporary(
                textureSource.width,
                textureSource.height,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.sRGB);

            renderTexture.filterMode = FilterMode.Point;
            renderTexture.autoGenerateMips = false;
            renderTexture.antiAliasing = 1;
            renderTexture.useDynamicScale = false;
            renderTexture.useMipMap = false;

            Graphics.Blit(textureSource, renderTexture);
            var activeRenderTexturePrevious = RenderTexture.active;
            RenderTexture.active = renderTexture;
            var textureReadable = new Texture2D(textureSource.width, textureSource.height);
            textureReadable.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            textureReadable.Apply();
            RenderTexture.active = activeRenderTexturePrevious;
            RenderTexture.ReleaseTemporary(renderTexture);
            return textureReadable;
        }

        public static byte[] GetBytes(Texture2D texture)
        {
            // Since we can't be sure the Texture2D was imported as readable,
            // we need to create a RenderTexture from the Texture2D, in order to get the raw byte data.
            var textureReadable = DuplicateTextureAsReadable(texture);

            // RGBA32 texture format data layout exactly matches Color32 struct
            var color32Array = textureReadable.GetPixels32(0);

            return Color32ArrayToByteArray(color32Array);
        }

        public static MzImage MzImageFromTexture2D(Texture2D texture)
        {
            return new MzImage(GetBytes(texture), texture.width, texture.height);
        }

        public static Texture2D Texture2DFromMzImage(MzImage image)
        {
            var texture = new Texture2D(image.Width, image.Height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                anisoLevel = 0
            };

            texture.LoadRawTextureData(image.GetBytes());
            texture.Apply(false, false);

            return texture;
        }
        
        public static void LoadTextureDataFromMzImage(Texture2D texture, MzImage image)
        {
            texture.LoadRawTextureData(image.GetBytes());
            texture.Apply(false, false);
        }

        // public static void FillFromTexture(
        //     MzImage image,
        //     Texture2D texture,
        //     MzRectangle rectangle,
        //     ImageChannel channels = ImageChannel.All
        // )
        // {
        //     var bytes = GetBytes(texture);
        //     image.SetBytes(bytes);
        // }

        public static MzColor[] Color32ArrayToMzColorArray(Color32[] colors, int width, int height)
        {
            var mzColorArray = new MzColor[colors.Length];

            for (var i = 0; i < colors.Length; i++)
            {
                var color32 = colors[i];
                var color = new MzColor(color32.r, color32.g, color32.b, color32.a);
                mzColorArray[i] = color;
            }

            return mzColorArray;
        }

        public static Color32[] MzColorArrayToColor32Array(MzColor[] colors, int width, int height)
        {
            var color32Array = new Color32[colors.Length];
            
            for (var y = height - 1; y >= 0; y--)
            {
                for (var i = 0; i < colors.Length; i++)
                {
                    var color = colors[i];
                    var color32 = new Color32(color.RByte, color.GByte, color.BByte, color.AByte);
                    color32Array[i] = color32;
                }
            }

            return color32Array;
        }
    }
}