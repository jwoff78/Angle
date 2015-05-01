using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ECLang.AST;
using ECLang.Internal.Primitives.Base;

namespace ECLang.Internal.AST.Statements
{
    public class ForEachStmt : MultilineStatement
    {
        public List<IAst> Nodes { get; set; }
        public Choice Choice { get; set; }
        public string Array { get; set; }
        public string ScopeName { get; set; }
        public string Header = "";

        public ForEachStmt()
        {
            Nodes = new List<IAst>();
        }

        public override void Clear()
        {
            Nodes = null;
            Choice = null;
            Array = null;
        }

        public override bool EndIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("foreachend").IsValid(src);
        }

        public override MultilineStatement Interprete(string src)
        {
            var returns = new ForEachStmt();
            string temp = "";
            for (int index = 0;
                index < src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries).Length;
                index++)
            {
                string i = src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)[index];
                if (index == 0)
                {
                    returns.Header = i;
                }
                else if (index > 0
                         && index < src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries).Length - 1)
                {
                    temp += i + ";\n";
                    ;
                }
            }
            returns.Nodes = Parser.ParseCodeBlock(temp, "").Nodes;


            return returns;
        }

        public override bool ParserHeader(string aHeader)
        {
            var gr = Parser.Grammar.GetPattern("foreachstart").Match(aHeader);

            ScopeName = gr.Groups["Name"].Value;
            Array = gr.Groups["In"].Value;

            try
            {
                Choice = new Choice() { Type = gr.Groups["Choice"].Value };
            }
            catch
            {
                
            }

            return true;
        }

        public override bool StartIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("foreachstart").IsValid(src);
        }
    }

    public class Choice
    {
        public string Type { get; set; }
    }
}
