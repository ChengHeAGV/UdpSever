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
    public partial class Process : Form
    {
        public Process()
        {
            InitializeComponent();

        }
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public bool Increase( int nValue ,int min, int max )
        {
            progressBar1.Minimum = min;
            progressBar1.Maximum = max;

            if (nValue >= 0)
            {
                if (progressBar1.Value < progressBar1.Maximum)
                {
                    progressBar1.Value = nValue;
                    label1.Text = ( nValue * 100 / max ).ToString() + "%";
                    return true;
                }
                else
                {
                    progressBar1.Value = progressBar1.Maximum;
                    label1.Text = ( nValue * 100 / max ).ToString() + "%";
                    this.Close();
                    return false;
                }
            }
            return false;
        }
    }
}
