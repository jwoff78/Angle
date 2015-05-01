using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angle.Core
{
    public class Engine
    {

        public void Run(string English)
        {
            TokenResolver.LoadTokens();
            RefineResolver.LoadRefiners();
            var tokens = Lexser.GetCodeTokens(English);
            var EcCode = Parser.BuildEcCodeFromTokens(tokens);
            ECLang.Engine en = new ECLang.Engine();
            en.Execute(EcCode);
        }

    }
}
