namespace ECLang.Internal.AST.Statements
{
    using System.Text.RegularExpressions;

    using ECLang.AST;

    public class ImportStmt : Statement
    {
        #region Fields

        public string Name = "";

        #endregion

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }

        #region Public Methods and Operators

        public override Statement Interprete(string src, int line)
        {
            var returns = new ImportStmt();
            returns.Line = line;
            var regexp = new Regex(Parser.Grammar.GetPattern("import").ToString());
            Match match = regexp.Match(src);

            if (match.Success)
            {
                returns.Name = match.Groups["NameSpace"].Value;
            }
            return returns;
        }

        public override bool IsStatement(string src)
        {
            return Parser.Grammar.GetPattern("import").IsValid(src);
        }

        #endregion
    }
}