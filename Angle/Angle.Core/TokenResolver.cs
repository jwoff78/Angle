using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Angle.Core
{
    public class TokenResolver
    {
        /*
         * At a later stage we need to beable to alow adding tokens using an adding system 
         */
        public static Dictionary<string, Token> Tokens = new Dictionary<string, Token>();
        private static  List<string> Actions = new List<string>();
        private static  List<string> Refines = new List<string>();

  

        //value patterns                    string
        private static string ValuePat = " (\"(.*)+\")";

        public static void LoadTokens()
        {
            Tokens.Clear();

         
        

            //load actions here
            Actions.Clear();
            Actions.Add("Print");
            Actions.Add("Prints");

            // add external action loading here
            
            //here poeple need to beable to add actions(just an string)

            // load Refines here
            Refines.Clear();
            Refines.Add("Console");
            
                        
            // add externalleading here

            //same here

            //add all tokens here
            Tokens.Add("Begin", new Token() { Name = "Begin", Names = new List<string>() { "Computer", "create ", "an", "program", "named", "that" } , UsesRegex = false});
            Tokens.Add("Action", new Token() {Name = "Action", Names = Actions , UsesRegex = false});
            Tokens.Add("Value", new Token(){ Name = "Value", Names = new List<string>{"(?<Value>(" + ValuePat + "))"}});
            Tokens.Add("Refine", new Token(){Name = "Refine", Names = Refines, UsesRegex = false});

            //add external token loading here

            // here people must be able to add tokens
        }

        public static List<Token> ResolveTextToToken(string t)
        {
            List<Token> ret = new List<Token>();
            foreach(var i in Tokens)
            {
                if(!i.Value.UsesRegex)
                {
                    int c = 0;
                    string fin = "";
                    foreach(var d in i.Value.Names)
                    {
                        if (t.ToLower().Contains(d.ToLower()))
                        {
                            c++;
                           t = t.ToLower().Remove(t.ToLower().IndexOf(d.ToLower()), d.Length);
                            fin += d.ToLower() + " ";
                        }
                    }
                    if(c > 0)
                    {
                        i.Value.Value = fin.Trim();
                        ret.Add(i.Value);
                    }
                }
                else
                {
                    int c = 0;
                    string fin = "";
                    foreach (var d in i.Value.Names)
                    {
                        if (Regex.IsMatch(t,d))
                        {
                            c++;
                            var m = Regex.Match(t, d);
                            t = t.Remove(t.IndexOf(m.Value), m.Value.Length);
                            fin += m.Value + " ";
                        }
                    }
                    if (c > 0)
                    {
                        i.Value.Value = fin.Trim();
                        ret.Add(i.Value);
                    }
                }
            }

            return ret;
        }

    }
}
