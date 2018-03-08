using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace DispatchSystem.Developer
{
    public partial class DbusTestForm : Form
    {
        int LocalAddress;
        int TargetAddress;
        EndPoint endPoint;
        public DbusTestForm()
        {
            InitializeComponent();
        }

        private void DbusTestForm_Load(object sender, EventArgs e)
        {
            textBoxTargetIp.Text = UdpSever.ipaddress.ToString();

            LocalAddress = int.Parse(textBoxLocalAddress.Text);
            TargetAddress = int.Parse(textBoxTargetAddress.Text);

            IPAddress IPadr = IPAddress.Parse(textBoxTargetIp.Text);//先把string类型转换成IPAddress类型
            endPoint = new IPEndPoint(IPadr, int.Parse(textBoxTargetPort.Text));//传递IPAddress和Port

            //心跳
            Thread th1 = new Thread(Heart);
            th1.Start();
            //写单个寄存器
            Thread th2 = new Thread(WriteRegister);
            th2.Start();
        }



        //心跳
        private void Heart()
        {
            while (true)
            {
                Thread.Sleep(10);
                try
                {
                    if (checkBoxHeart.Checked)
                    {
                        int time = int.Parse(textBoxTimeHeart.Text);
                        if (time > 1)
                        {
                            Thread.Sleep(time);
                        }
                        UdpSever.Heart(LocalAddress, TargetAddress, endPoint);
                        lbLedHeart.Trigger = true;
                    }
                }
                catch //(Exception)
                {

                    // throw;
                }
            }
        }

        //基本参数变化时触发
        private void Base_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LocalAddress = int.Parse(textBoxLocalAddress.Text);
                TargetAddress = int.Parse(textBoxTargetAddress.Text);

                IPAddress IPadr = IPAddress.Parse(textBoxTargetIp.Text);//先把string类型转换成IPAddress类型
                endPoint = new IPEndPoint(IPadr, int.Parse(textBoxTargetPort.Text));//传递IPAddress和Port
            }
            catch
            {
            }
        }

        #region 写单个寄存器
        UInt16 WriterRegisterValue = 0;
        Random rd = new Random();
        string WriteRegisterState = "固定值";
        private void WriteRegister()
        {
            while (true)
            {
                Thread.Sleep(10);
                try
                {
                    if (checkBoxWriteRegister.Checked)
                    {
                        int time = int.Parse(textBoxWriteRegisterTime.Text);
                        if (time > 1)
                        {
                            Thread.Sleep(time);
                        }
                        UInt16 value = 0;
                        switch (WriteRegisterState)
                        {
                            case "固定值":
                                value = UInt16.Parse(textBoxWriteRegisterValue.Text);
                                break;
                            case "随机数":
                                value = (ushort)rd.Next(0, 65535);
                                break;
                            case "顺序循环":
                                value = WriterRegisterValue++;
                                break;
                            default:
                                break;
                        }
                        UdpSever.Post_Register(LocalAddress, TargetAddress, int.Parse(textBoxWriteRegisterAddress.Text), value, endPoint);
                        lbLedWriteRegister.Trigger = true;
                    }
                }
                catch //(Exception)
                {

                    // throw;
                }
            }
        }
        private void comboBoxWriteRegister_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxWriteRegister.Text)
            {
                case "固定值":
                    textBoxWriteRegisterValue.ReadOnly = false;
                    break;
                case "随机数":
                    textBoxWriteRegisterValue.ReadOnly = true;
                    break;
                case "顺序循环":
                    textBoxWriteRegisterValue.ReadOnly = true;
                    WriterRegisterValue = 0;
                    break;
                default:
                    break;
            }
            WriteRegisterState = comboBoxWriteRegister.Text;
        }
        #endregion

    }
}
