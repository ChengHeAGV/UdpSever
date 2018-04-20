using AdvancedDataGridView;
using DispatchSystem.Developer;
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
    public partial class DataTransmission : Form
    {
        ModbusIpMaster modbusMaster;
        Thread modbusThread;
        Thread dbusThread;

        public DataTransmission()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Modbus数据
        /// </summary>
        private static class Modbus
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
            public static UInt16[] Profinet = new UInt16[200];
            //待发送数据临时数组，用于检测数据是否有变化
            public static UInt16[] ProfinetCompare = new UInt16[200];
            //消息
            public static string Msg;
        }

        private void DataTransmission_Load(object sender, EventArgs e)
        {
            #region 启动ModbusTcp
            try
            {
                TcpClient tcpClient = new TcpClient(Modbus.ModbusTcpSeverIPAddress, Modbus.ModbusTcpSeverPort);
                modbusMaster = ModbusIpMaster.CreateIp(tcpClient);
                modbusMaster.Transport.WriteTimeout = Modbus.Timeout;//写超时
                modbusMaster.Transport.ReadTimeout = Modbus.Timeout;//读超时
                modbusMaster.Transport.WaitToRetryMilliseconds = Modbus.WaitToRetryTime;//重试等待时间
                modbusMaster.Transport.Retries = Modbus.RetryNum;//重试次数

                //启动监听进程
                modbusThread = new Thread(new ThreadStart(SyncModbus));
                modbusThread.Start();
            }
            catch
            {
                ConsoleLog.WriteLog("连接Modbus设备失败！", Color.Red, 24);
            }
            #endregion

            #region 启动Dbus
            //启动监听进程
            dbusThread = new Thread(new ThreadStart(Syncdbus));
            dbusThread.Start();
            #endregion
        }

        private void Syncdbus()
        {

            while (true)
            {
                Thread.Sleep(Modbus.Cycle);
                try
                {

                }
                catch 
                {
                        
                }
            }
        }


        /// <summary>
        /// 同步Modbus设备数据
        /// </summary>
        private void SyncModbus()
        {
            while (true)
            {
                Thread.Sleep(Modbus.Cycle);
                try
                {
                    ushort start = 0;
                    ushort end = 0;
                    ushort[] temp;
                    #region MES
                    #region 读取 0
                    Modbus.Msg = "读取 0";
                    start = 0;
                    end = 0;
                    temp = modbusMaster.ReadHoldingRegisters(start, (ushort)(end - start + 1));
                    foreach (var item in temp)
                    {
                        Modbus.Profinet[start] = temp[start++];
                    }
                    #endregion
                    #region 读取 20
                    Modbus.Msg = "读取 20";
                    start = 20;
                    end = 20;
                    temp = modbusMaster.ReadHoldingRegisters(start, (ushort)(end - start + 1));
                    foreach (var item in temp)
                    {
                        Modbus.Profinet[start] = temp[start++];
                    }
                    #endregion
                    #region 写入 1-14
                    Modbus.Msg = "写入 1-14";
                    start = 1;
                    end = 14;
                    //检测待写入数据和上次写入是否有变化
                    for (int i = start; i <= end; i++)
                    {
                        if (Modbus.Profinet[i] != Modbus.ProfinetCompare[i])
                        {
                            temp = new ushort[end - start + 1];
                            for (int j = start; j <= end; j++)
                            {
                                temp[j] = Modbus.Profinet[j];
                            }
                            modbusMaster.WriteMultipleRegisters(start, temp);
                            //发送成功，更新比较数组
                            for (int j = start; j <= end; j++)
                            {
                                Modbus.ProfinetCompare[j] = Modbus.Profinet[j];
                            }
                        }
                    }
                    #endregion
                    #region 写入 21-34
                    Modbus.Msg = "写入 21-34";
                    start = 21;
                    end = 34;
                    //检测待写入数据和上次写入是否有变化
                    for (int i = start; i <= end; i++)
                    {
                        if (Modbus.Profinet[i] != Modbus.ProfinetCompare[i])
                        {
                            temp = new ushort[end - start + 1];
                            for (int j = start; j <= end; j++)
                            {
                                temp[j] = Modbus.Profinet[j];
                            }
                            modbusMaster.WriteMultipleRegisters(start, temp);
                            //发送成功，更新比较数组
                            for (int j = start; j <= end; j++)
                            {
                                Modbus.ProfinetCompare[j] = Modbus.Profinet[j];
                            }
                        }
                    }
                    #endregion
                    #endregion

                    #region PLC
                    #region 读取 56-60
                    Modbus.Msg = "读取 56-60";
                    start = 56;
                    end = 60;
                    temp = modbusMaster.ReadHoldingRegisters(start, (ushort)(end - start + 1));
                    foreach (var item in temp)
                    {
                        Modbus.Profinet[start] = temp[start++];
                    }
                    #endregion
                    #region 读取 67-71
                    Modbus.Msg = "读取 67-71";
                    start = 67;
                    end = 71;
                    temp = modbusMaster.ReadHoldingRegisters(start, (ushort)(end - start + 1));
                    foreach (var item in temp)
                    {
                        Modbus.Profinet[start] = temp[start++];
                    }
                    #endregion
                    #region 写入 50-55
                    Modbus.Msg = "写入 50-55";
                    start = 50;
                    end = 55;
                    //检测待写入数据和上次写入是否有变化
                    for (int i = start; i <= end; i++)
                    {
                        if (Modbus.Profinet[i] != Modbus.ProfinetCompare[i])
                        {
                            temp = new ushort[end - start + 1];
                            for (int j = start; j <= end; j++)
                            {
                                temp[j] = Modbus.Profinet[j];
                            }
                            modbusMaster.WriteMultipleRegisters(start, temp);
                            //发送成功，更新比较数组
                            for (int j = start; j <= end; j++)
                            {
                                Modbus.ProfinetCompare[j] = Modbus.Profinet[j];
                            }
                        }
                    }
                    #endregion
                    #region 写入 61-66
                    Modbus.Msg = "写入 61-66";
                    start = 61;
                    end = 66;
                    //检测待写入数据和上次写入是否有变化
                    for (int i = start; i <= end; i++)
                    {
                        if (Modbus.Profinet[i] != Modbus.ProfinetCompare[i])
                        {
                            temp = new ushort[end - start + 1];
                            for (int j = start; j <= end; j++)
                            {
                                temp[j] = Modbus.Profinet[j];
                            }
                            modbusMaster.WriteMultipleRegisters(start, temp);
                            //发送成功，更新比较数组
                            for (int j = start; j <= end; j++)
                            {
                                Modbus.ProfinetCompare[j] = Modbus.Profinet[j];
                            }
                        }
                    }
                    #endregion
                    #endregion
                }
                catch
                {
                    ConsoleLog.WriteLog(string.Format("Modbus同步数据:{0}失败!", Modbus.Msg), Color.Red, 20);
                }
            }

        }
    }
}
