using Angle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angle
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine en = new Engine();
            en.Run("Computer create an program that, prints \"Hello World\" to the console.");
            Console.ReadKey();
        }
    }
}
