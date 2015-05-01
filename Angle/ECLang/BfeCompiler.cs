using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECLang
{
    public class BfeCompiler
    {
        public static string Compile(string tocompile)
        {
            Parser.Init();
            Parser.Execute(tocompile);
            
            foreach(var o in Parser.CodeBlocks)
            {
                
            }


        }
    }
}