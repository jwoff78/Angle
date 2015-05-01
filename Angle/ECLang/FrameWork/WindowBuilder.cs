namespace ECLang.Framework
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class WindowBuilder
    {
        #region Fields

        private readonly List<Control> controls = new List<Control>();

        #endregion

        #region Public Methods and Operators

        public void Add(Control actrl)
        {
            this.controls.Add(actrl);
        }

        public Form Gen()
        {
            var returns = new Form();

            foreach (Control control in this.controls)
            {
                returns.Controls.Add(control);
            }

            return returns;
        }

        public void SetValue(string name, string property, params object[] obj)
        {
            MessageBox.Show("");
            foreach (Control control in this.controls)
            {
                if (control.Name == name)
                {
                    control.GetType().GetProperty(property).SetValue(control, this.GenObj(property, obj), null);
                    break;
                }
            }
        }

        #endregion

        #region Methods

        private object GenObj(string prop, object[] obj)
        {
            switch (prop)
            {
                case "Location":
                    return new Point((int)obj[0], (int)obj[1]);
            }
            return null;
        }

        #endregion
    }
}