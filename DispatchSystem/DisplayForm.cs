using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem
{
    public partial class DisplayForm : Form
    {
        Thread th;
        int deviceNum = 0;
        int selectDataNum = 0;
        public DisplayForm()
        {
            InitializeComponent();
            deviceNum = DataForm.outdeviceNum;
            selectDataNum = DataForm.selectDataNum;
            this.Text = string.Format("设备{0},数据{1}", deviceNum, selectDataNum);
        }

        private void DisplayForm_Load(object sender, EventArgs e)
        {
            //启动自动更新进程
            th = new Thread(fun);
            th.Start();
        }

        private void fun()
        {
            while (true)
            {
                Thread.Sleep(50);
                //更新数据
                label_Hex.Text = Form1.Ddata[deviceNum, selectDataNum, 0].ToString("X2");//十六进制

                label_Dec.Text = Form1.Ddata[deviceNum, selectDataNum, 0].ToString();//十进制


                string bin = Convert.ToString(Form1.Ddata[deviceNum, selectDataNum, 0], 2).PadLeft(16, '0');//二进制

                byte[] bt = new byte[2];
                bt[0] = (byte)(Form1.Ddata[deviceNum, selectDataNum, 0] >> 8);
                bt[1] = (byte)(Form1.Ddata[deviceNum, selectDataNum, 0]);
                string str = Encoding.GetEncoding("GB2312").GetString(bt, 0, 2).Replace("\0", "");
                label_str.Text = str;//ASCII字符串

                //显示时间
                DateTime dtstart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long itime = long.Parse(Form1.Ddata[deviceNum, selectDataNum, 1] + "0000000");
                TimeSpan tonow = new TimeSpan(itime);
                label_updataTime.Text = dtstart.Add(tonow).ToLongDateString() + " " + dtstart.Add(tonow).ToLongTimeString();

                string b = string.Empty;
                for (int i = 0; i < 16; i++)
                {
                    b = "button_" + i.ToString();
                    ((Button)this.Controls.Find(b, true)[0]).Text = bin.Substring(15 - i, 1);
                }

                Color trueColor = Color.Green;
                Color falseColor = Color.Red;

                for (int i = 0; i < 16; i++)
                {
                    b = "button_" + i.ToString();
                    Button btn = ((Button)this.Controls.Find(b, true)[0]);

                    if (btn.Text == "0")
                        btn.BackColor = falseColor;
                    else
                        btn.BackColor = trueColor;
                }
            }
        }

        private void DisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            th.Abort();
        }

        private void button_0_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_1_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 1;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_2_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 2;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_3_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 3;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_4_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 4;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_5_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 5;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_6_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 6;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_7_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 7;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_8_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 8;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_9_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 9;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_10_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 10;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_11_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 11;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_12_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 12;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_13_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 13;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_14_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 14;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void button_15_Click(object sender, EventArgs e)
        {
            UInt32 data = Form1.Ddata[deviceNum, selectDataNum, 0];
            data ^= 1 << 15;
            Form1.writeWord(deviceNum, selectDataNum, (UInt16)data);
            Form1.Ddata[deviceNum, selectDataNum, 0] = data;
        }

        private void sendhex(TextBox tx)
        {
            byte[] byt = HexStringToBytes(tx.Text);
            UInt16 data = 0;
            if (byt.Length > 0)
            {
                if (byt.Length == 1)
                {
                    data = (UInt16)(byt[0]);
                }
                else
                if (byt.Length == 2)
                {
                    data = (UInt16)((byt[0] << 8) | byt[1]);
                }
                Form1.writeWord(deviceNum, selectDataNum, data);
                Form1.Ddata[deviceNum, selectDataNum, 0] = data;
            }
        }
        //发送Hex数据
        private void button_hex1_Click(object sender, EventArgs e)
        {
            sendhex(textBox_hex1);
        }

        private void button_hex2_Click(object sender, EventArgs e)
        {
            sendhex(textBox_hex2);
        }

        private void button_hex3_Click(object sender, EventArgs e)
        {
            sendhex(textBox_hex3);
        }

        private void button_hex4_Click(object sender, EventArgs e)
        {
            sendhex(textBox_hex4);
        }
        private void senddec(TextBox tx)
        {
            try
            {
                if (tx.Text.Length > 0)
                {
                    UInt16 data = UInt16.Parse(tx.Text);
                    Form1.writeWord(deviceNum, selectDataNum, data);
                    Form1.Ddata[deviceNum, selectDataNum, 0] = data;
                }
            }
            catch
            {

            }

        }
        private void button_dec1_Click(object sender, EventArgs e)
        {
            senddec(textBox_dec1);
        }

        private void button_dec2_Click(object sender, EventArgs e)
        {
            senddec(textBox_dec2);
        }

        private void button_dec3_Click(object sender, EventArgs e)
        {
            senddec(textBox_dec3);
        }

        private void button_dec4_Click(object sender, EventArgs e)
        {
            senddec(textBox_dec4);
        }
        private void sendstr(TextBox tx)
        {
            if (tx.Text.Length > 0)
            {
                byte[] byt = Encoding.Default.GetBytes(textBox_str1.Text);
                UInt16 data = 0;
                if (byt.Length > 0)
                {
                    if (byt.Length == 1)
                    {
                        data = (UInt16)(byt[0]);
                    }
                    else
                    if (byt.Length == 2)
                    {
                        data = (UInt16)((byt[0] << 8) | byt[1]);
                    }
                    Form1.writeWord(deviceNum, selectDataNum, data);
                    Form1.Ddata[deviceNum, selectDataNum, 0] = data;
                }
            }
        }
        private void button_str1_Click(object sender, EventArgs e)
        {
            sendstr(textBox_str1);
        }

        private void button_str2_Click(object sender, EventArgs e)
        {
            sendstr(textBox_str2);
        }

        private void button_str3_Click(object sender, EventArgs e)
        {
            sendstr(textBox_str3);
        }

        private void button_str4_Click(object sender, EventArgs e)
        {
            sendstr(textBox_str4);
        }

        public const string PATTERN = @"([^A-Fa-f0-9]|\s+?)+";
        /// <summary>
        /// 判断并自动格式用户输入的十六进制数
        /// </summary>
        /// <param name="name"></param>
        private void hexfuc(string name)
        {
            TextBox tb = ((TextBox)this.Controls.Find(name, true)[0]);

            string str = tb.Text;
            str = str.Replace(" ", "");
            if (str.Length > 0)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    String buf = str.Substring(i, 1);
                    if (System.Text.RegularExpressions.Regex.IsMatch(buf, PATTERN))//不符合
                    {
                        MessageBox.Show("输入格式不正确！", "温馨提示");
                        str = str.Remove(i, 1);
                        tb.Text = str;
                        break;
                    }
                }

                if (tb.Text.Length > 2)
                {
                    tb.Text = str.Insert(2, " ");
                }

                //让文本框获取焦点 
                tb.Focus();
                //设置光标的位置到文本尾 
                tb.Select(tb.TextLength, 0);
                //滚动到控件光标处 
                tb.ScrollToCaret();
            }
        }
        private void textBox_hex1_TextChanged(object sender, EventArgs e)
        {
            hexfuc("textBox_hex1");
        }

        private void textBox_hex2_TextChanged(object sender, EventArgs e)
        {
            hexfuc("textBox_hex2");
        }

        private void textBox_hex3_TextChanged(object sender, EventArgs e)
        {
            hexfuc("textBox_hex3");
        }

        private void textBox_hex4_TextChanged(object sender, EventArgs e)
        {
            hexfuc("textBox_hex4");
        }

        public static byte[] HexStringToBytes(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
            {
                return new byte[0];
            }

            if (hexStr.StartsWith("0x"))
            {
                hexStr = hexStr.Remove(0, 2);
            }

            hexStr = hexStr.Replace(" ", "");

            var count = hexStr.Length;
            if (count % 2 == 1)
            {
                hexStr = hexStr.Insert(0, "0");
            }

            count = hexStr.Length;

            var byteCount = count / 2;
            var result = new byte[byteCount];
            for (int ii = 0; ii < byteCount; ++ii)
            {
                var tempBytes = Byte.Parse(hexStr.Substring(2 * ii, 2), System.Globalization.NumberStyles.HexNumber);
                result[ii] = tempBytes;
            }

            return result;
        }
    }
}
