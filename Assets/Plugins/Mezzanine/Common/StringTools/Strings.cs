using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mz.StringTools
{
    public static partial class Strings
    {
        private static Random _random;

        private static string _allPrintableCharacters = "";

        public static string AllPrintableCharacters
        {
            get
            {
                if (_allPrintableCharacters == "")
                {
                    _allPrintableCharacters = GetAllPrintableCharacters();
                }

                return _allPrintableCharacters;
            }
        }

        public static string GetAllPrintableCharacters()
        {
            const int firstPrintableAsciiCharacter = 32;
            const int lastPrintableAsciiCharacter = 126;

            var characters = new List<char>();

            for (var i = 0; i < lastPrintableAsciiCharacter - firstPrintableAsciiCharacter + 1; i++)
            {
                var charCode = firstPrintableAsciiCharacter + i;
                var character = Convert.ToChar(charCode);
                characters.Add(character);
            }

            return new string(characters.ToArray());
        }

        public static string GetRandomString(int length)
        {
            if (_random == null) _random = new Random();
            var random = _random;

            var characters = new List<char>();

            for (var i = 0; i < length; i++)
            {
                var index = random.Next(AllPrintableCharacters.Length);
                var character = AllPrintableCharacters[index];
                characters.Add(character);
            }

            return new string(characters.ToArray());
        }

        public static string GetCombinedString(string a, string b, double probability = 0.5)
        {
            if (_random == null) _random = new Random();
            var random = _random;

            var length = Math.Min(a.Length, b.Length);
            var characters = new List<char>();

            for (var i = 0; i < length; i++)
            {
                var character = random.NextDouble() < probability ? b[i] : a[i];
                characters.Add(character);
            }

            return new string(characters.ToArray());
        }

        public static string SentenceCase(string sourceString)
        {
            // start by converting entire string to lower case
            var lowerCase = sourceString.ToLower();
            // matches the first sentence of a string, as well as subsequent sentences
            var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
            // MatchEvaluator delegate defines replacement of sentence starts to uppercase
            var result = r.Replace(lowerCase, s => s.Value.ToUpper());
            return result;
        }
        
        public static string NotEmpty(IList<string> source, int index1, int index2)
        {
            return (source.Count > index1 && !String.IsNullOrEmpty(source[index1])) ? source[index1] : source[index2];
        }
    }
}