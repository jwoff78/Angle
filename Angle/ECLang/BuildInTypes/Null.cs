using ECLang.AST;

namespace ECLang.BuildInTypes
{
    public class Null : Statement
    {
        public override Statement Interprete(string src)
        {
            return new Null();
        }

        public override bool IsStatement(string src)
        {
            return src == "null";
        }
    }
}
