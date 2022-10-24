/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mz.Lexers
{
    public static class StringTools
    {
        public static string DecodeURIComponent(string str)
        {
            return Uri.UnescapeDataString(str);
        }

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
        
        public static string NotEmpty(IList<string> source, int index1, int index2)
        {
            return (source.Count > index1 && !String.IsNullOrEmpty(source[index1])) ? source[index1] : source[index2];
        }
        
        public static string ReplaceRegex(this string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement);
        }

        public static IList<string> SplitRegex(this string input, string pattern)
        {
            return Regex.Split(input, pattern);
        }

        public static IList<string> Exec(this Regex regex, string src)
        {
            var match = regex.Match(src);
            if (!match.Success) return new string[0];

            return match.Groups.Cast<Group>().Select(x => x.Value).ToArray();
        }

        public static string[] Match(this string src, Regex regex)
        {
            return regex.Matches(src).Cast<Match>().Select(x => x.Value).ToArray();
        }
    }
}
