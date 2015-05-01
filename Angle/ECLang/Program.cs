using System;
using System.Windows.Forms;

using ECLang.Internal;
using ECLang.Properties;

namespace ECLang
{
    public class Program
    {   public static Engine e = new Engine();

        public static void Main()
        {
            Console.Title = "Ec Runtime";
           
            e.AddItem("Stop", new Action(e.Stop));
            
      
            e.Evaluate(Resources.TestCode);
           
        }

    }
}