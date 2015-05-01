using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
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

        [ImportMany("sources")]
        private static List<ITokenSource> tokensources = new List<ITokenSource>();

        //value patterns
        private static string ValuePat = @"((([_a-zA-Z](([_a-zA-Z0-9])?)+(\.{0,})?(.*)?)|(" +'"' + '\\' + "w{0,}" + '"' + ")|((([+-]?)[0-9]+)(\\.[0-9]+)?))( (.*))?)";

        public static void LoadTokens()
        {
            Tokens.Clear();

            var dc = new System.ComponentModel.Composition.Hosting.DirectoryCatalog("path to plugins");
            var con = new System.ComponentModel.Composition.Hosting.CompositionContainer();
            con.ComposeParts(tokensources);

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
            Tokens.Add("Begin", new Token() { Name = "Begin", Names = new List<string>() { "Computer", "create ", "an", "program", "named", "that" } , UsesRegex = false;});
            Tokens.Add("Action", new Token() {Name = "Action", Names = Actions , UsesRegex = false});
            Tokens.Add("Value", new Token(){ Name = "Value", Names = new List<string>{"(?<Value>(" + ValuePat + "))"}});
            Tokens.Add("Refine", new Token(){Name = "Refine", Names = Refines});

            //add external token loading here

            // here people must be able to add tokens
        }

        public static Token ResolveTextToToken(string t)
        {

            return null;
        }

    }
}
