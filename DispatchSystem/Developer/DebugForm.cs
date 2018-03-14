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

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "心跳帧";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.HeartFrame;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "操作帧";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.OperationFrame;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "响应帧";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.ResponseFrame;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "实时帧";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.RealTimeFrame;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "读单个寄存器";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.ReadRegister;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "写单个寄存器";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.WriteRegister;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "读多个寄存器";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.ReadMuliteRegister;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "写多个寄存器";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.WriteMuliteRegister;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "无效帧";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.ValidFrames;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "收到数据";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.ReciveData;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "单包数据有效帧数量";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.VaildFramesNum;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "发送数据";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.SendData;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "错误";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.Error;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "系统消息";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.SystemMsg;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "服务器";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.Sever;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = "debug";
            checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = XmlHelper.Config.debug.debug;
            row.Cells.Add(textBoxCell);
            row.Cells.Add(checkBoxCell);
            dataGridView1.Rows.Add(row);
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
                XmlHelper.Config.debug.HeartFrame = (bool)dd.Rows[0].Cells[1].Value;
                XmlHelper.Config.debug.OperationFrame = (bool)dd.Rows[1].Cells[1].Value;
                XmlHelper.Config.debug.ResponseFrame = (bool)dd.Rows[2].Cells[1].Value;
                XmlHelper.Config.debug.RealTimeFrame = (bool)dd.Rows[3].Cells[1].Value;
                XmlHelper.Config.debug.ReadRegister = (bool)dd.Rows[4].Cells[1].Value;
                XmlHelper.Config.debug.WriteRegister = (bool)dd.Rows[5].Cells[1].Value;
                XmlHelper.Config.debug.ReadMuliteRegister = (bool)dd.Rows[6].Cells[1].Value;
                XmlHelper.Config.debug.WriteMuliteRegister = (bool)dd.Rows[7].Cells[1].Value;
                XmlHelper.Config.debug.ValidFrames = (bool)dd.Rows[8].Cells[1].Value;
                XmlHelper.Config.debug.ReciveData = (bool)dd.Rows[9].Cells[1].Value;
                XmlHelper.Config.debug.VaildFramesNum = (bool)dd.Rows[10].Cells[1].Value;
                XmlHelper.Config.debug.SendData = (bool)dd.Rows[11].Cells[1].Value;
                XmlHelper.Config.debug.Error = (bool)dd.Rows[12].Cells[1].Value;
                XmlHelper.Config.debug.SystemMsg = (bool)dd.Rows[13].Cells[1].Value;
                XmlHelper.Config.debug.Sever = (bool)dd.Rows[14].Cells[1].Value;
                XmlHelper.Config.debug.debug = (bool)dd.Rows[15].Cells[1].Value;

                //更新到xml
                XmlHelper.Updata();
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
            XmlHelper.Config.debug.HeartFrame = true;
            XmlHelper.Config.debug.OperationFrame = true;
            XmlHelper.Config.debug.ResponseFrame = true;
            XmlHelper.Config.debug.RealTimeFrame = true;
            XmlHelper.Config.debug.ReadRegister = true;
            XmlHelper.Config.debug.WriteRegister = true;
            XmlHelper.Config.debug.ReadMuliteRegister = true;
            XmlHelper.Config.debug.WriteMuliteRegister = true;
            XmlHelper.Config.debug.ValidFrames = true;
            XmlHelper.Config.debug.ReciveData = true;
            XmlHelper.Config.debug.VaildFramesNum = true;
            XmlHelper.Config.debug.SendData = true;
            XmlHelper.Config.debug.Error = true;
            XmlHelper.Config.debug.SystemMsg = true;
            XmlHelper.Config.debug.Sever = true;
            XmlHelper.Config.debug.debug = true;

            updateToForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlHelper.Config.debug.HeartFrame = false;
            XmlHelper.Config.debug.OperationFrame = false;
            XmlHelper.Config.debug.ResponseFrame = false;
            XmlHelper.Config.debug.RealTimeFrame = false;
            XmlHelper.Config.debug.ReadRegister = false;
            XmlHelper.Config.debug.WriteRegister = false;
            XmlHelper.Config.debug.ReadMuliteRegister = false;
            XmlHelper.Config.debug.WriteMuliteRegister = false;
            XmlHelper.Config.debug.ValidFrames = false;
            XmlHelper.Config.debug.ReciveData = false;
            XmlHelper.Config.debug.VaildFramesNum = false;
            XmlHelper.Config.debug.SendData = false;
            XmlHelper.Config.debug.Error = false;
            XmlHelper.Config.debug.SystemMsg = false;
            XmlHelper.Config.debug.Sever = false;
            XmlHelper.Config.debug.debug = false;

            updateToForm();
        }


    }
}
