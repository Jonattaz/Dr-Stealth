using System;
using System.Collections.Generic;
using System.Linq;

namespace Mz.Identifiers {
    public static class Identifier {
        static Identifier() {
            _hashids = new Hashids("mezz.tech");
            _random = new Random();
            _sessionUniqueIntegerCategories = new Dictionary<string, long>();
        }

        public static void Initialize(string seed) {
            _hashids = new Hashids(seed);
        }
        
        private static Hashids _hashids;
        private static Random _random;

        public static string Get() {
            // The number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight, January 1, 0001.
            // Tack on a random int to be sure we have a unique value, in case we're running on a fast system.
            var ticks = DateTime.Now.Ticks;
            var random = (long) _random.Next();
            return Encode(ticks, random);
        }

        private static Dictionary<string, long> _sessionUniqueIntegerCategories;
        private static long _sessionUniqueIntegerDefault = 0;
        public static long GetSessionUniqueInteger(string category = null) {
            if (category == null) {
                var sessionUniqueInteger = _sessionUniqueIntegerDefault;
                
                if (sessionUniqueInteger >= long.MaxValue) sessionUniqueInteger = 0;
                else sessionUniqueInteger++;

                _sessionUniqueIntegerDefault = sessionUniqueInteger;

                return sessionUniqueInteger;
            } else {
                var isSuccess = _sessionUniqueIntegerCategories.TryGetValue(category, out var sessionUniqueInteger);
                if (!isSuccess) sessionUniqueInteger = 0;

                if (sessionUniqueInteger >= long.MaxValue) sessionUniqueInteger = 0;
                else sessionUniqueInteger++;

                _sessionUniqueIntegerCategories[category] = sessionUniqueInteger;

                return sessionUniqueInteger;
            }
        }
        
        //===== Encode

        public static string Encode(Uri value) {
            return Encode(value.ToString());
        }
        
        public static string Encode(string value) {
            return Encode(System.Text.Encoding.UTF8.GetBytes(value));
        }
        
        public static string Encode(params byte[] values) {
            return Encode(values.Select(x => (int)x).ToArray());
        }
        
        public static string Encode(params int[] values) {
            return _hashids.Encode(values);
        }
        
        public static string Encode(params long[] values) {
            return _hashids.EncodeLong(values);
        }
        
        public static string Encode(Guid guid) {
            return Encode(guid.ToByteArray());
        }
        
        //===== Decode
        
        public static string Decode(string key) {
            byte[] byteArray = DecodeByteArray(key);
            return System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
        }

        public static Uri DecodeUri(string key) {
            return new Uri(Decode(key));
        }

        public static byte[] DecodeByteArray(string key) {
            var intArray = DecodeIntArray(key);
            return intArray.Select(x => (byte)x).ToArray();
        }
        
        public static int[] DecodeIntArray(string key) {
            return _hashids.Decode(key);
        }

        public static long[] DecodeLongArray(string key) {
            return _hashids.DecodeLong(key);
        }
        
        public static Guid DecodeGuid(string key) {
            return new Guid(DecodeByteArray(key));
        }
    }
}