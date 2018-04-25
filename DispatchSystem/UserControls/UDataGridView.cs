using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem.UserControls
{
    public partial class UDataGridView : DataGridView
    {
        public UDataGridView()
        {
            InitializeComponent();
            //设置间隔色
            this.RowsDefaultCellStyle.BackColor = Color.Bisque;
            this.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            //设置选中行的颜色
            this.DefaultCellStyle.SelectionBackColor = Color.Silver;
            this.DefaultCellStyle.SelectionForeColor = Color.Black;

            this.RowHeadersWidth = 60;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //事件
            this.RowLeave += UDataGridView_RowLeave; ;
            this.RowStateChanged += UDataGridView_RowStateChanged; ;
            this.CellMouseEnter += UDataGridView_CellMouseEnter;
            this.CellMouseLeave += UDataGridView_CellMouseLeave;
            this.SizeChanged += UDataGridView_SizeChanged;
            this.RowsAdded += UDataGridView_RowsAdded;
        }

        private void UDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            //清除选中行
            ClearSelection();
        }

        private void UDataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //添加序号
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }

        /// <summary>  
        /// 用来记录先前的颜色值  
        /// </summary>  
        Color colorTmp = Color.White;
        /// <summary>  
        /// 记录鼠标形状  
        /// </summary>  
        Cursor cursorTmp = Cursor.Current;
        private void UDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                colorTmp = Rows[e.RowIndex].DefaultCellStyle.BackColor;
                Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Silver;
                if (e.ColumnIndex == 1)//改变第二列鼠标形状  
                {
                    cursorTmp = this.Cursor;
                    this.Cursor = Cursors.Hand;
                }
            }
        }

        private void UDataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Rows[e.RowIndex].DefaultCellStyle.BackColor = colorTmp;
                if (e.ColumnIndex == 1)
                {
                    this.Cursor = cursorTmp;
                }
            }
        }

        private void UDataGridView_SizeChanged(object sender, EventArgs e)
        {
            //滚动到最后一行
            if (RowCount > 0)
            {
                FirstDisplayedScrollingRowIndex = RowCount - 1;
            }
        }


        private void UDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (this.Rows.Count>0)
            {
               // Rows.Add();
                //滚动到最后一行
                FirstDisplayedScrollingRowIndex = RowCount - 1;
            }
        }
    }
}
