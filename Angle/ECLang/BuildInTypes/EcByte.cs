using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ECLang.BuildInTypes
{
    using ECLang.BuildInTypes.Base;

    public class EcByte : EcBuiltInType
    {
        public static EcByte Null
        {
            get
            {
                return new EcByte() { value = "0" };
            }
        }

        public static explicit operator int(EcByte i)
        {
            return int.Parse(i.value);
        }

        public static explicit operator EcByte(int i)
        {
            return Parse(i.ToString());
        }

        private string value;

        public static bool IsValid(string s)
        {
            var pattern = @"[0-255]";
            return Regex.IsMatch(s, pattern);
        }

        public static EcByte Parse(string s)
        {
            if (IsValid(s)) return new EcByte() { value = s };
            return Null;
        }

        public static bool TryParse(string s, out EcByte num)
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