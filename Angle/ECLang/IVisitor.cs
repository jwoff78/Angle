using ECLang.AST.Statements;
using ECLang.Internal.AST.Statements;

namespace ECLang
{
    public class IVisitor
    {
        public virtual void Visit(AttributeStmt att) { }

        public virtual void Visit(CallStmt call) { }

        public virtual void Visit(DecSetStmt decset) { }

        public virtual void Visit(DecStmt dec) { }

        public virtual void Visit(CommentStmt dec) { }

        public virtual void Visit(ImportStmt dec) { }

        public virtual void Visit(DeIncreaseStmt dec) { }

        public virtual void Visit(DelStmt dec) { }

        public virtual void Visit(ModeStmt dec) { }

        public virtual void Visit(ObjCallStmt dec) { }

        public virtual void Visit(ThrowStmt dec) { }
    }
}