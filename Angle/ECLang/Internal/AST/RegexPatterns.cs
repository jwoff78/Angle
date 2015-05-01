namespace ECLang.AST
{
    public class RegexPatterns
    {
        // dec name : type  = value;

        #region Constants

        public const string DecSetStmt = @"([a-zA-Z]\w+) = (([a-zA-Z]\w+)|([0-9]+)|(\""\w+\""))";

        public const string DeclareVariable =
            @"dec ([a-zA-Z]\w+) : " + Primitives + @" = (([a-zA-Z]\w+)|([0-9]+)|(\""\w{0,}\""))";
            //ToDo: Fix DeclarePattern

        public const string Primitives = @"(number|byte|regex|object|string|bool)";

        #endregion
    }
}