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

        private void TaskForm_Load(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient("192.168.10.106", 502);
            ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);

            master.Transport.WriteTimeout = 100;
            master.Transport.ReadTimeout = 100;
            master.Transport.WaitToRetryMilliseconds = 10;
            master.Transport.Retries = 3;

            try
            {
                Random rd = new Random();
                ushort[] write = new ushort[10];
                for (int i = 0; i < 10; i++)
                {
                    write[i] = (ushort)rd.Next(1, 1000);
                }
                master.WriteMultipleRegisters(0, write);
                ushort[] read = new ushort[10];
                // read = master.ReadWriteMultipleRegisters(0, 10, 0, write);

                read = master.ReadHoldingRegisters(10, 10);
                UdpSever.Shell.WriteNotice("debug", "{0},{1}", read[0], read[1]);
            }
            catch (Exception ex)
            {
                UdpSever.Shell.WriteError("debug", ex.ToString());
            }

        }
    }
}
