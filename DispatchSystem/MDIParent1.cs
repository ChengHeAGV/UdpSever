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

    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;

        public MDIParent1()
        {
            InitializeComponent();
        }

        private void MDIParent1_Load(object sender, EventArgs e)
        {
            //treeView1.Nodes.Clear();
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
        DataForm dataForm;
        private void treeView1_DoubleClick(object sender, EventArgs e) //从光标所在的位置得到该位置上的节点
        {
            TreeNode node = this.treeView1.GetNodeAt(pi);
            if (pi.X < node.Bounds.Left || pi.X > node.Bounds.Right)
            {
                //textBox1.Text = "不触发事件";
            }
            else
            {
                //寄存器监控
                if (node.Index == 1)
                {
                    try
                    {
                        dataForm.WindowState = FormWindowState.Normal;
                        dataForm.Show();//弹出这个窗口
                        dataForm.Activate();//激活显示
                    }
                    catch (Exception)
                    {
                        dataForm = new DataForm(int.Parse(node.Parent.Name));
                        dataForm.Show();//弹出这个窗口
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

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        private void MDIParent1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //如果服务器运行，则关闭服务器
            if (UdpSever.State)
            {
                UdpSever.Stop();
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
                        treeView1.Nodes[i.ToString()].Nodes.Add("1", "设备状态");
                        treeView1.Nodes[i.ToString()].Nodes.Add("2", "寄存器监控");
                        treeView1.Nodes[i.ToString()].Nodes.Add("3", "设备操作");
                        treeView1.Nodes[i.ToString()].Nodes["1"].ImageIndex = 1;
                        treeView1.Nodes[i.ToString()].Nodes["1"].SelectedImageIndex = 1;
                    }
                }
            }
        }
    }
}
