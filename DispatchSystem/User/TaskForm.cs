using AdvancedDataGridView;
using Modbus.Device;
using Modbus.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem.User
{
    public partial class TaskForm : Form
    {
        public TaskForm()
        {
            InitializeComponent();
        }

        /*
         * 引用DLL
         * Modbus
         * log4net
         * unme.com
         * 
         * Modbus Master 是TcpClient，Slave是TcpSever
         * 
         */

        /// <summary>
        /// 任务状态
        /// </summary>
        enum TaskState
        {
            Waiting, //等待
            Runing,  //运行中
            Finished //完成
        }

        /// <summary>
        /// 任务
        /// </summary>
        class Task
        {
            public int TaskNum; //任务编号
            public int TaskState; //任务状态
            public DateTime CmdTime;//任务下发时间
            public int AgvNum; //AGV编号
            public int Connect; //通信状态
            public int Route;//当前路径
            public int Station;//当前站点
            public int Power;//电量
            public int Speed;//速度
            public int AgvState; //Agv状态
            public int Error;//报警信息
        }

        //任务列表
        List<Task> tasks = new List<Task>();
        private void TaskForm_Load(object sender, EventArgs e)
        {
            #region ModbusTcp 代码调试
            //TcpClient tcpClient = new TcpClient("192.168.10.106", 502);
            //ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);

            //master.Transport.WriteTimeout = 100;
            //master.Transport.ReadTimeout = 100;
            //master.Transport.WaitToRetryMilliseconds = 10;
            //master.Transport.Retries = 3;

            //try
            //{
            //    Random rd = new Random();
            //    ushort[] write = new ushort[10];
            //    for (int i = 0; i < 10; i++)
            //    {
            //        write[i] = (ushort)rd.Next(1, 1000);
            //    }
            //    master.WriteMultipleRegisters(0, write);
            //    ushort[] read = new ushort[10];
            //    // read = master.ReadWriteMultipleRegisters(0, 10, 0, write);

            //    read = master.ReadHoldingRegisters(10, 10);
            //    UdpSever.Shell.WriteNotice("debug", "{0},{1}", read[0], read[1]);
            //}
            //catch (Exception ex)
            //{
            //    UdpSever.Shell.WriteError("debug", ex.ToString());
            //}
            #endregion

            #region 任务列表测试
            //增加列
            dataGridView1.Columns.Add("1", "任务编号");
            var index = dataGridView1.Columns.Add("2", "MES下发时间");
            dataGridView1.Columns[index].Width = 130;
            dataGridView1.Columns.Add("3", "任务状态");
            dataGridView1.Columns.Add("4", "AGV编号");
            dataGridView1.Columns.Add("5", "通信状态");
            dataGridView1.Columns.Add("6", "当前路径");
            dataGridView1.Columns.Add("7", "当前站点");
            dataGridView1.Columns.Add("8", "电量%");
            dataGridView1.Columns.Add("9", "速度");
            dataGridView1.Columns.Add("10", "运行状态");
            dataGridView1.Columns.Add("11", "报警信息");

            //增加行
            for (int i = 0; i < 10; i++)
            {
                Task task = new Task();
                task.TaskNum = i;
                task.CmdTime = DateTime.Now;
                task.TaskState = (int)TaskState.Runing;
                task.AgvNum = 1;
                task.Connect = 2;
                task.Route = 2;
                task.Station = 2;
                task.Power = 2;
                task.Speed = 2;
                task.AgvState = 2;
                task.Error = 2;
                tasks.Add(task);
            }

            foreach (var item in tasks)
            {
                index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = item.TaskNum;
                this.dataGridView1.Rows[index].Cells[1].Value = item.CmdTime.ToString("yyyy-MM-dd HH:mm:ss");
                this.dataGridView1.Rows[index].Cells[2].Value = item.TaskState;
                this.dataGridView1.Rows[index].Cells[3].Value = item.AgvNum;
                this.dataGridView1.Rows[index].Cells[4].Value = item.Connect;
                this.dataGridView1.Rows[index].Cells[5].Value = item.Route;
                this.dataGridView1.Rows[index].Cells[6].Value = item.Station;
                this.dataGridView1.Rows[index].Cells[7].Value = item.Power;
                this.dataGridView1.Rows[index].Cells[8].Value = item.Speed;
                this.dataGridView1.Rows[index].Cells[9].Value = item.AgvState;
                this.dataGridView1.Rows[index].Cells[10].Value = item.Error;
            }
            #endregion
        }
    }
}
