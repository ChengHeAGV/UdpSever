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

namespace DispatchSystem.User
{
    public partial class DataMonitor : Form
    {
        Thread mainThread;
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
            mainThread = new Thread(new ThreadStart(func));
            mainThread.IsBackground = true;
            mainThread.Start();
        }

        private void func()
        {
            while (true)
            {
                if (this.Created)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        for (int i = 0; i < DataTransmission.Profinet.Register.Length; i++)
                        {
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
                            uDataGridView1.Rows[i].Cells[5].Value = "";
                            //描述
                            uDataGridView1.Rows[i].Cells[6].Value = "";
                        }
                    }));
                }
                Thread.Sleep(1000);
            }
        }
    }
}
