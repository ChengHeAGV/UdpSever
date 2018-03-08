using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DispatchSystem
{
    public partial class RegisterForm : Form
    {
        Thread th;
        //public static int selectDataNum = 0;
        //int outdeviceNum = 0;
        int deviceNum = 0;

        string[] datekey = new string[10];
        public RegisterForm(int num)
        {
            InitializeComponent();
            deviceNum = num;
            this.Text = string.Format("AGV{0}-寄存器", deviceNum);

            datekey[0] = "寄存器";
            datekey[1] = "时间戳";
            datekey[2] = "十进制";
            datekey[3] = "十六进制";
            datekey[4] = "二进制";
            datekey[5] = "字符串";

            doubleBufferListView1.FullRowSelect = true;//要选择就是一行
            doubleBufferListView1.Columns.Add(datekey[0], 80, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[1], 230, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[2], 100, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[3], 100, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[4], 200, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[5], 100, HorizontalAlignment.Center);
        }

        private void DataForm_Load(object sender, EventArgs e)
        {
            UdpSever.ReturnMsg rm = UdpSever.Read_Multiple_Registers( deviceNum, 0, UdpSever.RegisterNum);
            if (rm.resault)
            {
                for (int i = 0; i < rm.DataBuf.Length; i++)
                {
                    UdpSever.Register[deviceNum, i, 0] = rm.DataBuf[i];
                    UdpSever.Register[deviceNum, i, 1] = UdpSever.DateTimeToStamp(DateTime.Now);
                }
            }
            else
            {
                UdpSever.Shell.WriteError("错误信息","读取失败！");
            }
            //加载数据
            for (int i = 0; i < UdpSever.RegisterNum; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = i.ToString();//"寄存器"
                item.SubItems.Add(UdpSever.Register[deviceNum, i, 1].ToString());//"时间戳"
                item.SubItems.Add(UdpSever.Register[deviceNum, i, 0].ToString());//"十进制"
                item.SubItems.Add(UdpSever.Register[deviceNum, i, 0].ToString("X2"));// "十六进制"
                item.SubItems.Add(Convert.ToString(UdpSever.Register[deviceNum, i, 0], 2).PadLeft(16, '0'));//"二进制"
                byte[] bt = new byte[2];
                bt[0] = (byte)(UdpSever.Register[deviceNum, i, 0] >> 8);
                bt[1] = (byte)(UdpSever.Register[deviceNum, i, 0]);
                string str = Encoding.GetEncoding("GB2312").GetString(bt, 0, 2).Replace("\0", "");
                item.SubItems.Add(str);//"字符串"
                doubleBufferListView1.Items.Add(item);
            }
            //启动自动更新进程
            th = new Thread(fun);
            th.Start();
        }

        private void fun()
        {
            while (true)
            {
                Thread.Sleep(50);
                this.Invoke(new MethodInvoker(delegate
                {
                    //更新数据
                    for (int i = 0; i < UdpSever.RegisterNum; i++)
                    {//UdpSever.Ddata[deviceNum, i, 1].ToString();
                        doubleBufferListView1.Items[i].SubItems[1].Text = UdpSever.StampToString(UdpSever.Register[deviceNum, i, 1]);//时间戳
                        doubleBufferListView1.Items[i].SubItems[2].Text = UdpSever.Register[deviceNum, i, 0].ToString();//十进制
                        doubleBufferListView1.Items[i].SubItems[3].Text = UdpSever.Register[deviceNum, i, 0].ToString("X2");//十六进制
                        doubleBufferListView1.Items[i].SubItems[4].Text = Convert.ToString(UdpSever.Register[deviceNum, i, 0], 2).PadLeft(16, '0');//二进制
                        byte[] bt = new byte[2];
                        bt[0] = (byte)(UdpSever.Register[deviceNum, i, 0] >> 8);
                        bt[1] = (byte)(UdpSever.Register[deviceNum, i, 0]);
                        string str = Encoding.GetEncoding("GB2312").GetString(bt, 0, 2).Replace("\0", "");
                        doubleBufferListView1.Items[i].SubItems[5].Text = str;//ASCII字符串

                        if (i % 2 == 0)
                            doubleBufferListView1.Items[i].BackColor = Color.FromArgb(200, 0xf5, 0xf6, 0xeb);
                    }
                }));
            }
        }

        private void DataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            th.Abort();
        }

        DisplayForm[] displayform;
        private void doubleBufferListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (displayform == null)
            {
                displayform = new DisplayForm[UdpSever.RegisterNum];
            }
            if (doubleBufferListView1.SelectedItems.Count > 0)
            {
                int registerNum = int.Parse(doubleBufferListView1.SelectedItems[0].SubItems[0].Text);
                try
                {
                    displayform[registerNum].WindowState = FormWindowState.Normal;
                    displayform[registerNum].Show();//弹出这个窗口
                    displayform[registerNum].Activate();//激活显示
                }
                catch (Exception)
                {
                    displayform[registerNum] = new DisplayForm(deviceNum, registerNum);
                    displayform[registerNum].Show();//弹出这个窗口
                }
            }
        }
    }
}
