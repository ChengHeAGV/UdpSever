using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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

        private async void DoSomething(Button bb)
        {
            await Task.Run(() =>
            {
                UdpSever.ReturnMsg rm = UdpSever.Read_Multiple_Registers(deviceNum, 0, 128);
                Console.WriteLine("读取结果:\r\n{0}", rm.ToString());

            });
            bb.Enabled = true;
        }

        //读多个寄存器
        private void button1_Click(object sender, EventArgs e)
        {
            UdpSever.Shell.WriteNotice("系统消息", "开始执行{0}", DateTime.Now.ToLocalTime());
            var bb = sender as Button;
            bb.Enabled = false;
            DoSomething(bb);
            UdpSever.Shell.WriteNotice("系统消息", "结束{0}", DateTime.Now.ToLocalTime());
        }

        //写多个寄存器
        private void button2_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            UInt16[] data = new ushort[128];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (ushort)(rd.Next(1, 1000));
            }
            UdpSever.ReturnMsg rm = UdpSever.Write_Multiple_Registers(deviceNum, 0, 128, data);
            Console.WriteLine("写入结果:\r\n{0}", rm.ToString());
        }



        private async void WriteRegister(Button bb,int reg,int data, string str)
        {
            await Task.Run(() =>
            {
                UdpSever.Write_Register(deviceNum, reg, data);
            });
            UdpSever.Shell.WriteNotice("debug", str);
            bb.Enabled = true;
        }
        //滚筒左转
        private void buttonGunTongLeft_Click(object sender, EventArgs e)
        {
            var bb = sender as Button;
            bb.Enabled = false;
            WriteRegister(bb,1,1,string.Format("滚筒左转[{0}][{1}]", deviceNum, 1));
        }
        //滚筒右转
        private void buttonGunTongRight_Click(object sender, EventArgs e)
        {
            var bb = sender as Button;
            bb.Enabled = false;
            WriteRegister(bb, 2, 1, string.Format("滚筒右转[{0}][{1}]", deviceNum, 2));
        }
        //滚筒停止
        private void button_GunTongStop_Click(object sender, EventArgs e)
        {
            var bb = sender as Button;
            bb.Enabled = false;
            WriteRegister(bb, 3, 1, string.Format("滚筒停止[{0}][{1}]", deviceNum, 3));
        }
    }
}
