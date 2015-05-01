namespace ECLang.Internal.AST
{
    using System;

    using ECLang.AST;

    public abstract class MultilineStatement : IAst
    {
        #region Public Properties

        public int Line { get; set; }

        #endregion

        #region Public Methods and Operators

        public abstract void Clear();

        public abstract bool EndIsMatch(string src);

        public abstract MultilineStatement Interprete(string src);

        public abstract Boolean ParserHeader(string aHeader);

        public abstract bool StartIsMatch(string src);

        #endregion
    }
}