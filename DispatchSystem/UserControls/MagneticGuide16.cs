using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DispatchSystem.UserControls
{
    public partial class MagneticGuide16 : UserControl
    {
        /// <summary>
        /// 磁导航数据
        /// </summary>
        private int data = 0x5555;
        public int Data
        {
            get
            {
                return data;
            }
            set
            {
                if (data != value)
                {
                    data = value;
                    update();
                }
            }
        }

        /// <summary>
        /// 有效颜色
        /// </summary>
        private Color borderColor = Color.DarkGray;
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    update();
                }
            }
        }

        /// <summary>
        /// 有效颜色
        /// </summary>
        private Color trueColor = Color.Yellow;
        public Color TrueColor
        {
            get
            {
                return trueColor;
            }
            set
            {
                if (trueColor != value)
                {
                    trueColor = value;
                    update();
                }
            }
        }

        /// <summary>
        /// 无效颜色
        /// </summary>
        private Color falseColor = Color.Red;
        public Color FalseColor
        {
            get
            {
                return falseColor;
            }
            set
            {
                if (falseColor != value)
                {
                    falseColor = value;
                    update();
                }
            }
        }

        public MagneticGuide16()
        {
            InitializeComponent();
        }

        private void MagneticGuide_Load(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Parent = pictureBox1;
            update();
        }

        private void MagneticGuide_SizeChanged(object sender, EventArgs e)
        {
            update();
        }

        void update()
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            int x1, y1, w, h;
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            Brush bush;
            //矩形宽
            w = (int)(width * 0.029);
            //矩形高
            h = (int)(height * 0.4);
            //矩形y坐标
            y1 = (int)(height * 0.5);
            for (int i = 0; i < 16; i++)
            {
                //计算坐标
                x1 = (int)(width * 0.05 + width * 0.029 * i * 2);

                //画背景
                bush = new SolidBrush(Color.Gray);//填充的颜色
                g.FillRectangle(bush, x1 - 2, y1 - 2, w + 4, h + 4);

                //画LED
                if (GetBitValue(data, i))
                {
                    bush = new SolidBrush(trueColor);//填充的颜色
                }
                else
                {
                    bush = new SolidBrush(falseColor);//填充的颜色
                }
                g.FillRectangle(bush, x1, y1, w, h);
            }
            pictureBox2.Image = new Bitmap(bmp);
            g.Dispose();
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
