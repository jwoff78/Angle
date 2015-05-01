using System.Windows.Forms;

namespace ECLang.Internal.Primitives
{
    using System;
    using System.Collections.Generic;

    using ECLang.Internal.Primitives.Base;

    public class EcString : Primitive
    {
        #region Constructors and Destructors

        public EcString(string aValue = "", Boolean domath = true)
        {
            if (domath)
            {
                this.Value = "NOMATH " + aValue.TrimEnd('"').TrimStart('"');
            }
            else
            {
                this.Value = "" + aValue.TrimEnd('"').TrimStart('"');
            }
            this.Name = "string";
        }

        public EcString()
        {
        }

        #endregion

        #region Public Methods and Operators

        public override Primitive Parse(object src)
        {
            return new EcString(src as string);
        }

        public override string ToString()
        {
            return this.Value as string;
        }

        public override bool Validate(object src)
        {
            if (src is string)
            {
                return (src as string).StartsWith("\"") && (src as string).EndsWith("\"");
            }
            return false;
        }

        public override OperatorCollection CanOvveride
        {
            get { return new OperatorCollection(new Operator[] {new Increae(), new Decreae()}); }
        }

        public override Primitive Increase()
        {
            return new EcString((Value as string) + " ", false);
        }

        public override Primitive Decrease()
        {
            return new EcString((Value as string).Remove((Value as string).Length - 1, 1), false);
        }

        #endregion
    }
}