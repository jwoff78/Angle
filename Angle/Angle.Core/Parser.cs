using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angle.Core
{
    public class Parser
    {
        public static List<string> Import = new List<string>();

        public static string BuildEcCodeFromTokens(List<List<Token>> inp)
        {
            string ret = "";
            
            foreach(var i in inp)
            {
                ret += BuildLineFromTokenList(i) + "\n";
            }
            string imports = "";
            foreach(var i in Import)
            {
                imports += "import " +  i + "\n";
            }

            return imports + ret;
        }

        private static string BuildLineFromTokenList(List<Token> inp)
        {
            string ret = "";

            var refiner = RefineResolver.ResolveRefiner(inp);
            string perams = "";
            foreach(var i in inp)
            {
                if(i.Name == "Value")
                {
                    perams += i.Value.Trim() + ",";
                }
            }
            perams = perams.TrimEnd(',');
            ret += refiner.InvokeStatment.Replace("{Params}", perams);
            
            foreach(var i in refiner.Imports)
            {
                if(!Import.Contains(i))
                {
                    Import.Add(i);
                }
            }

            return ret;
        }

    }
}
