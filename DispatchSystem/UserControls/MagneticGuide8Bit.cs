using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DispatchSystem.UserControls
{
    public partial class MagneticGuide8Bit : UserControl
    {
        #region 属性变量
        private int value = 0x55;//磁导航数据
        private Color borderColor = Color.DarkGray; //有效颜色
        private Color colorOn = Color.Yellow; //有效颜色      
        private Color colorOff = Color.Red; //无效颜色    
        #endregion

        #region 属性
        [
            Category("Value"),
            Description("磁导航默认值。")
        ]
        public int Value
        {
            get
            { return value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    update();
                }
            }
        }

        [
             Category("BorderColor"),
             Description("磁导航指示灯边框颜色。")
        ]
        public Color BorderColor
        {
            get
            { return borderColor; }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    update();
                }
            }
        }

        [
             Category("ColorOn"),
             Description("磁导航指示灯有效状态颜色。")
        ]
        public Color ColorOn
        {
            get
            { return colorOn; }
            set
            {
                if (colorOn != value)
                {
                    colorOn = value;
                    update();
                }
            }
        }

        [
             Category("ColorOff"),
             Description("磁导航指示灯无效状态颜色。")
        ]
        public Color ColorOff
        {
            get
            { return colorOff; }
            set
            {
                if (colorOff != value)
                {
                    colorOff = value;
                    update();
                }
            }
        }
        #endregion


        public MagneticGuide8Bit()
        {
            InitializeComponent();
        }

        private void MagneticGuide_Load(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Parent = pictureBox1;
            update();
            this.SizeChanged += MagneticGuide8Bit_SizeChanged;
        }

        private void MagneticGuide8Bit_SizeChanged(object sender, EventArgs e)
        {
            update();
        }

        void update()
        {
            Thread th = new Thread(Func);
            th.Start();
        }

        private void Func()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(bmp);
                int x1, y1, w, h;
                int width = pictureBox1.Width;
                int height = pictureBox1.Height;
                Brush bush;
                //矩形宽
                w = (int)(width * 0.0529);
                //矩形高
                h = (int)(height * 0.4);
                //矩形y坐标
                y1 = (int)(height * 0.5);
                for (int i = 0; i < 8; i++)
                {
                    //计算坐标
                    x1 = (int)(width * 0.10 + width * 0.0529 * i * 2);

                    //画背景
                    bush = new SolidBrush(Color.Gray);//填充的颜色
                    g.FillRectangle(bush, x1 - 2, y1 - 2, w + 4, h + 4);

                    //画LED
                    if (GetBitValue(value, i))
                    {
                        bush = new SolidBrush(colorOn);//填充的颜色
                    }
                    else
                    {
                        bush = new SolidBrush(colorOff);//填充的颜色
                    }
                    g.FillRectangle(bush, x1, y1, w, h);
                }
                pictureBox2.Image = new Bitmap(bmp);
                g.Dispose();
            }));
        }

        /// <summary>
        /// 返回Int数据中某一位是否为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
        /// <returns>true表示该位为1，false表示该位为0</returns>
        public static bool GetBitValue(int value, int index)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index"); //索引出错
            var val = 1 << index;
            return (value & val) == val;
        }
    }
}
