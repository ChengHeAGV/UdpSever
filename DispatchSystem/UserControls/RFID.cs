using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace DispatchSystem.UserControls
{
    public partial class RFID : UserControl
    {
        #region 属性变量
        private int value = 0x00;//RFID数据
        private bool powerLed = false;//RFID电源指示灯
        private bool valueLed = false;//RFID数据指示灯
        private Color valueColor = Color.Magenta; //数值颜色 
        private Color powerOnColor = Color.Green; //电源LED亮颜色      
        private Color powerOffColor = Color.Red; //电源LED灭颜色    
        private Color valueOnColor = Color.Yellow; //数据LED亮颜色      
        private Color valueOffColor = Color.Red; //数据LED灭颜色    
        #endregion
        #region 属性
        [
            Category("Value"),
            Description("RFID默认值。")
        ]
        public int Value
        {
            get
            { return value; }
            set
            {
                powerLed = true;
                if (this.value != value)
                {
                    this.value = value;
                    lbDigitalMeter1.Value = value;
                    //控制指示灯闪烁一次
                    Thread th = new Thread(flicker);
                    th.Start();
                }
            }
        }

        [
            Category("Value"),
            Description("电源指示灯状态。")
        ]
        public bool PowerLed
        {
            get
            { return powerLed; }
            set
            {
                if (this.powerLed != value)
                {
                    this.powerLed = value;
                    update();
                }
            }
        }

        [
            Category("Value"),
            Description("数据指示灯状态。")
        ]
        public bool ValueLed
        {
            get
            { return valueLed; }
            set
            {
                if (this.valueLed != value)
                {
                    this.valueLed = value;
                    update();
                }
            }
        }


        [
             Category("Color1"),
             Description("电源指示灯点亮状态颜色。")
        ]
        public Color ValueColor
        {
            get
            { return valueColor; }
            set
            {
                if (valueColor != value)
                {
                    valueColor = value;
                    lbDigitalMeter1.ForeColor = value;
                }
            }
        }

        [
             Category("Color"),
             Description("电源指示灯点亮状态颜色。")
        ]
        public Color PowerOnColor
        {
            get
            { return powerOnColor; }
            set
            {
                if (powerOnColor != value)
                {
                    powerOnColor = value;
                    update();
                }
            }
        }

        [
             Category("Color"),
             Description("电源指示灯熄灭状态颜色。")
        ]
        public Color PowerOffColor
        {
            get
            { return powerOffColor; }
            set
            {
                if (powerOffColor != value)
                {
                    powerOffColor = value;
                    update();
                }
            }
        }

        [
            Category("Color"),
            Description("数据指示灯点亮状态颜色。")
        ]
        public Color ValueOnColor
        {
            get
            { return valueOnColor; }
            set
            {
                if (valueOnColor != value)
                {
                    valueOnColor = value;
                    update();
                }
            }
        }

        [
             Category("Color"),
             Description("数据指示灯熄灭状态颜色。")
        ]
        public Color ValueOffColor
        {
            get
            { return valueOffColor; }
            set
            {
                if (valueOffColor != value)
                {
                    valueOffColor = value;
                    update();
                }
            }
        }
        #endregion

        public RFID()
        {
            InitializeComponent();
        }

        private void RFID_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Transparent;
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Parent = pictureBox1;
            update();
            lbDigitalMeter1.Value = value;
            lbDigitalMeter1.ForeColor = valueColor;
        }

        private void RFID_SizeChanged(object sender, EventArgs e)
        {
            //重新调整数码管的尺寸和位置
            lbDigitalMeter1.Size = new Size((int)(pictureBox1.Width * 0.545), (int)(pictureBox1.Height * 0.2213));
            lbDigitalMeter1.Location = new Point((int)(pictureBox1.Width * 0.199), (int)(pictureBox1.Height * 0.3312));
            lbDigitalMeter1.Update();
            update();
        }

        void update()
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            //默认位置422*488
            //g.FillEllipse(new SolidBrush(Color.Red), 86, 360, 25, 25);
            //g.FillEllipse(new SolidBrush(Color.Red), 311, 360, 25, 25);
            int x1, y1, x2, y2, radiusX, radiusY;
            x1 = (int)(pictureBox1.Width * 0.2038);
            y1 = (int)(pictureBox1.Height * 0.7378);
            x2 = (int)(pictureBox1.Width * 0.737);
            y2 = (int)(pictureBox1.Height * 0.7378);
            radiusX = (int)(pictureBox1.Width * 0.07);
            radiusY = (int)(pictureBox1.Height * 0.06);
            //PowerLed
            if (powerLed)
                g.FillEllipse(new SolidBrush(powerOnColor), x1, y1, radiusX, radiusY);
            else
                g.FillEllipse(new SolidBrush(powerOffColor), x1, y1, radiusX, radiusY);

            //ValueLed
            if (valueLed)
                g.FillEllipse(new SolidBrush(valueOnColor), x2, y2, radiusX, radiusY);
            else
                g.FillEllipse(new SolidBrush(valueOffColor), x2, y2, radiusX, radiusY);

            pictureBox2.Image = new Bitmap(bmp);
            g.Dispose();
        }

        //闪烁
        void flicker()
        {
            valueLed = true;
            update();
            Thread.Sleep(50);
            valueLed = false;
            update();
        }
    }
}
