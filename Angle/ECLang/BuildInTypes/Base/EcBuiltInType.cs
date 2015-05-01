using System.Collections.Generic;

namespace ECLang.BuildInTypes.Base
{
    public abstract class EcBuiltInType
    {
        public abstract object call(string data, List<object> perams);
    }
}
