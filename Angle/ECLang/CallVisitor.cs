using System;
using System.Linq;
using ECLang.AST.Statements;

namespace ECLang
{
    public class CallVisitor : IVisitor
    {
        public string Result;

        public override void Visit(CallStmt call)
        {
            Result += call.Name + "\r";
        }
    }
}