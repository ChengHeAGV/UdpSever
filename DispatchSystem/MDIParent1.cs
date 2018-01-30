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

    }
}
