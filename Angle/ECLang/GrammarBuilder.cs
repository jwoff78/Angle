namespace ECLang
{
    using ECLang.Properties;

    public class GrammarBuilder
    {
        #region Public Properties

        public string Default
        {
            get
            {
                return Resources.Grammar;
            }
        }

        #endregion

        #region Public Methods and Operators

        public string BuildPattern(string start, string end)
        {
            return "";
        }

        #endregion
    }
}