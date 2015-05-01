using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angle.Core
{
    public class Parser
    {
        public static string BuildEcCodeFromTokens(List<List<Token>> inp)
        {
            string ret = "";

            foreach(var i in inp)
            {
                ret += BuildLineFromTokenList(i) + "\n";
            }

            return ret;
        }

        private static string BuildLineFromTokenList(List<Token> inp)
        {
            string ret = "";





            return ret;
        }

    }
}
