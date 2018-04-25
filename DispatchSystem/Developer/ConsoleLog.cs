using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DispatchSystem.Developer
{
    public partial class ConsoleLog : Form
    {
        public delegate void UpdateControlEventHandler(Object sender, EventArgs e, string msg, Color color, int fontSize);
        public static event UpdateControlEventHandler UpdateControl;

        public ConsoleLog()
        {
            InitializeComponent();
        }

        private void ConsoleLog_Load(object sender, EventArgs e)
        {
            //dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Silver;
            //dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

            UpdateControl += new UpdateControlEventHandler(this.Test);  //订阅UpdateControl事件，指定Test方法为事件处理函数
        }

        public static void WriteLog(string msg, Color color, int fontSize = 14)  //假设这个是静态的回调方法
        {
            UpdateControl(new ConsoleLog(), new EventArgs(), msg, color, fontSize);
        }

        public static void WriteLog(string msg)  //假设这个是静态的回调方法
        {
            UpdateControl(new ConsoleLog(), new EventArgs(), msg, Color.Black, 14);
        }
        public static void WriteLog(string format, params object[] args)  //假设这个是静态的回调方法
        {
            UpdateControl(new ConsoleLog(), new EventArgs(), string.Format(format, args), Color.Black, 14);
        }
        private void WriteLogFunc(string msg, Color color, int fontSize)
        {
            try
            {
                var index = 0;
                if (dataGridView1.Rows.Count > 0)
                {
                    index = dataGridView1.Rows.Count - 1;
                }
                else
                    index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = DateTime.Now.ToString("yyyy-MM-dd");
                dataGridView1.Rows[index].Cells[1].Value = DateTime.Now.ToString("HH:mm:ss");
                dataGridView1.Rows[index].Cells[2].Value = msg;

                //行样式
                if (index % 2 == 1)
                    this.dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.WhiteSmoke; //背景色
                else
                    this.dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.White; //背景色

                this.dataGridView1.Rows[index].DefaultCellStyle.ForeColor = color; //字体颜色
                this.dataGridView1.Rows[index].DefaultCellStyle.Font = new Font("新宋体", fontSize, FontStyle.Regular); //字体颜色
                                                                                                                     //this.dataGridView1.Rows[index].Cells[2].Style.Font = new Font("新宋体", 20, FontStyle.Regular); //字体颜色
                dataGridView1.Rows.Add();
                //滚动到最后一行
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            }
            catch
            {

            }

        }

        public void Test(Object o, EventArgs e, string msg, Color color, int fontSize)  //事件处理函数，用来更新控件
        {
            if (dataGridView1.InvokeRequired)
            {
                // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                Action<string, Color, int> actionDelegate = (_msg, _color, _fontSize) =>
                {
                    WriteLogFunc(_msg, _color, _fontSize);
                };
                dataGridView1.BeginInvoke(actionDelegate, msg, color, fontSize);
            }
            else
            {
                WriteLogFunc(msg, color, fontSize);
            }
        }
    }
}
