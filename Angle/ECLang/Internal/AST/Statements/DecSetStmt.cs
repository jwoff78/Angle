using System;
using System.Data.Common;

namespace ECLang.AST.Statements
{
    using System.Text.RegularExpressions;

    using ECLang.Internal.AST.Statements;
    using ECLang.Internal.Primitives.Base;

    public class DecSetStmt : Statement
    {
        #region Public Properties

        public string Name { get; set; }

        public Primitive Value { get; set; }
        public int Index;
        public string Operator { get; set; }
        #endregion

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }

        #region Public Methods and Operators

        public override Statement Interprete(string src, int line)
        {
            var returns = new DecSetStmt();
            returns.Line = line;
            var reg = new Regex(Parser.Grammar.GetPattern("varset").ToString());
            Match match = reg.Match(src);
            if (match.Success)
            {
                returns.Name = match.Groups["Name"].Value;

                returns.Operator = match.Groups["op"].Value;

                returns.Value = StatmentVarHandler.HandleVar(match.Groups["Value"].Value);
            }
            return returns;
        }

        public override bool IsStatement(string src)
        {
            bool r = Parser.Grammar.GetPattern("varset").IsValid(src);
            return r;
        }

        #endregion
    }
}