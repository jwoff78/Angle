namespace ECLang.Internal.AST.Statements
{
    using System.Text.RegularExpressions;

    using ECLang.AST;

    public class DeIncreaseStmt : Statement
    {
        #region Public Properties

        public DeIncreaseOperation Operation { get; set; }

        public string Variable { get; set; }

        #endregion

        #region Public Methods and Operators

        public override Statement Interprete(string src, int line)
        {
            var returns = new DeIncreaseStmt();
            returns.Line = line;
            var regexp = new Regex(Parser.Grammar.GetPattern("deincrease").ToString());
            Match m = regexp.Match(src);

            returns.Variable = m.Groups["Name"].Value;
            returns.Operation = m.Groups["op"].Value == "++"
                ? DeIncreaseOperation.Increase
                : DeIncreaseOperation.Decrease;

            return returns;
        }

        public override bool IsStatement(string src)
        {
            return Parser.Grammar.GetPattern("deincrease").IsValid(src);
        }

        #endregion

        public override void Accept(IVisitor v)
        {
            v.Visit(this);
        }
    }

    public enum DeIncreaseOperation
    {
        Increase,

        Decrease
    }
}