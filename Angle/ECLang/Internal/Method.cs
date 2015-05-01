namespace ECLang.Internal
{
    using System.Collections.Generic;

    using ECLang.AST;

    public class Method
    {
        #region Fields

        public string Name;

        public List<IAst> Nodes = new List<IAst>();

        public List<string> perms = new List<string>();

        #endregion

        #region Public Properties

        public string returns { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Run()
        {
            Program.e.ResolveTreeNode(this.Nodes);
        }

        #endregion
    }
}