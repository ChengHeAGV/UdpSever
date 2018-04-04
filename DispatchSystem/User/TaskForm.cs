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
using System.Threading;
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
        public enum TaskRunState
        {
            Waiting, //等待
            Runing,  //运行中
            Finished //完成
        }





        ModbusIpMaster modbusMaster;
        Thread modbusThread;
        Thread taskThread;
        private void TaskForm_Load(object sender, EventArgs e)
        {
            #region 启动ModbusTcp
            try
            {
                TcpClient tcpClient = new TcpClient(TaskData.Parameter.ModbusTcpSeverIPAddress, TaskData.Parameter.ModbusTcpSeverPort);
                modbusMaster = ModbusIpMaster.CreateIp(tcpClient);
                modbusMaster.Transport.WriteTimeout = TaskData.Parameter.ModbusWriteTimeout;//写超时
                modbusMaster.Transport.ReadTimeout = TaskData.Parameter.ModbusReadTimeout;//读超时
                modbusMaster.Transport.WaitToRetryMilliseconds = TaskData.Parameter.ModbusWaitToRetryTime;//重试等待时间
                modbusMaster.Transport.Retries = TaskData.Parameter.ModbusRetriesNum;//重试次数

                //启动监听进程
                modbusThread = new Thread(new ThreadStart(SyncModbus));
                modbusThread.Start();
            }
            catch
            {
                MessageBox.Show("连接Modbus设备失败！");
            }
            #endregion

            #region 启动任务调度
            taskThread = new Thread(new ThreadStart(taskFunc));
            taskThread.Start();
            #endregion

            TaskData.AGV1.AgvNum = 1;
            TaskData.AGV2.AgvNum = 2;

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
                task.TaskState = (int)TaskRunState.Runing;
                task.AgvNum = 1;
                task.Connect = true;
                task.Route = 2;
                task.Station = 2;
                task.Power = 2;
                task.Speed = 2;
                task.AgvState = 2;
                task.Error = 2;
                TaskData.taskWaiting.Add(task);
            }

            foreach (var item in TaskData.taskWaiting)
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


        /// <summary>
        /// 任务队列
        /// </summary>
        public class Task
        {
            public int TaskNum; //任务编号
            public int Level; //任务优先级
            public int LineNum;//产线编号
            public int TaskState = (int)TaskRunState.Waiting; //任务状态
            public DateTime CmdTime = DateTime.Now;//任务下发时间
            public DateTime StartTime;//任务开始时间
            public DateTime StopTime;//任务结束时间

            public int AgvNum = 0; //AGV编号
            public bool Connect = true; //通信状态
            public int Route = 0;//当前路径
            public int Station = 0;//当前站点
            public int Power = 0;//电量
            public int Speed = 0;//速度
            public int AgvState = 0; //Agv状态
            public int Error = 0;//报警信息
        }
        /// <summary>
        /// Modbus数据
        /// </summary>
        public static class TaskData
        {
            public static class Parameter
            {
                //modbus 寄存器检测个数
                public static int ModbusCount = 20;
                //modbus 检测时间
                public static int SyncModbusTime = 500;
                //任务调度 检测时间
                public static int taskFuncTime = 500;
                //modbus服务器IP地址
                public static string ModbusTcpSeverIPAddress = "192.168.10.106";
                //modbus服务器端口
                public static int ModbusTcpSeverPort = 502;
                //modbus写超时
                public static int ModbusWriteTimeout = 100;
                //modbus读超时
                public static int ModbusReadTimeout = 100;
                //modbus重试等待时间
                public static int ModbusWaitToRetryTime = 10;
                //modbus重试次数
                public static int ModbusRetriesNum = 3;
            }
            //modbus 监控寄存器
            public static ushort[] Modbus = new ushort[Parameter.ModbusCount];

            /// <summary>
            /// 寄存器绑定枚举
            /// </summary>
            public enum RegBind
            {
                /// <summary>
                /// 有新任务
                /// </summary>
                newTask = 0,
                /// <summary>
                /// 产线编号
                /// </summary>
                lineNum = 1,
                /// <summary>
                /// 任务编号
                /// </summary>
                taskNum = 2,
                /// <summary>
                /// 任务优先级
                /// </summary>
                taskLevel = 3
            }

            /// <summary>
            /// 等待任务队列
            /// </summary>
            public static List<Task> taskWaiting = new List<Task>();

            /// <summary>
            /// 进行中任务队列
            /// </summary>
            public static List<Task> taskRuning = new List<Task>();

            /// <summary>
            ///已完成任务队列
            /// </summary>
            public static List<Task> taskFinished = new List<Task>();


            //定义两台AGV
            public static Agv AGV1 = new Agv();
            public static Agv AGV2 = new Agv();
        }

        /// <summary>
        /// AGV类型
        /// </summary>
        public class Agv
        {
            /// <summary>
            /// AGV 状态枚举
            /// </summary>
            public enum State
            {
                Busying,
                Ready,
                Error,
                Outline
            }
            public enum Error
            {
                /// <summary>
                /// 出轨
                /// </summary>
                Derailed = 1,
                /// <summary>
                /// 障碍停止
                /// </summary>
                ObstacleStop = 2,
                /// <summary>
                /// 机械紧急停止
                /// </summary>
                EmergencyStop = 3,
                /// <summary>
                /// 电量低
                /// </summary>
                LowPower = 4
            }
            /// <summary>
            /// AGV当前状态
            /// </summary>
            public int state = (int)State.Busying;
            public int AgvNum = 0; //AGV编号
            public bool Connect = true; //通信状态
            public int Process = 0;//当前流程
            public int Route = 0;//当前路径
            public int Station = 0;//当前站点
            public int Power = 0;//电量
            public int Speed = 0;//速度
            public int error = 0;//报警
        }

        /// <summary>
        /// 任务方法
        /// </summary>
        private void taskFunc()
        {
            while (true)
            {
                #region 判断MES是否有新任务
                if (TaskData.Modbus[(int)TaskData.RegBind.newTask] == 1)
                {
                    //加入任务队列
                    Task task = new Task();
                    task.TaskNum = TaskData.Modbus[(int)TaskData.RegBind.taskNum];
                    task.Level = TaskData.Modbus[(int)TaskData.RegBind.taskLevel];
                    task.LineNum = TaskData.Modbus[(int)TaskData.RegBind.lineNum];

                    TaskData.taskWaiting.Add(task);
                    //按优先级排序(降序)
                    TaskData.taskWaiting = TaskData.taskWaiting.OrderByDescending(s => s.Level).ToList();
                    //按下单时间排序(升序)
                    //TaskData.taskWaiting = TaskData.taskWaiting.OrderBy(s => s.Level).ToList();                  
                }
                #endregion

                #region 派发任务到AGV
                //有需要完成的任务
                if (TaskData.taskWaiting.Count > 0)
                {
                    //1号产线有空闲的AGV
                    if (TaskData.AGV1.state == (int)Agv.State.Ready)
                    {
                        for (int i = 0; i < TaskData.taskWaiting.Count; i++)
                        {
                            //1号产线有任务
                            if (TaskData.taskWaiting[i].LineNum == 1)
                            {
                                //将1号产线的任务分配给1号车
                                ushort[] temp = new ushort[2];
                                temp[0] = 0x11;//有新任务 8
                                temp[1] = (ushort)TaskData.taskWaiting[i].TaskNum;//任务编号 9
                                //下发到AGV
                                UdpSever.Write_Multiple_Registers(1, 8, 2, temp);

                                //更新任务状态
                                TaskData.taskWaiting[i].StartTime = DateTime.Now;//任务启动时间
                                TaskData.taskWaiting[i].AgvNum = TaskData.AGV1.AgvNum;//执行该任务的AGV编号

                                //将该任务转至正在进行任务列表
                                TaskData.taskRuning.Add(TaskData.taskWaiting[i]);
                                //将该任务从等待执行任务列表删除
                                TaskData.taskWaiting.RemoveAt(i);
                                break;
                            }
                        }
                    }

                    //2号产线有空闲的AGV
                    if (TaskData.AGV2.state == (int)Agv.State.Ready)
                    {
                        for (int i = 0; i < TaskData.taskWaiting.Count; i++)
                        {
                            //2号产线有任务
                            if (TaskData.taskWaiting[i].LineNum == 2)
                            {
                                //将2号产线的任务分配给2号车
                                ushort[] temp = new ushort[2];
                                temp[0] = 0x11;//有任务
                                temp[1] = (ushort)TaskData.taskWaiting[i].TaskNum;//任务编号
                                //下发到AGV
                                UdpSever.Write_Multiple_Registers(2, 8, 2, temp);

                                //更新任务状态
                                TaskData.taskWaiting[i].StartTime = DateTime.Now;//任务启动时间
                                TaskData.taskWaiting[i].AgvNum = TaskData.AGV2.AgvNum;//执行该任务的AGV编号

                                //将该任务转至正在进行任务列表
                                TaskData.taskRuning.Add(TaskData.taskWaiting[i]);
                                //将该任务从等待执行任务列表删除
                                TaskData.taskWaiting.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region 更新任务执行状态
                //是否有正在执行的任务
                if (TaskData.taskRuning.Count > 0)
                {
                    for (int i = 0; i < TaskData.taskRuning.Count; i++)
                    {
                        //判断任务是否执行完成
                        if (UdpSever.Register[TaskData.taskRuning[i].AgvNum, 10, 0] == 0x12)
                        {
                            //正在执行
                            TaskData.taskRuning[i].TaskState = (int)TaskRunState.Runing;
                        }
                        else if (UdpSever.Register[TaskData.taskRuning[i].AgvNum, 10, 0] == 0x13)
                        {
                            //执行完成
                            TaskData.taskRuning[i].TaskState = (int)TaskRunState.Finished;
                            //将该任务转至执行完成任务列表
                            TaskData.taskFinished.Add(TaskData.taskRuning[i]);
                            //将该任务从正在执行任务列表删除
                            TaskData.taskRuning.RemoveAt(i);

                            //清空AGV执行状态标志位，AGV收到该信号后为设置车为空闲状态
                            UdpSever.Write_Register(TaskData.taskRuning[i].AgvNum, 10, 0x11);
                        }
                    }
                }
                #endregion

                #region 更新AGV状态
                //AGV1
                TaskData.AGV1.Connect = false;
                TaskData.AGV1.state = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 1, 0];
                TaskData.AGV1.Process = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 2, 0];
                TaskData.AGV1.Route = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 3, 0];
                TaskData.AGV1.Station = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 4, 0];
                TaskData.AGV1.Power = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 5, 0];
                TaskData.AGV1.Speed = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 6, 0];
                TaskData.AGV1.error = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 7, 0];

                //AGV2
                TaskData.AGV2.Connect = false;
                TaskData.AGV2.state = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 1, 0];
                TaskData.AGV2.Process = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 2, 0];
                TaskData.AGV2.Route = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 3, 0];
                TaskData.AGV2.Station = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 4, 0];
                TaskData.AGV2.Power = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 5, 0];
                TaskData.AGV2.Speed = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 6, 0];
                TaskData.AGV2.error = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 7, 0];

                #endregion

                #region 更新任务界面

                #endregion
                Thread.Sleep(TaskData.Parameter.taskFuncTime);
            }
        }


        /// <summary>
        /// 同步Modbus设备数据
        /// </summary>
        private void SyncModbus()
        {
            while (true)
            {
                Thread.Sleep(TaskData.Parameter.SyncModbusTime);
                try
                {
                    //读取Modbus寄存器
                    TaskData.Modbus = modbusMaster.ReadHoldingRegisters(0, (ushort)TaskData.Modbus.Length);

                    //Random rd = new Random();
                    //ushort[] write = new ushort[10];
                    //for (int i = 0; i < 10; i++)
                    //{
                    //    write[i] = (ushort)rd.Next(1, 1000);
                    //}
                    //modbusMaster.WriteMultipleRegisters(0, write);
                    //ushort[] read = new ushort[10];
                    //// read = master.ReadWriteMultipleRegisters(0, 10, 0, write);

                    //read = modbusMaster.ReadHoldingRegisters(10, 10);
                    //UdpSever.Shell.WriteNotice("debug", "{0},{1}", read[0], read[1]);
                }
                catch (Exception ex)
                {
                    UdpSever.Shell.WriteError("debug", ex.ToString());
                }
            }

        }

        private void TaskForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
