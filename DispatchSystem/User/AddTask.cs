using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem.User
{
    public partial class AddTask : Form
    {
        private string lineName;//产线名称
        private int taskNum = 0;//任务编号

        public string LinekName
        {
            get { return this.lineName; }

        }
        public int TaskNum
        {
            get { return this.taskNum; }
        }

        public AddTask()
        {
            InitializeComponent();
        }

        private void AddTask_Load(object sender, EventArgs e)
        {
            comboBoxLine.SelectedIndex = 0;
            comboBoxNum.SelectedIndex = 0;
            comboBoxAction.SelectedIndex = 0;
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            //十位是动作，个位是位置,放舟Put 1,接舟Get2
            //炉号
            int temp1 = comboBoxNum.SelectedIndex;
            //动作
            int temp2 = comboBoxAction.SelectedIndex + 1;

            lineName = comboBoxLine.Text;

            taskNum = temp2 * 10 + temp1;

            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            taskNum = 0;
        }
    }
}
