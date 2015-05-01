namespace ECLang.Internal
{
    using System.Collections.Generic;

    using ECLang.Internal.Primitives;
    using ECLang.Internal.Primitives.Base;

    public static class PrimitivesManager
    {
        #region Static Fields

        public static List<Primitive> Primitives = new List<Primitive>();

        #endregion

        #region Public Methods and Operators

        public static void Add<TPrimitive>() where TPrimitive : Primitive, new()
        {
            Primitives.Add(new TPrimitive());
        }

        public static Primitive HandlePrimitive(string aPrimitive, string name = "")
        {
            foreach (Primitive i in Primitives)
            {
                if (i.Validate(aPrimitive) || name == i.Name)
                {
                    return i.Parse(aPrimitive);
                }
            }
            return new EcString(aPrimitive);
        }

        public static void LoadPrimitives()
        {
            Add<EcObjectCallStmt>();
            Add<EcExpression>(); // this is to handle thins like 10 + 10 as a value to return 20
            Add<EcString>();
          
            
            Add<EcRegexp>();
            Add<EcLambda>();
            // Add<Fuck>();
            //TODO: handle byte first
            Add<EcArray>();
            Add<EcNumber>();
            Add<EcBool>();
            
            Add<EcObject>();
        }

        #endregion
    }
}