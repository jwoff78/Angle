namespace ECLang.AST
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Grammar
    {
        // name := rule

        #region Constants

        public const string Rule = @"(\s+)?(?<name>[a-zA-Z]+|<[a-zA-Z]+\>)((\s)?)+:=(\s+)?(?<pattern>.+)";

        #endregion

        #region Fields

        private readonly Dictionary<string, RegexPattern> groups = new Dictionary<string, RegexPattern>();

        #endregion

        #region Constructors and Destructors

        public Grammar(string s)
        {
            for (int i = 0; i < 3; i++)
            {


                Dictionary<string, RegexPattern> g =
                    s.Replace("\r\n", "\n")
                        .Split('\n')
                        .Select(line => Regex.Match(line, Rule))
                        .Where(m => m.Success)
                        .ToDictionary(m => m.Groups["name"].Value, m => new RegexPattern(m.Groups["pattern"].Value));
                s = g.Aggregate(
                    s,
                    (current, regexGroup) => current.Replace('<' + regexGroup.Key + '>', regexGroup.Value.ToString()));

            }
            foreach (
                    Match m in
                        s.Replace("\r\n", "\n")
                            .Split('\n')
                            .Select(line => Regex.Match(line, Rule))
                            .Where(m => m.Success))
                {
                    this.groups.Add(m.Groups["name"].Value, new RegexPattern(m.Groups["pattern"].Value));
                }
        }

        #endregion

        #region Public Methods and Operators

        public RegexPattern GetPattern(string name)
        {
            return this.groups[name];
        }

        #endregion
    }
}