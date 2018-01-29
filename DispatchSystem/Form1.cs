using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace DispatchSystem
{

    public partial class Form1 : Form
    {
        IPEndPoint ip;
        static Socket server;
        Thread thread1;
        Thread threadCheckConnet;//更新设备在线状态
        public static UInt32[,,] Ddata = new UInt32[100, 500, 2];//100个设备，每个设备500个寄存器地址，每个寄存器地址对应数据和时间戳
        public static EndPoint[] Dip = new EndPoint[100];
        public static int serverStation = 1;
        static int RxLength = 0;
        static int TxLength = 0;
        int ErrorLength = 0;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            listViewUser.FullRowSelect = true;//要选择就是一行
            listViewUser.Columns.Add("设备", 60, HorizontalAlignment.Center);
            listViewUser.Columns.Add("IP", 240, HorizontalAlignment.Center);
            listViewUser.Columns.Add("更新时间", 280, HorizontalAlignment.Center);
            listViewUser.Columns.Add("空闲时间", 100, HorizontalAlignment.Center);

            byte[] data = { 0x01, 0x03, 0x04, 0x44, 0x99, 0x9E, 0x14 };
            ushort data2 = CRC.crc_16(data);
        }

        public void addText(string str)
        {
            try
            {
                if (checkBox1.CheckState == CheckState.Checked)
                {
                    textBox3.AppendText(str);     // 追加文本，并且使得光标定位到插入地方。
                    textBox3.ScrollToCaret();
                }
            }
            catch
            {
            }

        }

        //启动按钮
        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonStart.Text == "启动")
                {
                    ip = new IPEndPoint(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    server.Bind(ip);
                    addText(string.Format("This Is A UDP Server\r\nHost Name Is {0}\r\n", Dns.GetHostName()));
                    addText("Waiting For Client\r\n");
                    thread1 = new Thread(th1);
                    thread1.Start();

                    //启动检测连接进程
                    threadCheckConnet = new Thread(CheckConnet);
                    threadCheckConnet.IsBackground = true;
                    threadCheckConnet.Start();

                    //通讯设置失效
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    //按钮功能变化
                    buttonStart.Text = "停止";
                    buttonStart.ForeColor = Color.Red;
                }
                else
                {

                    // con.ServerStop();
                    thread1.Abort();
                    listViewUser.Items.Clear();
                    server.Close();
                    server.Dispose();
                    //con.ServerStop();
                    //注销检测连接进程
                    threadCheckConnet.Abort();
                    
                    //通讯设置恢复
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    //按钮功能变化
                    buttonStart.Text = "启动";
                    buttonStart.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        //清空数据
        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }

        struct pointData
        {
            public EndPoint remote;
            public byte[] recv;
            public int length;
        };

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


        //启动监听
        private void th1()
        {
            pointData pd = new pointData();
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);
            while (true)
            {
                try
                {
                    Thread.Sleep(1);
                    labelTx.Refresh();
                    //从缓冲区读取数据
                    byte[] bytes = new byte[1000];
                    int length = server.ReceiveFrom(bytes, ref Remote);
                    RxLength += length;
                    labelRX.Text = string.Format("接收数据：{0}", RxLength);
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
                                pd = new pointData();
                                pd.length = L;
                                pd.remote = Remote;
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
                                        pd = new pointData();
                                        pd.length = L;
                                        pd.remote = Remote;
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
                                        pd = new pointData();
                                        pd.length = L;
                                        pd.remote = Remote;
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
                                            addText(string.Format("主进程，校验失败：{0}\r\n", byteToHexString(temp)));
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
                                        pd = new pointData();
                                        pd.length = L;
                                        pd.remote = Remote;
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
                                        pd = new pointData();
                                        pd.length = L;
                                        pd.remote = Remote;
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
                                            addText(string.Format("主进程，校验失败：{0}\r\n", byteToHexString(temp)));
                                        }

                                        #endregion
                                        break;
                                    default:
                                        ErrorLength += length;
                                        label_Error.Text = string.Format("错误数据：{0}", ErrorLength);
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
                                        pd = new pointData();
                                        pd.length = L;
                                        pd.remote = Remote;
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
                                        pd = new pointData();
                                        pd.length = L;
                                        pd.remote = Remote;
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
                                        pd = new pointData();
                                        pd.length = L;
                                        pd.remote = Remote;
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
                                        pd = new pointData();
                                        pd.length = L;
                                        pd.remote = Remote;
                                        pd.recv = tempByte;

                                        //发送到子进程处理
                                        clientThread = new Thread(new ParameterizedThreadStart(ThreadFunc));
                                        clientThread.IsBackground = true;
                                        clientThread.Start(pd);
                                        #endregion
                                        break;
                                    default:
                                        ErrorLength += length;
                                        label_Error.Text = string.Format("错误数据：{0}", ErrorLength);
                                        length = 0;
                                        break;
                                }
                                break;
                            default:
                                ErrorLength += length;
                                label_Error.Text = string.Format("错误数据：{0}", ErrorLength);
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
        private void ThreadFunc(object obj)
        {
            try
            {
                // Thread.Sleep(10);
                byte[] bytes = new byte[1024];
                pointData pd = (pointData)obj;
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
                        string SrcAddress = (pd.recv[0] << 8 | pd.recv[1]).ToString();
                        //目标地址
                        string DstAddress = (pd.recv[3] << 8 | pd.recv[4]).ToString();

                        if (DstAddress != serverStation.ToString())
                        {
                            //目标地址不是本机，转发数据到目标地址
                            //判断目标地址是否在线
                            if (Dip[int.Parse(SrcAddress)] != null)
                            {
                                sendToUdp(Dip[int.Parse(DstAddress)], pd.recv);
                                string str = "时间：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond + "\r\n";
                                str += "设备地址：" + SrcAddress + "，目标地址：" + DstAddress + "\r\n";
                                str += byteToHexString(pd.recv) + "\r\n";
                                str += "\r\n";
                                addText(str);
                            }
                            pd.recv[3] = 1;//将目标地址改为本机，更新本机数组内容
                        }
                        //else
                        //{
                        switch (pd.recv[2])//帧类型
                        {
                            case 0:
                                #region 心跳00
                                //存储设备端口信息到Dip
                                Dip[int.Parse(SrcAddress)] = pd.remote;
                                //获取设备index
                                int index = Helper.GetIndex(listViewUser, SrcAddress, 0);

                                if (index >= 0)
                                {
                                    //存在更新
                                    listViewUser.Items[index].SubItems[2].Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                                    listViewUser.Items[index].SubItems[3].Text = "0";
                                    listViewUser.Items[index].BackColor = Color.Green;
                                }
                                else
                                {
                                    //不存在增加
                                    ListViewItem lvi1 = Helper.listAdd(SrcAddress, pd.remote.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "0");
                                    lvi1.BackColor = Color.Green;
                                    listViewUser.Items.Add(lvi1);
                                    //对列表进行排序
                                    this.listViewUser.ListViewItemSorter = new ListViewItemComparer();
                                    listViewUser.Sort();
                                }
                                #endregion
                                break;
                            case 1:
                                #region 操作帧01
                                switch (pd.recv[5])//功能码
                                {
                                    case 1:
                                        #region 读单个寄存器01  
                                        //判断目标地址是否在线
                                        if (Dip[int.Parse(SrcAddress)] != null)
                                        {
                                            byte[] sendbyte = new byte[10];

                                            sendbyte[0] = (byte)(serverStation >> 8);
                                            sendbyte[1] = (byte)(serverStation);
                                            sendbyte[2] = 0x02;
                                            sendbyte[3] = (byte)(int.Parse(SrcAddress) >> 8);
                                            sendbyte[4] = (byte)(int.Parse(SrcAddress));
                                            sendbyte[5] = 0x01;
                                            sendbyte[6] = (byte)(pd.recv[6] >> 8);
                                            sendbyte[7] = (byte)(pd.recv[7]);

                                            sendbyte[8] = (byte)(Ddata[int.Parse(SrcAddress), (pd.recv[6] << 8 | pd.recv[7]), 0] >> 8);
                                            sendbyte[9] = (byte)(Ddata[int.Parse(SrcAddress), (pd.recv[6] << 8 | pd.recv[7]), 0]);
                                            int crcRes = CRC.crc_16(sendbyte);

                                            byte[] sendbyte1 = new byte[12];
                                            for (int i = 0; i < 10; i++)
                                            {
                                                sendbyte1[i++] = sendbyte[i];
                                            }

                                            sendbyte1[10] = (byte)(crcRes >> 8);
                                            sendbyte1[11] = (byte)(crcRes);
                                            sendToUdp(Dip[int.Parse(SrcAddress)], sendbyte1);
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
                                        if (Dip[int.Parse(SrcAddress)] != null)
                                        {
                                            byte[] sendbyte = new byte[8 + pd.recv[8] * 2];

                                            sendbyte[0] = (byte)(serverStation >> 8);
                                            sendbyte[1] = (byte)(serverStation);
                                            sendbyte[2] = 0x02;
                                            sendbyte[3] = (byte)(int.Parse(SrcAddress) >> 8);
                                            sendbyte[4] = (byte)(int.Parse(SrcAddress));
                                            sendbyte[5] = 0x03;
                                            sendbyte[6] = (byte)(pd.recv[6] >> 8);
                                            sendbyte[7] = (byte)(pd.recv[7]);

                                            for (int i = 0; i < pd.recv[8]; i++)
                                            {
                                                sendbyte[8 + 2 * i] = (byte)(Ddata[int.Parse(SrcAddress), (pd.recv[6] << 8 | pd.recv[7]) + i, 0] >> 8);
                                                sendbyte[9 + 2 * i] = (byte)(Ddata[int.Parse(SrcAddress), (pd.recv[6] << 8 | pd.recv[7]) + i, 0]);
                                            }

                                            int crcRes = CRC.crc_16(sendbyte);

                                            byte[] sendbyte1 = new byte[8 + pd.recv[8] * 2 + 2];
                                            for (int i = 0; i < sendbyte.Length; i++)
                                            {
                                                sendbyte1[i++] = sendbyte[i];
                                            }

                                            sendbyte1[sendbyte.Length] = (byte)(crcRes >> 8);
                                            sendbyte1[sendbyte.Length + 1] = (byte)(crcRes);
                                            sendToUdp(Dip[int.Parse(SrcAddress)], sendbyte1);
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
                        addText(string.Format("功能进程，校验失败：{0}\r\n", byteToHexString(pd.recv)));

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

        //检测连接进程
        private void CheckConnet()
        {
            while (true)
            {
                Thread.Sleep(1000);
                for (int i = 0; i < listViewUser.Items.Count; i++)
                {
                    //空闲时间小于5则加一，大于5显示离线
                    int time = int.Parse(listViewUser.Items[i].SubItems[3].Text.ToString());
                    if (time <= 4)
                    {
                        time++;
                        //更新数据
                        listViewUser.Items[i].SubItems[3].Text = time.ToString();
                        listViewUser.Items[i].BackColor = Color.Green;
                    }
                    else
                    {
                        //显示离线
                        listViewUser.Items[i].BackColor = Color.Red;
                        //更新设备端口信息到Dip
                        Dip[int.Parse(listViewUser.Items[i].SubItems[0].Text)] = null;
                    }
                }
                DeleteDuplicate(ref listViewUser);
            }
        }

        public void DeleteDuplicate(ref DoubleBufferListView lv)
        {
            for (int i = 0; i < lv.Items.Count; i++)
            {
                for (int j = i + 1; j < lv.Items.Count; j++)
                {
                    if (lv.Items[i].SubItems[0].Text == lv.Items[j].SubItems[0].Text)
                    {
                        lv.Items[j].Remove();
                        j--;//执行删除操作后j--,中和掉j++，让j的索引保持在原位，进行下一次比较，以免删除不干净
                    }
                }
            }
        }


        //发送数据
        private void button2_Click(object sender, EventArgs e)
        {
            //if (textBox8.Text != string.Empty && textBox5.Text != string.Empty)
            //    sendToUdp(textBox8.Text, textBox5.Text);
        }

        public static void writeWord(int dstAdress, int dstID, int data)
        {
            byte[] sendbyte = new byte[10];

            sendbyte[0] = (byte)(serverStation >> 8);
            sendbyte[1] = (byte)(serverStation);
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
            sendToUdp(Dip[dstAdress], sendbyte1);
        }

        /// <summary>
        /// 发送到目标ip
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
                    server.SendTo(str, EndPort);
                }
                else
                {
                    MessageBox.Show("请选择目标客户端");
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message.ToString());
                //addText("转发失败，设备不在线！");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = GetIp();
            if (textBox1.Text.Length < 5)
            {
                textBox1.Text = "192.168.2.23";
            }

            //ip = new IPEndPoint(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
            //server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //server.Bind(ip);
            //addText(string.Format("This Is A UDP Server\r\nHost Name Is {0}\r\n", Dns.GetHostName()));
            //addText("Waiting For Client\r\n");
            //thread1 = new Thread(th1);
            //thread1.Start();

            ////启动检测连接进程
            //threadCheckConnet = new Thread(CheckConnet);
            //threadCheckConnet.IsBackground = true;
            //threadCheckConnet.Start();
        }

        #region 获取本机IP
        // 尝试Ping指定IP是否能够Ping通
        public static bool IsPingIP(string strIP)
        {
            try
            {
                //创建Ping对象
                Ping ping = new Ping();
                //接受Ping返回值
                PingReply reply = ping.Send(strIP, 1000);
                //Ping通
                return true;
            }
            catch
            {
                //Ping失败
                return false;
            }
        }
        //得到网关地址
        public static string GetGateway()
        {
            //网关地址
            string strGateway = "";
            //获取所有网卡
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //遍历数组
            foreach (var netWork in nics)
            {
                //单个网卡的IP对象
                IPInterfaceProperties ip = netWork.GetIPProperties();
                //获取该IP对象的网关
                GatewayIPAddressInformationCollection gateways = ip.GatewayAddresses;
                foreach (var gateWay in gateways)
                {
                    //如果能够Ping通网关
                    if (IsPingIP(gateWay.Address.ToString()))
                    {
                        //得到网关地址
                        strGateway = gateWay.Address.ToString();
                        //跳出循环
                        break;
                    }
                }
                //如果已经得到网关地址
                if (strGateway.Length > 0)
                {
                    //跳出循环
                    break;
                }
            }
            //返回网关地址
            return strGateway;
        }
        //得到IP地址
        public static string GetIp()
        {
            string IPname = "";
            try
            {
                string name = Dns.GetHostName();
                IPAddress[] ips;
                ips = Dns.GetHostAddresses(name);

                string temp = GetGateway();
                string gateway = string.Empty;
                int num = 0;
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp.Substring(i, 1) == ".")
                    {
                        num += 1;
                        if (num == 3)
                            i = temp.Length;
                    }
                    if (num != 3)
                        gateway += temp.Substring(i, 1);
                }
                for (int i = 0; i < ips.Length; i++)
                {
                    if (ips[i].ToString().StartsWith(gateway))
                    {
                        IPname += ips[i];
                        i = ips.Length;
                    }
                }
            }
            catch
            { }
            return IPname;
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        #region mysql
        /// <summary>
        /// 建立mysql数据库链接
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection getMySqlCon()
        {
            String mysqlStr = "Database=webset;Data Source=120.27.45.38;User Id=root;Password=root;pooling=false;CharSet=utf8;port=3306";
            // String mySqlCon = ConfigurationManager.ConnectionStrings["MySqlCon"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mysqlStr);
            return mysql;
        }
        /// <summary>
        /// 建立执行命令语句对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="mysql"></param>
        /// <returns></returns>
        public static MySqlCommand getSqlCommand(String sql, MySqlConnection mysql)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mysql);
            //  MySqlCommand mySqlCommand = new MySqlCommand(sql);
            // mySqlCommand.Connection = mysql;
            return mySqlCommand;
        }
        /// <summary>
        /// 查询并获得结果集并遍历
        /// </summary>
        /// <param name="mySqlCommand"></param>
        public static void getResultset(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            try
            {
                string rs = string.Empty;
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        rs += reader.GetInt32(0) + "  ";
                        for (int i = 1; i < reader.FieldCount; i++)
                            rs += reader.GetString(i) + "  ";
                        rs += "\r\n";
                    }
                }
                MessageBox.Show(rs);
            }
            catch (Exception)
            {

                Console.WriteLine("查询失败了！");
            }
            finally
            {
                reader.Close();
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="mySqlCommand"></param>
        public static void getInsert(MySqlCommand mySqlCommand)
        {
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                MessageBox.Show(message);
                Console.WriteLine("插入数据失败了！" + message);
            }

        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="mySqlCommand"></param>
        public static void getUpdate(MySqlCommand mySqlCommand)
        {
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                String message = ex.Message;
                Console.WriteLine("修改数据失败了！" + message);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="mySqlCommand"></param>
        public static void getDel(MySqlCommand mySqlCommand)
        {
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                Console.WriteLine("删除数据失败了！" + message);
            }
        }

        //用户上传数据，并插入到尾部（data15）
        private void userUpdate(string userid, string sensorid, string data)
        {
            try
            {
                //创建数据库连接对象
                MySqlConnection mysql = getMySqlCon();

                //打开数据库
                mysql.Open();

                //查询用户的设备id是否存在
                String sqlSearch = "select * from usersensor where userid='" + userid + "' and sensorid = '" + sensorid + "' limit 0,1";
                MySqlCommand mySqlCommand = getSqlCommand(sqlSearch, mysql);

                MySqlDataReader reader = mySqlCommand.ExecuteReader();

                string[] update = new string[15];

                if (reader.Read())//存在，更新数据，不存在不处理
                {
                    if (reader.HasRows)
                    {
                        for (int i = 6; i < 20; i++)
                            update[i - 6] = reader.GetString(i);
                    }
                    reader.Close();
                    //将最新的数据添加到队列尾部
                    update[14] = data;

                    //修改sql
                    String sqlUpdate = "update usersensor set data1='" + update[0] + "',data2='" + update[1] + "',data3='" + update[2] + "', data4='" + update[3] + "' ,data5='" + update[4] + "', data6='" + update[5] + "', data7='" + update[6] + "', data8='" + update[7] + "', data9='" + update[8] + "', data10='" + update[9] + "', data11='" + update[10] + "' ,data12='" + update[11] + "', data13='" + update[12] + "',data14='" + update[13] + "',data15='" + update[14] + "'where userid='" + userid + "' and sensorid = '" + sensorid + "'";
                    mySqlCommand = getSqlCommand(sqlUpdate, mysql);
                    mySqlCommand.ExecuteNonQuery();
                }
                //关闭
                mysql.Close();
            }
            catch
            { }
        }
        #endregion

        public static int DataFormNum = 0;
        private void listViewUser_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(listViewUser.SelectedItems[0].SubItems[0].ToString());
            DataFormNum = int.Parse(listViewUser.SelectedItems[0].SubItems[0].Text);
        }

        private void listViewUser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (DataFormNum > 0)
            {
                Form f = new DataForm();
                f.Show();
                DataFormNum = 0;
            }
        }

        //开机启动
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                MessageBox.Show("已经设置为开机自启动，", "提示");
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("JcShutdown", path);
                rk2.Close();
                rk.Close();
            }
            else //取消开机自启动  
            {
                MessageBox.Show("取消开机自启动", "提示");
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue("JcShutdown", false);
                rk2.Close();
                rk.Close();
            }
        }

    }

    /// <summary>
    /// Description:ListView控件排序比较器
    /// </summary>
    public class ListViewItemComparer : IComparer
    {
        private int col;
        public int Compare(object x, object y)
        {
            int returnVal = -1;
            returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
            ((ListViewItem)y).SubItems[col].Text);
            return returnVal;
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



    /// <summary>
    /// 通信协议类
    /// </summary>
    public class DeviceHelper
    {
        public string type;//消息类型
        public string userid;//用户ID
        public string deviceid;//设备ID
        public string state;   //设备状态
    }


    public class Helper
    {
        /// <summary>
        /// 转对象到JSON字符串
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="deviceid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string SetJson(string type, string userid, string deviceid, string state)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            //js.Serialize();
            DeviceHelper info = new DeviceHelper();
            info.type = type;
            info.userid = userid;
            info.deviceid = deviceid;
            info.state = state;
            //转为json字符串
            string dd = js.Serialize(info);
            return dd;
        }
        /// <summary>
        /// 转对象到JSON字符串
        /// </summary>
        /// <param name="deviceHelper">DeviceHelper</param>
        /// <returns></returns>
        public static string SetJson(DeviceHelper deviceHelper)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            //转为json字符串
            string dd = js.Serialize(deviceHelper);
            return dd;
        }
        /// <summary>
        /// 转JSON字符串为对象
        /// </summary>
        /// <param name="src">json格式的字符串</param>
        /// <returns></returns>
        public static DeviceHelper GetJson(string src)
        {
            DeviceHelper inf;
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                inf = new DeviceHelper();
                inf = js.Deserialize<DeviceHelper>(src);
                return inf;
            }
            catch
            {
                inf = new DeviceHelper();
                return inf;
            }
        }

        /// <summary>
        /// 获取记录索引
        /// </summary>
        /// <param name="listview">待搜索的数据列表</param>
        /// <param name="keyValue">需要搜索的关键值，默认是第0个值</param>
        /// <returns></returns>

        public static int GetIndex(ListView listview, string keyValue)
        {
            ListViewItem li;
            try
            {
                li = listview.Items.Cast<ListViewItem>().First(x => x.Text == keyValue);
                return li.Index;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取记录索引
        /// </summary>
        /// <param name="listview"></param>
        /// <param name="keyValue"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetIndex(ListView listview, string keyValue, int index = 0)
        {
            ListViewItem li;
            try
            {
                if (index == 0)
                {
                    li = listview.Items.Cast<ListViewItem>().First(x => x.Text == keyValue);
                }
                else
                {
                    li = listview.Items.Cast<ListViewItem>().First(x => x.SubItems[index].Text == keyValue);
                }
                return li.Index;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="listview">待搜索的数据列表</param>
        /// <param name="keyValue">需要搜索的关键值，默认是第0个值</param>
        /// <returns></returns>

        public static ListViewItem GetItem(ListView listview, string keyValue)
        {
            ListViewItem li;
            try
            {
                li = listview.Items.Cast<ListViewItem>().First(x => x.Text == keyValue);
                return li;
            }
            catch
            {
                li = new ListViewItem();
                return li;
            }
        }

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="listview"></param>
        /// <param name="keyValue"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ListViewItem GetItem(ListView listview, string keyValue, int index = 0)
        {
            ListViewItem li;
            try
            {
                if (index == 0)
                {
                    li = listview.Items.Cast<ListViewItem>().First(x => x.Text == keyValue);
                }
                else
                {
                    li = listview.Items.Cast<ListViewItem>().First(x => x.SubItems[index].Text == keyValue);
                }
                return li;
            }
            catch
            {
                li = new ListViewItem();
                return li;
            }
        }


        //增加记录
        //listViewUser.Items.Add(Helper.listAdd("192.168.0.0", "123456789"));
        public static ListViewItem listAdd(string userName, string userId, string IP, string responseData = "")
        {
            ListViewItem item = new ListViewItem();
            item.Text = userName;
            item.SubItems.Add(userId);
            item.SubItems.Add(IP);
            item.SubItems.Add(responseData);
            return item;
        }
    }
}
