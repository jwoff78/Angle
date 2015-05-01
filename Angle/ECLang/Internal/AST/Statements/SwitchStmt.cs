namespace ECLang.Internal.AST.Statements
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using ECLang.AST;
    using ECLang.Internal.Primitives;
    using ECLang.Internal.Primitives.Base;
    using ECLang.Internal.Tables;

    public class SwitchStmt : MultilineStatement
    {
        #region Fields

        public Dictionary<Primitive, List<IAst>> Cases = new Dictionary<Primitive, List<IAst>>();

        public List<IAst> Default = new List<IAst>();

        public string Header = "";

        #endregion

        #region Public Properties

        public Primitive Parent { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Clear()
        {
            this.Header = "";
            this.Parent = null;
            this.Cases = new Dictionary<Primitive, List<IAst>>();
            this.Default = null;
        }

        public override bool EndIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("switchend").IsValid(src);
        }

        public override MultilineStatement Interprete(string src)
        {
            var stmt = new SwitchStmt();
            var Cases = new Dictionary<string, List<IAst>>();
            string tmp = "";
            string tmpheader = "";
            foreach (string line in src.Split('\n'))
            {
                if (Parser.Grammar.GetPattern("switchstart").IsValid(line))
                {
                    stmt.Header = line;
                }
                else
                {
                    if (Parser.Grammar.GetPattern("case").IsValid(line)
                        || Parser.Grammar.GetPattern("default").IsValid(line))
                    {
                        if (tmpheader != "")
                        {
                            Cases.Add(tmpheader.TrimEnd('\n').TrimEnd(';'), Parser.ParseCodeBlock(tmp, "").Nodes);
                        }
                        tmpheader = line;
                        tmp = "";
                    }
                    else
                    {
                        tmp += line + ";\n";
                    }
                }
                if (Parser.Grammar.GetPattern("switchend").IsValid(line))
                {
                    if (tmpheader != "")
                    {
                        Cases.Add(tmpheader.TrimEnd('\n').TrimEnd(';'), Parser.ParseCodeBlock(tmp, "").Nodes);
                    }
                }
            }
            foreach (var @case in Cases)
            {
                if (Parser.Grammar.GetPattern("case").IsValid(@case.Key))
                {
                    Match mc = Parser.Grammar.GetPattern("case").Match(@case.Key);
                    stmt.Cases.Add(new EcObject(mc.Groups["Name"].Value), @case.Value);
                }
                if (Parser.Grammar.GetPattern("default").IsValid(@case.Key))
                {
                    stmt.Default = @case.Value;
                }
            }
            return stmt;
        }

        public override bool ParserHeader(string aHeader)
        {
            Match values = Parser.Grammar.GetPattern("switchstart").Match(this.Header);
            Group par = values.Groups["Name"];

            if (SymbolTable.Contains(par.Value))
            {
                this.Parent = SymbolTable.Get(par.Value);
            }
            else
            {
                this.Parent = new EcObject(par.Value);
            }

            return true;
        }

        public override bool StartIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("switchstart").IsValid(src);
        }

        #endregion
    }
}