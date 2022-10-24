using System;
using System.Text.RegularExpressions;

namespace Mz.StringTools
{
    public static partial class Strings
    {
        public static string Escape(string html, bool encode = false)
        {
            return Regex.Replace(html, !encode ? @"&(?!#?\w+;)" : @"&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }

        public static string Unescape(string html)
        {
            return Regex.Replace(html, @"&([#\w]+);", (Match match) =>
            {
                var n = match.Groups[1].Value;

                n = n.ToLower();
                if (n == "colon") return ":";
                if (n[0] == '#')
                {
                    return n[1] == 'x'
                        ? ((char)Convert.ToInt32(n.Substring(2), 16)).ToString()
                        : ((char)Convert.ToInt32(n.Substring(1))).ToString();
                }
                return string.Empty;
            });
        }
    }
}