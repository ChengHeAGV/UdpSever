using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem
{
    class UdpSever
    {
        //服务器IP
        public static IPAddress ipaddress;
        //服务器端口
        public static int port;

        //服务器启动停止状态，默认停止
        public static bool State = false;

        //接收及发送的数据总长度
        public static int RxLength;
        public static int TxLength;

        //初始化设备数
        public static int DeviceNum = 20;
        //初始化寄存器数
        public static int RegisterNum = 100;
        //设备，寄存器，数据和时间戳
        public static UInt32[,,] Ddata = new UInt32[100, 500, 2];

        //设备端口及IP
        public static EndPoint[] EndPointArray = new EndPoint[DeviceNum];

        //设备更新时间(最后一次收到数据时间)
        public static DateTime[] UpdateTime = new DateTime[DeviceNum];

        //服务器地址
        public static int ServerAddress = 1;

        //设备心跳周期(单位：秒)
        public static int HeartCycle = 5;

        //数据接收结构体
        struct PointData
        {
            public EndPoint endpoint;
            public byte[] recv;
            public int length;
        };

        //监听服务总进程
        static Thread threadMain;

        //函数返回结构体
        public struct Resault
        {
            public bool Reault;
            public string Message;
        }

        /// <summary>
        /// 启动UDP服务器
        /// </summary>
        static IPEndPoint ipEndPoint;
        static Socket socket;
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

                threadMain = new Thread(mainFunc);
                threadMain.Start();

                //启动检测连接进程
                State = true;//更新服务器状态
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
                //注销检测连接进程
                threadMain.Abort();
                socket.Close();
                State = false;//更新服务器状态 

                rs.Reault = true;
                rs.Message = "停止成功";
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
            PointData pd = new PointData();
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);
            while (true)
            {
                try
                {
                    Thread.Sleep(1);
                    // labelTx.Refresh();
                    //从缓冲区读取数据
                    byte[] bytes = new byte[1000];
                    int length = socket.ReceiveFrom(bytes, ref Remote);
                    RxLength += length;
                    // labelRX.Text = string.Format("接收数据：{0}", RxLength);
                    byte[] temp = new byte[length];
                    for (int i = 0; i < length; i++)
                    {
                        temp[i] = bytes[i];
                    }

                    //显示为16进制数
                    //addText("收到数据：\r\n");
                    //addText(byteToHexString(temp) + "\r\n");
                    //解析数据，对数据进行分包
                    int num = 0;
                    int L = 0;
                    while (length >= 7)
                    {
                        //判断帧类型
                        switch (temp[2 + num])
                        {
                            case 0://心跳帧
                                L = 7;//长度固定为 7
                                byte[] tempByte = new byte[L];
                                for (int i = 0; i < L; i++)
                                {
                                    tempByte[i] = temp[i + num];
                                }
                                num += L;
                                length -= L;
                                pd = new PointData();
                                pd.length = L;
                                pd.endpoint = Remote;
                                pd.recv = tempByte;

                                //发送到子进程处理
                                Thread clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                clientThread.IsBackground = true;
                                clientThread.Start(pd);
                                break;
                            case 1://操作帧
                                switch (temp[5 + num])//功能码
                                {
                                    case 1:
                                        #region 读单个寄存器
                                        L = 10;
                                        tempByte = new byte[L];
                                        for (int i = 0; i < L; i++)
                                        {
                                            tempByte[i] = temp[i + num];
                                        }
                                        num += L;
                                        length -= L;
                                        pd = new PointData();
                                        pd.length = L;
                                        pd.endpoint = Remote;
                                        pd.recv = tempByte;

                                        //发送到子进程处理
                                        clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                        clientThread.IsBackground = true;
                                        clientThread.Start(pd);
                                        #endregion
                                        break;
                                    case 2:
                                        #region 写单个寄存器
                                        L = 12;
                                        tempByte = new byte[L];
                                        for (int i = 0; i < L; i++)
                                        {
                                            tempByte[i] = temp[i + num];
                                        }
                                        num += L;
                                        length -= L;
                                        pd = new PointData();
                                        pd.length = L;
                                        pd.endpoint = Remote;
                                        pd.recv = tempByte;

                                        //发送到子进程处理
                                        clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                        clientThread.IsBackground = true;
                                        clientThread.Start(pd);

                                        //1.CRC校验
                                        byte[] tempb = new byte[pd.length - 2];
                                        for (int i = 0; i < pd.length - 2; i++)
                                        {
                                            tempb[i] = pd.recv[i];
                                        }
                                        int crc16 = CRC.crc_16(tempb);
                                        if (crc16 == (pd.recv[pd.length - 2] << 8 | pd.recv[pd.length - 1]))
                                        {

                                        }
                                        else
                                        {
                                            //校验失败
                                            //addText(string.Format("主进程，校验失败：{0}\r\n", byteToHexString(temp)));
                                        }

                                        #endregion
                                        break;
                                    case 3:
                                        #region 读多个寄存器
                                        L = 11;
                                        tempByte = new byte[L];
                                        for (int i = 0; i < L; i++)
                                        {
                                            tempByte[i] = temp[i + num];
                                        }
                                        num += L;
                                        length -= L;
                                        pd = new PointData();
                                        pd.length = L;
                                        pd.endpoint = Remote;
                                        pd.recv = tempByte;

                                        //发送到子进程处理
                                        clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                        clientThread.IsBackground = true;
                                        clientThread.Start(pd);
                                        #endregion
                                        break;
                                    case 4://
                                        #region 写多个寄存器
                                        L = 9 + temp[8 + num] * 2 + 2;
                                        tempByte = new byte[L];
                                        for (int i = 0; i < L; i++)
                                        {
                                            tempByte[i] = temp[i + num];
                                        }
                                        num += L;
                                        length -= L;
                                        pd = new PointData();
                                        pd.length = L;
                                        pd.endpoint = Remote;
                                        pd.recv = tempByte;

                                        //发送到子进程处理
                                        clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                        clientThread.IsBackground = true;
                                        clientThread.Start(pd);

                                        //1.CRC校验
                                        tempb = new byte[pd.length - 2];
                                        for (int i = 0; i < pd.length - 2; i++)
                                        {
                                            tempb[i] = pd.recv[i];
                                        }
                                        crc16 = CRC.crc_16(tempb);
                                        if (crc16 == (pd.recv[pd.length - 2] << 8 | pd.recv[pd.length - 1]))
                                        {

                                        }
                                        else
                                        {
                                            //校验失败
                                            //  addText(string.Format("主进程，校验失败：{0}\r\n", byteToHexString(temp)));
                                        }

                                        #endregion
                                        break;
                                    default:
                                        // ErrorLength += length;
                                        // label_Error.Text = string.Format("错误数据：{0}", ErrorLength);
                                        length = 0;
                                        break;
                                }
                                break;
                            case 2://响应帧
                                switch (temp[5 + num])//功能码
                                {
                                    case 1:
                                        #region 读单个寄存器
                                        L = 12;
                                        tempByte = new byte[L];
                                        for (int i = 0; i < L; i++)
                                        {
                                            tempByte[i] = temp[i + num];
                                        }
                                        num += L;
                                        length -= L;
                                        pd = new PointData();
                                        pd.length = L;
                                        pd.endpoint = Remote;
                                        pd.recv = tempByte;

                                        //发送到子进程处理
                                        clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                        clientThread.IsBackground = true;
                                        clientThread.Start(pd);
                                        #endregion
                                        break;
                                    case 2:
                                        #region 写单个寄存器
                                        L = 9;
                                        tempByte = new byte[L];
                                        for (int i = 0; i < L; i++)
                                        {
                                            tempByte[i] = temp[i + num];
                                        }
                                        num += L;
                                        length -= L;
                                        pd = new PointData();
                                        pd.length = L;
                                        pd.endpoint = Remote;
                                        pd.recv = tempByte;

                                        //发送到子进程处理
                                        clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                        clientThread.IsBackground = true;
                                        clientThread.Start(pd);
                                        #endregion
                                        break;
                                    case 3:
                                        #region 读多个寄存器
                                        L = 13;
                                        tempByte = new byte[L];
                                        for (int i = 0; i < L; i++)
                                        {
                                            tempByte[i] = temp[i + num];
                                        }
                                        num += L;
                                        length -= L;
                                        pd = new PointData();
                                        pd.length = L;
                                        pd.endpoint = Remote;
                                        pd.recv = tempByte;

                                        //发送到子进程处理
                                        clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                        clientThread.IsBackground = true;
                                        clientThread.Start(pd);
                                        #endregion
                                        break;
                                    case 4://
                                        #region 写多个寄存器
                                        L = 9;
                                        tempByte = new byte[L];
                                        for (int i = 0; i < L; i++)
                                        {
                                            tempByte[i] = temp[i + num];
                                        }
                                        num += L;
                                        length -= L;
                                        pd = new PointData();
                                        pd.length = L;
                                        pd.endpoint = Remote;
                                        pd.recv = tempByte;

                                        //发送到子进程处理
                                        clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                        clientThread.IsBackground = true;
                                        clientThread.Start(pd);
                                        #endregion
                                        break;
                                    default:
                                        // ErrorLength += length;
                                        // label_Error.Text = string.Format("错误数据：{0}", ErrorLength);
                                        length = 0;
                                        break;
                                }
                                break;
                            default:
                                //ErrorLength += length;
                                //label_Error.Text = string.Format("错误数据：{0}", ErrorLength);
                                length = 0;
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // addText(ex.ToString());
                }
            }

        }

        //功能函数
        private static void ThreadFunc(object obj)
        {
            try
            {
                // Thread.Sleep(10);
                byte[] bytes = new byte[1024];
                PointData pd = (PointData)obj;
                //显示为16进制数
                //addText(byteToHexString(pd.recv) + "\r\n");
                //解析
                if (pd.length >= 7)
                {
                    //1.CRC校验
                    byte[] temp = new byte[pd.length - 2];
                    for (int i = 0; i < pd.length - 2; i++)
                    {
                        temp[i] = pd.recv[i];
                    }
                    int crc16 = CRC.crc_16(temp);
                    if (crc16 == (pd.recv[pd.length - 2] << 8 | pd.recv[pd.length - 1]))
                    {
                        //设备地址
                        int SrcAddress = (pd.recv[0] << 8 | pd.recv[1]);
                        //目标地址
                        int DstAddress = (pd.recv[3] << 8 | pd.recv[4]);

                        if (DstAddress != ServerAddress)
                        {
                            //目标地址不是本机，转发数据到目标地址
                            //判断目标地址是否在线
                            if (EndPointArray[SrcAddress] != null)
                            {
                                sendToUdp(EndPointArray[DstAddress], pd.recv);
                                string str = "时间：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond + "\r\n";
                                str += "设备地址：" + SrcAddress + "，目标地址：" + DstAddress + "\r\n";
                                str += byteToHexString(pd.recv) + "\r\n";
                                str += "\r\n";
                                // addText(str);
                            }
                            pd.recv[3] = (byte)ServerAddress;//将目标地址改为本机，更新本机数组内容
                        }
                        //else
                        //{
                        switch (pd.recv[2])//帧类型
                        {
                            case 0:
                                #region 心跳00
                                //存储设备端口信息到EndPointArray
                                EndPointArray[SrcAddress] = pd.endpoint;
                                //更新设备响应时间
                                UpdateTime[SrcAddress] = DateTime.Now;
                                #endregion
                                break;
                            case 1:
                                #region 操作帧01
                                switch (pd.recv[5])//功能码
                                {
                                    case 1:
                                        #region 读单个寄存器01  
                                        //判断目标地址是否在线
                                        if (EndPointArray[SrcAddress] != null)
                                        {
                                            byte[] sendbyte = new byte[10];

                                            sendbyte[0] = (byte)(ServerAddress >> 8);
                                            sendbyte[1] = (byte)(ServerAddress);
                                            sendbyte[2] = 0x02;
                                            sendbyte[3] = (byte)(SrcAddress >> 8);
                                            sendbyte[4] = (byte)(SrcAddress);
                                            sendbyte[5] = 0x01;
                                            sendbyte[6] = (byte)(pd.recv[6] >> 8);
                                            sendbyte[7] = (byte)(pd.recv[7]);

                                            sendbyte[8] = (byte)(Ddata[SrcAddress, (pd.recv[6] << 8 | pd.recv[7]), 0] >> 8);
                                            sendbyte[9] = (byte)(Ddata[SrcAddress, (pd.recv[6] << 8 | pd.recv[7]), 0]);
                                            int crcRes = CRC.crc_16(sendbyte);

                                            byte[] sendbyte1 = new byte[12];
                                            for (int i = 0; i < 10; i++)
                                            {
                                                sendbyte1[i++] = sendbyte[i];
                                            }

                                            sendbyte1[10] = (byte)(crcRes >> 8);
                                            sendbyte1[11] = (byte)(crcRes);
                                            sendToUdp(EndPointArray[SrcAddress], sendbyte1);
                                        }
                                        #endregion
                                        break;
                                    case 2:
                                        #region 写单个寄存器02
                                        //寄存器地址
                                        Ddata[(pd.recv[0] << 8 | pd.recv[1]), (pd.recv[6] << 8 | pd.recv[7]), 0] = (UInt16)(pd.recv[8] << 8 | pd.recv[9]);

                                        //更新时间戳
                                        DateTime datestart = new DateTime(1970, 1, 1, 8, 0, 0);
                                        Ddata[(pd.recv[0] << 8 | pd.recv[1]), (pd.recv[6] << 8 | pd.recv[7]), 1] = Convert.ToUInt32((DateTime.Now - datestart).TotalSeconds);

                                        #endregion
                                        break;
                                    case 3:
                                        #region 读多个寄存器03
                                        //判断目标地址是否在线
                                        if (EndPointArray[SrcAddress] != null)
                                        {
                                            byte[] sendbyte = new byte[8 + pd.recv[8] * 2];

                                            sendbyte[0] = (byte)(ServerAddress >> 8);
                                            sendbyte[1] = (byte)(ServerAddress);
                                            sendbyte[2] = 0x02;
                                            sendbyte[3] = (byte)(SrcAddress >> 8);
                                            sendbyte[4] = (byte)(SrcAddress);
                                            sendbyte[5] = 0x03;
                                            sendbyte[6] = (byte)(pd.recv[6] >> 8);
                                            sendbyte[7] = (byte)(pd.recv[7]);

                                            for (int i = 0; i < pd.recv[8]; i++)
                                            {
                                                sendbyte[8 + 2 * i] = (byte)(Ddata[SrcAddress, (pd.recv[6] << 8 | pd.recv[7]) + i, 0] >> 8);
                                                sendbyte[9 + 2 * i] = (byte)(Ddata[SrcAddress, (pd.recv[6] << 8 | pd.recv[7]) + i, 0]);
                                            }

                                            int crcRes = CRC.crc_16(sendbyte);

                                            byte[] sendbyte1 = new byte[8 + pd.recv[8] * 2 + 2];
                                            for (int i = 0; i < sendbyte.Length; i++)
                                            {
                                                sendbyte1[i++] = sendbyte[i];
                                            }

                                            sendbyte1[sendbyte.Length] = (byte)(crcRes >> 8);
                                            sendbyte1[sendbyte.Length + 1] = (byte)(crcRes);
                                            sendToUdp(EndPointArray[SrcAddress], sendbyte1);
                                        }
                                        #endregion
                                        break;
                                    case 4:
                                        #region 写多个寄存器04
                                        //判断准备写的寄存器数量是否有效，未限制最大值
                                        if (pd.recv[8] > 0)
                                        {
                                            //将待写入的数据循环写入对应的寄存器地址
                                            for (int i = 0; i < pd.recv[8]; i++)
                                            {
                                                Ddata[(pd.recv[0] << 8 | pd.recv[1]), (pd.recv[6] << 8 | pd.recv[7]) + i, 0] = (UInt16)(pd.recv[9 + 2 * i] << 8 | pd.recv[10 + 2 * i]);
                                                //更新时间戳
                                                datestart = new DateTime(1970, 1, 1, 8, 0, 0);
                                                Ddata[(pd.recv[0] << 8 | pd.recv[1]), (pd.recv[6] << 8 | pd.recv[7]), 1] = Convert.ToUInt32((DateTime.Now - datestart).TotalSeconds);
                                            }
                                        }
                                        #endregion
                                        break;
                                    default:
                                        break;
                                }

                                #endregion
                                break;
                            case 2:
                                #region 响应帧02
                                #endregion
                                break;
                            default:
                                break;
                        }
                        // }
                    }
                    else
                    {
                        //校验失败
                        //  addText(string.Format("功能进程，校验失败：{0}\r\n", byteToHexString(pd.recv)));

                    }
                    // addText("---\r\n\r\n");
                }

            }
            catch (Exception ex)
            {
                //  addText(ex.ToString() + "\r\n");
                //MessageBox.Show(ex.ToString());
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
                        str += " ";
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        public static string byteToHexString(byte[] bytes)
        {
            string str = string.Empty;
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    str += bytes[i].ToString("X2");
                    if (i != bytes.Length - 1)
                    {
                        str += " ";
                    }
                }
            }
            return str;
        }

        public static void writeWord(int dstAdress, int dstID, int data)
        {
            byte[] sendbyte = new byte[10];

            sendbyte[0] = (byte)(ServerAddress >> 8);
            sendbyte[1] = (byte)(ServerAddress);
            sendbyte[2] = 0x01;
            sendbyte[3] = (byte)(dstAdress >> 8);
            sendbyte[4] = (byte)(dstAdress);
            sendbyte[5] = 0x02;
            sendbyte[6] = (byte)(dstID >> 8);
            sendbyte[7] = (byte)(dstID);

            sendbyte[8] = (byte)(data >> 8);
            sendbyte[9] = (byte)(data);
            int crcRes = CRC.crc_16(sendbyte);

            byte[] sendbyte1 = new byte[12];
            for (int i = 0; i < 10; i++)
            {
                sendbyte1[i] = sendbyte[i];
            }

            sendbyte1[10] = (byte)(crcRes >> 8);
            sendbyte1[11] = (byte)(crcRes);
            sendToUdp(EndPointArray[dstAdress], sendbyte1);
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="EndPort">EndPoint</param>
        /// <param name="str">byte[]</param>
        public static void sendToUdp(EndPoint EndPort, byte[] str)
        {
            try
            {
                if (str != null && EndPort.ToString() != null)
                {
                    TxLength += str.Length;
                    //    labelTx.Text = string.Format("发送数据：{0}", TxLength);
                    socket.SendTo(str, EndPort);
                }
                else
                {
                    //MessageBox.Show("请选择目标客户端");
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message.ToString());
                //addText("转发失败，设备不在线！");
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
        }
    }
}
