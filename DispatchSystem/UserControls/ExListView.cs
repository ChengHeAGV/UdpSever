using System;
using System.Drawing;
using System.Windows.Forms;

namespace DispatchSystem
{
    public class ExListView : ListView
    {
        public ExListView()
        {
            SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            FullRowSelect = true;
            HideSelection = false;
            View = View.Details;

            //光标移动事件
            MouseMove += ListView_MouseMove;
        }


        //光标移动行颜色高亮
        int lastIndex = -1;
        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
          
            ListView _ListView = (ListView)sender;
            ListViewItem _Item = _ListView.GetItemAt(e.X, e.Y);
            if (_Item != null)
            {
                if (lastIndex != _Item.Index)
                {  
                    //恢复上一个
                    if (lastIndex >= 0)
                    {
                        Items[lastIndex].BackColor = lastIndex % 2 == 0 ? Color.White : Color.FromArgb(0xf0, 0xf5, 0xf5, 0xf5);
                    }
                    _ListView.Items[_Item.Index].BackColor = Color.FromArgb(0xf0, 0xe5, 0xe5, 0xe5);
                    
                    lastIndex = _Item.Index;
                }
            }

        }
    }
}
