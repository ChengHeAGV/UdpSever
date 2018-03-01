using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem
{
    public partial class SensorForm : Form
    {
        int deviceNum = 0;
        public SensorForm(int num)
        {
            deviceNum = num;
            InitializeComponent();
        }

        private void SensorForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("AGV{0}-传感器",deviceNum);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //读取寄存器值
            UdpSever.ReturnMsg  rm= UdpSever.Read_Register(deviceNum, 1);
            Console.WriteLine("读取结果:{0},{1}",rm.resault,rm.Data.ToString("X2"));
        }

        private void SensorForm_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
