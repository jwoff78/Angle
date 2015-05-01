namespace ECLang.Internal.AST.Statements
{
    using System.Text.RegularExpressions;

    using ECLang.AST;

    public class DelStmt : Statement
    {
        #region Public Properties

        public string Name { get; set; }

        #endregion

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }

        #region Public Methods and Operators

        public override Statement Interprete(string src, int line)
        {
            var returns = new DelStmt();
            returns.Line = line;
            var regexp = new Regex(Parser.Grammar.GetPattern("delvar").ToString());

            returns.Name = regexp.Match(src).Groups["Name"].Value;

            return returns;
        }

        public override bool IsStatement(string src)
        {
            return Parser.Grammar.GetPattern("delvar").IsValid(src);
        }

        #endregion
    }
}