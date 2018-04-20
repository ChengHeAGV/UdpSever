using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem.SystemConfig
{
    public partial class DbusConfig : Form
    {
        public DbusConfig()
        {
            InitializeComponent();
        }

        private void DbusConfig_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“databaseDataSet.DbusSever”中。您可以根据需要移动或删除它。
            this.dbusSeverTableAdapter.Fill(this.databaseDataSet.DbusSever);

        }
    }
}
