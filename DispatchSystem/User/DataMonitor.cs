using System;
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

        private void DataMonitor_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < DataTransmission.Profinet.Register.Length; i++)
            {
                uDataGridView1.Rows.Add();
            }

            update(false);

            mainThread = new Thread(new ThreadStart(func));
            mainThread.IsBackground = true;
            mainThread.Start();
        }
        private void func()
        {
            while (!formcloseing)
            {
                this.Invoke(new MethodInvoker(delegate
                 {
                     for (int i = 0; i < DataTransmission.Profinet.Register.Length; i++)
                     {
                         update(true);
                     }
                 }));
                Thread.Sleep(1000);
            }
            formcloseing = false;
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
                    //更新界面

                    //日期
                    uDataGridView1.Rows[i].Cells[0].Value = DateTime.Now.ToString("yyyy-MM-dd");
                    //时间
                    uDataGridView1.Rows[i].Cells[1].Value = DateTime.Now.ToString("HH:mm:ss fff");
                    //寄存器类型
                    uDataGridView1.Rows[i].Cells[2].Value = "[Word/U16]";
                    //寄存器地址
                    uDataGridView1.Rows[i].Cells[3].Value = i.ToString();
                    //值
                    uDataGridView1.Rows[i].Cells[4].Value = DataTransmission.Profinet.Register[i];
                    //更新次数
                    uDataGridView1.Rows[i].Cells[5].Value = change == false ? 0 : (int)(uDataGridView1.Rows[i].Cells[5].Value) + 1;
                    //描述
                    uDataGridView1.Rows[i].Cells[6].Value = "";

                    //更新对比缓存
                    DataCompare[i] = DataTransmission.Profinet.Register[i];
                }
            }
        }

        bool formcloseing = false;
        private void DataMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            formcloseing = true;
            while (formcloseing)
            {
                Thread.Sleep(10);
            }
        }
    }
}
