namespace ECLang.AST
{
    // 1+2*3
    // new array();
    // (\d{4})?\d{2}
    public abstract class Expression : IAst
    {
        #region Public Methods and Operators

        public abstract Expression Interprete(string src);

        #endregion
    }
}