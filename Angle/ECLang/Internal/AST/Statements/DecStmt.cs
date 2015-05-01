namespace ECLang.AST.Statements
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using ECLang.Internal.AST.Statements;
    using ECLang.Internal.Primitives.Base;

    public class DecStmt : Statement
    {
        #region Fields

        public List<AttributeStmt> Attributes = new List<AttributeStmt>();

        #endregion

        #region Public Properties

        public string Name { get; set; }

        public string Type { get; set; }

        public Primitive Value { get; set; }

        #endregion

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }

        #region Public Methods and Operators

        public override Statement Interprete(string src, int line)
        {
            var returns = new DecStmt();
            returns.Line = line;
            var regexp = new Regex(Parser.Grammar.GetPattern("vardec").ToString());
            Match match = regexp.Match(src);

            if (match.Success)
            {
                returns.Name = match.Groups["Name"].Value;
                returns.Type = match.Groups["Type"].Value;

                returns.Value = StatmentVarHandler.HandleVar(match.Groups["Value"].Value, match.Groups["Type"].Value);
                return returns;
            }

            return null;
        }

        public override bool IsStatement(string src)
        {
            return Parser.Grammar.GetPattern("vardec").IsValid(src);
        }

        #endregion
    }
}