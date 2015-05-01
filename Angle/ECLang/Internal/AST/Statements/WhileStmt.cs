namespace ECLang.Internal.AST.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using ECLang.AST;
    using ECLang.Internal.Primitives;
    using ECLang.Internal.Primitives.Base;

    public class WhileStmt : MultilineStatement
    {
        //is object for muiltiline and Statment suprot 

        #region Fields

        public string Header = "";

        public Primitive Left = null;

        public Primitive Right = null;

        public string op = "";

        #endregion

        #region Public Properties

        public List<IAst> Nodes { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Clear()
        {
            this.Nodes = new List<IAst>();
            this.Header = "";
            this.Left = null;
            this.Right = null;
            this.op = "";
        }

        public override bool EndIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("whileend").IsValid(src);
        }

        public override MultilineStatement Interprete(string src)
        {
            var returns = new WhileStmt();
            string temp = "";
            for (int index = 0;
                index < src.Split(new[] {'\n', ';'}, StringSplitOptions.RemoveEmptyEntries).Length;
                index++)
            {
                string i = src.Split(new[] {'\n', ';'}, StringSplitOptions.RemoveEmptyEntries)[index];
                if (index == 0)
                {
                    returns.Header = i;
                }
                else if (index > 0
                         && index < src.Split(new[] {'\n', ';'}, StringSplitOptions.RemoveEmptyEntries).Length - 1)
                {
                    temp += i + ";\n";
                }
            }
            returns.Nodes = Parser.ParseCodeBlock(temp, "").Nodes;

            return returns;
        }

        public override Boolean ParserHeader(string aHeader)
        {
            Boolean returns = false;
            var rg = new Regex(Parser.Grammar.GetPattern("whilestart").ToString());
            Match values = rg.Match(this.Header);
            if (values.Groups["val"].Value.ToLower() == "true")
            {
                this.Left = new EcString("1", false);
                this.Right = new EcString("1", false);
                this.op = "==";
            }
            else if (values.Groups["val"].Value.ToLower() == "false")
            {
                this.Left = new EcString("1");
                this.Right = new EcString("1");
                this.op = "!=";
            }
            else
            {
                this.Left = new EcString(values.Groups["left"].Value);
                this.Right = new EcString(values.Groups["right"].Value);
                this.op = values.Groups["operator"].Value;
            }
            return returns;
        }

        public override bool StartIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("whilestart").IsValid(src);
        }

        #endregion
    }
}