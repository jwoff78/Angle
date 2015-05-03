using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Angle.Core
{
    public class Engine
    {

        public void CompileAndRun(string English, string name)
        {

            RefineResolver.LoadRefiners();
            TokenResolver.LoadTokens();

            var tokens = Lexser.GetCodeTokens(English);
            var CSharpCode = Parser.BuildEcCodeFromTokens(tokens);


            CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<String, String> { { "CompilerVersion", "v4.0" } });
  

            CompilerParameters compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = true            
            };
            compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
            compilerParams.OutputAssembly = name + ".exe";
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams,  CSharpCode.Replace("{dot}", "."));

            if (results.Errors.Count != 0)
                throw new Exception("Mission failed!");



           

        }

    }
}
