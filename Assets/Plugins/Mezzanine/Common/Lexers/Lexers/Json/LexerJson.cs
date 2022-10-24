namespace Mz.Lexers.Json {
    public enum TokenType {
        ArrayLeft,
        ArrayRight,
        BracketLeft,
        BracketRight,
        Colon,
        Comma,
        Dot,
        Float,
        Int,
        Key,
        Space,
        String,
        Symbol,
        ParenthesisLeft,
        ParenthesisRight,
        True,
        False,
        Null
    }

    public class Lexer : LexerBase {
        public Lexer() : base(new TokenDefinition[0]) {
            _tokenDefinitions = new TokenDefinition[] {
                new TokenDefinition(this, TokenType.Key, @"(?:\""|\')(?<key>[^""""]*)(?:\""|\')(?=:)"),
                new TokenDefinition(this, TokenType.ArrayLeft, @"\["),
                new TokenDefinition(this, TokenType.ArrayRight, @"\]"),
                new TokenDefinition(this, TokenType.String, @"([""'])(?:\\\1|.)*?\1"),
                new TokenDefinition(this, TokenType.Float, @"[-+]?\d*\.\d+([eE][-+]?\d+)?"),
                new TokenDefinition(this, TokenType.Int, @"[-+]?\d+"),
                new TokenDefinition(this, TokenType.True, @"true"),
                new TokenDefinition(this, TokenType.False, @"false"),
                new TokenDefinition(this, TokenType.Null, @"null"),
                new TokenDefinition(this, TokenType.Symbol, @"[*<>\?\-+/A-Za-z->!]+"),
                new TokenDefinition(this, TokenType.Dot, @"\."),
                new TokenDefinition(this, TokenType.Comma, @"\,"),
                new TokenDefinition(this, TokenType.BracketLeft, @"\{"),
                new TokenDefinition(this, TokenType.BracketRight, @"\}"),
                new TokenDefinition(this, TokenType.ParenthesisLeft, @"\("),
                new TokenDefinition(this, TokenType.ParenthesisRight, @"\)"),
                new TokenDefinition(this, TokenType.Space, @"\s"),
                new TokenDefinition(this, TokenType.Colon, @"\:")
            };
        }
    }
}
