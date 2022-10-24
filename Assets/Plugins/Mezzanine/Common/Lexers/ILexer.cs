/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using System.Text.RegularExpressions;
using System.IO;

namespace Mz.Lexers {
    public interface ILexer : IDisposable {
        Token Token { get; }
        string MatchString { get; }
        int LineNumber { get; }
        int Position { get; }
        char PeekChar { get; }
        
        Token Run(string inputString);
        string Normalize(string src);

        Token GetToken(
            object type,
            string value,
            int indexStart,
            int indexEnd,
            int precendence
        );
        
        ILexer Start(TextReader reader);
        bool Next(bool isThrowErrors = true);
        void MoveNextChar();
        void MoveNextLine();
    }
}