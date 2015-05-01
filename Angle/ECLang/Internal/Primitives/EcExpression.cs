namespace ECLang.Internal.Primitives
{
    using System.Linq;

    using ECLang.Internal.Primitives.Base;

    public class EcExpression : Primitive
    {
        #region Public Methods and Operators

        public override Primitive Parse(object src)
        {
            return new EcString(src as string, false);
        }

        public override bool Validate(object src)
        {
            return (src as string).Contains(' ')
                   && ((src as string).Contains('+') || (src as string).Contains('-') || (src as string).Contains('*')
                       || (src as string).Contains('/'));
        }

        #endregion
    }
}