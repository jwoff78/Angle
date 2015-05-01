using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Angle.Core
{
    public class Lexser
    {
        // return a list with all the statmenst in and ther etokens
        public static List<List<Token>> GetCodeTokens(string English)
        {
            var ret = new List<List<Token>>();
            /*
             * We need to split the sentinces like follow
             * Content(, | .) = Statment
             * then get all the tokens in a statment
             * add them to a list then ad the list to the statment list
             */
            string[] Statments = English.Split('.');



            return ret;
        }


    }

    public class Token
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<string> Names { get; set; }
        public bool UsesRegex = true;

        public Token()
        {
            Names = new List<string>();
        }
    }


}
