using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem
{
    //定义一个需要string类型参数的委托
    public delegate void MyDelegate();
    public partial class UdpConfigForm : Form
    {
        //定义该委托的事件     
        public event MyDelegate MyEvent;
        public UdpConfigForm()
        {
            InitializeComponent();
        }

        private void UdpConfigForm_Load(object sender, EventArgs e)
        {
            //判断服务器状态
            if (UdpSever.State)//启动状态
            {
                buttonStart.Text = "停止";
                textBoxIP.ReadOnly = true;
                textBoxPort.ReadOnly = true;
            }
            else//停止状态
            {
                buttonStart.Text = "启动";
                textBoxIP.ReadOnly = false;
                textBoxPort.ReadOnly = false;
            }

            //加载IP和端口
            //string str = Ini.Read("网络配置", "IP");
            //if (str != "null")
            //{
            //    textBoxIP.Text = str;
            //}
            //else
            //{
            //    textBoxIP.Text = "192.168.2.23";
            //}

            //获取本机IP
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);

            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBoxIP.Text = ipa.ToString();
                    break;
                }
            }
           

            string str = Ini.Read("网络配置", "Port");
            if (str != "null")
            {
                textBoxPort.Text = str;
            }
            else
            {
                textBoxPort.Text = "18666";
            }
        }

        //转移焦点，避免输出框被选中
        private void UdpConfigForm_Activated(object sender, EventArgs e)
        {
            groupBox1.Focus();
        }

        // 启动或停止服务
        private void UdpStartStop(object sender, EventArgs e)
        {
            if (UdpSever.State)//启动状态
            {
                UdpSever.Resault rs = UdpSever.Stop();
                if (rs.Reault)
                {
                    buttonStart.Text = "启动";
                    textBoxIP.ReadOnly = false;
                    textBoxPort.ReadOnly = false;
                    //MessageBox.Show(rs.Message, "提示");
                    this.Close();
                }
                else
                {
                    MessageBox.Show(rs.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else//停止状态
            {
                UdpSever.ipaddress = IPAddress.Parse(textBoxIP.Text);
                UdpSever.port = int.Parse(textBoxPort.Text);
                UdpSever.Resault rs = UdpSever.Start();
                if (rs.Reault)
                {
                    buttonStart.Text = "停止";
                    textBoxIP.ReadOnly = true;
                    textBoxPort.ReadOnly = true;
                    this.Close();
                    //MessageBox.Show(rs.Message, "提示");
                }
                else
                {
                    MessageBox.Show(rs.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //触发事件     
            MyEvent();
        }

        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            Ini.Write("网络配置", "IP", textBoxIP.Text);
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            Ini.Write("网络配置", "Port", textBoxPort.Text);
        }
    }
}
