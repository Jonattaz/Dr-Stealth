/*
 * Initially based on MiniJson by Calvin Rien, which was in turn
 * based on the JSON parser by Patrick van Bergen
 * http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
 *
 * Moved most of the work into a simple, flexible lexer. The custom object builder
 * is based on DeJson by Gregg Tavares.
 */

using System.Collections.Generic;
using System.IO;
using Mz.Lexers.Json;
using Mz.TypeTools;

namespace Mz.Serializers {
    public class DeserializerJson {
        public static Dictionary<string, object> ToDictionary(TextReader reader) {
            var lexer = new Lexer();
            lexer.Start(reader);
            
            var containers = new Stack<object>();
            var keyCurrent = "root";
            containers.Push(new Dictionary<string, object>());

            while (lexer.Next()) {
                switch (lexer.Token.Type) {
                    case TokenType.Key:
                        keyCurrent = lexer.Token.Value;
                        keyCurrent = keyCurrent.Trim('"').Trim('\'');
                        break;
                    case TokenType.BracketLeft:
                        var objectDictionary = new Dictionary<string, object>();
                        _AddToParentContainer(containers, keyCurrent, objectDictionary);
                        containers.Push(objectDictionary);
                        break;
                    case TokenType.BracketRight:
                        containers.Pop();
                        break;
                    case TokenType.ArrayLeft:
                        var arrayList = new List<object>();
                        _AddToParentContainer(containers, keyCurrent, arrayList);
                        containers.Push(arrayList);
                        break;
                    case TokenType.ArrayRight:
                        containers.Pop();
                        break;
                    case TokenType.False:
                    case TokenType.True:
                        _AddToParentContainer(containers, keyCurrent, bool.Parse(lexer.Token.Value));
                        break;
                    case TokenType.Float:
                        _AddToParentContainer(containers, keyCurrent, double.Parse(lexer.Token.Value));
                        break;
                    case TokenType.Int:
                        _AddToParentContainer(containers, keyCurrent, int.Parse(lexer.Token.Value));
                        break;
                    case TokenType.String:
                        var text = lexer.Token.Value;
                        text = text.Trim('"').Trim('\'');
                        _AddToParentContainer(containers, keyCurrent, text);
                        break;
                }
            }

            if (!(containers.Peek() is Dictionary<string, object> root)) return new Dictionary<string, object>();
            
            root.TryGetValue("root", out var value);
            return (Dictionary<string, object>)value;
        }

        private static void _AddToParentContainer(Stack<object> containers, string key, object value)
        {
            switch (containers.Peek()) {
                case Dictionary<string, object> parentObject:
                    parentObject.Add(key, value);
                    break;
                case List<object> parentArray:
                    parentArray.Add(value);
                    break;
            }
        }

        public static TObject ToObject<TObject>(TextReader reader) {
            var dictionary = ToDictionary(reader);
            return (TObject)TypeConstructor.Construct(typeof(TObject), dictionary, false, false);
        }
    } 
}
