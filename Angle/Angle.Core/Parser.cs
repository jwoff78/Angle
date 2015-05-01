using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angle.Core
{
    public class Parser
    {
        public static List<string> Import = new List<string>();
        public static List<string> EndCode = new List<string>();

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

            string EndCodes = " ";
            foreach(var i in EndCode)
            {
                EndCodes += "" +  i + "\n";
            }

            return imports + ret + EndCodes;
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
            refiner.ParamsArray = perams.Split(',');
            refiner.ParamsArrayTrimed = perams.Replace("\"", "").Split(',');
            refiner.Params = perams;
            refiner.Invoke();

            ret += "\n" + File.ReadAllText(Global.DataSetLocation + "Bootstrap/" + refiner.Name + ".ec") + "\n" + refiner.InvokeStatmentBuilded + "\n";
            
            foreach(var i in refiner.Imports)
            {
                if(!Import.Contains(i))
                {
                    Import.Add(i);
                }
            }
            EndCode.Add(refiner.InvokeStatmentBuildedEnd);

            return ret;
        }

    }
}
