namespace ECLang.AST.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using ECLang.Internal.AST.Statements;
    using ECLang.Internal.Primitives.Base;

    public class CallStmt : Statement
    {
        #region Public Properties

        public string Name { get; set; } //call name

        public List<Primitive> Paramaters { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }

        public override Statement Interprete(string src, int line)
        {
            var returns = new CallStmt();
            returns.Line = line;
            var reg = new Regex(Parser.Grammar.GetPattern("call").ToString());
            Match match = reg.Match(src);
            if (match.Success)
            {
                returns.Paramaters = new List<Primitive>();
                returns.Name = match.Groups[1].Value;
                if (match.Groups["params"].Value.Contains(','))
                {
                    foreach (string i in match.Groups["params"].Value.TrimEnd(')').Split(','))
                    {
                        returns.Paramaters.Add(StatmentVarHandler.HandleVar(i));
                    }
                }
            }

            return returns;
        }

        public override bool IsStatement(string src)
        {
            if (src.Split('(')[0].Contains('.'))
            {
                return false;
            }
            bool b = Parser.Grammar.GetPattern("call").IsValid(src);
            return b;
        }

        #endregion
    }
}