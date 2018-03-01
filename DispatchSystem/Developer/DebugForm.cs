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
            try
            {
                UdpSever.DebugMsg.dt.Clear();
                da.Fill(UdpSever.DebugMsg.dt);
                if (UdpSever.DebugMsg.dt.Rows.Count >= 1)
                {
                    dataGridView1.DataSource = UdpSever.DebugMsg.dt;
                }
            }
            catch (Exception ex)
            {
                UdpSever.Shell.WriteLine("错误", "警告：{0}", ex.ToString());
            }
            finally
            {
                con.Close();
                con.Dispose();
                da.Dispose();
            }
        }


        //保存配置
        void SaveDbugConfig()
        {
            try
            {
                //    DataTable dt = new DataTable();
                UdpSever.DebugMsg.dt = dataGridView1.DataSource as DataTable;
                if (UdpSever.DebugMsg.dt != null)
                {
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    UdpSever.Shell.WriteLine(dt.Rows[i][2].ToString());
                    //}

                    //声明一个数据连接
                    con = new OleDbConnection(strFilePath);
                    da = new OleDbDataAdapter(sql, con);
                    OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                    da.UpdateCommand = cb.GetUpdateCommand();
                    da.Update(UdpSever.DebugMsg.dt);
                    da.Dispose();
                    con.Close();
                    con.Dispose();

                }
            }
            catch (Exception ex)
            {
                UdpSever.Shell.WriteLine("错误",ConsoleColor.Yellow, "警告：{0}", ex.ToString());
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SaveDbugConfig();
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDbugConfig();
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            Console.Clear();
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < UdpSever.DebugMsg.dt.Rows.Count; i++)
            {
                UdpSever.DebugMsg.dt.Rows[i][2] = true;
            }
            dataGridView1.DataSource = UdpSever.DebugMsg.dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < UdpSever.DebugMsg.dt.Rows.Count; i++)
            {
                UdpSever.DebugMsg.dt.Rows[i][2] = false;
            }

            dataGridView1.DataSource = UdpSever.DebugMsg.dt;
        }
    }
}
