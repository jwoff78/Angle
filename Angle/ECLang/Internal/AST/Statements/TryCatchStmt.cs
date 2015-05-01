namespace ECLang.Internal.AST.Statements
{
    using System;
    using System.Collections.Generic;

    using ECLang.AST;

    public class TryCatchStmt : MultilineStatement
    {
        #region Enums

        private enum TryCatchState
        {
            Try,

            Catch,

            Finally
        }

        #endregion

        #region Public Properties

        public string CatchExp { get; set; }

        public List<IAst> CatchNodes { get; set; }

        public List<IAst> FinallyNodes { get; set; }

        public List<IAst> TryNodes { get; set; }

        public string catchheader { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Clear()
        {
            this.TryNodes = new List<IAst>();
            this.CatchNodes = new List<IAst>();
            this.FinallyNodes = new List<IAst>();
            this.CatchExp = null;
        }

        public override bool EndIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("trycatchend").IsValid(src);
        }

        public override MultilineStatement Interprete(string src)
        {
            var returns = new TryCatchStmt();
            string trysrc = "";
            string catchsrc = "";
            string finalysrc = "";
            var state = TryCatchState.Try;
            for (int index = 0;
                index < src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries).Length;
                index++)
            {
                string i = src.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)[index];

                if (Parser.Grammar.GetPattern("catch").IsValid(i))
                {
                    returns.catchheader = i;
                    state = TryCatchState.Catch;
                }
                if (Parser.Grammar.GetPattern("finally").IsValid(i))
                {
                    state = TryCatchState.Finally;
                }

                if (state == TryCatchState.Try)
                {
                    if (index != 0)
                    {
                        trysrc += i + ";\n";
                    }
                }
                if (state == TryCatchState.Catch)
                {
                    catchsrc += i + ";\n";
                }
                if (state == TryCatchState.Finally)
                {
                    finalysrc += i + ";\n";
                }
            }
            returns.TryNodes = Parser.ParseCodeBlock(trysrc.TrimEnd('\n'), "").Nodes;
            returns.CatchNodes = Parser.ParseCodeBlock(catchsrc, "").Nodes;
            returns.FinallyNodes = Parser.ParseCodeBlock(finalysrc, "").Nodes;
            return returns;
        }

        public override Boolean ParserHeader(string aHeader)
        {
            return true;
        }

        public override bool StartIsMatch(string src)
        {
            return Parser.Grammar.GetPattern("trycatchstart").IsValid(src);
        }

        #endregion
    }
}