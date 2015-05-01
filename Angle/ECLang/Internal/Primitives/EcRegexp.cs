namespace ECLang.Internal.Primitives
{
    using System.Text.RegularExpressions;

    using ECLang.Internal.Primitives.Base;

    public class EcRegexp : Primitive
    {
        #region Constructors and Destructors

        public EcRegexp()
        {
            this.Name = "regexp";
        }

        public EcRegexp(string pattern)
        {
            this.Value = pattern;
            this.Name = "regexp";
        }

        #endregion

        #region Public Methods and Operators

        public bool IsMatch(string input)
        {
            return Regex.IsMatch(input, this.Value as string);
        }

        public Match Match(string input)
        {
            return Regex.Match(input, this.Value as string);
        }

        public override Primitive Parse(object src)
        {
            return new EcRegexp(src as string);
        }

        public override bool Validate(object src)
        {
            return Parser.Grammar.GetPattern("regexp").IsValid(src as string);
        }

        #endregion
    }
}