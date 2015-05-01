namespace ECLang.Internal.Attributes.Base
{
    using System.Collections.Generic;

    using ECLang.Internal.AST.Statements;

    public abstract class Attribute
    {
        #region Fields

        public List<string> Parameters = new List<string>();

        #endregion

        #region Public Methods and Operators

        public abstract void Handle(AttributeStmt src, string Owner);

        #endregion
    }
}