using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DispatchSystem.User
{
    public partial class DataMonitor : Form
    {
        Thread mainThread;
        ushort[] DataCompare = new ushort[DataTransmission.Profinet.Register.Length];
        public DataMonitor()
        {
            InitializeComponent();
        }

        string[] datekey = new string[10];
        private void DataMonitor_Load(object sender, EventArgs e)
        {
            datekey[0] = "日期";
            datekey[1] = "时间";
            datekey[2] = "寄存器类型";
            datekey[3] = "寄存器地址";
            datekey[4] = "值";
            datekey[5] = "更新次数";
            datekey[6] = "描述";

            doubleBufferListView1.FullRowSelect = true;//要选择就是一行
            doubleBufferListView1.Columns.Add(datekey[0], 150, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[1], 210, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[2], 130, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[3], 130, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[4], 100, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[5], 100, HorizontalAlignment.Center);
            doubleBufferListView1.Columns.Add(datekey[6], 300, HorizontalAlignment.Center);

            for (int i = 0; i < DataTransmission.Profinet.Register.Length; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = i.ToString();//"日期";
                item.SubItems.Add("1");//  "时间";
                item.SubItems.Add("2");//  "寄存器类型";
                item.SubItems.Add("3");//  "寄存器地址";
                item.SubItems.Add("4");//  "值";
                item.SubItems.Add("5");//  "更新次数";
                item.SubItems.Add("6");//  "描述";
                doubleBufferListView1.Items.Add(item);
            }

            update(false);
            mainThread = new Thread(new ThreadStart(func));
            mainThread.IsBackground = true;
            mainThread.Start();
        }
        private void func()
        {
            while (this.IsHandleCreated && this.IsDisposed == false)
            {
                this.BeginInvoke(new MethodInvoker(delegate
                 {
                     update(true);
                 }));
                Thread.Sleep(1000);
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
                        doubleBufferListView1.Items[i].Font = new Font("新宋体", 20,FontStyle.Bold);
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
                    doubleBufferListView1.Items[i].SubItems[6].Text = "";//

                    if (i % 2 == 0)
                        doubleBufferListView1.Items[i].BackColor = Color.FromArgb(200, 0xf5, 0xf6, 0xeb);

                    //更新对比缓存
                    DataCompare[i] = DataTransmission.Profinet.Register[i];
                }
            }
        }
    }
}
