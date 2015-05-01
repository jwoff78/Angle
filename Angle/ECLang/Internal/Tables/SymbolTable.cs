namespace ECLang.Internal.Tables
{
    using System.Collections.Generic;

    using ECLang.Internal.AST.Statements;
    using ECLang.Internal.Primitives.Base;

    public class SymbolTable
    {
        #region Static Fields

        public static Dictionary<string, List<AttributeStmt>> DeclarationAttributes =
            new Dictionary<string, List<AttributeStmt>>();

        private static Dictionary<string, Primitive> internals = new Dictionary<string, Primitive>();

        #endregion

        #region Public Methods and Operators

        public static void Clear()
        {
            internals = new Dictionary<string, Primitive>();
            DeclarationAttributes = new Dictionary<string, List<AttributeStmt>>();
        }

        public static bool Contains(string name)
        {
            return internals.ContainsKey(name.TrimEnd('"').TrimStart('"'));
        }

        public static Primitive Get(string name)
        {
            if (internals.ContainsKey(name))
            {
                return internals[name];
            }
            return null;
        }

        public static List<AttributeStmt> GetAttributes(string name)
        {
            if (DeclarationAttributes.ContainsKey(name))
            {
                return DeclarationAttributes[name];
            }
            return new List<AttributeStmt>();
        }

        public static void Remove(string name)
        {
            internals.Remove(name);
            DeclarationAttributes.Remove(name);
        }

        public static void Set(string name, Primitive newValue)
        {
            internals[name] = newValue;
        }

        public static void Store(string name, Primitive o, List<AttributeStmt> Attributes = null)
        {
            try
            {
                internals.Add(name, o);
                if (Attributes != null)
                {
                    DeclarationAttributes.Add(name, Attributes);
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}