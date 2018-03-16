using System;
using System.Data;
using System.Windows.Forms;

namespace DispatchSystem.Developer
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }

        //更新到界面
        private void updateToForm()
        {
            //清空DataGridViewRow
            dataGridView1.Rows.Clear();

            //加载debug配置
            DataGridViewRow row;
            DataGridViewTextBoxCell textBoxCell;
            DataGridViewCheckBoxCell checkBoxCell;

            foreach (var item in XmlHelper.DebugList)
            {
                row = new DataGridViewRow();
                textBoxCell = new DataGridViewTextBoxCell();
                textBoxCell.Value = item.Name;
                checkBoxCell = new DataGridViewCheckBoxCell();
                checkBoxCell.Value = item.Value;
                row.Cells.Add(textBoxCell);
                row.Cells.Add(checkBoxCell);
                dataGridView1.Rows.Add(row);
            }
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {
            updateToForm();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dd = sender as DataGridView;
            if (dd.Rows.Count != 0)
            {
                for (int i = 0; i < XmlHelper.DebugList.Count; i++)
                {
                    XmlHelper.DebugList[i].Value = (bool)dd.Rows[i].Cells[1].Value;
                }
                //更新到xml
                XmlHelper.UpdataDebug();
            }

        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            Console.Clear();
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < XmlHelper.DebugList.Count; i++)
            {
                XmlHelper.DebugList[i].Value = true;
            }
            updateToForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < XmlHelper.DebugList.Count; i++)
            {
                XmlHelper.DebugList[i].Value = false;
            }
            updateToForm();
        }
    }
}
