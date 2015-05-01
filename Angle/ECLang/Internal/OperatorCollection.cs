using System.Collections.Generic;

namespace ECLang.Internal
{
    public class OperatorCollection : List<string>
    {
        public OperatorCollection()
        {
            
        }

        public OperatorCollection(Operator[] os)
        {
            foreach (var @operator in os)
            {
                Add(@operator.ToString());
            }
        }

        public bool Contains(Operator o)
        {
            return Contains(o.ToString());
        }
    }
}
