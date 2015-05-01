namespace ECLang.Internal
{
    using System.Collections.Generic;

    using ECLang.AST;

    public class CodeBlock
    {
        #region Fields

        public List<IAst> Nodes = new List<IAst>();

        public List<string> Parameters = new List<string>();

        public string ReturnType;

        #endregion

        #region Public Properties

        public string Name { get; set; }

        public string Returns { get; set; }

        #endregion
    }
}