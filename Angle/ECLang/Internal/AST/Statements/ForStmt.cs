namespace ECLang.Internal.AST.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using ECLang.AST;
    using ECLang.Internal.Primitives;
    using ECLang.Internal.Primitives.Base;

    public class ForStmt : MultilineStatement
    {
        //is object for muiltiline and Statment suprot 

        #region Fields

        public string Header = "";

        public Primitive Left = null;

        public Primitive Right = null;

        public string op = "";

        #endregion

        #region Public Properties

        public List<IAst> Statements { get; set; }

        public string Value { get; set; }

        public string forop { get; set; }

        public string setValue { get; set; }

        public Step Step { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Clear()
        {
            this.Statements = new List<IAst>();
            this.Header = "";
            this.Left = null;
            this.Right = null;
            this.op = "";
            this.Value = null;
            this.setValue = null;
            this.forop = null;
            this.Step = null;
        }

        public override bool EndIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("forend").IsValid(src);
        }

        public override MultilineStatement Interprete(string src)
        {
            var returns = new ForStmt();
            string temp = "";
            for (int index = 0;
                index < src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries).Length;
                index++)
            {
                string i = src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)[index];
                if (index == 0)
                {
                    returns.Header = i;
                }
                else if (index > 0
                         && index < src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries).Length - 1)
                {
                    temp += i + ";\n";
                    ;
                }
            }
            returns.Statements = Parser.ParseCodeBlock(temp, "").Nodes;
            return returns;
        }

        public override Boolean ParserHeader(string aHeader)
        {
            Boolean returns = false;
            var rg = new Regex(Parser.Grammar.GetPattern("forstart").ToString());
            Match values = rg.Match(this.Header);
            this.Left = new EcString(values.Groups["left"].Value);
            this.Right = new EcString(values.Groups["right"].Value);
            this.op = values.Groups["operator"].Value;
            this.Value = values.Groups["Name"].Value;
            this.setValue = values.Groups["Type"].Value;
            this.forop = values.Groups["Forop"].Value;
            try
            {
                this.Step = new Step { Every = int.Parse(values.Groups["step"].Value) };
            }
            catch
            {
            }
            return true;
        }

        public override bool StartIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("forstart").IsValid(src);
        }

        #endregion
    }

    public class Step
    {
        public int Every { get; set; }
    }
}