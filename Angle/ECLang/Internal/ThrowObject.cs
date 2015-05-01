namespace ECLang.Internal
{
    using System;

    using ECLang.Internal.Primitives.Base;

    public class ThrowException : Exception
    {
        #region Fields

        private readonly Primitive p;

        #endregion

        #region Constructors and Destructors

        public ThrowException(Primitive p)
        {
            this.p = p;
        }

        #endregion

        #region Public Properties

        public Primitive Object
        {
            get
            {
                return this.p;
            }
        }

        #endregion
    }
}