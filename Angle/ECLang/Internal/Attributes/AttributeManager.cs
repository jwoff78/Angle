namespace ECLang.Internal.Attributes
{
    using System.Collections.Generic;

    using ECLang.Internal.AST.Statements;

    public static class AttributeManager
    {
        #region Public Methods and Operators

        public static bool ContainsAttribute(string at, List<AttributeStmt> list)
        {
            bool dodel = false;
            foreach (AttributeStmt o in list)
            {
                if (o.Attribute.ToLower() == at)
                {
                    dodel = true;
                    break;
                }
            }
            return dodel;
        }

        #endregion
    }
}