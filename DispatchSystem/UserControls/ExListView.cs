using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DispatchSystem
{
    public class ExListView : ListView
    {
        #region 变量
        private bool highLightNow = true;//高亮当前行
        private Color nowColor = Color.FromArgb(0xff, 0xff, 0x99, 0x00);//当前行高亮色
        private Color nowForceColor = Color.Black;//当前行前景色
        private Font nowFont = new Font("新宋体", 18, FontStyle.Regular);//当前行字体
        private int nowHight = 18;//当前行高
        #endregion

        #region 属性
        [Category("Author:孙毅明"), Description("动态高亮当前光标所在行！")]
        public bool HighLightNow
        {
            get { return highLightNow; }
            set { highLightNow = value; }
        }

        [Category("Author:孙毅明"), Description("当前光标所在行高亮色！")]
        public Color NowColor
        {
            get { return nowColor; }
            set { nowColor = value; }
        }

        [Category("Author:孙毅明"), Description("当前光标所在行前景(字体)色！")]
        public Color NowForceColor
        {
            get { return nowForceColor; }
            set { nowForceColor = value; }
        }

        [Category("Author:孙毅明"), Description("当前光标所在行当前行字体！")]
        public Font NowFont
        {
            get { return nowFont; }
            set { nowFont = value; }
        }

        [Category("Author:孙毅明"), Description("当前光标所在行当前行字体！")]
        public int NowHight
        {
            get { return nowHight; }
            set { nowHight = value; }
        }
        #endregion

        public ExListView()
        {
            SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
 
            FullRowSelect = true;//选择一整行
            HideSelection = false;
            GridLines = false;//不显示线条
            View = View.Details;//以数据表显示

            //判断是否需要高亮当前行
            if (highLightNow)
            {
                //光标移动事件
                MouseMove += ListView_MouseMove;
                //光标离开事件
                MouseLeave += ExListView_MouseLeave;
            }
        }

        private void ExListView_MouseLeave(object sender, EventArgs e)
        {
            //恢复上一个
            if (lastIndex >= 0)
            {
                Items[lastIndex].BackColor = lastBackColor;
                Items[lastIndex].Font = lastFont;
                Items[lastIndex].ForeColor = lastForceColor;
            }
        }

        //光标移动行颜色高亮
        int lastIndex = -1;
        Font lastFont;
        Color lastBackColor;
        Color lastForceColor;
        int lastHight;
        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {

            ListView listView = (ListView)sender;
            ListViewItem nowItem = listView.GetItemAt(e.X, e.Y);
            if (nowItem != null)
            {
                if (lastIndex != nowItem.Index)
                {
                    //恢复上一个
                    if (lastIndex >= 0)
                    {
                        listView.Items[lastIndex].BackColor = lastBackColor;
                        listView.Items[lastIndex].Font = lastFont;
                        listView.Items[lastIndex].ForeColor = lastForceColor;
                    }
                    //记录上一行状态
                    lastIndex = nowItem.Index;
                    lastBackColor = listView.Items[nowItem.Index].BackColor;
                    lastFont = listView.Items[nowItem.Index].Font;
                    lastForceColor = listView.Items[nowItem.Index].ForeColor;
                    //更新当前行
                    listView.Items[nowItem.Index].BackColor = nowColor;
                    listView.Items[nowItem.Index].Font = nowFont;
                    listView.Items[nowItem.Index].ForeColor = nowForceColor;
                }
            }

        }
    }
}
