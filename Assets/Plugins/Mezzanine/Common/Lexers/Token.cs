/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using Mz.Nodes;

namespace Mz.Lexers {
    public class Token : NodeBase<Token, Token>  {
        public Token(bool isRoot = false) : base(isRoot) {}
        
        public object Type { get; set; }
        public string Value { get; set; }
        
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Precedence { get; set; }

        public Token Add(Token token) => _Add(token);
    } 
}