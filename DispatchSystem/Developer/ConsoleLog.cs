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
        string[] datekey = new string[10];
        public ConsoleLog()
        {
            InitializeComponent();
        }

        private void ConsoleLog_Load(object sender, EventArgs e)
        {
            UpdateControl += new UpdateControlEventHandler(this.Test);  //订阅UpdateControl事件，指定Test方法为事件处理函数
            #region 数据列表
            datekey[0] = "日期";
            datekey[1] = "时间";
            datekey[2] = "事件";

            doubleBufferListView1.FullRowSelect = true;//要选择就是一行
            doubleBufferListView1.Columns.Add(datekey[0], 120, HorizontalAlignment.Left);
            doubleBufferListView1.Columns.Add(datekey[1], 140, HorizontalAlignment.Left);
            doubleBufferListView1.Columns.Add(datekey[2], 680, HorizontalAlignment.Left);
            #endregion
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
                ListViewItem item = new ListViewItem();
                item.Text = DateTime.Now.ToString("yyyy-MM-dd");//"日期";
                item.SubItems.Add(DateTime.Now.ToString("HH:mm:ss fff"));//  "时间";
                item.SubItems.Add(msg);//  "事件";

                item.ForeColor = color;//字体颜色
                item.Font = new Font("新宋体", fontSize, FontStyle.Regular); //字体颜色

                if (doubleBufferListView1.Items.Count % 2 == 0)
                   item.BackColor = Color.FromArgb(200, 0xf5, 0xf6, 0xeb);

                doubleBufferListView1.Items.Add(item);
                doubleBufferListView1.EnsureVisible(doubleBufferListView1.Items.Count - 1);//滚动到指定的行位置
            }
            catch
            {

            }
        }

        public void Test(Object o, EventArgs e, string msg, Color color, int fontSize)  //事件处理函数，用来更新控件
        {
            if (doubleBufferListView1.InvokeRequired)
            {
                // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                Action<string, Color, int> actionDelegate = (_msg, _color, _fontSize) =>
                {
                    WriteLogFunc(_msg, _color, _fontSize);
                };
                doubleBufferListView1.BeginInvoke(actionDelegate, msg, color, fontSize);
            }
            else
            {
                WriteLogFunc(msg, color, fontSize);
            }
        }
    }
}
