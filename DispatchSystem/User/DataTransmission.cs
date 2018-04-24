using DispatchSystem.Developer;
using Modbus.Device;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace DispatchSystem.User
{
    public partial class DataTransmission : Form
    {
        static ModbusIpMaster modbusMaster;
        static Thread modbusThread;
        static Thread dbusThread;
        static Thread MainThread;
        public DataTransmission()
        {
            InitializeComponent();
        }

        private void DataTransmission_Load(object sender, EventArgs e)
        {

        }

        public static void StartListen()
        {
            MainThread = new Thread(new ThreadStart(Start));
            MainThread.Start();
        }
        public static class ListenState
        {
            public static bool ModbusTcp = false;
            public static bool Dbus = false;
        }

        private static void Start()
        {

            #region 启动Dbus
            //启动监听进程
            if (ListenState.Dbus == false)
            {
                dbusThread = new Thread(new ThreadStart(Syncdbus));
                dbusThread.Start();
                ConsoleLog.WriteLog("监听Dbus启动成功！", Color.Green, 24);
                ListenState.Dbus = true;
            }
            #endregion

            #region 启动ModbusTcp
            if (ListenState.ModbusTcp == false)
            {
                try
                {
                    TcpClient tcpClient = new TcpClient(Profinet.ModbusTcpSeverIPAddress, Profinet.ModbusTcpSeverPort);
                    modbusMaster = ModbusIpMaster.CreateIp(tcpClient);
                    modbusMaster.Transport.WriteTimeout = Profinet.Timeout;//写超时
                    modbusMaster.Transport.ReadTimeout = Profinet.Timeout;//读超时
                    modbusMaster.Transport.WaitToRetryMilliseconds = Profinet.WaitToRetryTime;//重试等待时间
                    modbusMaster.Transport.Retries = Profinet.RetryNum;//重试次数

                    //启动监听进程
                    modbusThread = new Thread(new ThreadStart(SyncModbus));
                    modbusThread.Start();
                    ConsoleLog.WriteLog("监听ModbusTCP启动", Color.Orange, 24);
                    ListenState.ModbusTcp = true;
                }
                catch
                {
                    ConsoleLog.WriteLog("监听ModbusTCP失败！", Color.Red, 24);
                }
            }

            #endregion

        }

        public static void StopListen()
        {
            if (modbusThread.IsAlive)
            {
                modbusMaster.Dispose();
                modbusThread.Abort();
            }
            if (dbusThread.IsAlive)
            {
                dbusThread.Abort();
            }
        }

        /// <summary>
        /// Modbus数据
        /// </summary>
        public static class Profinet
        {
            //modbus 检测时间
            public static int Cycle = 1000;
            //modbus服务器IP地址
            public static string ModbusTcpSeverIPAddress = "192.168.10.106";
            //modbus服务器端口
            public static int ModbusTcpSeverPort = 502;
            //modbus超时
            public static int Timeout = 100;
            //modbus重试等待时间
            public static int WaitToRetryTime = 10;
            //modbus重试次数
            public static int RetryNum = 3;
            //Profinet空间对应数据
            public static UInt16[] Register = new UInt16[200];
            //待发送数据临时数组，用于检测数据是否有变化
            public static UInt16[] RegisterCompare = new UInt16[200];

            //设置数据
            public static void SetRegister(int start, int end)
            {
                ushort[] temp;
                string Msg = string.Format("设置:{0,-2}-{1,-2}", start, end);
                try
                {
                    //检测待写入数据和上次写入是否有变化
                    for (int i = start; i <= end; i++)
                    {
                        if (Register[i] != RegisterCompare[i])
                        {
                            temp = new ushort[end - start + 1];
                            for (int j = start; j <= end; j++)
                            {
                                temp[j] = Register[j];
                            }
                            modbusMaster.WriteMultipleRegisters((ushort)start, temp);
                            //发送成功，更新比较数组
                            for (int j = start; j <= end; j++)
                            {
                                RegisterCompare[j] = Register[j];
                            }
                            break;
                        }
                    }
                }
                catch
                {
                    ConsoleLog.WriteLog(string.Format("操作失败!:[{0}]", Msg), Color.Red, 20);
                }

            }

            //获取数据
            public static void GetRegister(int start, int end)
            {
                ushort[] temp;
                string Msg = string.Format("获取:{0,-2}-{1,-2}", start, end);
                try
                {
                    temp = modbusMaster.ReadHoldingRegisters((ushort)start, (ushort)(end - start + 1));
                    foreach (var item in temp)
                    {
                        Register[start] = temp[start++];
                    }
                }
                catch
                {
                    ConsoleLog.WriteLog(string.Format("操作失败!:[{0}]", Msg), Color.Red, 20);
                }
            }
        }

        /// <summary>
        /// Dbus数据
        /// </summary>
        public static class Dbus
        {
            //modbus 检测时间
            public static int Cycle = 1000;

            //待发送数据临时数组，用于检测数据是否有变化

            public static UInt16[,] DbusCompare = new UInt16[UdpSever.DeviceNum, UdpSever.RegisterNum];

            //设置多个数据
            public static void SetRegister(int deviceAddress, int start, int end)
            {
                ushort[] temp;
                string Msg = string.Format("设置:{0,-2}-{1,-2}", start, end);
                //检测待写入数据和上次写入是否有变化
                for (int i = start; i <= end; i++)
                {
                    if (UdpSever.Register[deviceAddress, i, 0] != DbusCompare[deviceAddress, i])
                    {
                        temp = new ushort[end - start + 1];
                        for (int j = start; j <= end; j++)
                        {
                            temp[j] = (ushort)UdpSever.Register[deviceAddress, j, 0];
                        }

                        UdpSever.ReturnMsg mg = UdpSever.Write_Multiple_Registers(deviceAddress, start, end - start + 1, temp);

                        //发送成功，更新比较数组
                        if (mg.resault)
                        {
                            for (int j = start; j <= end; j++)
                            {
                                DbusCompare[deviceAddress, j] = temp[j];
                            }
                        }
                        else
                            ConsoleLog.WriteLog(string.Format("Dbus操作失败!:[{0}]", Msg), Color.Red, 20);
                        break;
                    }
                }
            }
            //设置单个数据
            public static void SetRegister(int deviceAddress, int start)
            {
                string Msg = string.Format("设置:{0,-2}", start);

                if (UdpSever.Register[deviceAddress, start, 0] != DbusCompare[deviceAddress, start])
                {
                    UdpSever.ReturnMsg mg = UdpSever.Write_Register(deviceAddress, start, (ushort)UdpSever.Register[deviceAddress, start, 0]);

                    //发送成功，更新比较数组
                    if (mg.resault)
                    {
                        DbusCompare[deviceAddress, start] = (ushort)UdpSever.Register[deviceAddress, start, 0];
                    }
                    else
                        ConsoleLog.WriteLog(string.Format("Dbus操作失败!:[{0}]", Msg), Color.Red, 20);
                }
            }
        }

        //更新Modbus数据到Dbus
        public static void UpdateModbusToDbus(int deviceNum, int ModbusStart, int DbusStart, int Num)
        {
            int j = DbusStart;
            for (int i = ModbusStart; i < ModbusStart + Num; i++)
            {
                UdpSever.Register[deviceNum, j++, 0] = Profinet.Register[i];
            }
        }

        //更新Dbus数据到Modbus
        public static void UpdateDbusToModbus(int deviceNum, int DbusStart, int ModbusStart, int Num)
        {
            int j = ModbusStart;
            for (int i = DbusStart; i < DbusStart + Num; i++)
            {
                Profinet.Register[j++] = (ushort)UdpSever.Register[deviceNum, i, 0];
            }
        }

        private static void Syncdbus()
        {
            while (true)
            {
                Thread.Sleep(Dbus.Cycle);
                int AgbNum;

                #region AGV1
                AgbNum = 1;

                //Sever => AGV 1
                Dbus.SetRegister(AgbNum, 1);

                //PLC    => AGV 
                //56 -60 => 26-30
                //更新Modbus数据到Dbus
                UpdateModbusToDbus(AgbNum, 56, 26, 5);
                //设置到AGV
                Dbus.SetRegister(AgbNum, 26, 30);

                //AGV    => PLC
                //20 -25 => 50-55
                //更新Dbus数据到Modbus
                UpdateDbusToModbus(AgbNum, 20, 50, 6);
                //设置到Modbus
                Profinet.SetRegister(50, 55);
                #endregion

                #region AGV2
                //Sever => AGV 1
                AgbNum = 2;
                Dbus.SetRegister(AgbNum, 1);
                //PLC    => AGV 
                //67 -71 => 37-41
                //更新Modbus数据到Dbus
                UpdateModbusToDbus(AgbNum, 67, 37, 5);
                //设置到AGV
                Dbus.SetRegister(AgbNum, 37, 41);

                //AGV    => PLC
                //31 -36 => 61-66
                //更新Dbus数据到Modbus
                UpdateDbusToModbus(AgbNum, 31, 61, 6);
                //设置到Modbus
                Profinet.SetRegister(61, 66);
                #endregion
            }
        }

        /// <summary>
        /// 同步Modbus设备数据
        /// </summary>
        private static void SyncModbus()
        {
            while (true)
            {
                Thread.Sleep(Profinet.Cycle);
                #region MES
                //读取 0
                Profinet.GetRegister(0, 0);
                //读取 20
                Profinet.GetRegister(0, 0);

                //写入 1-14
                Profinet.SetRegister(1, 14);
                //写入 21-34
                Profinet.SetRegister(21, 34);
                #endregion

                #region PLC
                //读取 56-60
                Profinet.GetRegister(56, 60);
                //读取 67-71
                Profinet.GetRegister(67, 71);

                //写入 50-55
                Profinet.SetRegister(50, 55);
                //写入 61-66
                Profinet.SetRegister(61, 66);
                #endregion
            }
        }
    }
}
