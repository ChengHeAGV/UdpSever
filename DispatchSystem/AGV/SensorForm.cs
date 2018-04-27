using System;
using System.Windows.Forms;

namespace DispatchSystem
{
    public partial class SensorForm : Form
    {
        int deviceNum = 0;
        public SensorForm(int num)
        {
            deviceNum = num;
            InitializeComponent();
        }

        private void SensorForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("AGV{0}-传感器", deviceNum);
            Updata();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Updata();
        }

        private void Updata()
        {
            rfid1.Value = (int)UdpSever.Register[deviceNum, 66, 0];
            magneticGuide16Bit1.Value = (int)UdpSever.Register[deviceNum, 59, 0];
            magneticGuide16Bit2.Value = (int)UdpSever.Register[deviceNum, 60, 0];
            magneticGuide16Bit3.Value = (int)UdpSever.Register[deviceNum, 61, 0];
            magneticGuide16Bit4.Value = (int)UdpSever.Register[deviceNum, 62, 0];
        }
    }
}
