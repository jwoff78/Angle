namespace ECLang.Internal.Primitives
{
    using ECLang.Internal.Primitives.Base;

    public class EcLambda : Primitive
    {
        #region Constructors and Destructors

        public EcLambda()
        {
            this.Name = "lambda";
        }

        public EcLambda(string pattern)
        {
            this.Value = pattern;
            this.Name = "lambda";
        }

        #endregion

        #region Public Methods and Operators
        
        public override Primitive Parse(object src)
        {
            return new EcLambda(src as string);
        }

        public override bool Validate(object src)
        {
            return Parser.Grammar.GetPattern("lambda").IsValid(src as string);
        }

        #endregion
    }
}