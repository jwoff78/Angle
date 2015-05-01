namespace ECLang.Internal.Tables
{
    using System.Collections.Generic;

    public class FunctionTable
    {
        #region Static Fields

        private static Dictionary<string, Method> Internals = new Dictionary<string, Method>();

        #endregion

        #region Public Methods and Operators

        public static void Clear()
        {
            Internals = new Dictionary<string, Method>();
        }

        public static bool Contains(string key)
        {
            return Internals.ContainsKey(key);
        }

        public static Method Get(string name)
        {
            if (Internals.ContainsKey(name))
            {
                return Internals[name];
            }
            return null;
        }

        public static void Store(string name, Method o)
        {
            Internals.Add(name, o);
        }

        #endregion
    }
}