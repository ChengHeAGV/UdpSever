using DispatchSystem.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem.User
{
    public partial class UpdateDataInfo : Form
    {
        private string des;//寄存器描述
        private string dir;//传输方向

        public string Des
        {
            get { return this.des; }

        }
        public string Dir
        {
            get { return this.dir; }
        }

        int regNum;

        public UpdateDataInfo(int num)
        {
            InitializeComponent();
            regNum = num;
            comboBoxStart.SelectedIndex = 0;
            comboBoxStop.SelectedIndex = 1;
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            dir = comboBoxStart.Text + " -> " + comboBoxStop.Text;
            des = textBoxDes.Text;
            //读取数据库
            masterEntities db = new masterEntities();
            var config = db.DbProfinet.AsNoTracking().ToList();
            var temp = config.FirstOrDefault(m => m.reg == regNum.ToString());
            if (temp != null)
            {
                //更新数据
                DbProfinet u = new DbProfinet() { Id = temp.Id, reg = temp.reg, dir = dir, des = des };
                db.Entry<DbProfinet>(u).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                temp = new DbProfinet
                {
                    reg = regNum.ToString(),
                    dir = dir,
                    des = des
                };
                db.DbProfinet.Add(temp);
                db.SaveChanges();
            }
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
