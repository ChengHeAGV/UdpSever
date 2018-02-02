using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem
{
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;

        public MDIParent1()
        {
            InitializeComponent();
        }

        private void MDIParent1_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            //for (int i = 0; i < 5; i++)
            //{
            //    treeView1.Nodes.Add(i.ToString(), "AGV" + i.ToString());
            //    //传感器
            //    treeView1.Nodes[i.ToString()].Nodes.Add(AGV.Sensor.Key, AGV.Sensor.Text);
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.Sensor.Key].ImageIndex = AGV.Sensor.ImageIndex;
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.Sensor.Key].SelectedImageIndex = AGV.Sensor.SelectedImageIndex;
            //    //运行状态
            //    treeView1.Nodes[i.ToString()].Nodes.Add(AGV.State.Key, AGV.State.Text);
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.State.Key].ImageIndex = AGV.State.ImageIndex;
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.State.Key].SelectedImageIndex = AGV.State.SelectedImageIndex;
            //    //远程操作
            //    treeView1.Nodes[i.ToString()].Nodes.Add(AGV.Control.Key, AGV.Control.Text);
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.Control.Key].ImageIndex = AGV.Control.ImageIndex;
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.Control.Key].SelectedImageIndex = AGV.Control.SelectedImageIndex;
            //    //参数设置
            //    treeView1.Nodes[i.ToString()].Nodes.Add(AGV.Set.Key, AGV.Set.Text);
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.Set.Key].ImageIndex = AGV.Set.ImageIndex;
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.Set.Key].SelectedImageIndex = AGV.Set.SelectedImageIndex;
            //    //寄存器
            //    treeView1.Nodes[i.ToString()].Nodes.Add(AGV.Register.Key, AGV.Register.Text);
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.Register.Key].ImageIndex = AGV.Register.ImageIndex;
            //    treeView1.Nodes[i.ToString()].Nodes[AGV.Register.Key].SelectedImageIndex = AGV.Register.SelectedImageIndex;

            //}

        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "窗口 " + childFormNumber++;
            childForm.Show();

            UdpConfigForm udpConfig = new UdpConfigForm();
            udpConfig.MdiParent = this;
            udpConfig.Text = "网络配置界面";
            udpConfig.Show();
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
                udpConfigForm.WindowState = FormWindowState.Normal;
                udpConfigForm.Show();//弹出这个窗口
                udpConfigForm.Activate();//激活显示
            }
            catch (Exception)
            {
                udpConfigForm = new UdpConfigForm();
                //注册udpConfigForm_MyEvent方法的MyEvent事件     
                udpConfigForm.MyEvent += new MyDelegate(udpConfigForm_MyEvent);
                udpConfigForm.Show();//弹出这个窗口
            }
        }

        //处理     
        void udpConfigForm_MyEvent()
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
        DataForm[] dataForm;
        private void treeView1_DoubleClick(object sender, EventArgs e) //从光标所在的位置得到该位置上的节点
        {
            if (dataForm == null)
            {
                dataForm = new DataForm[UdpSever.DeviceNum];
            }
            TreeNode node = this.treeView1.GetNodeAt(pi);
            if (pi.X < node.Bounds.Left || pi.X > node.Bounds.Right)
            {
                //textBox1.Text = "不触发事件";
            }
            else if (node.Parent != null)
            {
                int parent = int.Parse(node.Parent.Name);
                //寄存器监控
                if (node.Index == (int.Parse(AGV.Register.Key) - 1))
                {
                    try
                    {
                        dataForm[parent].WindowState = FormWindowState.Normal;
                        dataForm[parent].Show();//弹出这个窗口
                        dataForm[parent].Activate();//激活显示
                    }
                    catch (Exception)
                    {
                        dataForm[parent] = new DataForm(int.Parse(node.Parent.Name));
                        dataForm[parent].Show();//弹出这个窗口
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
                }
            }
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
            if (UdpSever.State)//如果服务器运行，则关闭服务器
            {
                UdpSever.Stop();
            }
        }

        private void MDIParent1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭所有界面及线程
            System.Environment.Exit(0);
        }

        private void 路径规划ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 串口助手ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        UdpToolForm udpform;
        private void 网络助手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                udpform.WindowState = FormWindowState.Normal;
                udpform.Show();//弹出这个窗口
                udpform.Activate();//激活显示
            }
            catch (Exception)
            {
                udpform = new UdpToolForm();
                udpform.Show();//弹出这个窗口
            }
        }
    }
}
