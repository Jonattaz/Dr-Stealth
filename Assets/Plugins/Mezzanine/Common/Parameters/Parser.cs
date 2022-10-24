/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System.IO;
using System.Collections.Generic;
using Mz.Lexers.Json;

namespace Mz.Parameters {
    public class Parser
    {
        public Parser(IParameterFactory factory)
        {
            _factory = factory;
        }

        IParameterFactory _factory;

        public IParameterCollection Parse(TextReader reader) {
            var lexer = new Lexer();
            lexer.Start(reader);

            var isParsingArray = false;
            var parameterCurrent = _factory.ParameterInstance("root", null);
            var parameterStack = new Stack<IParameter>();
            var arrayList = new List<object>();

            while (lexer.Next()) {
                switch (lexer.Token.Type) {
                    case TokenType.Key:
                        var key = lexer.Token.Value;
                        key = key.Trim('"').Trim('\'');
                        parameterCurrent = _factory.ParameterInstance(key, null);
                        var parameterCollectionCurrent = parameterStack.Peek().GetValue<IParameterCollection>();
                        parameterCollectionCurrent.Add(parameterCurrent);
                        break;
                    case TokenType.BracketLeft:
                        if (!isParsingArray) {
                            parameterStack.Push(parameterCurrent);
                            var parameters = _factory.ParameterCollection();
                            parameterStack.Peek().Value = parameters;
                        }
                        break;
                    case TokenType.BracketRight:
                        if (!isParsingArray) {
                            var popped = parameterStack.Pop();
                            parameterCurrent = parameterStack.Count > 0 ? parameterStack.Peek() : popped;
                        }
                        break;
                    case TokenType.ArrayLeft:
                        isParsingArray = true;
                        break;
                    case TokenType.ArrayRight:
                        isParsingArray = false;
                        parameterCurrent.Value = arrayList.ToArray();
                        parameterCurrent.Type = ParameterType.Array;
                        arrayList.Clear();
                        break;
                    case TokenType.False:
                    case TokenType.True:
                        if (!isParsingArray) parameterCurrent.Value = bool.Parse(lexer.Token.Value);
                        else arrayList.Add(bool.Parse(lexer.Token.Value));
                        break;
                    case TokenType.Float:
                        if (!isParsingArray) parameterCurrent.Value = double.Parse(lexer.Token.Value);
                        else arrayList.Add(double.Parse(lexer.Token.Value));
                        break;
                    case TokenType.Int:
                        if (!isParsingArray) parameterCurrent.Value = int.Parse(lexer.Token.Value);
                        else arrayList.Add(int.Parse(lexer.Token.Value));
                        break;
                    case TokenType.String:
                        var text = lexer.Token.Value;
                        text = text.Trim('"').Trim('\'');
                        if (!isParsingArray) parameterCurrent.Value = text;
                        else arrayList.Add(text);
                        break;
                }
            }

            return parameterCurrent.GetValue<IParameterCollection>();
        }
    }
}
