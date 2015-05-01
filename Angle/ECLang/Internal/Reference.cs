namespace ECLang.Internal
{
    using System.Collections.Generic;

    using ECLang.AST;

    public class Reference
    {
        #region Fields

        private readonly Method m;

        #endregion

        #region Constructors and Destructors

        public Reference(Method m)
        {
            this.m = m;
        }

        #endregion

        #region Public Properties

        public List<Statement> Body { get; set; }

        public string Name
        {
            get
            {
                return this.m.Name;
            }
        }

        #endregion
    }
}