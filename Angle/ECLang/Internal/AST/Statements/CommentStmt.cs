namespace ECLang.AST.Statements
{
    public class CommentStmt : Statement
    {
        #region Public Methods and Operators

        public override Statement Interprete(string src, int line)
        {
            var ret = new CommentStmt();
            ret.Line = line;

            return ret;
        }

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }

        public override bool IsStatement(string src)
        {
            if (src.StartsWith('#'.ToString()))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}