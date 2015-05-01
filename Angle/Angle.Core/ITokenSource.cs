using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Angle.Core
{
    public interface ITokenSource
    {
        Token[] GetTokens();
    }
}
