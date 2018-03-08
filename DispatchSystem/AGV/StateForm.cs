using System;
using System.Windows.Forms;

namespace DispatchSystem.AGV
{
    public partial class StateForm : Form
    {
        int deviceNum = 0;
        public StateForm(int num)
        {
            deviceNum = num;
            InitializeComponent();
        }

        private void StateForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("AGV{0}-运行状态", deviceNum);
        }
    }
}
