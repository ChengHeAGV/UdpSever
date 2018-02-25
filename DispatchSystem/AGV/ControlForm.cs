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
    public partial class ControlForm : Form
    {
        int deviceNum = 0;
        public ControlForm(int num)
        {
            deviceNum = num;
            InitializeComponent();
        }

        private void ControlForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("AGV{0}-远程操作", deviceNum);
        }

        //读多个寄存器
        private void button1_Click(object sender, EventArgs e)
        {
            UdpSever.ReturnMsg rm = UdpSever.Read_Multiple_Registers(deviceNum, 0, 128);
            Console.WriteLine("读取结果:\r\n{0}", rm.ToString());
        }

        //写多个寄存器
        private void button2_Click(object sender, EventArgs e)
        {
            UInt16[] data = new ushort[128];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (ushort)i;
            }
            UdpSever.ReturnMsg rm = UdpSever.Write_Multiple_Registers(deviceNum, 0, 128, data);
            Console.WriteLine("写入结果:\r\n{0}", rm.ToString());
        }
    }
}
