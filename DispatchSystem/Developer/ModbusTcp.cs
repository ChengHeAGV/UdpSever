using Modbus.Device;
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

namespace DispatchSystem.Developer
{
    public partial class ModbusTcp : Form
    {
        public ModbusTcp()
        {
            InitializeComponent();
        }
        ModbusIpMaster modbusMaster;
        Thread modbusThread;
        private void ModbusTcp_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 32; i++)
            {
                dataGridView1.Rows.Add();
            }
            #region 启动ModbusTcp
            try
            {
                TcpClient tcpClient = new TcpClient("192.168.250.102", 502);
                modbusMaster = ModbusIpMaster.CreateIp(tcpClient);
                modbusMaster.Transport.WriteTimeout = 1000;//写超时
                modbusMaster.Transport.ReadTimeout = 1000;//读超时
                modbusMaster.Transport.WaitToRetryMilliseconds = 20;//重试等待时间
                modbusMaster.Transport.Retries = 3;//重试次数

                //启动监听进程
                modbusThread = new Thread(new ThreadStart(SyncModbus));
                modbusThread.Start();
            }
            catch
            {
                MessageBox.Show("连接Modbus设备失败！");
            }
            #endregion
        }


        ushort[] ModbusBuf = new ushort[32];
        /// <summary>
        /// 同步Modbus设备数据
        /// </summary>
        private void SyncModbus()
        {
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    //读取Modbus寄存器
                    ModbusBuf = modbusMaster.ReadHoldingRegisters(0, 32);

                    for (int i = 0; i < 32; i++)
                    {
                        var index = i;
                        //序号
                        dataGridView1.Rows[index].Cells[0].Value = i;
                        //地址
                        dataGridView1.Rows[index].Cells[1].Value = i;
                        //数据类型
                        dataGridView1.Rows[index].Cells[2].Value = "Word";
                        //读写性
                        dataGridView1.Rows[index].Cells[3].Value ="Read/Write";
                        //数值
                        dataGridView1.Rows[index].Cells[4].Value = ModbusBuf[i];
                        //更新时间
                        dataGridView1.Rows[index].Cells[5].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //更新次数
                        dataGridView1.Rows[index].Cells[5].Value = "N";
                    }

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
    }
}
