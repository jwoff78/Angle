namespace ECLang.AST
{
    // variable declaration
    // var set
    // call function
    // def func
    public abstract class Statement : IAst
    {
        #region Public Properties

        public int Line { get; set; }

        #endregion

        #region Public Methods and Operators

        public abstract Statement Interprete(string src, int line);

        public abstract void Accept(IVisitor v);

        public abstract bool IsStatement(string src);

        #endregion

        // public abstract object HandleAST(Statement src);
    }
}