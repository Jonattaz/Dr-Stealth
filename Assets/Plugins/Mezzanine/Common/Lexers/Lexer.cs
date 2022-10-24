/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mz.Lexers {
    public class LexerBase : ILexer {
        public LexerBase(TokenDefinition[] tokenDefinitions) {
            _tokenDefinitions = tokenDefinitions;
        }

        private TextReader _reader;
        private string _lineRemaining;
        protected TokenDefinition[] _tokenDefinitions;

        public Token Token { get; private set; }
        public string MatchString { get; private set; }
        public int LineNumber { get; private set; }
        public int Position { get; private set; }

        //===== Precedence-based tokenization
        //===== See: https://jack-vanlightly.com/blog/2016/2/24/a-more-efficient-regex-tokenizer

        public Token Run(string inputString)
        {
            inputString = Normalize(inputString);
            return _Tokenize(inputString, new Token(true));
        }
        
        internal Token _Tokenize(string inputString, Token tokenIn = null)
        {
            var tokenBlock = tokenIn ?? new Token();
            var tokenMatches = _FindAllTokenMatches(inputString);
            tokenMatches = _FilterMatches(tokenMatches);
            
            foreach (var token in tokenMatches)
            {
                if (token != null) tokenBlock.Add(token);
            }

            return tokenBlock;
        }

        public virtual Token GetToken(
            object type,
            string value,
            int indexStart,
            int indexEnd,
            int precedence
        )
        {
            return new Token
            {
                Type = type,
                Value = value,
                StartIndex = indexStart,
                EndIndex = indexEnd,
                Precedence = precedence
            };
        }
        
        public virtual string Normalize(string src)
        {
            src = src
                .Replace("\u2424", "\n")
                .ReplaceRegex(@"\r\n|\n\r|\n|\r|\r\n|\n\n", "\r\n") // Normalize paragraph breaks
                .Replace("    ", "\t")
                .Replace("\u00a0", " ");
            
            // Remove extra whitespace from blank lines.
            src = Regex.Replace(src, @"^\s+$", "", RegexOptions.Multiline);

            return src;
        }
        
        private IEnumerable<Token> _FindAllTokenMatches(string inputString)
        {
            var tokenMatches = new List<Token>();

            foreach (var tokenDefinition in _tokenDefinitions)
            {
                tokenMatches.AddRange(tokenDefinition.Matcher.MatchAll(inputString).ToList());
            }

            return tokenMatches;
        }
        
        private IEnumerable<Token> _FilterMatches(IEnumerable<Token> tokenMatches)
        {
            var groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex).OrderBy(x => x.Key).ToList();

            Token lastMatch = null;
            foreach (var group in groupedByIndex) {
                var bestMatch = group.OrderBy(x => x.Precedence).First();
                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex) continue;
                if (bestMatch != null)
                {
                    yield return bestMatch;
                }
                lastMatch = bestMatch;
            }
        }

        private Token _GetBestMatch(IEnumerable<Token> tokenMatches)
        {
            return tokenMatches.OrderBy(x => x.Precedence).First();
        }

        //===== Character-by-character tokenization
 
        public ILexer Start(TextReader reader) {
            _reader = reader;
            MoveNextLine();
            return this;
        }

        /// <summary>
        /// This will run through the text from the beginning and chunk off token after token.
        /// Every chunk of text is expected to be covered by a token definition. If any piece of text
        /// can't be recognized as a token, the process will fail and either return false, or throw an error.
        /// </summary>
        public bool Next(bool isThrowErrors = true) {
            if (_lineRemaining == null) return false;

            try
            {
                // Run all the token definitions against the start of the line to see if any match.
                // If there is more than one match, use the precedence values to choose the best.
                
                var tokenMatches = _tokenDefinitions.Select(tokenDefinition => tokenDefinition.Matcher.MatchLineStart(_lineRemaining)).Where(tokenMatch => tokenMatch != null).ToList();
                var token = tokenMatches.OrderBy(x => x.Precedence).First();

                var matchLength = token.EndIndex - token.StartIndex;
                
                Position += matchLength;
                Token = token;
                MatchString = _lineRemaining.Substring(0, matchLength);
                
                _lineRemaining = _lineRemaining.Substring(matchLength);
                if (_lineRemaining.Length == 0) MoveNextLine();

                return true;
            }
            catch (Exception x)
            {
                if (!isThrowErrors) return false;
                throw new Exception($"Unable to match against any tokens at line {LineNumber}, position {Position} \"{_lineRemaining}\". {x.Message}");
            }
        }

        public char PeekChar => Convert.ToChar(_reader.Peek());

        public void MoveNextChar() {
            Position++;
            _lineRemaining = _lineRemaining.Substring(Position);
            if (_lineRemaining.Length == 0) MoveNextLine();
        }

        public void MoveNextLine() {
            do {
                _lineRemaining = _reader.ReadLine();
                ++LineNumber;
                Position = 0;
            } while (_lineRemaining != null && _lineRemaining.Length == 0);
        }

        public void Dispose() => _reader.Dispose();
    }
}