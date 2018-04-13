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
            //dataGridViewRunning.Columns.Add("1", "任务编号");
            //var index = dataGridViewRunning.Columns.Add("2", "MES下发时间");
            //dataGridViewRunning.Columns[index].Width = 130;
            //dataGridViewRunning.Columns.Add("3", "任务状态");
            //dataGridViewRunning.Columns.Add("4", "AGV编号");
            //dataGridViewRunning.Columns.Add("5", "通信状态");
            //dataGridViewRunning.Columns.Add("6", "当前路径");
            //dataGridViewRunning.Columns.Add("7", "当前站点");
            //dataGridViewRunning.Columns.Add("8", "电量%");
            //dataGridViewRunning.Columns.Add("9", "速度");
            //dataGridViewRunning.Columns.Add("10", "运行状态");
            //dataGridViewRunning.Columns.Add("11", "报警信息");

            //增加行
            //for (int i = 0; i < 10; i++)
            //{
            //    Task task = new Task();
            //    task.TaskNum = i;
            //    task.CmdTime = DateTime.Now;
            //    task.TaskState = (int)TaskRunState.Runing;
            //    task.AgvNum = 1;
            //    task.Connect = true;
            //    task.Route = 2;
            //    task.Station = 2;
            //    task.Power = 2;
            //    task.Speed = 2;
            //    task.AgvState = 2;
            //    task.Error = 2;
            //    TaskData.taskWaiting.Add(task);
            //}

            //foreach (var item in TaskData.taskWaiting)
            //{
            //    index = this.dataGridView1.Rows.Add();
            //    this.dataGridView1.Rows[index].Cells[0].Value = item.TaskNum;
            //    this.dataGridView1.Rows[index].Cells[1].Value = item.CmdTime.ToString("yyyy-MM-dd HH:mm:ss");
            //    this.dataGridView1.Rows[index].Cells[2].Value = item.TaskState;
            //    this.dataGridView1.Rows[index].Cells[3].Value = item.AgvNum;
            //    this.dataGridView1.Rows[index].Cells[4].Value = item.Connect;
            //    this.dataGridView1.Rows[index].Cells[5].Value = item.Route;
            //    this.dataGridView1.Rows[index].Cells[6].Value = item.Station;
            //    this.dataGridView1.Rows[index].Cells[7].Value = item.Power;
            //    this.dataGridView1.Rows[index].Cells[8].Value = item.Speed;
            //    this.dataGridView1.Rows[index].Cells[9].Value = item.AgvState;
            //    this.dataGridView1.Rows[index].Cells[10].Value = item.Error;
            //}
            #endregion
        }


        /// <summary>
        /// 任务队列
        /// </summary>
        public class Task
        {
            public long OrderNum = 0;//订单号
            public int TaskNum; //任务编号
            public int Level; //任务优先级
            public int LineNum;//产线编号
            public int TaskState = (int)TaskRunState.Waiting; //任务状态
            public DateTime CreatTime = DateTime.Now;//任务下发时间
            public DateTime StartTime;//任务开始时间
            public DateTime StopTime;//任务结束时间

            public int AgvNum = 0; //AGV编号
            public bool Connect = true; //通信状态
            public int Route = 0;//当前路径
            public int StationNow = 0;//当前站点
            public int StationNext = 0;//下一个站点
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
                public static int ModbusCount = 10;
                //modbus 检测时间
                public static int SyncModbusTime = 500;
                //任务调度 检测时间
                public static int taskFuncTime = 500;
                //modbus服务器IP地址
                public static string ModbusTcpSeverIPAddress = "192.168.250.102";
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
                Ready = 0x11,
                Busying = 0x12,
                Error = 0x13,
                Outline = 0x14
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
                if (TaskData.Modbus[(int)TaskData.RegBind.newTask] == 0x11)
                {
                    //加入任务队列
                    Task task = new Task();

                    task.OrderNum = GetTimeStamp();//[订单号]毫秒时间戳
                    task.TaskNum = TaskData.Modbus[(int)TaskData.RegBind.taskNum];//[任务编号]
                    task.Level = TaskData.Modbus[(int)TaskData.RegBind.taskLevel];//[任务优先级]
                    task.LineNum = TaskData.Modbus[(int)TaskData.RegBind.lineNum];//[产线编号]

                    TaskData.taskWaiting.Add(task);
                    //按优先级排序(降序)
                    TaskData.taskWaiting = TaskData.taskWaiting.OrderByDescending(s => s.Level).ToList();
                    //按下单时间排序(升序)
                    //TaskData.taskWaiting = TaskData.taskWaiting.OrderBy(s => s.Level).ToList();  

                    this.Invoke(new MethodInvoker(delegate
                    {
                        //更新到界面
                        var index = dataGridViewWaiting.Rows.Add();
                        //序号
                        dataGridViewWaiting.Rows[index].Cells[0].Value = index;
                        //订单号
                        dataGridViewWaiting.Rows[index].Cells[1].Value = task.OrderNum;
                        //任务编号
                        dataGridViewWaiting.Rows[index].Cells[2].Value = task.TaskNum;
                        //产线编号
                        dataGridViewWaiting.Rows[index].Cells[3].Value = task.LineNum;
                        //下单时间
                        dataGridViewWaiting.Rows[index].Cells[4].Value = task.CreatTime.ToString("yyyy-MM-dd HH:mm:ss fff");
                        //任务优先级
                        dataGridViewWaiting.Rows[index].Cells[5].Value = task.Level;
                    }));

                    //清除MES任务标志寄存器
                    modbusMaster.WriteSingleRegister((int)TaskData.RegBind.newTask, 0); 
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

                                UdpSever.Write_Multiple_Registers(1, 8, 2, temp); //下发到AGV

                                //更新任务状态
                                TaskData.taskWaiting[i].StartTime = DateTime.Now;//任务启动时间
                                TaskData.taskWaiting[i].AgvNum = TaskData.AGV1.AgvNum;//执行该任务的AGV编号

                                //将该任务转至正在进行任务列表
                                TaskData.taskRuning.Add(TaskData.taskWaiting[i]);

                                #region 更新-正在执行-到界面
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    var index = dataGridViewRunning.Rows.Add();
                                    //序号
                                    dataGridViewRunning.Rows[index].Cells[0].Value = index;
                                    //订单编号
                                    dataGridViewRunning.Rows[index].Cells[1].Value = TaskData.taskWaiting[i].OrderNum;
                                    //任务编号
                                    dataGridViewRunning.Rows[index].Cells[2].Value = TaskData.taskWaiting[i].TaskNum;
                                    //产线编号
                                    dataGridViewRunning.Rows[index].Cells[3].Value = TaskData.taskWaiting[i].LineNum;
                                    //AGV编号
                                    dataGridViewRunning.Rows[index].Cells[4].Value = TaskData.taskWaiting[i].AgvNum;
                                    //下单时间
                                    dataGridViewRunning.Rows[index].Cells[5].Value = TaskData.taskWaiting[i].CreatTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //启动时间
                                    dataGridViewRunning.Rows[index].Cells[6].Value = TaskData.taskWaiting[i].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //任务优先级
                                    dataGridViewRunning.Rows[index].Cells[7].Value = TaskData.taskWaiting[i].Level;
                                    //当前站点
                                    dataGridViewRunning.Rows[index].Cells[8].Value = TaskData.taskWaiting[i].StationNow;
                                    //下一个站点
                                    dataGridViewRunning.Rows[index].Cells[9].Value = TaskData.taskWaiting[i].StationNext;
                                    //报警信息
                                    dataGridViewRunning.Rows[index].Cells[10].Value = TaskData.taskWaiting[i].Error;
                                }));
                                #endregion


                                //从等待执行任务列表删除
                                TaskData.taskWaiting.RemoveAt(i);

                                #region 更新-等待执行-界面
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    dataGridViewWaiting.Rows.RemoveAt(i);
                                }));
                                #endregion

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

                                #region 更新-正在执行-到界面
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    var index = dataGridViewRunning.Rows.Add();
                                    //序号
                                    dataGridViewRunning.Rows[index].Cells[0].Value = index;
                                    //订单编号
                                    dataGridViewRunning.Rows[index].Cells[1].Value = TaskData.taskWaiting[i].OrderNum;
                                    //任务编号
                                    dataGridViewRunning.Rows[index].Cells[2].Value = TaskData.taskWaiting[i].TaskNum;
                                    //产线编号
                                    dataGridViewRunning.Rows[index].Cells[3].Value = TaskData.taskWaiting[i].LineNum;
                                    //AGV编号
                                    dataGridViewRunning.Rows[index].Cells[4].Value = TaskData.taskWaiting[i].AgvNum;
                                    //下单时间
                                    dataGridViewRunning.Rows[index].Cells[5].Value = TaskData.taskWaiting[i].CreatTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //启动时间
                                    dataGridViewRunning.Rows[index].Cells[6].Value = TaskData.taskWaiting[i].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //任务优先级
                                    dataGridViewRunning.Rows[index].Cells[7].Value = TaskData.taskWaiting[i].Level;
                                    //当前站点
                                    dataGridViewRunning.Rows[index].Cells[8].Value = TaskData.taskWaiting[i].StationNow;
                                    //下一个站点
                                    dataGridViewRunning.Rows[index].Cells[9].Value = TaskData.taskWaiting[i].StationNext;
                                    //报警信息
                                    dataGridViewRunning.Rows[index].Cells[10].Value = TaskData.taskWaiting[i].Error;
                                }));
                                #endregion

                                //从等待执行任务列表删除
                                TaskData.taskWaiting.RemoveAt(i);

                                #region 更新-等待执行-界面
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    dataGridViewWaiting.Rows.RemoveAt(i);
                                }));
                                #endregion
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
                        if (UdpSever.Register[TaskData.taskRuning[i].AgvNum, 7, 0] == 0x10)
                        {
                            //执行完成
                            TaskData.taskRuning[i].TaskState = (int)TaskRunState.Finished;
                            TaskData.taskRuning[i].StopTime = DateTime.Now;

                            //将该任务转至执行完成任务列表
                            TaskData.taskFinished.Add(TaskData.taskRuning[i]);
                            #region 更新-完成任务-到界面
                            this.Invoke(new MethodInvoker(delegate
                            {
                                var index = dataGridViewFinished.Rows.Add();
                                //序号
                                dataGridViewFinished.Rows[index].Cells[0].Value = index;
                                //订单编号
                                dataGridViewFinished.Rows[index].Cells[1].Value = TaskData.taskRuning[i].OrderNum;
                                //任务编号
                                dataGridViewFinished.Rows[index].Cells[2].Value = TaskData.taskRuning[i].TaskNum;
                                //产线编号
                                dataGridViewFinished.Rows[index].Cells[3].Value = TaskData.taskRuning[i].LineNum;
                                //AGV编号
                                dataGridViewFinished.Rows[index].Cells[4].Value = TaskData.taskRuning[i].AgvNum;
                                //下单时间
                                dataGridViewFinished.Rows[index].Cells[5].Value = TaskData.taskRuning[i].CreatTime.ToString("yyyy-MM-dd HH:mm:ss");
                                //启动时间
                                dataGridViewFinished.Rows[index].Cells[6].Value = TaskData.taskRuning[i].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                                //完成时间
                                dataGridViewFinished.Rows[index].Cells[7].Value = TaskData.taskRuning[i].StopTime.ToString("yyyy-MM-dd HH:mm:ss");
                                //执行时间
                                dataGridViewFinished.Rows[index].Cells[8].Value = (TaskData.taskRuning[i].StopTime - TaskData.taskRuning[i].StartTime).ToString("HH:mm:ss");
                            }));
                            #endregion

                            //将该任务从正在执行任务列表删除
                            TaskData.taskRuning.RemoveAt(i);

                            #region 更新-正在执行-界面
                            this.Invoke(new MethodInvoker(delegate
                            {
                                dataGridViewRunning.Rows.RemoveAt(i);
                            }));
                            #endregion

                        }
                        else if (UdpSever.Register[TaskData.taskRuning[i].AgvNum, 10, 0] == 0x12)
                        {
                            //正在执行
                            TaskData.taskRuning[i].TaskState = (int)TaskRunState.Runing;
                        }
                    }
                }
                #endregion

                #region 更新AGV状态
                //AGV1
                TaskData.AGV1.Connect = false;
                TaskData.AGV1.state = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 0, 0];
                TaskData.AGV1.Process = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 1, 0];
                TaskData.AGV1.Route = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 2, 0];
                TaskData.AGV1.Station = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 3, 0];
                TaskData.AGV1.Power = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 4, 0];
                TaskData.AGV1.Speed = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 5, 0];
                TaskData.AGV1.error = (ushort)UdpSever.Register[TaskData.AGV1.AgvNum, 6, 0];

                //AGV2
                TaskData.AGV2.Connect = false;
                TaskData.AGV2.state = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 0, 0];
                TaskData.AGV2.Process = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 1, 0];
                TaskData.AGV2.Route = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 2, 0];
                TaskData.AGV2.Station = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 3, 0];
                TaskData.AGV2.Power = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 4, 0];
                TaskData.AGV2.Speed = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 5, 0];
                TaskData.AGV2.error = (ushort)UdpSever.Register[TaskData.AGV2.AgvNum, 6, 0];

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

        private long GetTimeStamp()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
            //System.Console.WriteLine(timeStamp);
            return timeStamp;
        }

        private void TaskForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
