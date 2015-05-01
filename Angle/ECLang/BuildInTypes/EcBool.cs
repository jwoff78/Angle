using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ECLang.BuildInTypes
{
    using ECLang.BuildInTypes.Base;

    public class EcBool : EcBuiltInType
    {
        public string value = "false";

        public static EcBool False
        {
            get
            {
                return new EcBool("false");
            }
        }
        public static EcBool True
        {
            get
            {
                return new EcBool("true");
            }
        }

        public EcBool(string b)
        {
            value = b;
        }

        public static bool IsValid(string s)
        {
            var pattern = @"true|false";
            return Regex.IsMatch(s, pattern);
        }
        public static EcBool Parse(string s)
        {
            if (IsValid(s))
                return new EcBool(s);
            return new EcBool("false");
        }
        //TODO: add more Functions.
        public override object call(string data, List<object> perams)
        {
            throw new NotImplementedException();
        }
    }
}
