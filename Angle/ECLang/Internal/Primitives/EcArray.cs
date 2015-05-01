using System.Collections.Generic;

namespace ECLang.Internal.Primitives
{
    using System.Configuration;

    using ECLang.Internal.Primitives.Base;

    public class EcArray : Primitive
    {
        #region Constructors and Destructors

        public EcArray(string src)
        {
            string trimedbrackets = src.Trim().TrimStart('[').TrimEnd(']');
            string[] arrayitems = trimedbrackets.Split(',');

            List<object> FinalList = new List<object>();
            foreach (var arrayitem in arrayitems)
            {
                var val = PrimitivesManager.HandlePrimitive(arrayitem.Trim());
                if (val is EcString)
                {
                    FinalList.Add(new EcString((val as EcString).Value.ToString().Replace("NOMATH ", ""),false));
                }
                else
                {
                    FinalList.Add(val);
                }
            }
            this.data = FinalList.ToArray();

            this.Name = "array";
            //this.Value = src;
        }

        public EcArray()
        {
            this.Name = "array";

        }

        #endregion

        #region Public Methods and Operators

        public override Primitive Parse(object src)
        {
            return new EcArray((string)src);
        }

        public override bool Validate(object src)
        {
            return Parser.Grammar.GetPattern("array").IsValid(src as string);
        }


        public object Item(int index)
        {
            return data[index];
        }


        public void SetItem(int index,object val)
        {
            data[index] = val;
        }
        public object[] data;


        public override object call(string data, List<object> Params)
        {
            return GetType().GetMethod(data).Invoke(this, Params.ToArray());
        }

        #endregion
    }
}