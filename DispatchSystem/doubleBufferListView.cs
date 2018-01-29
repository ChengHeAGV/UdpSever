using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem
{
    public partial class doubleBufferListView : UserControl
    {
        public doubleBufferListView()
        {
            InitializeComponent();
            DoubleBufferListView doubleBufferListView1 = new DoubleBufferListView();
            // 
            // doubleBufferListView1
            // 
            doubleBufferListView1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            doubleBufferListView1.FullRowSelect = true;
            doubleBufferListView1.HideSelection = false;
            doubleBufferListView1.Location = new System.Drawing.Point(50, 37);
            doubleBufferListView1.Name = "doubleBufferListView1";
            doubleBufferListView1.Size = new System.Drawing.Size(400, 191);
            doubleBufferListView1.TabIndex = 2;
            doubleBufferListView1.UseCompatibleStateImageBehavior = false;
            doubleBufferListView1.View = System.Windows.Forms.View.Details;


            Controls.Add(doubleBufferListView1);
        }
    }

    public class DoubleBufferListView : ListView
    {
        public DoubleBufferListView()
        {
            SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}
