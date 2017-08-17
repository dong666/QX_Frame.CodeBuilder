using QX_Frame.Helper_DG;
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
    public partial class ToolsForm : Form
    {
        public ToolsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox_OutPut.Text = Guid.NewGuid().ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox_OutPut.Text = DateTime_Helper_DG.GetCurrentTimeStamp().ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox_OutPut.Text = Encrypt_Helper_DG.MD5_Encrypt(richTextBox_Input.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Encrypt_Helper_DG.RSA_Keys keys = Encrypt_Helper_DG.RSA_GetKeys();

            StringBuilder builder = new StringBuilder();

            builder.Append( "RSA_PublicKey ----- ----- ----- ----- ----->\n\n");
            builder.Append(keys.PublicKey);
            builder.Append("\n\nRSA_PrivateKey ----- ----- ----- ----- ----->\n\n");
            builder.Append(keys.PrivateKey);

            richTextBox_OutPut.Text = builder.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            richTextBox_OutPut.SelectAll();
            richTextBox_OutPut.Copy();
            label3.Text = "copied !";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox_OutPut.Text = richTextBox_Input.Text.Length.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox_OutPut.Text = Encrypt_Helper_DG.Base64_Encode(richTextBox_Input.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox_OutPut.Text = Encrypt_Helper_DG.Base64_Decode(richTextBox_Input.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            richTextBox_OutPut.Text = Info.CopyRight;
        }
    }
}
