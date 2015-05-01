namespace ECLang.Internal
{
    using System;

    public abstract class EcCustomClass : IDisposable
    {
        #region Public Methods and Operators

        public abstract object Call(string name, params object[] parameters);

        public void Dispose()
        {
            this.OnClean();
        }

        #endregion

        #region Methods

        protected abstract void OnClean();

        #endregion
    }
}