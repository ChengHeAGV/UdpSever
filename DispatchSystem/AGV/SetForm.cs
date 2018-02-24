using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem.AGV
{
    public partial class SetForm : Form
    {
        int deviceNum = 0;
        public SetForm(int num)
        {
            deviceNum = num;
            InitializeComponent();
        }

        private void SetForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("AGV{0}-参数设置", deviceNum);
        }
    }
}
