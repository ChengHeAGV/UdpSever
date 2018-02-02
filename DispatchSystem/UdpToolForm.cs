using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem
{
    public partial class UdpToolForm : Form
    {
        public UdpToolForm()
        {
            InitializeComponent();
        }
        //处理  
        private void UdpSever_UdpReciveEvent(string str)
        {
            textBox1.AppendText(str);
        }
    }
}
