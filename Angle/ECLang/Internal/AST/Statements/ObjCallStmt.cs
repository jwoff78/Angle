namespace ECLang.AST.Statements
{
    using System.Collections.Generic;
    using System.Linq;

    using ECLang.Internal.AST.Statements;
    using ECLang.Internal.Primitives.Base;

    public class ObjCallStmt : Statement
    {
        #region Constructors and Destructors

        public ObjCallStmt()
        {
            this.Paramaters = new List<Primitive>();
            // Paramaters.Add("emile");
        }

        #endregion

        #region Public Properties

        public string Name { get; set; } //call name

        public List<Primitive> Paramaters { get; set; }

        public string Target { get; set; }

        #endregion

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }

        #region Public Methods and Operators

        public override Statement Interprete(string src, int line)
        {
            var returns = new ObjCallStmt();
            returns.Line = line;
            string tmp1 = "";
            for (int index = 0; index < src.Split('(').Length; index++)
            {
                string i = src.Split('(')[index];
                if (index == 0)
                {
                }
                else
                {
                    tmp1 += i + "(";
                }
            }
            string value = tmp1.Remove(tmp1.Length - 2, 2);
            string tmp = "";
            for (int index = 0; index < src.Split('(')[0].Split('.').Length; index++)
            {
                string i = src.Split('(')[0].Split('.')[index];
                int l = src.Split('(')[0].Split('.').Length;
                if (index == l - 1)
                {
                    returns.Name = i;
                }
                else
                {
                    tmp += i + ".";
                }
            }
            tmp = tmp.TrimEnd('.');

            returns.Target = tmp;

            if (value.Contains(','))
            {
                foreach (string i in value.Split(','))
                {
                    returns.Paramaters.Add(StatmentVarHandler.HandleVar(i));
                }
            }
            else
            {
                if (value != "")
                {
                    returns.Paramaters.Add(StatmentVarHandler.HandleVar(value));
                }
            }
            return returns;
        }

        public override bool IsStatement(string src)
        {
            int lng = src.Split('(')[0].Split('.').Length;
            if (lng != 1 && !src.ToLower().StartsWith("import"))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}