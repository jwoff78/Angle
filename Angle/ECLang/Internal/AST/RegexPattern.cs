namespace ECLang.AST
{
    using System.Text.RegularExpressions;

    public class RegexPattern
    {
        #region Fields

        private readonly string pattern;

        #endregion

        #region Constructors and Destructors

        internal RegexPattern(string pattern)
        {
            this.pattern = pattern;
        }

        #endregion

        #region Public Methods and Operators

        public Regex GetRegex()
        {
            return new Regex(this.pattern);
        }

        public bool IsValid(string input)
        {
            return Regex.IsMatch(input, this.pattern);
        }

        public Match Match(string input)
        {
            return Regex.Match(input, this.pattern);
        }

        public override string ToString()
        {
            return this.pattern;
        }

        #endregion
    }
}