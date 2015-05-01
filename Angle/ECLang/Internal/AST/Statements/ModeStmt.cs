namespace ECLang.Internal.AST.Statements
{
    using System.Text.RegularExpressions;

    using ECLang.AST;

    public class ModeStmt : Statement
    {
        #region Public Properties

        public string Type { get; set; }

        #endregion

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }

        #region Public Methods and Operators

        public override Statement Interprete(string src, int line)
        {
            var returns = new ModeStmt();
            returns.Line = line;
            var regexp = new Regex(Parser.Grammar.GetPattern("mode").ToString());

            returns.Type = regexp.Match(src).Groups["type"].Value;

            return returns;
        }

        public override bool IsStatement(string src)
        {
            return Parser.Grammar.GetPattern("mode").IsValid(src);
        }

        #endregion
    }
}