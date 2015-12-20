using System;
using System.Windows.Forms;

namespace PredatorDev.Spudz.TestForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FogBugzManager fogbugz = new FogBugzManager("https://predatordev.fogbugz.com/", null, null);
            this.richTextBox1.Text = fogbugz.signon();
        }
    }
}
