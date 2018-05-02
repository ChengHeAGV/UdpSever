using DispatchSystem.Class;
using DispatchSystem.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DispatchSystem.User
{
    public partial class DataMonitor : Form
    {
        ExThread mainThread;


        ushort[] DataCompare = new ushort[DataTransmission.Profinet.Register.Length];

        masterEntities db = new masterEntities();
        List<DbProfinet> dbProfinet = new List<DbProfinet>();

        //右键菜单
        ContextMenuStrip contextMenu;
        public DataMonitor()
        {
            InitializeComponent();
        }

        string[] datekey = new string[10];
        private void DataMonitor_Load(object sender, EventArgs e)
        {
            this.FormClosing += DataMonitor_FormClosing;
            //创建菜单
            contextMenu = new ContextMenuStrip();
            contextMenu.Font = new Font("新宋体", 14);
            contextMenu.Items.Add("更新描述");
            contextMenu.Items.Add("清除描述");
            //添加点击事件
            contextMenu.Items[0].Click += contextMenu_AddDes_Click;
            contextMenu.Items[1].Click += contextMenu_ClearDes_Click;

            //添加单元格点击事件
            doubleBufferListView1.MouseClick += DoubleBufferListView1_MouseClick;

            #region 数据列表
            datekey[0] = "日期";
            datekey[1] = "时间";
            datekey[2] = "寄存器类型";
            datekey[3] = "寄存器地址";
            datekey[4] = "值";
            datekey[5] = "更新次数";
            datekey[6] = "传输方向";
            datekey[7] = "描述";

            doubleBufferListView1.FullRowSelect = true;//要选择就是一行
            doubleBufferListView1.Columns.Add(datekey[0], 180, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[1], 220, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[2], 140, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[3], 140, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[4], 100, HorizontalAlignment.Left);
            doubleBufferListView1.Columns.Add(datekey[5], 100, HorizontalAlignment.Left);
            doubleBufferListView1.Columns.Add(datekey[6], 150, HorizontalAlignment.Left);
            doubleBufferListView1.Columns.Add(datekey[7], 300, HorizontalAlignment.Left);

            for (int i = 0; i < DataTransmission.Profinet.Register.Length; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = DateTime.Now.ToString("yyyy-MM-dd");//"日期";
                item.SubItems.Add(DateTime.Now.ToString("HH:mm:ss fff"));//  "时间";
                item.SubItems.Add("WORD");//  "寄存器类型";
                item.SubItems.Add("0");//  "寄存器地址";
                item.SubItems.Add("0");//  "值";
                item.SubItems.Add("0");//  "更新次数";
                item.SubItems.Add("");//  "传输方向";
                item.SubItems.Add("");//  "描述";
                doubleBufferListView1.Items.Add(item);
                doubleBufferListView1.Items[i].ForeColor = Color.Gray;
                if (i % 2 == 0)
                    doubleBufferListView1.Items[i].BackColor = Color.FromArgb(200, 0xf5, 0xf6, 0xeb);
            }
            #endregion


            mainThread = new ExThread(func);
            mainThread.Start();
        }

        private void DataMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainThread.Stop();
        }

        private void DoubleBufferListView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (doubleBufferListView1.SelectedItems.Count > 0)
                {
                    doubleBufferListView1.SelectedItems[0].Selected = true;
                    contextMenu.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        /// <summary>
        /// 清除描述
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenu_ClearDes_Click(object sender, EventArgs e)
        {
            if (doubleBufferListView1.SelectedItems.Count > 0)
            {
                DialogResult dr = MessageBox.Show("确定要清除寄存器信息吗?", "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    //获取寄存器编号
                    int regNum = int.Parse(doubleBufferListView1.SelectedItems[0].SubItems[3].Text);

                    //更新数据库
                    var temp = dbProfinet.FirstOrDefault(m => m.reg == regNum.ToString());
                    if (temp != null)
                    {
                        //更新数据
                        DbProfinet u = new DbProfinet() { Id = temp.Id, reg = temp.reg, dir = "", des = "" };
                        db.Entry<DbProfinet>(u).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    //更新传输方向
                    doubleBufferListView1.SelectedItems[0].SubItems[6].Text = "";
                    //更新描述
                    doubleBufferListView1.SelectedItems[0].SubItems[7].Text = "";
                }
            }
        }

        //增加描述
        private void contextMenu_AddDes_Click(object sender, EventArgs e)
        {
            if (doubleBufferListView1.SelectedItems.Count > 0)
            {
                //获取寄存器编号
                int regNum = int.Parse(doubleBufferListView1.SelectedItems[0].SubItems[3].Text);
                UpdateDataInfo updateDataInfo = new UpdateDataInfo(regNum, doubleBufferListView1.SelectedItems[0].SubItems[6].Text, doubleBufferListView1.SelectedItems[0].SubItems[7].Text);
                updateDataInfo.ShowDialog();
                if (updateDataInfo.DialogResult == DialogResult.OK)
                {
                    //更新传输方向
                    doubleBufferListView1.SelectedItems[0].SubItems[6].Text = updateDataInfo.Dir;
                    //更新描述
                    doubleBufferListView1.SelectedItems[0].SubItems[7].Text = updateDataInfo.Des;
                }
            }
        }

        private void func()
        {
            //加载所有记录,此处AsNoTracking是为了更新
            dbProfinet = db.DbProfinet.AsNoTracking().ToList();
            this.BeginInvoke(new MethodInvoker(delegate
            {
                update(false);
            }));
            while (this.IsHandleCreated && this.IsDisposed == false)
            {
                if (mainThread.exitEvent.WaitOne(1000))//延时1000ms
                    break;
                this.BeginInvoke(new MethodInvoker(delegate
                 {
                     update(true);
                 }));
            }
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        /// <param name="change">有变化才更新</param>
        private void update(bool change)
        {
            for (int i = 0; i < DataTransmission.Profinet.Register.Length; i++)
            {
                //有变化
                if ((DataCompare[i] != DataTransmission.Profinet.Register[i]) || change == false)
                {
                    if (change)
                    {
                        doubleBufferListView1.Items[i].ForeColor = Color.Blue;
                        doubleBufferListView1.Items[i].Font = new Font("新宋体", 18, FontStyle.Bold);
                    }
                    else
                        doubleBufferListView1.Items[i].ForeColor = Color.Gray;

                    //更新界面
                    doubleBufferListView1.Items[i].SubItems[0].Text = DateTime.Now.ToString("yyyy-MM-dd");//
                    doubleBufferListView1.Items[i].SubItems[1].Text = DateTime.Now.ToString("HH:mm:ss fff");//
                    doubleBufferListView1.Items[i].SubItems[2].Text = "[Word]";//
                    doubleBufferListView1.Items[i].SubItems[3].Text = i.ToString();//
                    doubleBufferListView1.Items[i].SubItems[4].Text = DataTransmission.Profinet.Register[i].ToString();//
                    doubleBufferListView1.Items[i].SubItems[5].Text = (false ? 0 : int.Parse(doubleBufferListView1.Items[i].SubItems[5].Text) + 1).ToString();//

                    //第一次加载时从数据库读取
                    if (change == false)
                    {
                        //检索当前寄存器
                        var temp = dbProfinet.FirstOrDefault(m => m.reg == i.ToString());
                        if (temp != null)
                        {
                            //更新传输方向
                            doubleBufferListView1.Items[i].SubItems[6].Text = temp.dir;
                            //更新描述
                            doubleBufferListView1.Items[i].SubItems[7].Text = temp.des;
                        }

                    }

                    if (i % 2 == 0)
                        doubleBufferListView1.Items[i].BackColor = Color.FromArgb(200, 0xf5, 0xf6, 0xeb);

                    //更新对比缓存
                    DataCompare[i] = DataTransmission.Profinet.Register[i];
                }
            }
        }


    }
}
