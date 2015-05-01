using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECLang.Internal.Primitives.Base;

namespace ECLang.Internal.Primitives
{
    public class EcObjectCallStmt : Primitive
    {
        

        public EcObjectCallStmt()
        {
            this.Name = "EcObjectCallStmt";
        }

        public EcObjectCallStmt(string src)
        {
            this.Name = "EcObjectCallStmt";
        }

        public override Primitive Parse(object src)
        {
            return new EcObject("NOMATH " + src as string);
        }

        public override bool Validate(object src)
        {
            var l = (src as string).Split('(')[0].Split('.').Length;
            return l >= 2;
        }
    }
}
