using System.Drawing.Imaging;

namespace ECLang.Internal.Primitives.Base
{
    using System;
    using System.Collections.Generic;

    public abstract class Primitive
    {
        #region Fields

        /// <summary>
        ///     The Name of the Primitive This is used to Identify it in the script
        /// </summary>
        public String Name;

        /// <summary>
        ///     Sould Engin handle mathmatical exspretions for this Primirive
        /// </summary>
        public Boolean UseMaths = false;

        /// <summary>
        ///     The Return Value for this prmitive when it is Used
        /// </summary>
        public object Value;

        #endregion

        #region Constructors and Destructors

        public Primitive()
        {
        }

        public Primitive(object o)
        {
            this.Value = o;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     This return a EcBuiltInType that is parsed from src
        /// </summary>
        /// <param name="src">The raw string of the value</param>
        /// ///
        /// <returns>Primitive</returns>
        public abstract Primitive Parse(object src);

        public override string ToString()
        {
            return Convert.ToString(this.Value);
        }

        /// <summary>
        ///     Validates the given value as this type
        /// </summary>
        /// <param name="src">The src to validate</param>
        /// <returns>True or Flase baced on the src type</returns>
        public abstract bool Validate(object src);

        /// <summary>
        ///     Calls method in object
        /// </summary>
        /// <param name="data">Method to call</param>
        /// <param name="perams">Perms of method</param>
        /// <returns></returns>
        public virtual object call(string data, List<object> Params)
        {
            return GetType().GetMethod(data).Invoke(this, Params.ToArray());
           // return null;
        }

        #endregion

        #region "Operator overloading stuff EC"

        public virtual OperatorCollection CanOvveride
        {
            get { return null; }
        }

        public virtual Primitive PlusEquals(Primitive first, Primitive second)
        {
            return null;
        }

        public virtual Primitive MinusEquals(Primitive first, Primitive second)
        {
            return null;
        }
        public virtual bool Equals(Primitive first, Primitive second)
        {
            return false;
        }
        public virtual bool NotEquals(Primitive first, Primitive second)
        {
            return false;
        }
        public virtual bool LessThan(Primitive first, Primitive second)
        {
            return false;
        }
        public virtual bool GreaterThan(Primitive first, Primitive second)
        {
            return false;
        }
        public virtual bool Mod(Primitive first, Primitive second)
        {
            return false;
        }
        public virtual bool Or(Primitive first, Primitive second)
        {
            return false;
        }
        public virtual bool And(Primitive first, Primitive second)
        {
            return false;
        }
        public virtual Primitive Increase()
        {
            return null;
        }
        public virtual Primitive Decrease()
        {
            return null;
        }

        #endregion

        public virtual Primitive defaulter()
        {
            return new EcObject(null);
        }
    }
}