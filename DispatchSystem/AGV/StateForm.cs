using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DispatchSystem.AGV
{
    public partial class StateForm : Form
    {
        int deviceNum = 0;
        Thread threaMain;
        public StateForm(int num)
        {
            deviceNum = num;
            InitializeComponent();
        }

        private void StateForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("AGV{0}-运行状态", deviceNum);
            this.dataGridView1.DefaultCellStyle.Font = new Font("新宋体", 20, FontStyle.Regular); //字体颜色

            for (int i = 0; i < 16; i++)
            {
                dataGridView1.Rows.Add();
                //行样式
                if (i % 2 == 1)
                    this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.WhiteSmoke; //背景色
                else
                    this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White; //背景色
            }

            threaMain = new Thread(new ThreadStart(func));
            threaMain.Start();

        }

        private void func()
        {
            while (close==false)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    var num = 0;
                    var reg = 51;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "电量";
                    dataGridView1.Rows[num].Cells[3].Value = string.Format("{0}%", UdpSever.Register[deviceNum, reg, 0]);

                    num = 1;
                    reg = 52;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "电压";
                    dataGridView1.Rows[num].Cells[3].Value = string.Format("{0}V", UdpSever.Register[deviceNum, reg, 0]);

                    num = 2;
                    reg = 53;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "电池循环次数";
                    dataGridView1.Rows[num].Cells[3].Value = string.Format("{0}次", UdpSever.Register[deviceNum, reg, 0]);

                    num = 3;
                    reg = 54;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "低电压报警阈值";
                    dataGridView1.Rows[num].Cells[3].Value = string.Format("{0}%", UdpSever.Register[deviceNum, reg, 0]);

                    num = 4;
                    reg = 55;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "当前速度档位";
                    switch (UdpSever.Register[deviceNum, reg, 0])
                    {
                        case 0:
                            dataGridView1.Rows[num].Cells[3].Value = "高速";
                            break;
                        case 1:
                            dataGridView1.Rows[num].Cells[3].Value = "中速";
                            break;
                        case 2:
                            dataGridView1.Rows[num].Cells[3].Value = "低速";
                            break;
                        case 3:
                            dataGridView1.Rows[num].Cells[3].Value = "对接速度";
                            break;
                    }

                    num = 5;
                    reg = 56;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "当前模式";
                    dataGridView1.Rows[num].Cells[3].Value = UdpSever.Register[deviceNum, reg, 0] == 0 ? "自动" : "手动";

                    num = 6;
                    reg = 57;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "当前方向";
                    dataGridView1.Rows[num].Cells[3].Value = UdpSever.Register[deviceNum, reg, 0] == 0 ? "前进" : "后退";

                    num = 7;
                    reg = 58;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "当前分叉";

                    switch (UdpSever.Register[deviceNum, reg, 0])
                    {
                        case 1:
                            dataGridView1.Rows[num].Cells[3].Value = "左分叉";
                            break;
                        case 2:
                            dataGridView1.Rows[num].Cells[3].Value = "右分叉";
                            break;
                        case 11:
                            dataGridView1.Rows[num].Cells[3].Value = "前左分叉";
                            break;
                        case 12:
                            dataGridView1.Rows[num].Cells[3].Value = "前左分叉";
                            break;
                        case 21:
                            dataGridView1.Rows[num].Cells[3].Value = "后左分叉";
                            break;
                        case 22:
                            dataGridView1.Rows[num].Cells[3].Value = "后又分叉";
                            break;
                    }

                    num = 8;
                    reg = 59;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "磁导航1";
                    dataGridView1.Rows[num].Cells[3].Value = Convert.ToString(UdpSever.Register[deviceNum, num, 0], 2).PadLeft(16, '0');

                    num = 9;
                    reg = 60;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "磁导航2";
                    dataGridView1.Rows[num].Cells[3].Value = Convert.ToString(UdpSever.Register[deviceNum, num, 0], 2).PadLeft(16, '0');

                    num = 10;
                    reg = 61;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "磁导航3";
                    dataGridView1.Rows[num].Cells[3].Value = Convert.ToString(UdpSever.Register[deviceNum, num, 0], 2).PadLeft(16, '0');

                    num = 11;
                    reg = 62;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "磁导航4";
                    dataGridView1.Rows[num].Cells[3].Value = Convert.ToString(UdpSever.Register[deviceNum, num, 0], 2).PadLeft(16, '0');

                    num = 12;
                    reg = 63;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "急停状态";
                    dataGridView1.Rows[num].Cells[3].Value = UdpSever.Register[deviceNum, reg, 0] == 0 ? "正常" : "急停";

                    num = 13;
                    reg = 64;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "滚筒左侧红外";
                    dataGridView1.Rows[num].Cells[3].Value = UdpSever.Register[deviceNum, reg, 0] == 0 ? "正常" : "触发";

                    num = 14;
                    reg = 65;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "滚筒右侧红外";
                    dataGridView1.Rows[num].Cells[3].Value = UdpSever.Register[deviceNum, reg, 0] == 0 ? "正常" : "触发";

                    num = 15;
                    reg = 8;
                    dataGridView1.Rows[num].Cells[0].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "yyyy - MM - dd");
                    dataGridView1.Rows[num].Cells[1].Value = UdpSever.StampToString(UdpSever.Register[deviceNum, reg, 1], "HH: mm:ss");
                    dataGridView1.Rows[num].Cells[2].Value = "运行状态";
                    switch (UdpSever.Register[deviceNum, reg, 0])
                    {
                        case 0:
                            dataGridView1.Rows[num].Cells[3].Value = "未就绪";
                            break;
                        case 1:
                            dataGridView1.Rows[num].Cells[3].Value = "运行";
                            break;
                        case 2:
                            dataGridView1.Rows[num].Cells[3].Value = "待命";
                            break;
                        case 3:
                            dataGridView1.Rows[num].Cells[3].Value = "充电";
                            break;
                        default:
                            break;
                    }
                }));
                Thread.Sleep(2000);
            }
        }

        bool close = false;
        private void StateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            close = true;
        }
    }
}
