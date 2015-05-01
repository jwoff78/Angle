namespace ECLang.Internal.AST.Statements
{
    using System.Text.RegularExpressions;

    using ECLang.AST;

    public class AttributeStmt : Statement
    {
        #region Fields

        public string Attribute = "";

        #endregion

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }

        #region Public Methods and Operators

        public override Statement Interprete(string src, int line)
        {
            var returns = new AttributeStmt();
            returns.Line = line;
            var regexp = new Regex(Parser.Grammar.GetPattern("attribute").ToString());
            Match match = regexp.Match(src);
            returns.Attribute = match.Value;
            return returns;
        }

        public override bool IsStatement(string src)
        {
            return Parser.Grammar.GetPattern("attribute").IsValid(src);
        }

        #endregion
    }
}