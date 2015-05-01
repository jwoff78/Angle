using System.Collections.Generic;

namespace ECLang.BuildInTypes
{
    using System.Text.RegularExpressions;

    using ECLang.BuildInTypes.Base;

    public class Number : EcBuiltInType
    {
        public static Number Null
        {
            get
            {
                return new Number() {value = "0"};
            }
        }

        public static explicit operator int(Number i)
        {
            return int.Parse(i.value);
        }
        public static explicit operator Number(int i)
        {
            return Parse(i.ToString());
        }

        private string value;

        public static bool IsValid(string s)
        {
            var pattern = @"((([+-]?)[0-9]+)(\.[0-9]+)?)";
            return Regex.IsMatch(s, pattern);
        }
        public static Number Parse(string s)
        {
            if(IsValid(s))
                return new Number() {value = s};
            return Null;
        }
        public static bool TryParse(string s, out Number num)
        {
            var valid = IsValid(s);

            num = Parse(s);
            return valid;
        }

        public override string ToString()
        {
            return this.value;
        }

        public override object call(string data, List<object> perams)
        {
            throw new System.NotImplementedException();
        }

        //ToDo: add more functions

    }
}
