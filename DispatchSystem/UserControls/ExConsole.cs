using System;
using System.Drawing;
using System.Windows.Forms;

namespace DispatchSystem.UserControls
{
    public partial class ExConsole : UserControl
    {
        string[] datekey = new string[10];
        bool MouseHover = false;

        public int Count
        {
            get
            {
                return exListView1.Items.Count;
            }
        }

        public ExConsole()
        {
            InitializeComponent();
        }

        private void ExConsole_Load(object sender, EventArgs e)
        {
            exListView1.MouseHover += ExListView1_MouseHover;//鼠标进入事件
            exListView1.MouseLeave += ExListView1_MouseLeave;//鼠标离开事件
            #region 数据列表
            datekey[0] = "日期";
            datekey[1] = "时间";
            datekey[2] = "事件";

            exListView1.FullRowSelect = true;//要选择就是一行
            exListView1.Columns.Add(datekey[0], 200, HorizontalAlignment.Left);
            exListView1.Columns.Add(datekey[1], 200, HorizontalAlignment.Left);
            exListView1.Columns.Add(datekey[2], -2, HorizontalAlignment.Left);//根据内容自适应宽度
            #endregion

        }

        //光标进入
        private void ExListView1_MouseLeave(object sender, EventArgs e)
        {
            MouseHover = false;
        }
        //光标离开
        private void ExListView1_MouseHover(object sender, EventArgs e)
        {
            MouseHover = true;
        }

        public void WriteLine(string msg, int fontSize = 14)
        {
            Write(msg, Color.Black, fontSize);
        }

        public void WriteLine(string msg, Color color, int fontSize = 14)
        {
            Write(msg, color, fontSize);
        }

        private void Write(string msg, Color color, int fontSize = 14)
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

                    if (exListView1.Items.Count % 2 == 1)
                        item.BackColor = Color.FromArgb(0xf0, 0xf5, 0xf5, 0xf5);

                    exListView1.Items.Add(item);

                    //光标在控件内时不自动滚动
                    if (MouseHover == false)
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
