namespace Mz.Images
{
    public enum ImageChannel
    {
        Alpha = 1, 
        Red = 1 << 1, 
        Green = 1 << 2, 
        Blue = 1 << 3, 
        All = Alpha | Red | Green | Blue
    }
}