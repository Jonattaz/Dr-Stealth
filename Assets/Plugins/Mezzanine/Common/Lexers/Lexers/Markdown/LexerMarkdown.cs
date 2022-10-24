using System.Text.RegularExpressions;

namespace Mz.Lexers.Markdown {
    public enum TokenType {
        HeaderOne,
        HeaderTwo,
        HeaderThree,
        HeaderFour,
        Link,
        Bold,
        Emphasis,
        Strikethrough,
        Quote,
        Code,
        UnorderedListItem,
        OrderedListItem,
        Blockquote,
        HorizontalRule,
        Text,
        Newline,
        Html,
        Space
    }

    public class Lexer : LexerBase {
        public Lexer() : base(new TokenDefinition[0]) {
            _tokenDefinitions = new [] {
                new TokenDefinition(this, TokenType.HeaderOne, @"(#)(\s*)([^#].*)", 3),
                new TokenDefinition(this, TokenType.HeaderTwo, @"(##)(\s*)([^#].*)", 3),
                new TokenDefinition(this, TokenType.HeaderThree, @"(###)(\s*)([^#].*)", 3),
                new TokenDefinition(this, TokenType.HeaderFour, @"(####)(\s*)([^#].*)", 3),
                new TokenDefinition(this, TokenType.Link, @"\[([^\[]+)\]\(([^\)]+)\)"),
                new TokenDefinition(this, TokenType.Bold, @"(\*{2})([^\*].*?)\1", 2),
                new TokenDefinition(this, TokenType.Bold, @"(_{2})([^_].*?)\1", 2),
                new TokenDefinition(this, TokenType.Emphasis, @"((?<!\*)\*{1})([^\*].*?)\1", 2),
                new TokenDefinition(this, TokenType.Emphasis, @"((?<!_)_{1})([^_].*?)\1", 2),
                new TokenDefinition(this, TokenType.Strikethrough, @"\~\~(.*?)\~\~", 1),
                new TokenDefinition(this, TokenType.Quote, @"\:""(.*?)""\:", 1),
                new TokenDefinition(this, TokenType.Code, @"`(.*?)`", 1),
                new TokenDefinition(this, TokenType.UnorderedListItem, @"\n\*\s*(.*)"),
                new TokenDefinition(this, TokenType.OrderedListItem, @"\n[0-9]+\.\s*(.*)"),
                new TokenDefinition(this, TokenType.Blockquote, @"\n(&gt;|\>)\s*(.*)", 2),
                new TokenDefinition(this, TokenType.HorizontalRule, @"( *[-*_]){3,} *(?:\n+|$)"),
                new TokenDefinition(this, TokenType.Text, @"[\s\S]+?(?=[\\!\[_*`]| {2,}\n|$)", -1, 2),
                new TokenDefinition(this, TokenType.Newline, @"\n+"),
                new TokenDefinition(this, TokenType.Html, @"<.+?(\/>|(.+?<\/.+?>))"),
                new TokenDefinition(this, TokenType.Space, @"(\s+?)(?=.*)", 1)
           };
        }

        public override Token GetToken(object type, string value, int indexStart, int indexEnd, int precedence)
        {
            switch (type)
            {
                case TokenType.Bold:
                    return base.GetToken(type, value, indexStart, indexEnd, precedence);
                default:
                    return base.GetToken(type, value, indexStart, indexEnd, precedence);
            }
        }
    }
}