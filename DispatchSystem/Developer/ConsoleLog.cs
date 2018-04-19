using System;
using System.Drawing;
using System.Windows.Forms;

namespace DispatchSystem.Developer
{
    public partial class ConsoleLog : Form
    {
        public ConsoleLog()
        {
            InitializeComponent();
        }

        private void ConsoleLog_Load(object sender, EventArgs e)
        {
        }

        static RichTextBox rtb = new RichTextBox();

        /// <summary>
        /// 写Log
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="color">颜色</param>
        /// <param name="fontSize">字体</param>
        public static void WriteLog(string msg, Color color, int fontSize = 18)
        {
            Font font = new Font(FontFamily.GenericMonospace, fontSize, FontStyle.Regular);
            rtb.Font = font;
            rtb.ForeColor = color;
           // rtb.SelectionColor = Color.DarkGray;
            rtb.AppendText(msg);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (richTextBox1.Text != rtb.Text)
            {
                richTextBox1.ForeColor = Color.White;
                rtb.Copy();
                richTextBox1.Paste();
            }
        }
    }
}
