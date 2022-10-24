using System;

namespace Mz.StringTools
{
    public static partial class Strings
    {
        public static string DecodeUriComponent(string str)
        {
            return Uri.UnescapeDataString(str);
        }
    }
}