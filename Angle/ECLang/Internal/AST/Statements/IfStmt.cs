namespace ECLang.Internal.AST.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using ECLang.AST;
    using ECLang.Internal.Primitives;
    using ECLang.Internal.Primitives.Base;

    public class IfStmt : MultilineStatement
    {
        #region Fields

        public string Header = "";

        public Primitive Left = null;

        public Primitive Right = null;

        public string op = "";

        #endregion

        #region Public Properties

        public List<IAst> ElseStatements { get; set; }

        public List<IAst> Statements { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Clear()
        {
            this.Statements = new List<IAst>();
            this.Header = "";
            this.Left = null;
            this.Right = null;
            this.op = "";
        }

        public override bool EndIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("ifend").IsValid(src);
        }

        public override MultilineStatement Interprete(string src)
        {
            string main = "";
            string elsemain = "";
            bool tmp = false;
            for (int index = 0;
                index < src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries).Length;
                index++)
            {
                string i = src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)[index];
                if (Parser.Grammar.GetPattern("else").IsValid(i.TrimEnd(' ').TrimStart(' ')))
                {
                    tmp = true;
                    main += i + ";\n";
                }
                else if (tmp == false)
                {
                    main += i + ";\n";
                }
                else if (tmp)
                {
                    elsemain += i + ";\n";
                }
            }
            src = main;

            var returns = new IfStmt();
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
            returns.ElseStatements = Parser.ParseCodeBlock(elsemain, "").Nodes;
            return returns;
        }

        public override Boolean ParserHeader(string aHeader)
        {
            Boolean returns = false;
            var rg = new Regex(Parser.Grammar.GetPattern("ifstart").ToString());
            Match values = rg.Match(this.Header);
            if (values.Groups["val"].Value.ToLower() == "true")
            {
                this.Left = new EcString("1");
                this.Right = new EcString("1");
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
                this.Left = new EcString(values.Groups["left"].Value, false);
                this.Right = new EcString(values.Groups["right"].Value, false);
                this.op = values.Groups["operator"].Value;
            }
            return returns;
        }

        public override bool StartIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("ifstart").IsValid(src);
        }

        #endregion
    }
}