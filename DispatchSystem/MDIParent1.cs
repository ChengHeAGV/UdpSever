using DispatchSystem.AGV;
using DispatchSystem.Class;
using DispatchSystem.Database;
using DispatchSystem.Developer;
using DispatchSystem.Set;
using DispatchSystem.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DispatchSystem
{
    public partial class MDIParent1 : Form
    {
        #region Console信息同步进程
        ExThread ConsoleThread;
        private void ConsoleFunc()
        {
            while (true)
            {
                if (ConsoleThread.exitEvent.WaitOne(100))
                {
                    break;
                }
                if (exConsole1.Count < MyConsole.ConsoleList.Count && exConsole1.Count >= 0)
                {
                    for (int i = exConsole1.Count; i < MyConsole.ConsoleList.Count; i++)
                    {
                        exConsole1.WriteLine(MyConsole.ConsoleList[i].Msg, MyConsole.ConsoleList[i].color, MyConsole.ConsoleList[i].Size);
                    }
                }
            }
        }
        #endregion

        public MDIParent1()
        {
            InitializeComponent();
        }

        private void MDIParent1_Load(object sender, EventArgs e)
        {
            //清除AGV列表
            treeView1.Nodes.Clear();
            //启动主线程
            Thread th = new Thread(new ThreadStart(mainThread));
            th.Start();
        }

        private void mainThread()
        {
            //启动Console信息同步进程
            ConsoleThread = new ExThread(ConsoleFunc);
            ConsoleThread.Start();

            MyConsole.Add("系统启动中...");

            MyConsole.Add(string.Format("服务器地址:{0}", UdpSever.ServerAddress));
            MyConsole.Add(string.Format("设备数:{0}", UdpSever.DeviceNum));
            MyConsole.Add(string.Format("寄存器数:{0}", UdpSever.RegisterNum));
            MyConsole.Add(string.Format("单帧数据长度:{0}", UdpSever.FrameLen));
            MyConsole.Add(string.Format("心跳周期:{0}秒", UdpSever.HeartCycle));
            MyConsole.Add(string.Format("重发次数:{0}", UdpSever.RepeatNum));
            MyConsole.Add(string.Format("超时时间:{0}", UdpSever.ResponseTimeout));
            MyConsole.Add(string.Format("响应帧缓冲池容量:{0}", UdpSever.RESPONSE_MAX_LEN));
            MyConsole.Add(string.Format("设备总数:{0}", UdpSever.DeviceNum));

            //测试ModbusTcp服务器连接
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send("192.168.250.102", 100);//第一个参数为ip地址，第二个参数为ping的时间
            if (reply.Status == IPStatus.Success)
                MyConsole.Add("ModbusTcp服务器连接成功!", Color.Green);
            else
                MyConsole.Add("ModbusTcp服务器连接失败!", Color.Red);

            #region 获取本机IP，自动开启Dbus服务器
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    MyConsole.Add(string.Format("本机IP:[{0}]", ipa));
                    UdpSever.ipaddress = ipa;
                }
            }

            //自动启动服务器
            UdpSever.Resault rs = UdpSever.Start();
            //更新界面
            udpConfigForm_MyEvent();
            #endregion

            #region 启动任务界面
            this.Invoke(new MethodInvoker(delegate
            {
                taskForm.TopLevel = false;
            taskForm.Parent = splitContainer3.Panel1;
            taskForm.Show();//弹出这个窗口
            taskForm.Focus();//激活显示
            }));
            #endregion
        }

        /// <summary>
        /// 网络配置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        UdpConfigForm udpConfigForm = null;

        private void UdpConfig(object sender, EventArgs e)
        {
            try
            {
                if (udpConfigForm == null)
                {
                    udpConfigForm = new UdpConfigForm();
                    //注册udpConfigForm_MyEvent方法的MyEvent事件     
                    udpConfigForm.MyEvent += new MyDelegate(udpConfigForm_MyEvent);
                    udpConfigForm.Show();//弹出这个窗口
                }
                else
                {
                    if (udpConfigForm.IsDisposed != true)
                    {
                        udpConfigForm.Show();//弹出这个窗口
                        udpConfigForm.Focus();//激活显示
                    }
                    else
                    {
                        udpConfigForm = new UdpConfigForm();
                        //注册udpConfigForm_MyEvent方法的MyEvent事件     
                        udpConfigForm.MyEvent += new MyDelegate(udpConfigForm_MyEvent);
                        udpConfigForm.Show();//弹出这个窗口
                        udpConfigForm.Focus();//激活显示
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        //处理     
        void udpConfigForm_MyEvent()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                if (UdpSever.State)
                {
                    toolStripStatusLabel1.ForeColor = Color.Green;
                    toolStripStatusLabel1.Text = "运行中";
                    timerState.Enabled = false;
                }
                else
                {
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "已断开";
                    //断开连接时以500ms闪烁显示
                    timerState.Interval = 500;
                    timerState.Enabled = true;
                }
            }));
        }
        //断开连接时状态自动闪烁
        private void timerState_Tick(object sender, EventArgs e)
        {
            if (toolStripStatusLabel1.ForeColor == Color.LightGray)
            {
                toolStripStatusLabel1.ForeColor = Color.Red;
            }
            else
            {
                toolStripStatusLabel1.ForeColor = Color.LightGray;
            }
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        #region 双击AGV列表子节点
        private Point pi; //用pi记录光标所在的位置
        private void treeView1_MouseMove(object sender, MouseEventArgs e) //得到光标所在的位置
        {
            pi = new Point(e.X, e.Y);
        }
        //传感器
        SensorForm[] sensorForm = new SensorForm[UdpSever.DeviceNum];
        //运行状态
        StateForm[] stateForm = new StateForm[UdpSever.DeviceNum];
        //远程操作
        ControlForm[] controlForm = new ControlForm[UdpSever.DeviceNum];
        //参数设置
        SetForm[] setForm = new SetForm[UdpSever.DeviceNum];
        //寄存器
        RegisterForm[] registerForm = new RegisterForm[UdpSever.DeviceNum];

        private void treeView1_DoubleClick(object sender, EventArgs e) //从光标所在的位置得到该位置上的节点
        {
            TreeNode node = this.treeView1.GetNodeAt(pi);
            if (pi.X < node.Bounds.Left || pi.X > node.Bounds.Right)
            {
                //不触发事件;
            }
            else if (node.Parent != null)
            {
                int parent = int.Parse(node.Parent.Name);
                //传感器
                if (node.Index == (int.Parse(AGV.Sensor.Key) - 1))
                {
                    try
                    {
                        sensorForm[parent].WindowState = FormWindowState.Normal;
                        sensorForm[parent].Show();//弹出这个窗口
                        sensorForm[parent].Activate();//激活显示
                    }
                    catch (Exception)
                    {
                        sensorForm[parent] = new SensorForm(int.Parse(node.Parent.Name));
                        sensorForm[parent].Show();//弹出这个窗口
                    }
                }
                //运行状态
                else if (node.Index == (int.Parse(AGV.State.Key) - 1))
                {
                    try
                    {
                        stateForm[parent].WindowState = FormWindowState.Normal;
                        stateForm[parent].Show();//弹出这个窗口
                        stateForm[parent].Activate();//激活显示
                    }
                    catch (Exception)
                    {
                        stateForm[parent] = new StateForm(int.Parse(node.Parent.Name));
                        stateForm[parent].Show();//弹出这个窗口
                    }
                }
                //远程操作
                else if (node.Index == (int.Parse(AGV.Control.Key) - 1))
                {
                    //try
                    //{
                    //    controlForm[parent].WindowState = FormWindowState.Normal;
                    //    controlForm[parent].Show();//弹出这个窗口
                    //    controlForm[parent].Activate();//激活显示
                    //}
                    //catch (Exception)
                    //{
                    //    controlForm[parent] = new ControlForm(int.Parse(node.Parent.Name));
                    //    controlForm[parent].Show();//弹出这个窗口
                    //}
                }
                //参数设置
                else if (node.Index == (int.Parse(AGV.Set.Key) - 1))
                {
                    //try
                    //{
                    //    setForm[parent].WindowState = FormWindowState.Normal;
                    //    setForm[parent].Show();//弹出这个窗口
                    //    setForm[parent].Activate();//激活显示
                    //}
                    //catch (Exception)
                    //{
                    //    setForm[parent] = new SetForm(int.Parse(node.Parent.Name));
                    //    setForm[parent].Show();//弹出这个窗口
                    //}
                }
                //寄存器
                else if (node.Index == (int.Parse(AGV.Register.Key) - 1))
                {
                    try
                    {
                        registerForm[parent].WindowState = FormWindowState.Normal;
                        registerForm[parent].Show();//弹出这个窗口
                        registerForm[parent].Activate();//激活显示
                    }
                    catch (Exception)
                    {
                        registerForm[parent] = new RegisterForm(int.Parse(node.Parent.Name));
                        registerForm[parent].Show();//弹出这个窗口
                    }
                }
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }
        #endregion


        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        //设备在线监测
        private void timerOnlineCheck_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < UdpSever.DeviceNum; i++)
            {
                //超过心跳周期未更新认为设备离线
                if ((DateTime.Now - UdpSever.UpdateTime[i]).TotalSeconds > UdpSever.HeartCycle)
                {
                    if (treeView1.Nodes[i.ToString()] != null)
                    {
                        treeView1.Nodes[i.ToString()].Remove();
                    }
                    UdpSever.EndPointArray[i] = null;
                }
                else
                {
                    if (treeView1.Nodes[i.ToString()] == null)
                    {
                        treeView1.Nodes.Add(i.ToString(), "AGV" + i.ToString());


                        //传感器
                        treeView1.Nodes[i.ToString()].Nodes.Add(AGV.Sensor.Key, AGV.Sensor.Text);
                        treeView1.Nodes[i.ToString()].Nodes[AGV.Sensor.Key].ImageIndex = AGV.Sensor.ImageIndex;
                        treeView1.Nodes[i.ToString()].Nodes[AGV.Sensor.Key].SelectedImageIndex = AGV.Sensor.SelectedImageIndex;
                        //运行状态
                        treeView1.Nodes[i.ToString()].Nodes.Add(AGV.State.Key, AGV.State.Text);
                        treeView1.Nodes[i.ToString()].Nodes[AGV.State.Key].ImageIndex = AGV.State.ImageIndex;
                        treeView1.Nodes[i.ToString()].Nodes[AGV.State.Key].SelectedImageIndex = AGV.State.SelectedImageIndex;
                        //远程操作
                        treeView1.Nodes[i.ToString()].Nodes.Add(AGV.Control.Key, AGV.Control.Text);
                        treeView1.Nodes[i.ToString()].Nodes[AGV.Control.Key].ImageIndex = AGV.Control.ImageIndex;
                        treeView1.Nodes[i.ToString()].Nodes[AGV.Control.Key].SelectedImageIndex = AGV.Control.SelectedImageIndex;
                        //参数设置
                        treeView1.Nodes[i.ToString()].Nodes.Add(AGV.Set.Key, AGV.Set.Text);
                        treeView1.Nodes[i.ToString()].Nodes[AGV.Set.Key].ImageIndex = AGV.Set.ImageIndex;
                        treeView1.Nodes[i.ToString()].Nodes[AGV.Set.Key].SelectedImageIndex = AGV.Set.SelectedImageIndex;
                        //寄存器
                        treeView1.Nodes[i.ToString()].Nodes.Add(AGV.Register.Key, AGV.Register.Text);
                        treeView1.Nodes[i.ToString()].Nodes[AGV.Register.Key].ImageIndex = AGV.Register.ImageIndex;
                        treeView1.Nodes[i.ToString()].Nodes[AGV.Register.Key].SelectedImageIndex = AGV.Register.SelectedImageIndex;
                    }

                    //判断AGV是否就绪
                    if (UdpSever.Register[i, 8, 0] == 2)//就绪
                        treeView1.Nodes[i.ToString()].ForeColor = Color.Green;
                    else
                    {
                        if (treeView1.Nodes[i.ToString()].ForeColor == Color.Red)
                            treeView1.Nodes[i.ToString()].ForeColor = Color.Black;
                        else
                            treeView1.Nodes[i.ToString()].ForeColor = Color.Red;
                    }

                }
            }
            UdpSever.OnlieDeviceNum = treeView1.GetNodeCount(false);
        }

        /// <summary>
        /// AGV类
        /// </summary>
        static class AGV
        {
            /// <summary>
            /// 传感器
            /// </summary>
            public static class Sensor
            {
                public static string Key = "1";
                public static string Text = "传感器";
                public static int ImageIndex = 1;
                public static int SelectedImageIndex = 1;
            }
            /// <summary>
            /// 运行状态
            /// </summary>
            public static class State
            {
                public static string Key = "2";
                public static string Text = "运行状态";
                public static int ImageIndex = 2;
                public static int SelectedImageIndex = 2;
            }
            /// <summary>
            /// 远程操作
            /// </summary>
            public static class Control
            {
                public static string Key = "3";
                public static string Text = "远程操作";
                public static int ImageIndex = 3;
                public static int SelectedImageIndex = 3;
            }
            /// <summary>
            /// 参数设置
            /// </summary>
            public static class Set
            {
                public static string Key = "4";
                public static string Text = "参数设置";
                public static int ImageIndex = 4;
                public static int SelectedImageIndex = 4;
            }
            /// <summary>
            /// 寄存器
            /// </summary>
            public static class Register
            {
                public static string Key = "5";
                public static string Text = "寄存器";
                public static int ImageIndex = 5;
                public static int SelectedImageIndex = 5;
            }
        }

        private void MDIParent1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("真的要退出程序吗？", "退出程序", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                ConsoleThread.Stop();
                if (UdpSever.State)//如果服务器运行，则关闭服务器
                {
                    UdpSever.Stop();
                }
                //关闭任务调度线程
                taskForm.TaskStop();
            }

        }

        private void MDIParent1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭所有界面及线程
            //System.Environment.Exit(0);
        }

        private void 路径规划ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region 工具
        private void 地标读写ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(@"C:\Windows\Temp\Animal_Tag_Read_Writer.exe"))
            {
                FileStream str = new FileStream(@"C:\Windows\Temp\Animal_Tag_Read_Writer.exe", FileMode.OpenOrCreate);
                str.Write(ThirdAppResource.Animal_Tag_Read_Writer, 0, ThirdAppResource.Animal_Tag_Read_Writer.Length);
                str.Close();
            }
            System.Diagnostics.Process.Start(@"C:\Windows\Temp\Animal_Tag_Read_Writer.exe");
        }

        private void 串口助手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(@"C:\Windows\Temp\XCOM.exe"))
            {
                FileStream str = new FileStream(@"C:\Windows\Temp\XCOM.exe", FileMode.OpenOrCreate);
                str.Write(ThirdAppResource.XCOM_V2_0, 0, ThirdAppResource.XCOM_V2_0.Length);
                str.Close();
            }
            System.Diagnostics.Process.Start(@"C:\Windows\Temp\XCOM.exe");
        }

        //UdpToolForm udpform = new UdpToolForm();
        private void 网络助手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(@"C:\Windows\Temp\NetAssist.exe"))
            {
                FileStream str = new FileStream(@"C:\Windows\Temp\NetAssist.exe", FileMode.OpenOrCreate);
                str.Write(ThirdAppResource.NetAssist, 0, ThirdAppResource.NetAssist.Length);
                str.Close();
            }
            System.Diagnostics.Process.Start(@"C:\Windows\Temp\NetAssist.exe");
            //try
            //{
            //    if (udpform.IsDisposed != true)
            //    {
            //        udpform.Show();//弹出这个窗口
            //        udpform.Focus();//激活显示
            //    }
            //    else
            //    {
            //        udpform = new UdpToolForm();
            //        udpform.Show();//弹出这个窗口
            //        udpform.Focus();//激活显示
            //    }
            //}
            //catch
            //{
            //}
        }
        AgvParameter agvParameter = new AgvParameter();
        private void 参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (agvParameter.IsDisposed != true)
                {
                    agvParameter.Show();//弹出这个窗口
                    agvParameter.Focus();//激活显示
                }
                else
                {
                    agvParameter = new AgvParameter();
                    agvParameter.Show();//弹出这个窗口
                    agvParameter.Focus();//激活显示
                }
            }
            catch
            {

            }
        }
        #endregion

        //调试信息
        DebugForm debugForm = new DebugForm();
        private void DebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //启动调试界面
            if (debugForm.IsDisposed != true)
            {
                debugForm.Show();//弹出这个窗口
                debugForm.Focus();//激活显示
            }
            else
            {
                debugForm = new DebugForm();
                debugForm.Show();//弹出这个窗口
                debugForm.Focus();//激活显示
            }
        }
        /// <summary>
        /// 网络监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ListenForm listenForm = new ListenForm();
        private void ListenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listenForm.IsDisposed != true)
            {
                listenForm.Show();//弹出这个窗口
                listenForm.Focus();//激活显示
            }
            else
            {
                listenForm = new ListenForm();
                listenForm.Show();//弹出这个窗口
                listenForm.Focus();//激活显示
            }
        }

        DbusTestForm dbusForm = new DbusTestForm();
        private void dBUSTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbusForm.IsDisposed != true)
            {
                dbusForm.Show();//弹出这个窗口
                dbusForm.Focus();//激活显示
            }
            else
            {
                dbusForm = new DbusTestForm();
                dbusForm.Show();//弹出这个窗口
                dbusForm.Focus();//激活显示
            }
        }

        //任务监控界面
        TaskForm taskForm = new TaskForm();
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (taskForm.IsDisposed != true)
            {
                taskForm.TopLevel = false;
                taskForm.Parent = splitContainer3.Panel1;
                taskForm.Show();//弹出这个窗口
                taskForm.Focus();//激活显示
            }
            else
            {
                taskForm = new TaskForm();
                taskForm.Show();//弹出这个窗口
                taskForm.Focus();//激活显示
            }
        }

        private void toolStripModbus_Click(object sender, EventArgs e)
        {
            //if (modbusSetForm.IsDisposed != true)
            //{
            //    modbusSetForm.Show();//弹出这个窗口
            //    modbusSetForm.Focus();//激活显示
            //}
            //else
            //{
            //    modbusSetForm = new ModbusForm();
            //    modbusSetForm.Show();//弹出这个窗口
            //    modbusSetForm.Focus();//激活显示
            //}
        }

        //数据交换监控
        DataMonitorForm monitorForm = new DataMonitorForm();
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (monitorForm.IsDisposed != true)
            {
                monitorForm.Show();//弹出这个窗口
                monitorForm.Focus();//激活显示
            }
            else
            {
                monitorForm = new DataMonitorForm();
                monitorForm.Show();//弹出这个窗口
                monitorForm.Focus();//激活显示
            }
        }

        CanForm canForm = new CanForm();
        private void cANToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (canForm.IsDisposed != true)
            {
                canForm.Show();//弹出这个窗口
                canForm.Focus();//激活显示
            }
            else
            {
                canForm = new CanForm();
                canForm.Show();//弹出这个窗口
                canForm.Focus();//激活显示
            }
        }

        private void dbus服务器配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer3_Panel2_SizeChanged(object sender, EventArgs e)
        {
            //consoleLog.WindowState = FormWindowState.Normal;
            //consoleLog.WindowState = FormWindowState.Maximized;

            taskForm.WindowState = FormWindowState.Normal;
            taskForm.WindowState = FormWindowState.Maximized;
        }

        //ModbusTcp配置
        private void mESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModbusTcpConfigForm modbusConfig = new ModbusTcpConfigForm();
            modbusConfig.ShowDialog();
        }
        //数据中继配置
        private void 数据中继ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
