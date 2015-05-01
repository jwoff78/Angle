namespace ECLang.Internal.Tables
{
    using System.Collections.Generic;

    public class ErrorTable
    {
        #region Static Fields

        private static readonly List<Error> errors = new List<Error>();

        #endregion

        #region Public Methods and Operators

        public static void Add(int line, string message)
        {
            errors.Add(new Error(line, message));
        }

        public static void Clear()
        {
            errors.Clear();
        }

        public static Error[] ToArray()
        {
            return errors.ToArray();
        }

        #endregion
    }

    public class Error
    {
        #region Fields

        public int Line;

        public string Message;

        #endregion

        #region Constructors and Destructors

        public Error(int line, string message)
        {
            this.Line = line;
            this.Message = message;
        }

        #endregion
    }
}