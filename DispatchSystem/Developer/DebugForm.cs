using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem.Developer
{
    public partial class DebugForm : Form
    {
        OleDbConnection con;
        OleDbDataAdapter da;
        string strFilePath = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + Application.StartupPath + "\\Database.mdb";
        string sql = "select * from Debug";
        public DebugForm()
        {
            InitializeComponent();
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {
            //声明一个数据连接
            con = new OleDbConnection(strFilePath);
            da = new OleDbDataAdapter(sql, con);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                UdpSever.Shell.WriteLine(ConsoleColor.Yellow, "警告：{0}", ex.ToString());
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
                con.Dispose();
                da.Dispose();
            }
            // TODO: 这行代码将数据加载到表“databaseDataSet1.Debug”中。您可以根据需要移动或删除它。
            //this.debugTableAdapter.Fill(this.databaseDataSet1.Debug);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UdpSever.Shell.WriteLine(dt.Rows[i][2].ToString());
                }

                //声明一个数据连接
                con = new OleDbConnection(strFilePath);
                da = new OleDbDataAdapter(sql, con);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                da.UpdateCommand = cb.GetUpdateCommand();
                da.Update(dt);
                con.Close();
                con.Dispose();
                da.Dispose();
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            //DataGridViewRow dgvr = new DataGridViewRow();
            //dgvr = dataGridView1.Rows[0];
            ////设置当前单元格
            //dataGridView1.CurrentCell = dgvr.Cells[0];
            ////设置选中状态
            //dgvr.Cells[0].Selected = true;
            //dataGridView1.Columns[0].Selected = true;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
