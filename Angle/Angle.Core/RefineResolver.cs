using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Noesis.Javascript;


namespace Angle.Core
{
    public class RefineResolver
    {
        public static List<Refiner> Refinerys = new List<Refiner>();

        public static void LoadRefiners()
        {
            foreach (var i in Directory.GetFiles(Global.DataSetLocation + "Refinerys"))
            {
                if(i.EndsWith(".js"))
                {
                    FileInfo f = new FileInfo(i);
                    var r = new Refiner() { Name = f.Name.Replace(".js", ""),Code = File.ReadAllText(i) };
                    r.Invoke();
                    Refinerys.Add(r);
                }
            }
        }

        public static Refiner ResolveRefiner(List<Token> Tokens)
        {
            Refiner ret = new Refiner();

            foreach (var re in Refinerys)
            {
                bool ac = false;
                bool refi = false;
                foreach (var i in Tokens)
                {
                    if(i.Name == "Action")
                    {
                        if(re.Actions.Contains(i.Value))
                        {
                            ac = true;
                        }
                    }
                    if (i.Name == "Refine")
                    {
                        if (re.Refine.Contains(i.Value))
                        {
                            refi = true;
                        }
                    }
                }
                if(ac && refi)
                {
                    ret = re;
                    break;
                }
            }
            return ret;
        }

    }

    public class Refiner
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string UsedAction { get; set; }
        public string InvokeStatment { get; set; }
        public string InvokeStatmentBuilded { get; set; }
        public string InvokeStatmentBuildedEnd { get; set; }
        public List<string> Actions { get; set; }
        public List<string> Imports { get; set; }
        public List<string> Refine { get; set; }
        public string[] ParamsArray { get; set; }
        public string[] ParamsArrayTrimed { get; set; }
        public string Params { get; set; }
        public Refiner()
        {
            Name = "";
            Code = "";
            InvokeStatment = "";
            InvokeStatmentBuilded = "";
            Params = "";
            ParamsArray = new string[] { "","","","","","","","","","","",""};
            ParamsArrayTrimed = new string[] { "", "", "", "", "", "", "", "", "", "", "", "" };
            Imports = new List<string>();
            Actions = new List<string>();
            Refine = new List<string>();
        }

        public void AddActions(string s)
        {
            Actions.Add(s);
        }

        public void AddRefine(string s)
        {
            Refine.Add(s);
        }

        public void AddImport(string s)
        {
            Imports.Add(s);
        }

        public void Invoke()
        {
     
            using (JavascriptContext context = new JavascriptContext())
            {

             
                context.SetParameter("InvokeStatement", InvokeStatment);
                context.SetParameter("InvokeStatmentBuilded", InvokeStatmentBuilded);
                context.SetParameter("InvokeStatmentBuildedEnd", InvokeStatmentBuildedEnd);
                context.SetParameter("ParamsArray", ParamsArray);
                context.SetParameter("ParamsArrayTrimed", ParamsArrayTrimed);
                context.SetParameter("Params", Params);
                context.SetParameter("ParamsTrimed", Params.TrimEnd('"').TrimStart('"'));
                context.SetParameter("AddActions", new Action<string>(AddActions));
                context.SetParameter("AddRefine", new Action<string>(AddRefine));
                context.SetParameter("AddImport", new Action<string>(AddImport));  

                context.Run(Code);

                InvokeStatment = (string)context.GetParameter("InvokeStatement");
                InvokeStatmentBuilded = (string)context.GetParameter("InvokeStatmentBuilded");
                InvokeStatmentBuildedEnd = (string)context.GetParameter("InvokeStatmentBuildedEnd");
            }
        }


    }

}
