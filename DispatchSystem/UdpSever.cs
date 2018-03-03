using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem
{

    //定义委托
    public delegate void UdpReciveDelegate(string str);
    class UdpSever
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        public static class Shell
        {
            /// <summary>
            /// 系统默认
            /// </summary>
            /// <param name="color"></param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteLine(string format, params object[] args)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(string.Format(format, args));
                WriteLog(string.Format(format, args));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sleep">打印完后延时，单位ms</param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteLine(int sleep, string format, params object[] args)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(string.Format(format, args));
                WriteLog(string.Format(format, args));
                Thread.Sleep(sleep);
            }

            /// <summary>
            /// 输出自定义颜色信息
            /// </summary>
            /// <param name="type">消息类型</param>
            /// <param name="color">显示颜色</param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteLine(string type, ConsoleColor color, string format, params object[] args)
            {
                if (DebugMsg.IsChecked(type))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("[{0}]", DateTimeOffset.Now);
                    WriteLog(string.Format("[{0}]", DateTimeOffset.Now));
                    Console.ForegroundColor = color;
                    Console.WriteLine("[{0}]{1}\r\n", type, string.Format(format, args));
                    WriteLog(string.Format("[{0}]{1}\r\n", type, string.Format(format, args)));
                }
            }
            /// <summary>
            /// 输出自定义颜色信息
            /// </summary>
            /// <param name="sleep">打印完后延时，单位ms</param>
            /// <param name="type"></param>
            /// <param name="color"></param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteLine(int sleep, string type, ConsoleColor color, string format, params object[] args)
            {
                if (DebugMsg.IsChecked(type))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("[{0}]", DateTimeOffset.Now);
                    WriteLog(string.Format("[{0}]", DateTimeOffset.Now));
                    Console.ForegroundColor = color;
                    Console.WriteLine("[{0}]{1}\r\n", type, string.Format(format, args));
                    WriteLog(string.Format("[{0}]{1}\r\n", type, string.Format(format, args)));
                    Thread.Sleep(sleep);
                }
            }

            /// <summary>
            /// 警告信息
            /// </summary>
            /// <param name="type">消息类型</param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteWarning(string type, string format, params object[] args)
            {
                if (DebugMsg.IsChecked(type))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("[{0}]", DateTimeOffset.Now);
                    WriteLog(string.Format("[{0}]", DateTimeOffset.Now));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[{0}]{1}\r\n", type, string.Format(format, args));
                    WriteLog(string.Format("[{0}]{1}\r\n", type, string.Format(format, args)));
                }
            }
            /// <summary>
            /// 警告信息
            /// </summary>
            /// <param name="sleep">打印完后延时，单位ms</param>
            /// <param name="type"></param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteWarning(int sleep, string type, string format, params object[] args)
            {
                if (DebugMsg.IsChecked(type))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("[{0}]", DateTimeOffset.Now);
                    WriteLog(string.Format("[{0}]", DateTimeOffset.Now));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[{0}]{1}\r\n", type, string.Format(format, args));
                    WriteLog(string.Format("[{0}]{1}\r\n", type, string.Format(format, args)));
                    Thread.Sleep(sleep);
                }
            }

            /// <summary>
            /// 提示信息
            /// </summary>
            /// <param name="type">消息类型</param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteNotice(string type, string format, params object[] args)
            {
                if (DebugMsg.IsChecked(type))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("[{0}]", DateTimeOffset.Now);
                    WriteLog(string.Format("[{0}]", DateTimeOffset.Now));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[{0}]{1}\r\n", type, string.Format(format, args));
                    WriteLog(string.Format("[{0}]{1}\r\n", type, string.Format(format, args)));
                }
            }
            /// <summary>
            /// 提示信息
            /// </summary>
            /// <param name="sleep">打印完后延时，单位ms</param>
            /// <param name="type"></param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteNotice(int sleep, string type, string format, params object[] args)
            {
                if (DebugMsg.IsChecked(type))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("[{0}]", DateTimeOffset.Now);
                    WriteLog(string.Format("[{0}]", DateTimeOffset.Now));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[{0}]{1}\r\n", type, string.Format(format, args));
                    WriteLog(string.Format("[{0}]{1}\r\n", type, string.Format(format, args)));
                    Thread.Sleep(sleep);
                }
            }
            /// <summary>
            /// 错误信息
            /// </summary>
            /// <param name="type">消息类型</param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteError(string type, string format, params object[] args)
            {
                if (DebugMsg.IsChecked(type))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("[{0}]", DateTimeOffset.Now);
                    WriteLog(string.Format("[{0}]", DateTimeOffset.Now));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[{0}]{1}\r\n", type, string.Format(format, args));
                    WriteLog(string.Format("[{0}]{1}\r\n", type, string.Format(format, args)));
                }
            }
            /// <summary>
            /// 错误信息
            /// </summary>
            /// <param name="sleep">打印完后延时，单位ms</param>
            /// <param name="type"></param>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public static void WriteError(int sleep, string type, string format, params object[] args)
            {
                if (DebugMsg.IsChecked(type))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("[{0}]", DateTimeOffset.Now);
                    WriteLog(string.Format("[{0}]", DateTimeOffset.Now));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[{0}]{1}\r\n", type, string.Format(format, args));
                    WriteLog(string.Format("[{0}]{1}\r\n", type, string.Format(format, args)));
                    Thread.Sleep(sleep);
                }
            }

            #region 写log
            static string filePath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            static string fileDayPath = AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd");
            static string logPath = fileDayPath +"\\"+ DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".txt";
            public static void WriteLog(string msg)
            {
                try
                {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    if (!Directory.Exists(fileDayPath))
                    {
                        Directory.CreateDirectory(fileDayPath);
                    }
                    File.AppendAllText(logPath, msg + "\r\n");
                }
                catch
                {
                    
                }
            }
            #endregion
        }

        /// <summary>
        /// 调试信息打印配置
        /// </summary>
        public static class DebugMsg
        {
            /// <summary>
            /// 心跳帧
            /// </summary>
            public static DataTable dt = new DataTable();

            /// <summary>
            /// 查看关键字对应的选择状态
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static bool IsChecked(string key)
            {
                bool reault = true;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][1].ToString() == key)
                    {
                        reault = (bool)dt.Rows[i][2];
                        break;
                    }
                    else
                    {
                        if (i == dt.Rows.Count - 1)
                        {
                            reault = false;
                        }
                    }
                }
                return reault;
            }
        }

        //服务器IP
        public static IPAddress ipaddress;
        //服务器端口
        public static int port = 18666;
        //服务器启动停止状态，默认停止
        public static bool State = false;
        //接收及发送的数据总长度
        public static UInt64 RxLength;
        public static UInt64 TxLength;
        public static int ErrorCount;//错误次数

        //初始化设备数
        public static int DeviceNum = 20;
        //初始化寄存器数
        public static int RegisterNum = 128;
        //单帧数据长度
        public static int FrameLen = 2048;
        //设备，寄存器，数据和时间戳
        public static Int64[,,] Register = new Int64[DeviceNum, RegisterNum, 2];
        //设备端口及IP
        public static EndPoint[] EndPointArray = new EndPoint[DeviceNum];
        //设备更新时间(最后一次收到数据时间)
        public static DateTime[] UpdateTime = new DateTime[DeviceNum];
        //服务器地址
        public static int ServerAddress = 0;
        //设备心跳周期(单位：秒)
        public static int HeartCycle = 100;

        //错误重发次数
        public static int RepeatNum = 3;
        //响应超时时间，单位ms
        public static int ResponseTimeout = 200;

        //响应帧缓冲池大小
        public static int RESPONSE_MAX_LEN = 20;
        //响应帧缓冲池
        public static byte[,] ResponseBuf = new byte[RESPONSE_MAX_LEN, FrameLen];

        //监听服务总进程
        static Thread MainThread;

        //帧ID
        public static int FrameID = 0;

        public struct ReturnMsg
        {
            public bool resault;
            public UInt16 Data;
            public UInt16[] DataBuf;

            public override string ToString()
            {
                string str = String.Empty;
                if (DataBuf != null)
                {
                    int num = 0;
                    foreach (var item in DataBuf)
                    {
                        str += String.Format("DataBuf[{0}]:{1}\r\n", num, item.ToString("X2"));
                        num++;
                    }
                }
                return string.Format("Resault:{0}\r\nData:{1}\r\n{2}", resault, Data.ToString("X2"), str);
            }
        }

        //函数返回结构体
        public struct Resault
        {
            public bool Reault;
            public string Message;
        }

        static IPEndPoint ipEndPoint;
        static Socket socket;
        //主线程状态
        public static bool MainThreadIsClosed = true;
        /// <summary>
        /// 启动UDP服务器
        /// </summary>
        public static Resault Start()
        {
            Resault rs = new Resault();
            try
            {
                ipEndPoint = new IPEndPoint(ipaddress, port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(ipEndPoint);
                rs.Reault = true;
                rs.Message = "启动成功";

                MainThreadIsClosed = false;
                MainThread = new Thread(mainFunc);
                MainThread.Start();

                //启动检测连接进程
                State = true;//更新服务器状态

                Shell.WriteNotice("系统消息", "服务已启动!");
                return rs;
            }
            catch (Exception ex)
            {
                rs.Reault = false;
                rs.Message = ex.Message;
                return rs;
            }
        }

        /// <summary>
        /// 停止UDP服务器
        /// </summary>
        public static Resault Stop()
        {
            Resault rs = new Resault();
            try
            {
                //退出线程
                MainThreadIsClosed = true;
                Shell.WriteWarning("系统消息", "正在关闭服务器，请稍等...!");
                socket.Close();
                //等待线程退出
                while (MainThread.ThreadState != System.Threading.ThreadState.Stopped)
                {
                    Thread.Sleep(10);
                }
                //注销检测连接进程
                MainThread.Abort();
               
                State = false;//更新服务器状态 

                rs.Reault = true;
                rs.Message = "停止成功";

                Shell.WriteNotice("系统消息", "服务已停止!");
                return rs;
            }
            catch (Exception ex)
            {
                rs.Reault = false;
                rs.Message = ex.Message;
                return rs;
            }

        }

        //启动监听
        private static void mainFunc()
        {
            try
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint endPoint = (EndPoint)(sender);
                //定义接收池字符串
                string StringBuf = string.Empty;
                while (MainThreadIsClosed == false)
                {
                    Thread.Sleep(1);
                    //从缓冲区读取数据
                    byte[] bytes = new byte[3096];
                    int length = socket.ReceiveFrom(bytes, ref endPoint);
                    //更新接收到的数据总长度
                    RxLength += (UInt64)length;
                    ////定义临时数组,将数据转存到临时数组
                    //byte[] Buf = new byte[length];
                    //for (int i = 0; i < length; i++)
                    //{
                    //    Buf[i] = bytes[i];
                    //}
                    //#region 数据解析
                    ////将byte数组转换成字符串
                    //string str = Encoding.ASCII.GetString(Buf);
                    ////清除多余的\0
                    //str = str.Replace("\0", "");
                    //Shell.WriteNotice("收到数据", string.Format("收到数据：{0}", str));
                    ////将收到的数据追加到缓冲池
                    //StringBuf += str;
                    //#endregion


                    ////开始标志
                    //int Start = 0;
                    ////结束标志
                    //int Stop = 0;
                    ////搜索结果
                    //bool reault;
                    ////循环搜索并解析有效数据
                    //while (StringBuf.Length > (Stop + 1))
                    //{
                    //    int count = 0;//单次解析有效帧计数
                    //    reault = false;
                    //    for (int k = Stop; k < StringBuf.Length; k++)
                    //    {
                    //        //查找开始符
                    //        if (StringBuf[k] == '$')
                    //        {
                    //            Start = k;
                    //        }
                    //        //查找结束符
                    //        if ((Start >= Stop) && StringBuf[k] == '!')
                    //        {
                    //            Stop = k;
                    //            reault = true;
                    //            break;
                    //        }
                    //    }
                    //    if (reault)//找到了有效的数据
                    //    {
                    //        count++;
                    //        //如果头尾中间没有数据，不处理
                    //        if ((Stop - Start) == 1)
                    //        {
                    //            continue;
                    //        }
                    //        else
                    //        {
                    //            Buf = StrToHexByte(StringBuf.Substring(Start + 1, Stop - Start - 1));

                    //            //CRC校验
                    //            int crc16 = CRC.crc_16(Buf, Buf.Length - 2);
                    //            if (crc16 == (Buf[Buf.Length - 2] << 8 | Buf[Buf.Length - 1]))
                    //            {
                    //                //帧ID
                    //                int FrameID = Buf[0] << 8 | Buf[1];
                    //                //设备地址
                    //                int DeviceAddress = Buf[2] << 8 | Buf[3];
                    //                //帧类型
                    //                int FrameType = Buf[4];
                    //                //目标地址
                    //                int DstAddress = Buf[5] << 8 | Buf[6];

                    //                #region 判断帧类型
                    //                switch (FrameType)
                    //                {
                    //                    case 0:
                    //                        #region 心跳帧
                    //                        Shell.WriteNotice("心跳帧", string.Format("[设备ID:{0}]", DeviceAddress));
                    //                        //存储设备端口信息到EndPointArray
                    //                        EndPointArray[DeviceAddress] = endPoint;
                    //                        //更新设备响应时间
                    //                        UpdateTime[DeviceAddress] = DateTime.Now;
                    //                        #endregion
                    //                        break;
                    //                    case 1:
                    //                        #region 操作帧
                    //                        Shell.WriteNotice("操作帧", string.Format("[ALL][{0}]", ByteToHexStr(Buf)));
                    //                        switch (Buf[7])//功能码
                    //                        {
                    //                            case 1:
                    //                                #region 读单个寄存器
                    //                                Shell.WriteNotice("读单个寄存器", string.Format("[{0}]", ByteToHexStr(Buf)));
                    //                                //判断目标地址是否在线
                    //                                if (EndPointArray[DeviceAddress] != null)
                    //                                {
                    //                                    byte[] sendbyte = new byte[14];

                    //                                    sendbyte[0] = Buf[0];//帧ID高
                    //                                    sendbyte[1] = Buf[1];//帧ID底
                    //                                    sendbyte[2] = (byte)(ServerAddress >> 8);//本机ID高
                    //                                    sendbyte[3] = (byte)(ServerAddress);//本机ID低
                    //                                    sendbyte[4] = 0x02;//帧类型
                    //                                    sendbyte[5] = (byte)(DeviceAddress >> 8);//目标地址高
                    //                                    sendbyte[6] = (byte)(DeviceAddress);//目标地址低
                    //                                    sendbyte[7] = 0x01;//功能码
                    //                                    sendbyte[8] = Buf[8];//寄存器地址高
                    //                                    sendbyte[9] = Buf[9];//寄存器地址低

                    //                                    sendbyte[10] = (byte)(Register[DeviceAddress, (Buf[8] << 8 | Buf[9]), 0] >> 8);//数据高
                    //                                    sendbyte[11] = (byte)(Register[DeviceAddress, (Buf[8] << 8 | Buf[9]), 0]);//数据低

                    //                                    int crcRes = CRC.crc_16(sendbyte, 12);

                    //                                    sendbyte[12] = (byte)(crcRes >> 8);
                    //                                    sendbyte[13] = (byte)(crcRes);
                    //                                    sendToUdp(EndPointArray[DeviceAddress], sendbyte);
                    //                                }
                    //                                else
                    //                                {
                    //                                    Shell.WriteError("错误", string.Format("[目标设备不在线][设备ID:{0}]", DeviceAddress));
                    //                                }
                    //                                #endregion
                    //                                break;
                    //                            case 2:
                    //                                #region 写单个寄存器
                    //                                Shell.WriteNotice("写单个寄存器", string.Format("[写单个寄存器][{0}]", ByteToHexStr(Buf)));
                    //                                //判断目标地址是否在线
                    //                                if (EndPointArray[DeviceAddress] != null)
                    //                                {
                    //                                    byte[] sendbyte = new byte[11];
                    //                                    int RegAddress = Buf[8] << 8 | Buf[9];
                    //                                    sendbyte[0] = Buf[0];//帧ID高
                    //                                    sendbyte[1] = Buf[1];//帧ID底
                    //                                    sendbyte[2] = (byte)(ServerAddress >> 8);//本机ID高
                    //                                    sendbyte[3] = (byte)(ServerAddress);//本机ID低
                    //                                    sendbyte[4] = 0x02;//帧类型
                    //                                    sendbyte[5] = (byte)(DeviceAddress >> 8);//目标地址高
                    //                                    sendbyte[6] = (byte)(DeviceAddress);//目标地址低
                    //                                    sendbyte[7] = 0x02;//功能码

                    //                                    if (RegAddress <= RegisterNum)
                    //                                    {
                    //                                        //写入寄存器
                    //                                        Register[DeviceAddress, RegAddress, 0] = (UInt16)(Buf[10] << 8 | Buf[11]);
                    //                                        //更新时间戳
                    //                                        Register[DeviceAddress, RegAddress, 1] = DateTimeToStamp(DateTime.Now);
                    //                                        sendbyte[8] = 1;//结果
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        sendbyte[8] = 0;//结果
                    //                                    }

                    //                                    int crcRes = CRC.crc_16(sendbyte, 9);

                    //                                    sendbyte[9] = (byte)(crcRes >> 8);
                    //                                    sendbyte[10] = (byte)(crcRes);
                    //                                    sendToUdp(EndPointArray[DeviceAddress], sendbyte);
                    //                                }
                    //                                else
                    //                                {
                    //                                    Shell.WriteError("错误", string.Format("[目标设备不在线][设备ID:{0}]", DeviceAddress));
                    //                                }
                    //                                #endregion
                    //                                break;
                    //                            case 3:
                    //                                #region 读多个寄存器
                    //                                Shell.WriteNotice("读多个寄存器", string.Format("[读多个寄存器][{0}]", ByteToHexStr(Buf)));
                    //                                //判断目标地址是否在线
                    //                                if (EndPointArray[DeviceAddress] != null)
                    //                                {
                    //                                    int Num = Buf[10];
                    //                                    byte[] sendbyte = new byte[13 + Num * 2];
                    //                                    //寄存器起始地址
                    //                                    int RegStartAdd = Buf[8] << 8 | Buf[9];
                    //                                    sendbyte[0] = Buf[0];//帧ID高
                    //                                    sendbyte[1] = Buf[1];//帧ID底
                    //                                    sendbyte[2] = (byte)(ServerAddress >> 8);//本机ID高
                    //                                    sendbyte[3] = (byte)(ServerAddress);//本机ID低
                    //                                    sendbyte[4] = 0x02;//帧类型
                    //                                    sendbyte[5] = (byte)(DeviceAddress >> 8);//目标地址高
                    //                                    sendbyte[6] = (byte)(DeviceAddress);//目标地址低
                    //                                    sendbyte[7] = 0x03;//功能码
                    //                                    sendbyte[8] = Buf[8];//寄存器起始地址高
                    //                                    sendbyte[9] = Buf[9];//寄存器起始地址低

                    //                                    sendbyte[10] = Buf[10];//数量

                    //                                    for (int i = 0; i < Num; i++)
                    //                                    {
                    //                                        sendbyte[11 + i * 2] = (byte)(Register[DeviceAddress, RegStartAdd + i, 0] >> 8);//数据高
                    //                                        sendbyte[12 + i * 2] = (byte)(Register[DeviceAddress, RegStartAdd + i, 0]);//数据低
                    //                                    }

                    //                                    int crcRes = CRC.crc_16(sendbyte, 11 + 2 * Num);
                    //                                    sendbyte[11 + 2 * Num] = (byte)(crcRes >> 8);
                    //                                    sendbyte[11 + 2 * Num + 1] = (byte)(crcRes);
                    //                                    sendToUdp(EndPointArray[DeviceAddress], sendbyte);
                    //                                }
                    //                                else
                    //                                {
                    //                                    Shell.WriteError("错误", string.Format("[目标设备不在线][设备ID:{0}]", DeviceAddress));
                    //                                }
                    //                                #endregion
                    //                                break;
                    //                            case 4://
                    //                                #region 写多个寄存器
                    //                                Shell.WriteNotice("写多个寄存器", string.Format("操作帧[写多个寄存器][{0}]", ByteToHexStr(Buf)));
                    //                                //判断目标地址是否在线
                    //                                if (EndPointArray[DeviceAddress] != null)
                    //                                {
                    //                                    byte[] sendbyte = new byte[11];
                    //                                    int RegStartAddress = Buf[8] << 8 | Buf[9];
                    //                                    int Num = Buf[10];
                    //                                    sendbyte[0] = Buf[0];//帧ID高
                    //                                    sendbyte[1] = Buf[1];//帧ID底
                    //                                    sendbyte[2] = (byte)(ServerAddress >> 8);//本机ID高
                    //                                    sendbyte[3] = (byte)(ServerAddress);//本机ID低
                    //                                    sendbyte[4] = 0x02;//帧类型
                    //                                    sendbyte[5] = (byte)(DeviceAddress >> 8);//目标地址高
                    //                                    sendbyte[6] = (byte)(DeviceAddress);//目标地址低
                    //                                    sendbyte[7] = 0x04;//功能码

                    //                                    if ((RegStartAddress + Num) <= RegisterNum)
                    //                                    {
                    //                                        for (int i = 0; i < Num; i++)
                    //                                        {
                    //                                            //写入寄存器
                    //                                            Register[DeviceAddress, RegStartAddress + i, 0] = (UInt16)(Buf[11 + i * 2] << 8 | Buf[12 + i * 2]);
                    //                                            //更新时间戳
                    //                                            Register[DeviceAddress, RegStartAddress + i, 1] = DateTimeToStamp(DateTime.Now);
                    //                                        }
                    //                                        sendbyte[8] = 1;//结果
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        sendbyte[8] = 0;//结果
                    //                                    }

                    //                                    int crcRes = CRC.crc_16(sendbyte, 9);
                    //                                    sendbyte[9] = (byte)(crcRes >> 8);
                    //                                    sendbyte[10] = (byte)(crcRes);
                    //                                    sendToUdp(EndPointArray[DeviceAddress], sendbyte);
                    //                                }
                    //                                else
                    //                                {
                    //                                    Shell.WriteError("错误", string.Format("[目标设备不在线][设备ID:{0}]", DeviceAddress));
                    //                                }
                    //                                #endregion
                    //                                break;
                    //                            default:
                    //                                ErrorCount++;
                    //                                Shell.WriteError("无效误帧", string.Format("[无效误帧][Count:{0}][{1}]", ErrorCount, ByteToHexStr(Buf)));
                    //                                length = 0;
                    //                                break;
                    //                        }
                    //                        #endregion
                    //                        break;
                    //                    case 2:
                    //                        #region 响应帧
                    //                        Shell.WriteNotice("响应帧", string.Format("[ALL]:", ByteToHexStr(Buf)));
                    //                        //添加到响应帧缓冲池
                    //                        for (int i = 0; i < RESPONSE_MAX_LEN; i++)
                    //                        {
                    //                            //将响应帧编入空闲的响应帧缓冲池
                    //                            if (ResponseBuf[i, 0] == 0)
                    //                            {
                    //                                //缓冲池第1字节为帧长度
                    //                                ResponseBuf[i, 0] = (byte)Buf.Length;
                    //                                //将该响应帧加入缓冲池
                    //                                for (int j = 0; j < Buf.Length; j++)
                    //                                    ResponseBuf[i, j + 1] = Buf[j];
                    //                            }
                    //                            //响应池被占满,清空缓冲池，并将本次响应加入第一个缓冲区
                    //                            else if (i == RESPONSE_MAX_LEN - 1)
                    //                            {
                    //                                //清空缓冲池
                    //                                for (int k = 0; k < RESPONSE_MAX_LEN; k++)
                    //                                    ResponseBuf[k, 0] = 0;

                    //                                //缓冲池第1字节为帧长度
                    //                                ResponseBuf[0, 0] = (byte)Buf.Length;
                    //                                //将该响应帧加入缓冲池
                    //                                for (int j = 0; j < (byte)Buf.Length; j++)
                    //                                    ResponseBuf[i, j + 1] = Buf[j];
                    //                            }
                    //                        }
                    //                        #endregion
                    //                        break;
                    //                    case 0x10:
                    //                        #region 实时帧（无响应）
                    //                        Shell.WriteNotice("实时帧", string.Format("[ALL][{0}]", ByteToHexStr(Buf)));
                    //                        switch (Buf[7])//功能码
                    //                        {
                    //                            case 2:
                    //                                #region 发送单个寄存器
                    //                                Shell.WriteNotice("写单个寄存器", string.Format("[发送单个寄存器][{0}]", ByteToHexStr(Buf)));
                    //                                //判断目标地址是否在线
                    //                                if (EndPointArray[DeviceAddress] != null)
                    //                                {
                    //                                    int RegAddress = Buf[8] << 8 | Buf[9];
                    //                                    if (RegAddress <= RegisterNum)
                    //                                    {
                    //                                        //写入寄存器
                    //                                        Register[DeviceAddress, RegAddress, 0] = (UInt16)(Buf[10] << 8 | Buf[11]);
                    //                                        //更新时间戳
                    //                                        Register[DeviceAddress, RegAddress, 1] = DateTimeToStamp(DateTime.Now);
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        Shell.WriteError("错误", string.Format("[目标寄存器地址超出索引][{0}]", RegAddress));
                    //                                    }
                    //                                }
                    //                                else
                    //                                {
                    //                                    Shell.WriteError("错误", string.Format("[目标设备不在线][设备ID:{0}]", DeviceAddress));
                    //                                }
                    //                                #endregion
                    //                                break;
                    //                            case 4://
                    //                                #region 写多个寄存器
                    //                                Shell.WriteNotice("发送多个寄存器", string.Format("[发送多个寄存器][{0}]", ByteToHexStr(Buf)));
                    //                                //判断目标地址是否在线
                    //                                if (EndPointArray[DeviceAddress] != null)
                    //                                {
                    //                                    int RegStartAddress = Buf[8] << 8 | Buf[9];
                    //                                    int Num = Buf[10];
                    //                                    if ((RegStartAddress + Num) <= RegisterNum)
                    //                                    {
                    //                                        for (int i = 0; i < Num; i++)
                    //                                        {
                    //                                            //写入寄存器
                    //                                            Register[DeviceAddress, RegStartAddress + i, 0] = (UInt16)(Buf[11 + i * 2] << 8 | Buf[12 + i * 2]);
                    //                                            //更新时间戳
                    //                                            Register[DeviceAddress, RegStartAddress + i, 1] = DateTimeToStamp(DateTime.Now);
                    //                                        }
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        Shell.WriteError("错误", string.Format("[目标寄存器地址超出索引][{0}]", RegStartAddress + Num));
                    //                                    }
                    //                                }
                    //                                else
                    //                                {
                    //                                    Shell.WriteError("错误", string.Format("[目标设备不在线][设备ID:{0}]", DeviceAddress));
                    //                                }
                    //                                #endregion
                    //                                break;
                    //                            default:
                    //                                ErrorCount++;
                    //                                Shell.WriteError("错误", string.Format("[无效误帧][Count:{0}][{1}]", ErrorCount, ByteToHexStr(Buf)));
                    //                                length = 0;
                    //                                break;
                    //                        }
                    //                        #endregion
                    //                        break;
                    //                    default:
                    //                        //出错次数加一
                    //                        ErrorCount++;
                    //                        //打印消息
                    //                        Shell.WriteError("无效误帧", string.Format("[无效误帧][Count:{0}][{1}]", ErrorCount, ByteToHexStr(Buf)));
                    //                        break;
                    //                }
                    //                #endregion
                    //            }
                    //        }
                    //    }
                    //    else//没有找到有效数据
                    //    {
                    //        //退出循环
                    //        break;
                    //    }
                    //    Shell.WriteNotice("单包数据有效帧数量", string.Format("[{0}]", count));
                    //}
                    //if (Stop < StringBuf.Length - 1)
                    //    StringBuf = StringBuf.Substring(Stop + 1, StringBuf.Length - Stop - 2);
                    //else
                    //    StringBuf = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Shell.WriteError("错误", string.Format(":{0}", ex.ToString()));
            }
        }

        /// <summary>
        /// 字节数组转10进制字符串
        /// </summary>
        public static string byteToString(byte[] bytes)
        {
            string str = string.Empty;
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    str += bytes[i].ToString();
                    if (i != bytes.Length - 1)
                    {
                        //str += " ";
                    }
                }
            }
            return str;
        }

        //写单个字
        public static ReturnMsg Write_Register(int TargetAddress, int RegisterAddress, int Data)
        {
            byte[] sendbyte = new byte[14];
            FrameID++;
            ReturnMsg msg = new ReturnMsg();
            int frameID = FrameID;
            sendbyte[0] = (byte)(frameID >> 8);
            sendbyte[1] = (byte)(frameID);
            sendbyte[2] = (byte)(ServerAddress >> 8);
            sendbyte[3] = (byte)(ServerAddress);
            sendbyte[4] = 0x01;
            sendbyte[5] = (byte)(TargetAddress >> 8);
            sendbyte[6] = (byte)(TargetAddress);
            sendbyte[7] = 0x02;
            sendbyte[8] = (byte)(RegisterAddress >> 8);
            sendbyte[9] = (byte)(RegisterAddress);
            sendbyte[10] = (byte)(Data >> 8);
            sendbyte[11] = (byte)(Data);

            int crcRes = CRC.crc_16(sendbyte, 12);

            sendbyte[12] = (byte)(crcRes >> 8);
            sendbyte[13] = (byte)(crcRes);
            for (int j = 0; j < RepeatNum; j++)
            {
                //发送数据
                sendToUdp(EndPointArray[TargetAddress], sendbyte);
                //等待响应
                for (int k = 0; k < ResponseTimeout; k++)
                {
                    for (int i = 0; i < RESPONSE_MAX_LEN; i++)
                    {
                        if (ResponseBuf[i, 0] != 0)
                        {
                            if (((ResponseBuf[i, 1] << 8) | ResponseBuf[i, 2]) == frameID)
                            {
                                msg.resault = true;
                                ResponseBuf[i, 0] = 0;

                                //更新寄存器
                                UdpSever.Register[TargetAddress, RegisterAddress, 0] = Data;
                                //更新时间戳
                                UdpSever.Register[TargetAddress, RegisterAddress, 1] = UdpSever.DateTimeToStamp(DateTime.Now);
                                return msg;
                            }
                        }
                    }
                    Thread.Sleep(1);
                }
            }
            msg.resault = false;
            return msg;
        }
        //写多个字
        public static ReturnMsg Write_Multiple_Registers(int TargetAddress, int RegisterAddress, int Num, UInt16[] Data)
        {
            byte[] sendbyte = new byte[13 + Num * 2];
            FrameID++;
            ReturnMsg msg = new ReturnMsg();
            int frameID = FrameID;
            sendbyte[0] = (byte)(frameID >> 8);
            sendbyte[1] = (byte)(frameID);
            sendbyte[2] = (byte)(ServerAddress >> 8);
            sendbyte[3] = (byte)(ServerAddress);
            sendbyte[4] = 0x01;
            sendbyte[5] = (byte)(TargetAddress >> 8);
            sendbyte[6] = (byte)(TargetAddress);
            sendbyte[7] = 0x04;
            sendbyte[8] = (byte)(RegisterAddress >> 8);
            sendbyte[9] = (byte)(RegisterAddress);
            sendbyte[10] = (byte)(Num);

            for (int i = 0; i < Num; i++)
            {
                sendbyte[11 + 2 * i] = (byte)(Data[i] >> 8);
                sendbyte[12 + 2 * i] = (byte)(Data[i]);
            }

            int crcRes = CRC.crc_16(sendbyte, 11 + Num * 2);
            sendbyte[11 + Num * 2] = (byte)(crcRes >> 8);
            sendbyte[12 + Num * 2] = (byte)(crcRes);

            for (int j = 0; j < RepeatNum; j++)
            {
                //发送数据
                sendToUdp(EndPointArray[TargetAddress], sendbyte);
                //等待响应
                for (int k = 0; k < ResponseTimeout; k++)
                {
                    for (int i = 0; i < RESPONSE_MAX_LEN; i++)
                    {
                        if (ResponseBuf[i, 0] != 0)
                        {
                            if (((ResponseBuf[i, 1] << 8) | ResponseBuf[i, 2]) == frameID)
                            {
                                msg.resault = true;
                                ResponseBuf[i, 0] = 0;

                                for (int t = 0; t < Num; t++)
                                {
                                    //更新寄存器
                                    UdpSever.Register[TargetAddress, RegisterAddress + t, 0] = Data[t];
                                    //更新时间戳
                                    UdpSever.Register[TargetAddress, RegisterAddress + t, 1] = UdpSever.DateTimeToStamp(DateTime.Now);
                                }

                                return msg;
                            }
                        }
                    }
                    Thread.Sleep(1);
                }
            }
            msg.resault = false;
            return msg;
        }

        //读单个寄存器
        public static ReturnMsg Read_Register(int TargetAddress, int RegisterAddress)
        {
            byte[] sendbyte = new byte[12];
            FrameID++;
            ReturnMsg msg = new ReturnMsg();
            int frameID = FrameID;
            sendbyte[0] = (byte)(frameID >> 8);
            sendbyte[1] = (byte)(frameID);
            sendbyte[2] = (byte)(ServerAddress >> 8);
            sendbyte[3] = (byte)(ServerAddress);
            sendbyte[4] = 0x01;
            sendbyte[5] = (byte)(TargetAddress >> 8);
            sendbyte[6] = (byte)(TargetAddress);
            sendbyte[7] = 0x01;
            sendbyte[8] = (byte)(RegisterAddress >> 8);
            sendbyte[9] = (byte)(RegisterAddress);

            int crcRes = CRC.crc_16(sendbyte, 10);

            sendbyte[10] = (byte)(crcRes >> 8);
            sendbyte[11] = (byte)(crcRes);

            for (int j = 0; j < RepeatNum; j++)
            {
                //发送数据
                sendToUdp(EndPointArray[TargetAddress], sendbyte);
                //等待响应
                for (int k = 0; k < ResponseTimeout; k++)
                {
                    for (int i = 0; i < RESPONSE_MAX_LEN; i++)
                    {
                        if (ResponseBuf[i, 0] != 0)
                        {
                            if (((ResponseBuf[i, 1] << 8) | ResponseBuf[i, 2]) == frameID)
                            {
                                msg.resault = true;
                                msg.Data = (ushort)(ResponseBuf[i, 11] << 8 | ResponseBuf[i, 12]);
                                ResponseBuf[i, 0] = 0;
                                return msg;
                            }
                        }
                    }
                    Thread.Sleep(1);
                }
            }
            msg.resault = false;
            return msg;
        }
        //读多个寄存器
        public static ReturnMsg Read_Multiple_Registers(int TargetAddress, int RegisterAddress, int Num)
        {
            byte[] sendbyte = new byte[13];
            FrameID++;
            ReturnMsg msg = new ReturnMsg();
            msg.DataBuf = new UInt16[Num];
            int frameID = FrameID;
            sendbyte[0] = (byte)(frameID >> 8);
            sendbyte[1] = (byte)(frameID);
            sendbyte[2] = (byte)(ServerAddress >> 8);
            sendbyte[3] = (byte)(ServerAddress);
            sendbyte[4] = 0x01;
            sendbyte[5] = (byte)(TargetAddress >> 8);
            sendbyte[6] = (byte)(TargetAddress);
            sendbyte[7] = 0x03;
            sendbyte[8] = (byte)(RegisterAddress >> 8);
            sendbyte[9] = (byte)(RegisterAddress);
            sendbyte[10] = (byte)(Num);

            int crcRes = CRC.crc_16(sendbyte, 11);

            sendbyte[11] = (byte)(crcRes >> 8);
            sendbyte[12] = (byte)(crcRes);

            for (int j = 0; j < RepeatNum; j++)
            {
                //发送数据
                sendToUdp(EndPointArray[TargetAddress], sendbyte);
                //等待响应
                for (int k = 0; k < ResponseTimeout; k++)
                {
                    for (int i = 0; i < RESPONSE_MAX_LEN; i++)
                    {
                        if (ResponseBuf[i, 0] != 0)
                        {
                            if (((ResponseBuf[i, 1] << 8) | ResponseBuf[i, 2]) == frameID)
                            {
                                msg.resault = true;
                                for (int t = 0; t < Num; t++)
                                {
                                    msg.DataBuf[t] = (ushort)(ResponseBuf[i, 12 + 2 * t] << 8 | ResponseBuf[i, 13 + 2 * t]);
                                }
                                ResponseBuf[i, 0] = 0;
                                return msg;
                            }
                        }
                    }
                    Thread.Sleep(1);
                }
            }
            msg.resault = false;
            return msg;
        }

        /****************实时帧******************/
        //写单个字
        public static void Post_Register(int TargetAddress, int RegisterAddress, int Data)
        {
            byte[] sendbyte = new byte[14];
            FrameID++;
            int frameID = FrameID;
            sendbyte[0] = (byte)(frameID >> 8);
            sendbyte[1] = (byte)(frameID);
            sendbyte[2] = (byte)(ServerAddress >> 8);
            sendbyte[3] = (byte)(ServerAddress);
            sendbyte[4] = 0x01;
            sendbyte[5] = (byte)(TargetAddress >> 8);
            sendbyte[6] = (byte)(TargetAddress);
            sendbyte[7] = 0x02;
            sendbyte[8] = (byte)(RegisterAddress >> 8);
            sendbyte[9] = (byte)(RegisterAddress);
            sendbyte[10] = (byte)(Data >> 8);
            sendbyte[11] = (byte)(Data);

            int crcRes = CRC.crc_16(sendbyte, 12);

            sendbyte[12] = (byte)(crcRes >> 8);
            sendbyte[13] = (byte)(crcRes);
            //发送数据
            sendToUdp(EndPointArray[TargetAddress], sendbyte);
        }
        //写多个字
        public static void Post_Multiple_Registers(int TargetAddress, int RegisterAddress, int Num, UInt16[] Data)
        {
            byte[] sendbyte = new byte[13 + Num * 2];
            FrameID++;
            int frameID = FrameID;
            sendbyte[0] = (byte)(frameID >> 8);
            sendbyte[1] = (byte)(frameID);
            sendbyte[2] = (byte)(ServerAddress >> 8);
            sendbyte[3] = (byte)(ServerAddress);
            sendbyte[4] = 0x01;
            sendbyte[5] = (byte)(TargetAddress >> 8);
            sendbyte[6] = (byte)(TargetAddress);
            sendbyte[7] = 0x04;
            sendbyte[8] = (byte)(RegisterAddress >> 8);
            sendbyte[9] = (byte)(RegisterAddress);
            sendbyte[10] = (byte)(Num);

            for (int i = 0; i < Num; i++)
            {
                sendbyte[11 + 2 * i] = (byte)(Data[i] >> 8);
                sendbyte[12 + 2 * i] = (byte)(Data[i]);
            }

            int crcRes = CRC.crc_16(sendbyte, 11 + Num * 2);
            sendbyte[11 + Num * 2] = (byte)(crcRes >> 8);
            sendbyte[12 + Num * 2] = (byte)(crcRes);
            //发送数据
            sendToUdp(EndPointArray[TargetAddress], sendbyte);
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="EndPort">EndPoint</param>
        /// <param name="str">byte[]</param>
        public static void sendToUdp(EndPoint EndPort, byte[] str)
        {
            if (str != null && EndPort != null)
            {
                byte[] buf = new byte[str.Length * 2 + 2];
                buf[0] = (byte)('$');
                string sting = ByteToHexStr(str);
                for (int i = 0; i < str.Length * 2; i++)
                {
                    buf[i + 1] = (byte)(sting[i]);
                }
                buf[buf.Length - 1] = (byte)('!');
                TxLength += (UInt64)buf.Length;
                socket.SendTo(buf, EndPort);
                Shell.WriteNotice("发送数据", string.Format("[{0}]", System.Text.Encoding.ASCII.GetString(buf)));
            }
        }

        public class CRC
        {
            public static ushort crc_16(byte[] data)
            {
                uint IX, IY;
                ushort crc = 0xFFFF;//set all 1

                int len = data.Length;
                if (len <= 0)
                    crc = 0;
                else
                {
                    len--;
                    for (IX = 0; IX <= len; IX++)
                    {
                        crc = (ushort)(crc ^ (data[IX]));
                        for (IY = 0; IY <= 7; IY++)
                        {
                            if ((crc & 1) != 0)
                                crc = (ushort)((crc >> 1) ^ 0xA001);
                            else
                                crc = (ushort)(crc >> 1); //
                        }
                    }
                }

                byte buf1 = (byte)((crc & 0xff00) >> 8);//高位置
                byte buf2 = (byte)(crc & 0x00ff); //低位置

                crc = (ushort)(buf1 << 8);
                crc += buf2;
                return crc;
            }
            public static ushort crc_16(byte[] data, int len)
            {
                uint IX, IY;
                ushort crc = 0xFFFF;//set all 1

                if (len <= 0)
                    crc = 0;
                else
                {
                    len--;
                    for (IX = 0; IX <= len; IX++)
                    {
                        crc = (ushort)(crc ^ (data[IX]));
                        for (IY = 0; IY <= 7; IY++)
                        {
                            if ((crc & 1) != 0)
                                crc = (ushort)((crc >> 1) ^ 0xA001);
                            else
                                crc = (ushort)(crc >> 1); //
                        }
                    }
                }

                byte buf1 = (byte)((crc & 0xff00) >> 8);//高位置
                byte buf2 = (byte)(crc & 0x00ff); //低位置

                crc = (ushort)(buf1 << 8);
                crc += buf2;
                return crc;
            }
        }

        /// <summary>
        /// 日期转时间戳
        /// </summary>
        /// <param name="datetime">日期时间</param>
        /// <returns></returns>
        public static long DateTimeToStamp(DateTime datetime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, datetime.Kind);
            return Convert.ToInt64((datetime - start).TotalSeconds);
        }

        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime StampToDateTime(long timestamp)
        {
            var datetime = new DateTime();
            var start = new DateTime(1970, 1, 1, 0, 0, 0, datetime.Kind);
            return start.AddSeconds(timestamp);
        }

        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="format">输出格式，DateTimeFormatInfo</param>
        /// <returns></returns>
        public static string StampToString(long timestamp)
        {
            var datetime = new DateTime();
            var start = new DateTime(1970, 1, 1, 0, 0, 0, datetime.Kind);
            return start.AddSeconds(timestamp).ToString("yyyy-MM-dd HH:mm:ss");
        }

        #region 字符串和Byte之间的转化
        /// <summary>
        /// 数字和字节之间互转
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int IntToBitConverter(int num)
        {
            int temp = 0;
            byte[] bytes = BitConverter.GetBytes(num);//将int32转换为字节数组
            temp = BitConverter.ToInt32(bytes, 0);//将字节数组内容再转成int32类型
            return temp;
        }

        /// <summary>
        /// 将字符串转为16进制字符，允许中文
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode, string spanString)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16) + spanString;
            }
            return result;
        }
        /// <summary>
        /// 将16进制字符串转为字符串
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }
        /// <summary>
        /// byte[]转为16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 将16进制的字符串转为byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToHexByte(string hexString)
        {
            byte[] returnBytes = new byte[1];
            try
            {
                hexString = hexString.Trim();
                if ((hexString.Length % 2) != 0)
                    hexString += " ";
                returnBytes = new byte[hexString.Length / 2];
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            catch
            {

                //throw;
            }

            return returnBytes;
        }

        #endregion
    }
}
