using System.Collections.Generic;

namespace ECLang.Internal.Primitives
{
    using System;

    using ECLang.Internal.Primitives.Base;

    public class EcNumber : Primitive
    {
        #region Constructors and Destructors

        public EcNumber()
        {
        }

        public EcNumber(decimal src)
        {
            this.Value = src;
        }

        #endregion

        #region Public Methods and Operators

        public override Primitive Parse(object src)
        {
            return new EcNumber(Convert.ToDecimal((src as string).TrimEnd('\n').TrimStart('\n')));
        }

        public override bool Validate(object src)
        {
            return Parser.Grammar.GetPattern("number").IsValid(src as string) && !(src as string).Contains(" ");
        }

        #endregion

        #region "operator overloading"

        public override OperatorCollection CanOvveride
        {
            get { return new OperatorCollection(new Operator[] {new Decreae(), 
                new Increae(), new PlusEquals(), new MinusEquals(), new Equals(), new NotEquals(),new LessThan(), new GreaterThan(),  }); }
        }

        public override Primitive Increase()
        {
            var d = (decimal)Value;
            d++;
            return new EcNumber(d);
        }

        public override Primitive Decrease()
        {
            var d = (decimal) Value;
            d--;
            return new EcNumber(d);
        }

        public override Primitive PlusEquals(Primitive first, Primitive second)
        {
            return new EcNumber(((decimal)first.Value) + ((decimal)second.Value));
        }

        public override Primitive MinusEquals(Primitive first, Primitive second)
        {
            return new EcNumber(((decimal)first.Value) - ((decimal)second.Value));
        }

        public override object call(string data, List<object> Params)
        {
            return GetType().GetMethod(data).Invoke(this, Params.ToArray());
        }

        public override bool Equals(Primitive first, Primitive second)
        {
            var f = (decimal)first.Value;
            var s = (decimal)second.Value;

            return f == s;
        }

        public override bool NotEquals(Primitive first, Primitive second)
        {
            var f = (decimal)first.Value;
            var s = (decimal)second.Value;

            return f != s;
        }

        public override bool LessThan(Primitive first, Primitive second)
        {
            var f = (decimal)first.Value;
            var s = (decimal)second.Value;

            return f < s;
        }

        public override bool GreaterThan(Primitive first, Primitive second)
        {
            var f = (decimal)first.Value;
            var s = (decimal)second.Value;

            return f > s;
        }

        #endregion
    }
}