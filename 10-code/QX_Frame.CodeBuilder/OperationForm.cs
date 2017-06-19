using CSharp_FlowchartToCode_DG.Options;
using System;
using System.Windows.Forms;

namespace CSharp_FlowchartToCode_DG
{
    public delegate void OperationEvent(Opt_OperationType operationType);
    public partial class OperationForm : Form
    {
        public event OperationEvent operationEvent;

        public OperationForm() => InitializeComponent();

        private void OperationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            MainForm.isOperationFormShow = false;
        }

        //Build Entities
        private void button1_Click(object sender, EventArgs e) => operationEvent(Opt_OperationType.Entities);

        private void button2_Click(object sender, EventArgs e) => operationEvent(Opt_OperationType.CodeForInstance);

        private void button3_Click(object sender, EventArgs e) => operationEvent(Opt_OperationType.CodeForInstance_Another);

        private void button4_Click(object sender, EventArgs e) => operationEvent(Opt_OperationType.SqlStatements);

        //Build QX_Frame.Data
        private void button5_Click(object sender, EventArgs e) => operationEvent(Opt_OperationType.QX_Frame_Data_QueryObject);

        private void button6_Click(object sender, EventArgs e) => operationEvent(Opt_OperationType.QX_Frame_Data_Contract);

        private void button7_Click(object sender, EventArgs e) => operationEvent(Opt_OperationType.QX_Frame_Data_Service);

        //Build Controller
        private void button8_Click(object sender, EventArgs e) => operationEvent(Opt_OperationType.REST_WebApiController);

        //Build Javascript
        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        //Build Form
        private void button11_Click(object sender, EventArgs e)
        {

        }
    }
}
