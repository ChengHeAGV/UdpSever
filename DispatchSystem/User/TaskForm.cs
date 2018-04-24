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


        /// <summary>
        /// 任务状态
        /// </summary>
        public enum TaskRunState
        {
            Waiting, //等待
            Runing,  //运行中
            Finished //完成
        }


        Thread taskThread;
        private void TaskForm_Load(object sender, EventArgs e)
        {
            TaskData.AGV[0] = new Agv();
            TaskData.AGV[1] = new Agv();

            //启动数据监听
            DataTransmission.StartListen();

            #region 启动任务调度
            taskThread = new Thread(new ThreadStart(taskFunc));
            taskThread.Start();
            #endregion

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
            public string LineName;//产线名称
            public int TaskState = (int)TaskRunState.Waiting; //任务状态
            public DateTime CreatTime = DateTime.Now;//任务下发时间
            public DateTime StartTime;//任务开始时间
            public DateTime StopTime;//任务结束时间

            public int AgvNum = 0; //AGV编号
            public bool Connect = true; //通信状态
            public int Route = 0;//当前路径
            public int LastPosition = 0;//当前站点
            public int NowPosition = 0;//当前站点
            public int NextPosition = 0;//下一个站点
            public int Power = 0;//电量
            public int Speed = 0;//速度
            public int State = 0; //Agv状态
            public int Error = 0;//报警信息
        }

        public static class TaskData
        {
            public static class Parameter
            {
                //modbus 检测时间
                public static int SyncModbusTime = 500;
                //任务调度 检测时间
                public static int taskFuncTime = 1000;
            }

            /// <summary>
            /// 等待任务队列
            /// </summary>
            public static List<Task> Waiting = new List<Task>();

            /// <summary>
            /// 进行中任务队列
            /// </summary>
            public static List<Task> Runing = new List<Task>();

            /// <summary>
            ///已完成任务队列
            /// </summary>
            public static List<Task> Finished = new List<Task>();

            //定义两台AGV
            public static Agv[] AGV = new Agv[2];

        }

        /// <summary>
        /// AGV类型
        /// </summary>
        public class Agv
        {
            /// <summary>
            /// AGV运行状态
            /// </summary>
            public string RunState = "";
            /// <summary>
            /// AGV编号
            /// </summary>
            public int AgvNum = 0;
            /// <summary>
            /// 通信状态
            /// </summary>
            public string Connect;
            /// <summary>
            /// 当前路径
            /// </summary>
            public int Route = 0;
            /// <summary>
            /// 上一个位置
            /// </summary>
            public int LastPostion = 0;
            /// <summary>
            /// 当前位置
            /// </summary>
            public int NowPostion = 0;
            /// <summary>
            /// 下一个位置
            /// </summary>
            public int NextPostion = 0;
            /// <summary>
            /// 电量
            /// </summary>
            public int Power = 0;
            /// <summary>
            /// 速度
            /// </summary>
            public string Speed = "";
            /// <summary>
            /// 机械碰撞
            /// </summary>
            public bool MechanicalError = true;
            /// <summary>
            /// 出轨
            /// </summary>
            public bool OutLine = true;
            /// <summary>
            /// 红外避障
            /// </summary>
            public bool Infrared = true;
            /// <summary>
            /// 电量低
            /// </summary>
            public bool LowPower = true;
            /// <summary>
            /// 电机故障
            /// </summary>
            public bool MotorError = true;
            /// <summary>
            /// 急停
            /// </summary>
            public bool Emergency = true;
        }

        /// <summary>
        /// 任务方法
        /// </summary>
        private void taskFunc()
        {
            while (true)
            {
                if (DataTransmission.ListenState.ModbusTcp == false)
                {
                    DataTransmission.StartListen();
                }
                else
                {

                    #region 判断MES是否有新任务
                    //扩散线
                    if (DataTransmission.Profinet.Register[0] > 0)
                    {
                        //创建任务
                        Task task = new Task();
                        //[订单号]毫秒时间戳
                        task.OrderNum = GetTimeStamp();
                        //[任务编号]
                        task.TaskNum = DataTransmission.Profinet.Register[0];
                        //[产线名称]
                        task.LineName = "扩散线";
                        //AGV编号
                        task.AgvNum = 1;

                        //添加任务到待执行列表
                        TaskData.Waiting.Add(task);

                        //按优先级排序(降序)
                        // TaskData.taskWaiting = TaskData.taskWaiting.OrderByDescending(s => s.Level).ToList();
                        //按下单时间排序(升序)
                        //TaskData.taskWaiting = TaskData.taskWaiting.OrderBy(s => s.Level).ToList();  


                        //更新到界面
                        this.Invoke(new MethodInvoker(delegate
                        {
                            var index = dataGridViewWaiting.Rows.Add();
                        //序号
                        dataGridViewWaiting.Rows[index].Cells[0].Value = index;
                        //订单号
                        dataGridViewWaiting.Rows[index].Cells[1].Value = task.OrderNum;
                        //任务编号
                        dataGridViewWaiting.Rows[index].Cells[2].Value = task.TaskNum;
                        //产线名称
                        dataGridViewWaiting.Rows[index].Cells[3].Value = task.LineName;
                        //下单时间
                        dataGridViewWaiting.Rows[index].Cells[4].Value = task.CreatTime.ToString("yyyy-MM-dd HH:mm:ss fff");
                        }));
                        //清除MES任务标志寄存器
                        //DataTransmission.Profinet.Register[0] = 0;
                        //modbusMaster.WriteSingleRegister((int)TaskData.RegBind.newTask, 0); 
                    }
                    //PE线
                    if (DataTransmission.Profinet.Register[20] > 0)
                    {
                        //创建任务
                        Task task = new Task();
                        //[订单号]毫秒时间戳
                        task.OrderNum = GetTimeStamp();
                        //[任务编号]
                        task.TaskNum = DataTransmission.Profinet.Register[0];
                        //[产线名称]
                        task.LineName = "PE线";
                        //AGV编号
                        task.AgvNum = 2;

                        //添加任务到待执行列表
                        TaskData.Waiting.Add(task);

                        //按优先级排序(降序)
                        // TaskData.taskWaiting = TaskData.taskWaiting.OrderByDescending(s => s.Level).ToList();
                        //按下单时间排序(升序)
                        //TaskData.taskWaiting = TaskData.taskWaiting.OrderBy(s => s.Level).ToList();  


                        //更新到界面
                        this.Invoke(new MethodInvoker(delegate
                        {
                            var index = dataGridViewWaiting.Rows.Add();
                        //序号
                        dataGridViewWaiting.Rows[index].Cells[0].Value = index;
                        //订单号
                        dataGridViewWaiting.Rows[index].Cells[1].Value = task.OrderNum;
                        //任务编号
                        dataGridViewWaiting.Rows[index].Cells[2].Value = task.TaskNum;
                        //产线名称
                        dataGridViewWaiting.Rows[index].Cells[3].Value = task.LineName;
                        //下单时间
                        dataGridViewWaiting.Rows[index].Cells[4].Value = task.CreatTime.ToString("yyyy-MM-dd HH:mm:ss fff");
                        }));

                        //清除MES任务标志寄存器
                        //DataTransmission.Profinet.Register[0] = 0;
                        //modbusMaster.WriteSingleRegister((int)TaskData.RegBind.newTask, 0); 
                    }
                    #endregion

                    #region 派发任务到AGV
                    //有需要完成的任务
                    if (TaskData.Waiting.Count > 0)
                    {
                        //1号产线有空闲的AGV
                        if (UdpSever.Register[1, 8, 0] == 2)
                        {
                            for (int i = 0; i < TaskData.Waiting.Count; i++)
                            {
                                if (TaskData.Waiting[i].LineName == "扩散线")
                                {
                                    //下发任务到AGV
                                    UdpSever.Register[TaskData.Waiting[i].AgvNum, 1, 0] = TaskData.Waiting[i].TaskNum;

                                    //更新任务状态
                                    TaskData.Waiting[i].StartTime = DateTime.Now;//任务启动时间

                                    //将该任务转至正在进行任务列表
                                    TaskData.Runing.Add(TaskData.Waiting[i]);

                                    #region 更新-正在执行-到界面
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        var index = dataGridViewRunning.Rows.Add();
                                    //序号
                                    dataGridViewRunning.Rows[index].Cells[0].Value = index;
                                    //订单编号
                                    dataGridViewRunning.Rows[index].Cells[1].Value = TaskData.Waiting[i].OrderNum;
                                    //任务编号
                                    dataGridViewRunning.Rows[index].Cells[2].Value = TaskData.Waiting[i].TaskNum;
                                    //产线编号
                                    dataGridViewRunning.Rows[index].Cells[3].Value = TaskData.Waiting[i].LineName;
                                    //AGV编号
                                    dataGridViewRunning.Rows[index].Cells[4].Value = TaskData.Waiting[i].AgvNum;
                                    //下单时间
                                    dataGridViewRunning.Rows[index].Cells[5].Value = TaskData.Waiting[i].CreatTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //启动时间
                                    dataGridViewRunning.Rows[index].Cells[6].Value = TaskData.Waiting[i].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //当前站点
                                    dataGridViewRunning.Rows[index].Cells[8].Value = TaskData.Waiting[i].NowPosition;
                                    //下一个站点
                                    dataGridViewRunning.Rows[index].Cells[9].Value = TaskData.Waiting[i].NextPosition;
                                    //报警信息
                                    dataGridViewRunning.Rows[index].Cells[10].Value = TaskData.Waiting[i].Error;
                                    }));
                                    #endregion

                                    //从等待执行任务列表删除
                                    TaskData.Waiting.RemoveAt(i);
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
                        if (UdpSever.Register[2, 8, 0] == 2)
                        {
                            for (int i = 0; i < TaskData.Waiting.Count; i++)
                            {
                                if (TaskData.Waiting[i].LineName == "PE线")
                                {
                                    //下发任务到AGV
                                    UdpSever.Register[TaskData.Waiting[i].AgvNum, 1, 0] = TaskData.Waiting[i].TaskNum;

                                    //更新任务状态
                                    TaskData.Waiting[i].StartTime = DateTime.Now;//任务启动时间

                                    //将该任务转至正在进行任务列表
                                    TaskData.Runing.Add(TaskData.Waiting[i]);

                                    #region 更新-正在执行-到界面
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        var index = dataGridViewRunning.Rows.Add();
                                    //序号
                                    dataGridViewRunning.Rows[index].Cells[0].Value = index;
                                    //订单编号
                                    dataGridViewRunning.Rows[index].Cells[1].Value = TaskData.Waiting[i].OrderNum;
                                    //任务编号
                                    dataGridViewRunning.Rows[index].Cells[2].Value = TaskData.Waiting[i].TaskNum;
                                    //产线编号
                                    dataGridViewRunning.Rows[index].Cells[3].Value = TaskData.Waiting[i].LineName;
                                    //AGV编号
                                    dataGridViewRunning.Rows[index].Cells[4].Value = TaskData.Waiting[i].AgvNum;
                                    //下单时间
                                    dataGridViewRunning.Rows[index].Cells[5].Value = TaskData.Waiting[i].CreatTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //启动时间
                                    dataGridViewRunning.Rows[index].Cells[6].Value = TaskData.Waiting[i].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //当前站点
                                    dataGridViewRunning.Rows[index].Cells[8].Value = TaskData.Waiting[i].NowPosition;
                                    //下一个站点
                                    dataGridViewRunning.Rows[index].Cells[9].Value = TaskData.Waiting[i].NextPosition;
                                    //报警信息
                                    dataGridViewRunning.Rows[index].Cells[10].Value = TaskData.Waiting[i].Error;
                                    }));
                                    #endregion

                                    //从等待执行任务列表删除
                                    TaskData.Waiting.RemoveAt(i);
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
                    if (TaskData.Runing.Count > 0)
                    {
                        for (int i = 0; i < TaskData.Runing.Count; i++)
                        {
                            //判断任务是否执行完成
                            if (TaskData.Runing[i].LineName == "扩散线")
                            {
                                if (UdpSever.Register[TaskData.Runing[i].AgvNum, 8, 0] == 2)
                                {
                                    //执行完成
                                    TaskData.Runing[i].TaskState = (int)TaskRunState.Finished;
                                    TaskData.Runing[i].StopTime = DateTime.Now;

                                    //将该任务转至执行完成任务列表
                                    TaskData.Finished.Add(TaskData.Runing[i]);
                                    #region 更新-完成任务-到界面
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        var index = dataGridViewFinished.Rows.Add();
                                    //序号
                                    dataGridViewFinished.Rows[index].Cells[0].Value = index;
                                    //订单编号
                                    dataGridViewFinished.Rows[index].Cells[1].Value = TaskData.Runing[i].OrderNum;
                                    //任务编号
                                    dataGridViewFinished.Rows[index].Cells[2].Value = TaskData.Runing[i].TaskNum;
                                    //产线名称
                                    dataGridViewFinished.Rows[index].Cells[3].Value = TaskData.Runing[i].LineName;
                                    //AGV编号
                                    dataGridViewFinished.Rows[index].Cells[4].Value = TaskData.Runing[i].AgvNum;
                                    //下单时间
                                    dataGridViewFinished.Rows[index].Cells[5].Value = TaskData.Runing[i].CreatTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //启动时间
                                    dataGridViewFinished.Rows[index].Cells[6].Value = TaskData.Runing[i].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //完成时间
                                    dataGridViewFinished.Rows[index].Cells[7].Value = TaskData.Runing[i].StopTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    //执行时间
                                    dataGridViewFinished.Rows[index].Cells[8].Value = (TaskData.Runing[i].StopTime - TaskData.Runing[i].StartTime).ToString("HH:mm:ss");
                                    }));
                                    #endregion

                                    //将该任务从正在执行任务列表删除
                                    TaskData.Runing.RemoveAt(i);

                                    #region 更新-正在执行-界面
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        dataGridViewRunning.Rows.RemoveAt(i);
                                    }));
                                    #endregion
                                }
                                else if (UdpSever.Register[TaskData.Runing[i].AgvNum, 8, 0] == 1)
                                {
                                    //正在执行
                                    TaskData.Runing[i].TaskState = (int)TaskRunState.Runing;
                                }
                            }
                        }
                    }
                    #endregion

                    #region 更新AGV状态
                    for (int i = 0; i < 2; i++)
                    {
                        //连接状态
                        // TaskData.AGV[i].Connect = UdpSever.EndPointArray[i] == null ? "离线" : "正常";
                        //运行状态
                        switch (UdpSever.Register[i, 0, 0])
                        {
                            case 0:
                                TaskData.AGV[i].RunState = "未就绪";
                                break;
                            case 1:
                                TaskData.AGV[i].RunState = "执行任务";
                                break;
                            case 2:
                                TaskData.AGV[i].RunState = "待命";
                                break;
                            case 3:
                                TaskData.AGV[i].RunState = "充电";
                                break;
                            default:
                                break;
                        }
                        //当前路径
                        TaskData.AGV[i].Route = (ushort)UdpSever.Register[i, 1, 0];
                        //上一个位置
                        TaskData.AGV[i].LastPostion = (ushort)UdpSever.Register[i, 5, 0];
                        //当前位置
                        TaskData.AGV[i].LastPostion = (ushort)UdpSever.Register[i, 6, 0];
                        //下一个位置
                        TaskData.AGV[i].LastPostion = (ushort)UdpSever.Register[i, 7, 0];
                        //电量
                        TaskData.AGV[i].Power = (ushort)UdpSever.Register[i, 51, 0];
                        //档位
                        switch (UdpSever.Register[i, 55, 0])
                        {
                            case 0:
                                TaskData.AGV[i].Speed = "高速";
                                break;
                            case 1:
                                TaskData.AGV[i].Speed = "中速";
                                break;
                            case 2:
                                TaskData.AGV[i].Speed = "低速";
                                break;
                            case 3:
                                TaskData.AGV[i].Speed = "对接速度";
                                break;
                        }
                    }
                    #endregion

                    #region 更新MES状态
                    //正在执行
                    if (TaskData.Runing.Count > 0)
                    {
                        foreach (var item in TaskData.Runing)
                        {
                            if (item.LineName == "扩散线")
                            {
                                //正在执行任务
                                DataTransmission.Profinet.Register[1] = (ushort)item.TaskNum;
                            }
                            else
                            if (item.LineName == "PE线")
                            {
                                //正在执行任务
                                DataTransmission.Profinet.Register[21] = (ushort)item.TaskNum;
                            }
                        }
                    }
                    //待执行
                    int[] waiting = new int[2];
                    if (TaskData.Waiting.Count > 0)
                    {
                        foreach (var item in TaskData.Waiting)
                        {
                            if (item.LineName == "扩散线" && waiting[0] < 5)
                            {
                                //待执行任务
                                DataTransmission.Profinet.Register[2 + waiting[0]++] = (ushort)item.TaskNum;
                            }
                            else
                            if (item.LineName == "PE线" && waiting[1] < 5)
                            {
                                //待执行任务
                                DataTransmission.Profinet.Register[22 + waiting[1]++] = (ushort)item.TaskNum;
                            }
                        }
                    }
                    //已完成
                    int[] finished = new int[2];
                    if (TaskData.Finished.Count > 0)
                    {
                        foreach (var item in TaskData.Finished)
                        {
                            if (item.LineName == "扩散线" && finished[0] < 5)
                            {
                                //已完成任务
                                DataTransmission.Profinet.Register[7 + finished[0]++] = (ushort)item.TaskNum;
                            }
                            else
                            if (item.LineName == "PE线" && finished[1] < 5)
                            {
                                //已完成任务
                                DataTransmission.Profinet.Register[27 + finished[1]++] = (ushort)item.TaskNum;
                            }
                        }
                    }
                    #endregion

                }

                Thread.Sleep(TaskData.Parameter.taskFuncTime);
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
            taskThread.Abort();
            DataTransmission.StopListen();
        }
    }
}
