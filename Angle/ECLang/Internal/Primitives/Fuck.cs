namespace ECLang.Internal.Primitives
{
    using ECLang.Internal.Primitives.Base;

    public class Fuck : Primitive
    {
        #region Constructors and Destructors

        public Fuck()
        {
            this.Name = "fuck";
        }

        public Fuck(string src)
        {
            this.Value = src;
            this.Name = "fuck";
        }

        #endregion

        #region Public Methods and Operators

        public override Primitive Parse(object src)
        {
            return new Fuck(src as string);
        }

        public override bool Validate(object src)
        {
            return Parser.Grammar.GetPattern("fuck").IsValid((string)src);
        }

        public void run()
        {
            new Brainfuck(this.Value as string).run();
        }

        #endregion
    }
}