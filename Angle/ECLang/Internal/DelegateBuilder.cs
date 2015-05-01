namespace ECLang.Internal
{
    using System;

    public class DelegateBuilder
    {
        #region Public Methods and Operators

        public static Delegate Build(Func<object[], object> func)
        {
            return func;
        }

        #endregion
    }
}