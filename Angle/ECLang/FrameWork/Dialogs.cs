using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ECLang.Framework
{
    public static class Dialogs
    {
        public static string InPutDialog(string Message)
        {
            string returns = "";
            InDialgo dlg = new InDialgo();
            dlg.label1.Text = Message;
            dlg.Text = "";
            dlg.ShowDialog();
            returns = dlg.textBox1.Text;
            dlg.Close();
            dlg.Dispose();
            
            return returns;
        }

    }
}
