namespace ECLang.BuildInTypes
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using ECLang.BuildInTypes.Base;

    public class RegExp : EcBuiltInType
    {
        public string Pattern { get; set; }

        private readonly Regex r;
        public RegExp(string p)
        {
            this.Pattern = p;
            this.r = new Regex(p, RegexOptions.Compiled);
        }

        public List<string> Match(string input)
        {
            //return r.Match(input).Groups.Cast<object>().Cast<IEnumerable<string>>();
            return null;
        }

        public bool IsValid(string input)
        {
            return this.r.IsMatch(input);
        }

        public string Replace(string input, string newInpt)
        {
           return this.r.Replace(input, newInpt);
        }

        public override object call(string data, List<object> perams)
        {
            throw new System.NotImplementedException();
        }
    }
}
