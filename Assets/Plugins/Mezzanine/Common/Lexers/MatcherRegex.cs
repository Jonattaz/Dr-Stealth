/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mz.Lexers {
    sealed class MatcherRegex : IMatcher {
        private readonly Regex _regex;
        private readonly Regex _regexLineStart;
        private readonly TokenDefinition _tokenDefinition;

        public MatcherRegex(TokenDefinition tokenDefinition, string regexPattern) {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _regexLineStart = new Regex($"^{regexPattern}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _tokenDefinition = tokenDefinition;
        }

        public Token MatchLineStart(string inputString) {
            var match = _regexLineStart.Match(inputString);

            if (!match.Success) return null;
            
            var token = _tokenDefinition.Lexer.GetToken(
                _tokenDefinition.Type,
                _tokenDefinition.CaptureGroup > 0 ? match.Groups[_tokenDefinition.CaptureGroup].Value : match.Value,
                match.Index,
                match.Index + match.Length,
                _tokenDefinition.Precedence
            );

            return token;

        }

        public IEnumerable<Token> MatchAll(string inputString) {
            var matches = _regex.Matches(inputString);
            for (var i = 0; i < matches.Count; i++) {
                yield return _tokenDefinition.Lexer.GetToken(
                    _tokenDefinition.Type,
                    _tokenDefinition.CaptureGroup > 0 ? matches[i].Groups[_tokenDefinition.CaptureGroup].Value : matches[i].Value,
                    matches[i].Index,
                    matches[i].Index + matches[i].Length,
                    _tokenDefinition.Precedence
                );
            }
        }

        public override string ToString() => _regex.ToString();
    }
}
