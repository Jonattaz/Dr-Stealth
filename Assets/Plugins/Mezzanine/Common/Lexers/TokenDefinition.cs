/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

namespace Mz.Lexers {
    public sealed class TokenDefinition
    {
        public readonly ILexer Lexer;
        public readonly IMatcher Matcher;
        public readonly object Type;
        public readonly int Precedence;
        public readonly int CaptureGroup;

        /// <summary>
        /// Define a token type and the regex pattern that should be used to identify the token within a given block of text.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="regexPattern"></param>
        /// <param name="captureGroup">
        /// If this value is set, the corresponding capture group in the regex will be used to set the value of matched tokens.
        /// </param>
        /// <param name="precedence">
        /// When two TokenDefinitions could identify a match with the same Start index then the precedence becomes important.
        /// For example, Token definitions for DateTime and Number values might both match a given string.
        /// We assign the DateTime TokenDefinition a precedence of 1 and the Number TokenDefinition a precedence of 2.
        /// This means that when both have a match for a date, the DateTime token is selected as the best match.
        /// </param>
        public TokenDefinition(ILexer lexer, object type, string regexPattern, int captureGroup = -1, int precedence = 1)
        {
            Lexer = lexer;
            Matcher = new MatcherRegex(this, regexPattern);
            Type = type;
            Precedence = precedence;
            CaptureGroup = captureGroup;
        }
    }
}
