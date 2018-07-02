using DispatchSystem.Class;
using DispatchSystem.Database;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace DispatchSystem.User
{
    class DataSync
    {
        static ModbusIpMaster modbusMaster;
        static ExThread modbusThread;
        static ExThread dbusThread;
        static ExThread MainThread;


        static List<ModbusConfig> modbusConfig = new List<ModbusConfig>();

        //开始同步
        public static void Start()
        {
            //加载modbus配置,有配置则更新为配置，没有则不更新
            ExTimeOut et = new ExTimeOut(2000);
            modbusConfig = et.Connect();

            if (modbusConfig != null)
            {
                //modbus 检测时间
                var data = modbusConfig.FirstOrDefault(m => m.key == "circle");
                if (data != null)
                    Profinet.Cycle = int.Parse(data.value);

                //modbus服务器IP地址
                data = modbusConfig.FirstOrDefault(m => m.key == "ip");
                if (data != null)
                    Profinet.ModbusTcpSeverIPAddress = data.value;

                //modbus服务器端口
                data = modbusConfig.FirstOrDefault(m => m.key == "port");
                if (data != null)
                    Profinet.ModbusTcpSeverPort = int.Parse(data.value);

                MyConsole.Add("数据库打开成功！", Color.Green);
            }
            else
            {
                MyConsole.Add("数据库打开失败！", Color.Red);
            }


            MainThread = new ExThread(mainThreadFunc);
            MainThread.thread.IsBackground = true;
            MainThread.Start();
        }

        //结束同步
        public static void Stop()
        {
            if (SyncState.Dbus)
                dbusThread.Stop();

            if (SyncState.ModbusTcp)
                modbusThread.Stop();
        }
        public static class SyncState
        {
            public static bool ModbusTcp = false;
            public static bool Dbus = false;
        }
        private static void mainThreadFunc()
        {
            while (true)
            {
                if (MainThread.exitEvent.WaitOne(1000)) { break; }

                #region 启动Dbus
                //启动监听进程
                if (SyncState.Dbus == false)
                {
                    dbusThread = new ExThread(Syncdbus);
                    dbusThread.Start();
                    MyConsole.Add("监听Dbus线程已启动!", Color.Green);
                    SyncState.Dbus = true;
                }
                #endregion

                #region 启动ModbusTcp
                if (SyncState.ModbusTcp == false)
                {
                    try
                    {
                        MyConsole.Add(string.Format("开始连接ModbusTcp客户端,IP地址:{0},Port:{1}", Profinet.ModbusTcpSeverIPAddress, Profinet.ModbusTcpSeverPort));
                        //连接超时1000ms
                        TcpClient tcpClient = new ExTcpClient(Profinet.ModbusTcpSeverIPAddress, Profinet.ModbusTcpSeverPort, 1000).Connect();
                        modbusMaster = ModbusIpMaster.CreateIp(tcpClient);
                        modbusMaster.Transport.WriteTimeout = Profinet.Timeout;//写超时
                        modbusMaster.Transport.ReadTimeout = Profinet.Timeout;//读超时
                        modbusMaster.Transport.WaitToRetryMilliseconds = Profinet.WaitToRetryTime;//重试等待时间
                        modbusMaster.Transport.Retries = Profinet.RetryNum;//重试次数

                        //启动监听进程
                        modbusThread = new ExThread(SyncModbus);
                        modbusThread.Start();
                        SyncState.ModbusTcp = true;
                        MyConsole.Add("监听ModbusTCP线程启动成功!", Color.Green);
                    }
                    catch
                    {
                        MyConsole.Add("监听ModbusTCP线程启动失败!", Color.Red);
                    }
                }
                #endregion

            }
        }
        private delegate string ConnectSocketDelegate(IPEndPoint ipep, Socket sock);
        private static string ConnectSocket(IPEndPoint ipep, Socket sock)
        {
            string exmessage = "";
            try
            {
                sock.Connect(ipep);
            }
            catch (System.Exception ex)
            {
                exmessage = ex.Message;
            }
            finally
            {
            }

            return exmessage;
        }
        /// <summary>
        /// Modbus配置
        /// </summary>
        public static class Profinet
        {
            //modbus 检测时间
            public static int Cycle = 1000;
            //modbus服务器IP地址
            public static string ModbusTcpSeverIPAddress = "192.168.250.102";
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

            //清除任务标志
            public static bool[] Clear = new bool[200];

            //错误次数
            public static int ErrorNum = 0;

            //设置数据
            public static bool SetRegister(int start, int end)
            {
                ushort[] temp;
                string Msg = string.Format("设置:{0,-2}-{1,-2}", start, end);
                try
                {
                    //检测待写入数据和上次写入是否有变化
                    //for (int i = start; i <= end; i++)
                    //{
                    //    if (Register[i] != RegisterCompare[i])
                    //    {
                    //        temp = new ushort[end - start + 1];
                    //        for (int j = start; j <= end; j++)
                    //        {
                    //            temp[j - start] = Register[j];
                    //        }
                    //        modbusMaster.WriteMultipleRegisters((ushort)start, temp);
                    //        //发送成功，更新比较数组
                    //        for (int j = start; j <= end; j++)
                    //        {
                    //            RegisterCompare[j] = Register[j];
                    //        }
                    //        return true;
                    //    }
                    //}


                    temp = new ushort[end - start + 1];
                    for (int j = start; j <= end; j++)
                    {
                        temp[j - start] = Register[j];
                    }
                    modbusMaster.WriteMultipleRegisters((ushort)start, temp);
                    ErrorNum = 0;
                    return true;
                }
                catch
                {
                    //ConsoleLog.WriteLog(string.Format("ProfiNet操作失败!:[{0}]", Msg), Color.Red, 20);
                    ErrorNum++;
                    return false;
                }

            }

            //获取数据
            public static void GetRegister(int start, int end)
            {
                ushort[] temp;
                int i = 0;
                string Msg = string.Format("获取:{0,-2}-{1,-2}", start, end);
                try
                {
                    temp = modbusMaster.ReadHoldingRegisters((ushort)start, (ushort)(end - start + 1));
                    foreach (var item in temp)
                    {
                        Register[start++] = temp[i++];
                    }
                    ErrorNum = 0;
                }
                catch
                {
                    //ConsoleLog.WriteLog(string.Format("ProfiNet操作失败!:[{0}]", Msg), Color.Red, 20);
                    ErrorNum++;
                }
            }
        }

        /// <summary>
        /// Dbus配置
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
                ////检测待写入数据和上次写入是否有变化
                //for (int i = start; i <= end; i++)
                //{
                //    if (UdpSever.Register[deviceAddress, i, 0] != DbusCompare[deviceAddress, i])
                //    {
                //        temp = new ushort[end - start + 1];
                //        for (int j = start; j <= end; j++)
                //        {
                //            temp[j - start] = (ushort)UdpSever.Register[deviceAddress, j, 0];
                //        }

                //        UdpSever.ReturnMsg mg = UdpSever.Write_Multiple_Registers(deviceAddress, start, end - start + 1, temp);

                //        //发送成功，更新比较数组
                //        if (mg.resault)
                //        {
                //            for (int j = start; j <= end; j++)
                //            {
                //                DbusCompare[deviceAddress, j] = temp[j];
                //            }
                //        }
                //        else
                //            ConsoleLog.WriteLog(string.Format("Dbus操作失败!:[{0}]", Msg), Color.Red, 20);
                //        break;
                //    }
                //}

                //检测待写入数据和上次写入是否有变化
                temp = new ushort[end - start + 1];
                for (int j = start; j <= end; j++)
                {
                    temp[j - start] = (ushort)UdpSever.Register[deviceAddress, j, 0];
                }

                UdpSever.Post_Multiple_Registers(deviceAddress, start, end - start + 1, temp);

                //if (mg.resault == false)
                //    ConsoleLog.WriteLog(string.Format("Dbus操作失败!:[{0}]", Msg), Color.Red, 20);
            }
            //设置单个数据
            public static void SetRegister(int deviceAddress, int start)
            {
                string Msg = string.Format("设置:{0,-2}", start);


                UdpSever.Post_Register(deviceAddress, start, (ushort)UdpSever.Register[deviceAddress, start, 0]);
                //if (mg.resault == false)
                //    ConsoleLog.WriteLog(string.Format("Dbus操作失败!:[{0}]", Msg), Color.Red, 20);

                //if (UdpSever.Register[deviceAddress, start, 0] != DbusCompare[deviceAddress, start])
                //{
                //    UdpSever.ReturnMsg mg = UdpSever.Write_Register(deviceAddress, start, (ushort)UdpSever.Register[deviceAddress, start, 0]);

                //    //发送成功，更新比较数组
                //    if (mg.resault)
                //    {
                //        DbusCompare[deviceAddress, start] = (ushort)UdpSever.Register[deviceAddress, start, 0];
                //    }
                //    else
                //        ConsoleLog.WriteLog(string.Format("Dbus操作失败!:[{0}]", Msg), Color.Red, 20);
                //}
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
                if (dbusThread.exitEvent.WaitOne(Dbus.Cycle)) { break; }
                int AgvNum;

                #region AGV1
                AgvNum = 1;
                if (UdpSever.EndPointArray[AgvNum] != null)
                {
                    //Sever => AGV 1
                    Dbus.SetRegister(AgvNum, 1);

                    //PLC    => AGV 
                    //56 -60 => 26-30
                    //更新Modbus数据到Dbus
                    UpdateModbusToDbus(AgvNum, 56, 26, 5);
                    //设置到AGV
                    Dbus.SetRegister(AgvNum, 26, 30);

                    //AGV    => PLC
                    //20 -25 => 50-55
                    //更新Dbus数据到Modbus
                    UpdateDbusToModbus(AgvNum, 20, 50, 6);
                    //设置到Modbus
                    Profinet.SetRegister(50, 55);
                }
                #endregion

                #region AGV2
                //Sever => AGV 1
                AgvNum = 2;
                if (UdpSever.EndPointArray[AgvNum] != null)
                {
                    Dbus.SetRegister(AgvNum, 1);
                    //PLC    => AGV 
                    //67 -71 => 37-41
                    //更新Modbus数据到Dbus
                    UpdateModbusToDbus(AgvNum, 67, 37, 5);
                    //设置到AGV
                    Dbus.SetRegister(AgvNum, 37, 41);

                    //AGV    => PLC
                    //31 -36 => 61-66
                    //更新Dbus数据到Modbus
                    UpdateDbusToModbus(AgvNum, 31, 61, 6);
                    //设置到Modbus
                    Profinet.SetRegister(61, 66);
                }
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
                if (Profinet.ErrorNum > 10)
                {
                    //连续10次出错，重新连接
                    SyncState.ModbusTcp = false;
                    break;//退出当前进程
                }
                if (SyncState.ModbusTcp)
                {
                    if (modbusThread.exitEvent.WaitOne(Profinet.Cycle)) { break; }
                    #region MES
                    int num;
                    //读取 0
                    num = 0;
                    if (Profinet.Clear[num])
                    {
                        //此处循环是防止清除失败，做了重读验证
                        while (true)
                        {
                            //清除寄存器
                            Profinet.Register[num] = 0;
                            Profinet.SetRegister(num, num);

                            Thread.Sleep(100);

                            //读取结果
                            Profinet.GetRegister(num, num);
                            if (Profinet.Register[num] == 0)
                            {
                                Profinet.Clear[num] = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Profinet.GetRegister(num, num);
                        Profinet.RegisterCompare[num] = Profinet.Register[num];
                    }

                    //读取 20
                    num = 20;
                    if (Profinet.Clear[num])
                    {
                        //此处循环是防止清除失败，做了重读验证
                        while (true)
                        {
                            //清除寄存器
                            Profinet.Register[num] = 0;
                            Profinet.SetRegister(num, num);

                            Thread.Sleep(100);

                            //读取结果
                            Profinet.GetRegister(num, num);
                            if (Profinet.Register[num] == 0)
                            {
                                Profinet.Clear[num] = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Profinet.GetRegister(num, num);
                        Profinet.RegisterCompare[num] = Profinet.Register[num];
                    }

                    //写入 1-14
                    // Profinet.SetRegister(1, 14);
                    Profinet.SetRegister(1, 6);
                    Profinet.SetRegister(8, 14);


                    //写入 21-34
                    Profinet.SetRegister(21, 26);
                    Profinet.SetRegister(28, 34);

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
}
