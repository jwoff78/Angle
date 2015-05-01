namespace ECLang.Internal.Primitives
{
    using ECLang.Internal.Primitives.Base;

    public class EcBool : Primitive
    {
        #region Constructors and Destructors

        public EcBool()
        {
        }

        public EcBool(string src)
        {
            this.Value = src;
        }

        #endregion

        #region Public Methods and Operators

        public override Primitive Parse(object src)
        {
            return new EcBool(src as string);
        }

        public override bool Validate(object src)
        {
            return Parser.Grammar.GetPattern("bool").IsValid((string)src);
        }

        #endregion
    }
}