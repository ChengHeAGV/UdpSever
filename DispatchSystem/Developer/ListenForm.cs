using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DispatchSystem.Developer
{
    public partial class ListenForm : Form
    {
        public ListenForm()
        {
            InitializeComponent();
        }

        private List<double> listX = new List<double>();
        private List<double> listY = new List<double>();
        private List<double> listY2 = new List<double>();
        private void ListenForm_Load(object sender, EventArgs e)
        {

            Series s1 = new Series();
            Series s2 = new Series();
            chart1.Series.Clear();
            chart1.Series.Add(s1);
            chart1.Series.Add(s2);
            //标题
            chart1.Titles.Add("服务器监控");
            chart1.Titles[0].ForeColor = Color.Black;
            chart1.Titles[0].Font = new Font("微软雅黑", 18f, FontStyle.Regular);
            chart1.Titles[0].Alignment = ContentAlignment.TopCenter;

            //X轴标签间距
            chart1.ChartAreas[0].AxisX.Interval = 1;
            //Y轴标签间距
            chart1.ChartAreas[0].AxisY.Interval = 5;
            //X轴网络线条
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray; ;
            //Y坐标轴标题
            chart1.ChartAreas[0].AxisY.Title = "带宽(Byte)";
            chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Rotated270;
            //Y轴网格线条
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

            #region 发送流量
            //名称
            chart1.Series[0].Name = "发送流量";
            //图颜色
            chart1.Series[0].Color = Color.FromArgb(255, Color.DodgerBlue);
            //图类型
            chart1.Series[0].ChartType = SeriesChartType.SplineArea;
            #endregion
            #region 接收流量
            //名称
            chart1.Series[1].Name = "接收流量";
            //图颜色
            chart1.Series[1].Color = Color.FromArgb(255, Color.Fuchsia);
            //图类型
            chart1.Series[1].ChartType = SeriesChartType.SplineArea;
            #endregion
            for (int i = 0; i < 20; i++)
            {
                listX.Add(i);
                listY.Add(0);
                listY2.Add(0);
            }
            timer1.Enabled = true;
        }

        UInt64 rx = 0, tx = 0;

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (buttonRun.Text=="暂停")
            {
                buttonRun.Text = "运行";
                timer1.Enabled = false;
            }
            else
            {
                buttonRun.Text = "暂停";
                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UInt64 temp1 = (UInt64)(UdpSever.RxLength - rx);
            listY.RemoveAt(0);
            listY.Add(temp1);
            chart1.Series[0].Points.DataBindXY(listX, listY);
            chart1.Series[0].Name = string.Format("接收:{0}", temp1);

            rx = UdpSever.RxLength;

            UInt64 temp2 = (UInt64)((UdpSever.TxLength - tx));
            listY2.RemoveAt(0);
            listY2.Add(UdpSever.TxLength - tx);
            chart1.Series[1].Points.DataBindXY(listX, listY2);
            tx = UdpSever.TxLength;

            chart1.Series[1].Name = string.Format("发送:{0}", temp2);

            //搜索最大值
            //Y轴标签间距
            chart1.ChartAreas[0].AxisY.Interval = listY.Max() > listY2.Max() ? listY.Max() / 20 : listY2.Max() / 20;
        }
    }
}
