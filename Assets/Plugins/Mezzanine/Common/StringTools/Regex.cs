using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mz.StringTools
{
    public static partial class Strings
    {
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