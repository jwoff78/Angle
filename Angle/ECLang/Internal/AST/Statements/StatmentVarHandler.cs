namespace ECLang.Internal.AST.Statements
{
    using ECLang.Internal.Primitives.Base;

    public static class StatmentVarHandler
    {
        #region Public Methods and Operators

        public static Primitive HandleVar(string value, string name = "")
        {
            if (name != "")
            {
                return PrimitivesManager.HandlePrimitive(value);
            }
            return PrimitivesManager.HandlePrimitive(value, name);
        }

        #endregion
    }
}