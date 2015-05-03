using Angle.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Angle
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Engine en = new Engine();


            var f = new FileInfo(args[0]);
            en.CompileAndRun(File.ReadAllText(args[0]),f.Name.Replace(f.Extension,""));

 
        }
    }
}
