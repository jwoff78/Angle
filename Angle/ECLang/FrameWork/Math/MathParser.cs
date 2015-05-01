namespace ECLang.Framework
{
    using System;

    /// <summary>
    ///     This is a mathematical expression parser that allows you to parser a string value,
    ///     perform the required calculations, and return a value in form of a decimal.
    /// </summary>
    public static class MathParser
    {
        #region Public Methods and Operators

        public static int Parse(string inp)
        {
            Console.WriteLine(new Internal.MathParser().Parse(inp));
            return (int)new Internal.MathParser().Parse(inp);
        }

        #endregion
    }
}