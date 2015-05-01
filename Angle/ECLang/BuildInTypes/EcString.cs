using System;
using System.Collections.Generic;

namespace ECLang.BuildInTypes
{
    using System.Linq;

    using ECLang.BuildInTypes.Base;

    public class EcString : EcBuiltInType
    {
        private readonly string value;


        public EcString(string s)
        {
            value = s;
        }
        public override string ToString()
        {
            return value;
        }

        public EcString Substr(Number n, Number length)
        {
            return new EcString(value.Substring((int) n, (int) length));
        }

        public EcString Replace(EcString old, EcString New)
        {
            return new EcString(value.Replace(old.value, New.value));
            ;
        }

        public EcString[] Match(EcString pattern)
        {
            var r = new RegExp(pattern.value);
            return r.Match(this.value).Select(match => new EcString(match)).ToArray();
        }

        //ToDo: add more functions
        public override object call(string data, List<object> perams)
        {
            if (data == "Substr")
            {
                return Substr(Number.Parse((string) perams[0]), Number.Parse((string) perams[0]));
            }
            return new Null();
        }
    }
}
