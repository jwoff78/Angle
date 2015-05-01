using Angle.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Angle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            Engine en = new Engine();
            en.Run("Computer create an program that, prints \"Hello World\" to the console. Create an form. set the forms \"Text\" to \"Emile\" ");
            while (true)
            {
                Application.DoEvents();
            }
            Console.ReadKey();
            
        }
    }
}
