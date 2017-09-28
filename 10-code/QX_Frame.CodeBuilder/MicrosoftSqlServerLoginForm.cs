using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharp_FlowchartToCode_DG
{

    public partial class MicrosoftSqlServerLoginForm : Form
    {
        MainForm mainForm;
        public MicrosoftSqlServerLoginForm()
        {
            InitializeComponent();
        }
        public MicrosoftSqlServerLoginForm(MainForm form)
        {
            InitializeComponent();
            this.mainForm = form;
        }

        //cancel button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        //connect button
        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
