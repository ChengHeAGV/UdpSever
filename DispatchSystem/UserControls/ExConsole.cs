using System;
using System.Drawing;
using System.Windows.Forms;

namespace DispatchSystem.UserControls
{
    public partial class ExConsole : UserControl
    {
        string[] datekey = new string[10];
        public ExConsole()
        {
            InitializeComponent();
        }

        private void ExConsole_Load(object sender, EventArgs e)
        {
            #region 数据列表
            datekey[0] = "日期";
            datekey[1] = "时间";
            datekey[2] = "事件";

            exListView1.FullRowSelect = true;//要选择就是一行
            exListView1.Columns.Add(datekey[0], 120, HorizontalAlignment.Left);
            exListView1.Columns.Add(datekey[1], 140, HorizontalAlignment.Left);
            exListView1.Columns.Add(datekey[2], -2, HorizontalAlignment.Left);//根据内容自适应宽度
            #endregion
        }

        public void WriteLine(string msg, Color color, int fontSize = 14)
        {
            Write(msg, color, fontSize);
        }
        public void WriteLine(string msg, int fontSize = 14)
        {
            Write(msg, Color.Black, fontSize);
        }

        public void WriteLine(string msg)
        {
            Write(msg, Color.Black, 14);
        }
        public void WriteLine(string format, params object[] args)
        {
            Write(string.Format(format, args), Color.Black, 14);
        }
        public void Write(string msg, Color color, int fontSize)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = DateTime.Now.ToString("yyyy-MM-dd");//"日期";
                    item.SubItems.Add(DateTime.Now.ToString("HH:mm:ss fff"));//  "时间";
                    item.SubItems.Add(msg);//  "事件";

                    item.ForeColor = color;//字体颜色
                    item.Font = new Font("新宋体", fontSize, FontStyle.Regular); //字体颜色

                    if (exListView1.Items.Count % 2 == 0)
                        item.BackColor = Color.FromArgb(200, 0xf5, 0xf6, 0xeb);

                    exListView1.Items.Add(item);
                    exListView1.EnsureVisible(exListView1.Items.Count - 1);//滚动到指定的行位置
                }));
            }
            catch
            {
                throw;
            }
        }
    }
}
