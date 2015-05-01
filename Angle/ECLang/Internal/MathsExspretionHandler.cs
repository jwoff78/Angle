namespace ECLang.Internal
{
    using System;
    using System.Text.RegularExpressions;

    using ECLang.Internal.Tables;

    public static class MathsExspretionHandler
    {
        #region Public Methods and Operators

        public static string ParseValue(string exspretion)
        {
            return Regex.Replace(
                exspretion,
                Parser.Grammar.GetPattern("name").ToString(),
                delegate(Match m)
                {
                    if (SymbolTable.Get(m.Value) != null)
                    {
                        return Convert.ToString(SymbolTable.Get(m.Value));
                    }
                    return m.Value;
                });
        }

        #endregion
    }
}