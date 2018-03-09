using AdvancedDataGridView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DispatchSystem
{
    public partial class AgvParameter : Form
    {
        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString( string section, string key,
            string val, string filePath );

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString( string section, string key,
            string def, StringBuilder retVal, int size, string filePath );

        TreeGridNode node_parment;
        TreeGridNode node_lujingparment;
        TreeGridNode node_luchengparment;
        Font boldFont;

        private Process myProcessBar = null;
        private delegate bool IncreaseHandle( int nValue , int min, int max );
        private IncreaseHandle myIncrease = null;

        public AgvParameter()
        {
//            new Sunisoft.IrisSkin.SkinEngine().SkinFile = "skins/Midsummer.ssk";
            InitializeComponent();
            SetStyle();
            tvTaskList.Rows[0].ReadOnly = true;            
        }
        private void SetStyle()
        {
            Column1.DefaultCellStyle.NullValue = null;

            // load image strip
            this.imageStrip.ImageSize = new System.Drawing.Size(16, 16);
            this.imageStrip.TransparentColor = System.Drawing.Color.Magenta;
            this.imageStrip.ImageSize = new Size(16, 16);
            this.imageStrip.Images.AddStrip(Properties.Resources.newGroupPostIconStrip);

            tvTaskList.ImageList = imageStrip;

            // attachment header cell
            this.Column1.HeaderCell = new AttachmentColumnHeader(imageStrip.Images[0]);

            FillFormInfo();
        }

        private void FillFormInfo()
        {
            boldFont = new Font(tvTaskList.DefaultCellStyle.Font, FontStyle.Bold);

            node_parment = tvTaskList.Nodes.Add("系统参数", "", "", "", "", "");
            node_parment.ImageIndex = 0;
            node_lujingparment = tvTaskList.Nodes.Add("路径参数", "", "", "", "", "");
            node_lujingparment.ImageIndex = 0;
            node_luchengparment = tvTaskList.Nodes.Add("流程参数", "", "", "", "", "");
            node_luchengparment.ImageIndex = 0;
            node_parment.DefaultCellStyle.Font = boldFont;
            node_lujingparment.DefaultCellStyle.Font = boldFont;
            node_luchengparment.DefaultCellStyle.Font = boldFont;

        }

        internal class AttachmentColumnHeader : DataGridViewColumnHeaderCell
        {
            public Image _image;
            public AttachmentColumnHeader( Image img )
                : base()
            {
                this._image = img;
            }
            protected override void Paint( Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts )
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                graphics.DrawImage(_image, cellBounds.X + 4, cellBounds.Y + 2);
            }
            protected override object GetValue( int rowIndex )
            {
                return null;
            }
        }


        const long lengh = 100;
        private string[] str;

        private void Form1_Load( object sender, EventArgs e )
        {
            comboBox1.Visible = false;
            comboBox1.Width = 0;
            tvTaskList.Controls.Add(comboBox1);
            DateTime dt = DateTime.Now;
            label6.Text = string.Format("{0:F}", dt);
            button1_openclose.Text = "打开串口";
            comboBox2_BaudRate.SelectedIndex = 2;
            str = SerialPort.GetPortNames();
            if (str.Length != 0)
            {
                comboBox1_PortName.Items.AddRange(str);
                comboBox1_PortName.SelectedItem = comboBox1_PortName.Items[0];

                try
                {
                    serialPort1.PortName = comboBox1_PortName.SelectedItem.ToString();
                    serialPort1.BaudRate = int.Parse(comboBox2_BaudRate.SelectedItem.ToString());
                    serialPort1.Open();
                    serialPort1.DataReceived += serialPort1_DataReceived;
                    button1_openclose.Text = "关闭串口";
                    label5_LED.ForeColor = Color.Red;
                }
                catch (Exception)
                {
                    MessageBox.Show("打开串口失败,串口被占用或选择正确的串口!", "错误提示");
                }
            }

        }

        public string IniReadValue( string section, string skey, string path )
        {
            StringBuilder temp = new StringBuilder(500);
            long i = GetPrivateProfileString(section, skey, "", temp, 500, path);
            return temp.ToString();
        }

        public void IniWrite( string section, string key, string value, string path )
        {
            WritePrivateProfileString(section, key, value, path);
        }

        private bool Listening = false;//是否没有执行完invoke相关操作
        private new bool Closing = false;//是否正在关闭串口，执行Application.DoEvents，并阻止再次invoke
        private void button1_openclose_Click( object sender, EventArgs e )
        {
            try
            {
                if (button1_openclose.Text == "打开串口")
                {
                    serialPort1.PortName = comboBox1_PortName.SelectedItem.ToString();
                    serialPort1.BaudRate = int.Parse(comboBox2_BaudRate.SelectedItem.ToString());
                    serialPort1.Open();
                    serialPort1.DataReceived += serialPort1_DataReceived;
                    button1_openclose.Text = "关闭串口";
                    label5_LED.ForeColor = Color.Red;
                }
                else
                {
                    Closing = true;
                    while (Listening) Application.DoEvents();
                    serialPort1.Close();
                    button1_openclose.Text = "打开串口";
                    label5_LED.ForeColor = Color.Black;
                    Closing = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("打开串口失败,串口被占用或选择正确的串口!", "错误提示");
            }
        }

        private void Form1_FormClosing( object sender, FormClosingEventArgs e )
        {
            Closing = true;
            while (Listening) Application.DoEvents();
            serialPort1.Close();
            button1_openclose.Text = "打开串口";
            label5_LED.ForeColor = Color.Black;
            Closing = false;
            if (thsyspara_flag == 1)
            {
                if (thsyspara.IsAlive)
                {
                    thsyspara.Abort();
                }
            }
            if (duquparment_flag == 1)
            {
                if (duquparment.IsAlive)
                {
                    duquparment.Abort();
                }
            }
        }

        private void comboBox2_BaudRate_SelectionChangeCommitted( object sender, EventArgs e )
        {
            serialPort1.BaudRate = int.Parse(comboBox2_BaudRate.SelectedItem.ToString());
        }

        private void comboBox1_PortName_SelectionChangeCommitted( object sender, EventArgs e )
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                try
                {
                    serialPort1.PortName = comboBox1_PortName.SelectedItem.ToString();
                    serialPort1.Open();
                }
                catch (Exception)
                {
                    Closing = true;
                    while (Listening) Application.DoEvents();
                    serialPort1.Close();
                    button1_openclose.Text = "打开串口";
                    label5_LED.ForeColor = Color.Black;
                    MessageBox.Show("打开串口失败,串口被占用或选择正确的串口!", "错误提示");
                }
            }
        }

        private void comboBox1_PortName_Click( object sender, EventArgs e )
        {
            try
            {
                comboBox1_PortName.Items.Clear();
                str = SerialPort.GetPortNames();
                if (str.Length > 0)
                {
                    comboBox1_PortName.Items.AddRange(str);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("打开串口失败,串口被占用或选择正确的串口!", "错误提示");
            }
        }
        private void button2_readformpc_Click_1( object sender, EventArgs e )//读取xml文件，获得参数
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "文本文件|*.xml";
            if (file.ShowDialog() == DialogResult.OK)
            {
                ////清空子节点
                //node_parment.Nodes.Clear();
                //node_lujingparment.Nodes.Clear();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file.FileName);

                XmlNode xn = xmlDoc.SelectSingleNode("Flash参数");

                XmlNodeList xnn = xn.ChildNodes;

                int i = 0, j = 0, m = 0, n = 0;
                foreach (XmlNode xnk in xnn)
                {
                    i = 0;
                    if (xnk.Name == "系统参数")
                    {
                        XmlElement xe = (XmlElement)xnk;
                        XmlNodeList xnm = xe.ChildNodes;
                        foreach (XmlNode xnkk in xnm)
                        {
                            XmlElement xee = (XmlElement)xnkk;
                            XmlNodeList xng = xee.ChildNodes;
                            if (node_parment.Nodes.Count > i)
                            {
                                node_parment.Nodes[i].Cells[1].Value = xng.Item(0).InnerText.ToString();
                                node_parment.Nodes[i].Cells[2].Value = xng.Item(1).InnerText.ToString();
                                node_parment.Nodes[i].Cells[3].Value = xng.Item(2).InnerText.ToString();
                                node_parment.Nodes[i].Cells[4].Value = xng.Item(3).InnerText.ToString();
                                node_parment.Nodes[i].Cells[5].Value = xng.Item(4).InnerText.ToString();
                            }
                            else
                            {
                                node_parment.Nodes.Add("", xng.Item(0).InnerText.ToString(), xng.Item(1).InnerText.ToString(),
                                    xng.Item(2).InnerText.ToString(), xng.Item(3).InnerText.ToString(), xng.Item(4).InnerText.ToString());
                                node_parment.Nodes[i].Parent.Nodes[i].ImageIndex = 1;
                            }
                            if (node_parment.Nodes[i].Cells[3].Value.ToString() != node_parment.Nodes[i].Cells[4].Value.ToString())
                            {
                                node_parment.Nodes[i].DefaultCellStyle.BackColor = Color.Red;
                            }
                            else
                            {
                                node_parment.Nodes[i].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                            i++;
                        }
                        zhandianxinxi_sum = int.Parse(node_parment.Nodes[96].Cells[3].Value.ToString());
                        xuhaoxinxi_sum = int.Parse(node_parment.Nodes[97].Cells[3].Value.ToString());
                        lujing_num = int.Parse(node_parment.Nodes[99].Cells[3].Value.ToString());
                        liucheng_num = int.Parse(node_parment.Nodes[98].Cells[3].Value.ToString());
                        label3.Text = "系  统：" + 100;
                        label4.Text = "路径数：" + lujing_num.ToString();
                        label5.Text = "流程数：" + liucheng_num.ToString();
                    }
                    if (xnk.Name == "路径参数")
                    {
                        Array.Clear(lujing_rewindex, 0, lujing_rewindex.Length);
                        i = 0; j = 0; m = 0; n = 0;
                        XmlElement xe = (XmlElement)xnk;
                        XmlNodeList xnm = xe.ChildNodes;
                        foreach (XmlNode xnkk in xnm)
                        {
                            XmlElement xee = (XmlElement)xnkk;
                            XmlNodeList xng = xee.ChildNodes;
                            lujing_rewindex[j] = int.Parse(Regex.Replace(xee.Name, "[\u4e00-\u9fa5]", string.Empty));
                            if (node_lujingparment.Nodes.Count > xnm.Count)
                            {
                                for (int k = node_lujingparment.Nodes.Count - 1; k >= xnm.Count; k--)
                                {
                                    node_lujingparment.Nodes.RemoveAt(k);
                                }
                            }
                            if (node_lujingparment.Nodes.Count > j)
                            {
                                node_lujingparment.Nodes[j].Cells[0].Value = xee.Name;
                                node_lujingparment.Nodes[j].Parent.ImageIndex = 0;
                            }
                            else
                            {
                                node_lujingparment.Nodes.Add(xee.Name, "", "", "", "", "");
                                node_lujingparment.Nodes[j].Parent.Nodes[j].ImageIndex = 0;

                            }
                            if (node_lujingparment.Nodes[j].Nodes.Count > xng.Count)
                            {
                                for (int k = node_lujingparment.Nodes[j].Nodes.Count - 1; k >= xng.Count; k--)
                                {
                                    node_lujingparment.Nodes[j].Nodes.RemoveAt(k);
                                }
                            }
                            foreach (XmlNode xnkkk in xng)
                            {
                                XmlElement xeee = (XmlElement)xnkkk;
                                XmlNodeList xngg = xeee.ChildNodes;

                                if (node_lujingparment.Nodes[j].Nodes.Count > m)
                                {
                                    node_lujingparment.Nodes[j].Nodes[m].Cells[0].Value = xeee.Name;
                                }
                                else
                                {
                                    node_lujingparment.Nodes[j].Nodes.Add(xeee.Name, "", "", "", "", "");
                                    node_lujingparment.Nodes[j].Nodes[m].Parent.ImageIndex = 0;
                                }
                                foreach (XmlNode xnkkkk in xngg)
                                {
                                    XmlElement xeeee = (XmlElement)xnkkkk;
                                    XmlNodeList xnggg = xeeee.ChildNodes;
                                    if (node_lujingparment.Nodes[j].Nodes[m].Nodes.Count > n)
                                    {
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes[n].Cells[1].Value = xnggg.Item(0).InnerText.ToString();
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes[n].Cells[2].Value = xnggg.Item(1).InnerText.ToString();
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes[n].Cells[3].Value = xnggg.Item(2).InnerText.ToString();
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes[n].Cells[4].Value = xnggg.Item(3).InnerText.ToString();
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes[n].Cells[5].Value = xnggg.Item(4).InnerText.ToString();
                                    }
                                    else
                                    {
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes.Add("", xnggg.Item(0).InnerText.ToString(), xnggg.Item(1).InnerText.ToString()
                                            , xnggg.Item(2).InnerText.ToString(), xnggg.Item(3).InnerText.ToString(), xnggg.Item(4).InnerText.ToString());
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes[n].Parent.ImageIndex = 0;
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes[n].Parent.Nodes[n].ImageIndex = 1;
                                    }
                                    if (node_lujingparment.Nodes[j].Nodes[m].Nodes[n].Cells[3].Value.ToString() != node_lujingparment.Nodes[j].Nodes[m].Nodes[n].Cells[4].Value.ToString())
                                    {
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes[n].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                    else
                                    {
                                        node_lujingparment.Nodes[j].Nodes[m].Nodes[n].DefaultCellStyle.BackColor = Color.LightGray;
                                    }
                                    n++;
                                }
                                n = 0;
                                m++;
                            }
                            m = 0;
                            j++;
                        }
                    }
                    if (xnk.Name == "流程参数")
                    {
                        Array.Clear(liucheng_rewindex, 0, liucheng_rewindex.Length);
                        i = 0; j = 0; m = 0; n = 0;
                        XmlElement xe = (XmlElement)xnk;
                        XmlNodeList xnm = xe.ChildNodes;
                        foreach (XmlNode xnkk in xnm)
                        {
                            XmlElement xee = (XmlElement)xnkk;
                            XmlNodeList xng = xee.ChildNodes;
                            liucheng_rewindex[j] = int.Parse(Regex.Replace(xee.Name, "[\u4e00-\u9fa5]", string.Empty));
                            if (node_luchengparment.Nodes.Count > xnm.Count)
                            {
                                for (int k = node_luchengparment.Nodes.Count - 1; k >= xnm.Count; k--)
                                {
                                    node_luchengparment.Nodes.RemoveAt(k);
                                }
                            }
                            if (node_luchengparment.Nodes.Count > j)
                            {
                                node_luchengparment.Nodes[j].Cells[0].Value = xee.Name;
                                node_luchengparment.Nodes[j].Parent.ImageIndex = 0;
                            }
                            else
                            {
                                node_luchengparment.Nodes.Add(xee.Name, "", "", "", "", "");
                                node_luchengparment.Nodes[j].Parent.Nodes[j].ImageIndex = 0;
                            }
                            if (node_luchengparment.Nodes[j].Nodes.Count > xng.Count)
                            {
                                for (int k = node_luchengparment.Nodes[j].Nodes.Count - 1; k >= xng.Count; k--)
                                {
                                    node_luchengparment.Nodes[j].Nodes.RemoveAt(k);
                                }
                            }
                            foreach (XmlNode xnkkk in xng)
                            {
                                XmlElement xeee = (XmlElement)xnkkk;
                                XmlNodeList xngg = xeee.ChildNodes;

                                if (node_luchengparment.Nodes[j].Nodes.Count > m)
                                {
                                    node_luchengparment.Nodes[j].Nodes[m].Cells[0].Value = xeee.Name;
                                }
                                else
                                {
                                    node_luchengparment.Nodes[j].Nodes.Add(xeee.Name, "", "", "", "", "");
                                    node_luchengparment.Nodes[j].Nodes[m].Parent.ImageIndex = 0;
                                }
                                foreach (XmlNode xnkkkk in xngg)
                                {
                                    XmlElement xeeee = (XmlElement)xnkkkk;
                                    XmlNodeList xnggg = xeeee.ChildNodes;
                                    if (node_luchengparment.Nodes[j].Nodes[m].Nodes.Count > n)
                                    {
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes[n].Cells[1].Value = xnggg.Item(0).InnerText.ToString();
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes[n].Cells[2].Value = xnggg.Item(1).InnerText.ToString();
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes[n].Cells[3].Value = xnggg.Item(2).InnerText.ToString();
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes[n].Cells[4].Value = xnggg.Item(3).InnerText.ToString();
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes[n].Cells[5].Value = xnggg.Item(4).InnerText.ToString();
                                    }
                                    else
                                    {
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes.Add("", xnggg.Item(0).InnerText.ToString(), xnggg.Item(1).InnerText.ToString()
                                            , xnggg.Item(2).InnerText.ToString(), xnggg.Item(3).InnerText.ToString(), xnggg.Item(4).InnerText.ToString());
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes[n].Parent.ImageIndex = 0;
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes[n].Parent.Nodes[n].ImageIndex = 1;
                                    }
                                    if (node_luchengparment.Nodes[j].Nodes[m].Nodes[n].Cells[3].Value.ToString() != node_luchengparment.Nodes[j].Nodes[m].Nodes[n].Cells[4].Value.ToString())
                                    {
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes[n].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                    else
                                    {
                                        node_luchengparment.Nodes[j].Nodes[m].Nodes[n].DefaultCellStyle.BackColor = Color.LightGray;
                                    }
                                    n++;
                                }
                                n = 0;
                                m++;
                            }
                            m = 0;
                            j++;
                        }
                    }
                }
            }
        }

        private void button4_savetopc_Click( object sender, EventArgs e )//将参数写入PC
        {
            if (node_parment.Nodes.Count>0||node_luchengparment.Nodes.Count>0||node_luchengparment.Nodes.Count>0)
            {
                SaveFileDialog file1 = new SaveFileDialog();
                file1.Filter = "文本文件|*.xml";
                if (file1.ShowDialog() == DialogResult.OK)
                {
                    XmlTextWriter writer = new XmlTextWriter(file1.FileName, null);
                    //使用自动缩进便于阅读
                    writer.Formatting = Formatting.Indented;
                
                    //写入根元素
                    writer.WriteStartElement("Flash参数");

                    writer.WriteStartElement("系统参数");
                    //加入子元素
                    for (int i = 0; i < node_parment.Nodes.Count; i++)
                    {
                        writer.WriteStartElement("参数" + i.ToString());
                        writer.WriteElementString("参数号", node_parment.Nodes[i].Cells[1].Value.ToString());
                        writer.WriteElementString("参数名", "");
                        writer.WriteElementString("当前值", node_parment.Nodes[i].Cells[3].Value.ToString());
                        writer.WriteElementString("设定值", node_parment.Nodes[i].Cells[4].Value.ToString());
                        writer.WriteElementString("描述", "");
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("路径参数");
                    //加入子元素
                    for (int i = 0; i < node_lujingparment.Nodes.Count; i++)
                    {
                        writer.WriteStartElement("路径" + (lujing_rewindex[i]).ToString());                   
                        for (int j = 0; j < node_lujingparment.Nodes[i].Nodes.Count; j++)
                        {
                            writer.WriteStartElement("站点" + ( j + 1 ).ToString());
                            //站点信息  
                            for (int k = 0; k < node_lujingparment.Nodes[i].Nodes[j].Nodes.Count; k++)
                            {
                                writer.WriteStartElement("参数" + k.ToString());
                                writer.WriteElementString("参数号", node_lujingparment.Nodes[i].Nodes[j].Nodes[k].Cells[1].Value.ToString());
                                writer.WriteElementString("参数名", node_lujingparment.Nodes[i].Nodes[j].Nodes[k].Cells[2].Value.ToString());
                                writer.WriteElementString("当前值", node_lujingparment.Nodes[i].Nodes[j].Nodes[k].Cells[3].Value.ToString());
                                writer.WriteElementString("设定值", node_lujingparment.Nodes[i].Nodes[j].Nodes[k].Cells[4].Value.ToString());
                                writer.WriteElementString("描述", node_lujingparment.Nodes[i].Nodes[j].Nodes[k].Cells[5].Value.ToString());
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("流程参数");
                    //加入子元素
                    for (int i = 0; i < node_luchengparment.Nodes.Count; i++)
                    {
                        writer.WriteStartElement("流程" + (liucheng_rewindex[i]).ToString());                   
                        for (int j = 0; j < node_luchengparment.Nodes[i].Nodes.Count; j++)
                        {
                            writer.WriteStartElement("序号" + ( j + 1 ).ToString());
                            //流程信息  
                            for (int k = 0; k < node_luchengparment.Nodes[i].Nodes[j].Nodes.Count; k++)
                            {
                                writer.WriteStartElement("参数" + k.ToString());
                                writer.WriteElementString("参数号", node_luchengparment.Nodes[i].Nodes[j].Nodes[k].Cells[1].Value.ToString());
                                writer.WriteElementString("参数名", node_luchengparment.Nodes[i].Nodes[j].Nodes[k].Cells[2].Value.ToString());
                                writer.WriteElementString("当前值", node_luchengparment.Nodes[i].Nodes[j].Nodes[k].Cells[3].Value.ToString());
                                writer.WriteElementString("设定值", node_luchengparment.Nodes[i].Nodes[j].Nodes[k].Cells[4].Value.ToString());
                                writer.WriteElementString("描述", node_luchengparment.Nodes[i].Nodes[j].Nodes[k].Cells[5].Value.ToString());
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    //关闭根元素，并书写结束标签
                    writer.WriteEndElement();
                    //将XML写入文件并且关闭XmlTextWriter
                    writer.Close();
                }
            }
            else
            {
                MessageBox.Show("没有参数可保存到PC！","提示");
            }
            
        }
        private long received_count = 0, send_count = 0;
        Thread duquparment;
        int duquparment_flag = 0;
        int key_presss = 0;
        private void button3_savetoflash_Click( object sender, EventArgs e )
        {
            if (serialPort1.IsOpen)
            {
                if (node_parment.Nodes.Count == 0 && node_lujingparment.Nodes.Count == 0 && node_luchengparment.Nodes.Count == 0)
                {
                    MessageBox.Show("没有参数可上传！", "提示");
                }
                else
                {
                    duquparment = new Thread(duquparmentFun);
                    duquparment.Start();
                    duquparment_flag = 1;
                    buffer.Clear();
                    parmentdown_flag = 0;
                    lujingdown_flag = 0;
                    liuchengdown_flag = 0;
                    key_presss = 2;
                    if (checkBox_liucheng.Checked == false && checkBox_lujing.Checked == false && checkBox_parment.Checked == false)
                    {
                        MessageBox.Show("没有参数可上传！", "提示");
                    }
                    else
                    {
                        //启动线程
                        Thread thdSub = new Thread(ThreadFun);
                        thdSub.Start();
                        numm = 0;
                        timer1.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("请打开串口!", "错误提示");
            }
        }
        int lujingdown_index = 0,liuchengdown_index = 0;
        private void duquparmentFun()//PC下载参数到Flash
        {
            send_count = 4;
            int sum = 0, time = 0;
            if (serialPort1.IsOpen)
            {
                if (checkBox_parment.Checked == true)
                {
                    if (node_parment.Nodes.Count != 0)
                    {
                        parment_flag = 0;
                        send_count = 8;
                        sum = 0;
                        Byte[] buf = new byte[send_count];
                        for (int i = 0; i < node_parment.Nodes.Count; i++)
                        {
                            if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环   
                            try
                            {
                                Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                                buf[0] = 0xAA;
                                buf[1] = 0xaa;
                                buf[2] = (byte)( i >> 8 );
                                buf[3] = (byte)( i & 0xff );
                                buf[4] = Convert.ToByte(int.Parse(node_parment.Nodes[i].Cells[4].Value.ToString()) >> 8);
                                buf[5] = Convert.ToByte(int.Parse(node_parment.Nodes[i].Cells[4].Value.ToString()) & 0xff);
                                sum = buf[0] + buf[1] + buf[2] + buf[3] + buf[4] + buf[5];
                                buf[6] = Convert.ToByte(sum >> 8);
                                buf[7] = Convert.ToByte(sum & 0xff);
                                serialPort1.Write(buf, 0, buf.Length);
                                parment_flag = 0;
                                time = 0;
                            }
                            finally
                            {
                                Listening = false;//我用完了，ui可以关闭串口了。
                            }
                            while (parment_flag != 1)//
                            {
                                if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环 
                                if (busy == 0)
                                {
                                    time++;
                                    if (time == 5)
                                    {
                                        time = 0;
                                        sum = 0;
                                        buffer.Clear();
                                        parment_flag = 2;
                                    }
                                    if (parment_flag == 2)
                                    {
                                        try
                                        {
                                            Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                                                             //重发
                                            buf[0] = 0xAA;
                                            buf[1] = 0xaa;
                                            buf[2] = (byte)( i >> 8 );
                                            buf[3] = (byte)( i & 0xff );
                                            buf[4] = Convert.ToByte(int.Parse(node_parment.Nodes[i].Cells[4].Value.ToString()) >> 8);
                                            buf[5] = Convert.ToByte(int.Parse(node_parment.Nodes[i].Cells[4].Value.ToString()) & 0xff);
                                            sum = buf[0] + buf[1] + buf[2] + buf[3] + buf[4] + buf[5];
                                            buf[6] = Convert.ToByte(sum >> 8);
                                            buf[7] = Convert.ToByte(sum & 0xff);
                                            serialPort1.Write(buf, 0, buf.Length);
                                            parment_flag = 0;
                                        }
                                        finally
                                        {
                                            Listening = false;//我用完了，ui可以关闭串口了。
                                        }
                                    }
                                }
                                Thread.Sleep(10);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("没有系统参数可上传到Flash!", "提示");
                    }
                }
                if (checkBox_lujing.Checked == true)
                {
                    if (node_parment.Nodes[0].Cells[3].Value.ToString() == node_parment.Nodes[0].Cells[4].Value.ToString())
                    {
                        if (node_lujingparment.Nodes.Count != 0)
                        {
                            lujing_flag = 0;
                            sum = 0;
                            send_count = 10+ zhandianxinxi_sum*2;
                            Byte[] buf = new byte[send_count];
                            for (int i = 1; i <= node_lujingparment.Nodes.Count; i++)
                            {
                                if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环 
                                lujingdown_index = i;
                                this.Invoke((EventHandler)(delegate
                                {

                                }));

                                for (int j = 0; j < node_lujingparment.Nodes[i-1].Nodes.Count; j++)
                                {
                                    try
                                    {
                                        Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                                        int k = node_lujingparment.Nodes[i-1].Nodes.Count;
                                        buf[0] = 0xAA;
                                        buf[1] = 0xbb;
                                        buf[2] = (byte)(lujing_rewindex[i-1] >> 8 );
                                        buf[3] = (byte)(lujing_rewindex[i-1] & 0xff );

                                        buf[4] = (byte)( k >> 8 );
                                        buf[5] = (byte)( k & 0xff );

                                        buf[6] = (byte)( j >> 8 );
                                        buf[7] = (byte)( j & 0xff );
                                        for (int n = 0; n < node_lujingparment.Nodes[i-1].Nodes[j].Nodes.Count; n++)
                                        {
                                            buf[8+2*n] = Convert.ToByte(int.Parse(node_lujingparment.Nodes[i - 1].Nodes[j].Nodes[n].Cells[4].Value.ToString()) >> 8);
                                            buf[9+2*n] = Convert.ToByte(int.Parse(node_lujingparment.Nodes[i - 1].Nodes[j].Nodes[n].Cells[4].Value.ToString()) & 0xff);
                                        }
                                        for (int m = 0; m < (8 + zhandianxinxi_sum * 2); m++)
                                        {
                                            sum += buf[m];
                                        }
                                        buf[send_count - 2] = Convert.ToByte(sum >> 8);
                                        buf[send_count - 1] = Convert.ToByte(sum & 0xff);
                                        serialPort1.Write(buf, 0, buf.Length);

                                        lujing_flag = 0;
                                        time = 0;
                                    }
                                    finally
                                    {
                                        Listening = false;//我用完了，ui可以关闭串口了。
                                    }
                                    while (lujing_flag != 1)//
                                    {
                                        if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环 
                                        if (busy == 0)
                                        {
                                            time++;
                                            if (time == 5)
                                            {
                                                time = 0;
                                                sum = 0;
                                                buffer.Clear();
                                                lujing_flag = 2;
                                            }
                                            if (lujing_flag == 2)
                                            {
                                                try
                                                {
                                                    Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                                                                     //重发
                                                    int k = node_lujingparment.Nodes[i - 1].Nodes.Count;
                                                    buf[0] = 0xAA;
                                                    buf[1] = 0xbb;
                                                    buf[2] = (byte)(lujing_rewindex[i - 1] >> 8);
                                                    buf[3] = (byte)(lujing_rewindex[i - 1] & 0xff);

                                                    buf[4] = (byte)(k >> 8);
                                                    buf[5] = (byte)(k & 0xff);

                                                    buf[6] = (byte)(j >> 8);
                                                    buf[7] = (byte)(j & 0xff);
                                                    for (int n = 0; n < node_lujingparment.Nodes[i - 1].Nodes[j].Nodes.Count; n++)
                                                    {
                                                        buf[8 + 2 * n] = Convert.ToByte(int.Parse(node_lujingparment.Nodes[i - 1].Nodes[j].Nodes[n].Cells[4].Value.ToString()) >> 8);
                                                        buf[9 + 2 * n] = Convert.ToByte(int.Parse(node_lujingparment.Nodes[i - 1].Nodes[j].Nodes[n].Cells[4].Value.ToString()) & 0xff);
                                                    }
                                                    for (int m = 0; m < (8 + zhandianxinxi_sum * 2); m++)
                                                    {
                                                        sum += buf[m];
                                                    }
                                                    buf[send_count - 2] = Convert.ToByte(sum >> 8);
                                                    buf[send_count - 1] = Convert.ToByte(sum & 0xff);
                                                    serialPort1.Write(buf, 0, buf.Length);
                                                    lujing_flag = 0;
                                                }
                                                finally
                                                {
                                                    Listening = false;//我用完了，ui可以关闭串口了。
                                                }
                                            }
                                        }
                                        Thread.Sleep(10);
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("没有路径参数可上传到Flash!", "提示");
                        }
                    }
                    else
                    {
                        lujing_num = 0;
                    }
                }
                if (checkBox_liucheng.Checked == true)
                {
                    if (node_parment.Nodes[0].Cells[3].Value.ToString() == node_parment.Nodes[0].Cells[4].Value.ToString())
                    {
                        if (node_luchengparment.Nodes.Count != 0)
                        {
                            liucheng_flag = 0;
                            sum = 0;
                            send_count = 10+xuhaoxinxi_sum*2;
                            Byte[] buf = new byte[send_count];
                            for (int i = 1; i <= node_luchengparment.Nodes.Count; i++)
                            {
                                if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环 
                                liuchengdown_index = i;
                                this.Invoke((EventHandler)(delegate
                                {

                                }));
                                
                                for (int j = 0; j < node_luchengparment.Nodes[i-1].Nodes.Count; j++)
                                {
                                    try
                                    {
                                        Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                                        int k = node_luchengparment.Nodes[i-1].Nodes.Count;
                                        buf[0] = 0xAA;
                                        buf[1] = 0xee;
                                        buf[2] = (byte)(liucheng_rewindex[i-1] >> 8 );
                                        buf[3] = (byte)(liucheng_rewindex[i-1] & 0xff );

                                        buf[4] = (byte)( k >> 8 );
                                        buf[5] = (byte)( k & 0xff );

                                        buf[6] = (byte)( j >> 8 );
                                        buf[7] = (byte)( j & 0xff );
                                        for (int n = 0; n < node_luchengparment.Nodes[i-1].Nodes[j].Nodes.Count; n++)
                                        {
                                            buf[8+2*n] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i-1].Nodes[j].Nodes[n].Cells[4].Value.ToString()) >> 8);
                                            buf[9+2*n] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i - 1].Nodes[j].Nodes[n].Cells[4].Value.ToString()) & 0xff);
                                        }

                                        //buf[10] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i-1].Nodes[j].Nodes[1].Cells[4].Value.ToString()) >> 8);
                                        //buf[11] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i-1].Nodes[j].Nodes[1].Cells[4].Value.ToString()) & 0xff);
                                        //buf[12] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i-1].Nodes[j].Nodes[2].Cells[4].Value.ToString()) >> 8);
                                        //buf[13] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i - 1].Nodes[j].Nodes[2].Cells[4].Value.ToString()) & 0xff);
                                        for (int m = 0; m < (8+xuhaoxinxi_sum * 2); m++)
                                        {
                                            sum += buf[m];
                                        }
                                        //sum = buf[0] + buf[1] + buf[2] + buf[3] + buf[4] + buf[5] + buf[6] + buf[7] + buf[8] + buf[9] + buf[10] + buf[11]
                                        //        + buf[12] + buf[13];
                                        buf[send_count - 2] = Convert.ToByte(sum >> 8);
                                        buf[send_count - 1] = Convert.ToByte(sum & 0xff);
                                        serialPort1.Write(buf, 0, buf.Length);

                                        liucheng_flag = 0;
                                        time = 0;
                                    }
                                    finally
                                    {
                                        Listening = false;//我用完了，ui可以关闭串口了。
                                    }
                                    while (liucheng_flag != 1)//
                                    {
                                        if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环 
                                        if (busy == 0)
                                        {
                                            time++;
                                            if (time == 5)
                                            {
                                                time = 0;
                                                sum = 0;
                                                buffer.Clear();
                                                liucheng_flag = 2;
                                            }
                                            if (liucheng_flag == 2)
                                            {
                                                try
                                                {
                                                    Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                                                                     //重发
                                                    int k = node_luchengparment.Nodes[i - 1].Nodes.Count;
                                                    buf[0] = 0xAA;
                                                    buf[1] = 0xee;
                                                    buf[2] = (byte)(liucheng_rewindex[i - 1] >> 8);
                                                    buf[3] = (byte)(liucheng_rewindex[i - 1] & 0xff);

                                                    buf[4] = (byte)(k >> 8);
                                                    buf[5] = (byte)(k & 0xff);

                                                    buf[6] = (byte)(j >> 8);
                                                    buf[7] = (byte)(j & 0xff);
                                                    for (int n = 0; n < node_luchengparment.Nodes[i - 1].Nodes[j].Nodes.Count; n++)
                                                    {
                                                        buf[8 + 2 * n] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i - 1].Nodes[j].Nodes[n].Cells[4].Value.ToString()) >> 8);
                                                        buf[9 + 2 * n] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i - 1].Nodes[j].Nodes[n].Cells[4].Value.ToString()) & 0xff);
                                                    }

                                                    //buf[10] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i-1].Nodes[j].Nodes[1].Cells[4].Value.ToString()) >> 8);
                                                    //buf[11] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i-1].Nodes[j].Nodes[1].Cells[4].Value.ToString()) & 0xff);
                                                    //buf[12] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i-1].Nodes[j].Nodes[2].Cells[4].Value.ToString()) >> 8);
                                                    //buf[13] = Convert.ToByte(int.Parse(node_luchengparment.Nodes[i - 1].Nodes[j].Nodes[2].Cells[4].Value.ToString()) & 0xff);
                                                    for (int m = 0; m < (8 + xuhaoxinxi_sum * 2); m++)
                                                    {
                                                        sum += buf[m];
                                                    }
                                                    //sum = buf[0] + buf[1] + buf[2] + buf[3] + buf[4] + buf[5] + buf[6] + buf[7] + buf[8] + buf[9] + buf[10] + buf[11]
                                                    //        + buf[12] + buf[13];
                                                    buf[send_count - 2] = Convert.ToByte(sum >> 8);
                                                    buf[send_count - 1] = Convert.ToByte(sum & 0xff);
                                                    serialPort1.Write(buf, 0, buf.Length);
                                                    liucheng_flag = 0;
                                                }
                                                finally
                                                {
                                                    Listening = false;//我用完了，ui可以关闭串口了。
                                                }
                                            }
                                        }
                                        Thread.Sleep(10);
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("没有流程参数可上传到Flash!", "提示");
                        }
                    }
                    else
                    {
                        liucheng_num = 0;
                    }
                }               
            }
            else
            {
                MessageBox.Show("请打开串口!", "错误提示");
            }
            timer1.Stop();
            duquparment.Abort();
        }
        Thread thsyspara;
        int thsyspara_flag = 0;

        private void button5_readformflash_Click( object sender, EventArgs e )
        {
            //设置指定节点的值
            //tvTaskList.Nodes[1].Nodes[0].Nodes[0].SetValues("df4", "df4", "df4", "df4", "df4");
            //tvTaskList.Nodes[1].Nodes[0].Nodes[0].Cells[2].Value = "d";
            //tvTaskList.Nodes[1].Nodes[0].Nodes[1].Cells[2].Value = tvTaskList.Nodes[1].Nodes[0].Nodes[0].Cells[2].Value;
            //删除指定节点
            //node_parment.Nodes.RemoveAt(1);
            //清空子节点
            //node_parment.Nodes.Clear();
            //删除所有
            //node_parment.Nodes.Remove(node_parment);
 
            if (serialPort1.IsOpen)
            {
                thsyspara = new Thread(thsysparaFun);
                thsyspara.Start();
                thsyspara_flag = 1;
                lijingread_now = 0;
                liuchengread_now = 0;
                countnum = 0;
                lujing_rewnum = 0;
                liucheng_rewnum = 0;
                buffer.Clear();
                Array.Clear(lujing_rewindex, 0, lujing_rewindex.Length);
                Array.Clear(liucheng_rewindex, 0, liucheng_rewindex.Length);
                key_presss = 1;
                if (node_parment.Nodes.Count > 0)
                {
                    if (node_parment.Nodes[0].Cells[3].Value.ToString() != node_parment.Nodes[0].Cells[4].Value.ToString())
                    {
                        node_parment.Nodes.Clear();
                        node_lujingparment.Nodes.Clear();
                        node_luchengparment.Nodes.Clear();
                    }
                }
                //启动线程
                Thread thdSub = new Thread(ThreadFun);
                thdSub.Start();
                numm = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("请打开串口!", "错误提示");
            }
        }
        private void ShowProcessBar()
        {
            //进度条窗体对象
            myProcessBar = new Process();
            //委托
            myIncrease = new IncreaseHandle(myProcessBar.Increase);
            //弹出模式窗体
            myProcessBar.StartPosition = FormStartPosition.CenterParent;
            myProcessBar.ShowDialog();
            myProcessBar = null;
        }
        private void ThreadFun()
        {
            MethodInvoker mi = new MethodInvoker(ShowProcessBar);
            this.BeginInvoke(mi);
            Thread.Sleep(50);//Sleep a while to show window
            bool blnIncreased = false;
            object objReturn = null;
            if (key_presss == 1)//读取到PC
            {
                this.Invoke((EventHandler)(delegate
                {
                    myProcessBar.Text = "正在读取...";
                }));                  
                do
                {
                    Thread.Sleep(1);
                    try
                    {
                        objReturn = this.Invoke(this.myIncrease, countnum + lijingread_now + liuchengread_now, 0, 100 + lujing_num + liucheng_num);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("读取未完成！");
                        break;
                    }

                    blnIncreased = (bool)objReturn;
                }
                while (blnIncreased);
            }
            else//下载到Flash
            {
                this.Invoke((EventHandler)(delegate
                {
                    myProcessBar.Text = "正在下载...";                   
                }));
                do
                {
                    Thread.Sleep(2);
                    if (checkBox_parment.Checked == true && checkBox_lujing.Checked == true && checkBox_liucheng.Checked == true)
                    {
                        try
                        {
                            objReturn = this.Invoke(this.myIncrease, parmentdown_flag + lujingdown_flag + liuchengdown_flag, 0, 100 + lujing_num + liucheng_num);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("下载未完成！");
                            break;
                        }
 
                    }
                    else if (checkBox_parment.Checked == true && checkBox_lujing.Checked == true && checkBox_liucheng.Checked == false)
                    {
                        try
                        {
                            objReturn = this.Invoke(this.myIncrease, parmentdown_flag + lujingdown_flag, 0, 100 + lujing_num);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("下载未完成！");
                            break;
                        }
                   }
                    else if (checkBox_parment.Checked == true && checkBox_lujing.Checked == false && checkBox_liucheng.Checked == true)
                    {
                        try
                        {
                            objReturn = this.Invoke(this.myIncrease, parmentdown_flag + liuchengdown_flag, 0, 100 + liucheng_num);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("下载未完成！");
                            break;
                        }

                    }
                    else if (checkBox_parment.Checked == true && checkBox_lujing.Checked == false && checkBox_liucheng.Checked == false)
                    {
                        try
                        {
                            objReturn = this.Invoke(this.myIncrease, parmentdown_flag, 0, 100);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("下载未完成！");
                            break;
                        }

                    }
                    else if (checkBox_parment.Checked == false && checkBox_lujing.Checked == true && checkBox_liucheng.Checked == true)
                    {
                        try
                        {
                            objReturn = this.Invoke(this.myIncrease, lujingdown_flag + liuchengdown_flag, 0, lujing_num + liucheng_num);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("下载未完成！");
                            break;
                        }

                    }
                    else if (checkBox_parment.Checked == false && checkBox_lujing.Checked == false && checkBox_liucheng.Checked == true)
                    {
                        try
                        {
                            objReturn = this.Invoke(this.myIncrease, liuchengdown_flag, 0,liucheng_num);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("下载未完成！");
                            break;
                        }

                    }
                    else if (checkBox_parment.Checked == false && checkBox_lujing.Checked == true && checkBox_liucheng.Checked == false)
                    {
                        try
                        {
                            objReturn = this.Invoke(this.myIncrease, lujingdown_flag, 0, lujing_num);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("下载未完成！");
                            break;
                        }

                    }
                    blnIncreased = (bool)objReturn;
                }
                while (blnIncreased);
            }

        }

        int readstate = 0, lujing_num = 0,liucheng_num;//0:空,1:ok,2:重发
        TreeGridNode node_lujing,node_liucheng;
        private void thsysparaFun()//PC读取Flash参数
        {
            int timeout = 0;
            if (serialPort1.IsOpen)
            {
                ////系统参数获取
                Byte[] buf = new byte[6];
                for (int i = 0; i <= 100; i++)
                {
                    if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环   
                    try
                    {
                        Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                        buf[0] = 0xAA;
                        buf[1] = (byte)( i >> 8 );

                        buf[2] = (byte)( i & 0xff );
                        buf[3] = 0xb4;
                        int sum = buf[0] + buf[1] + buf[2] + buf[3];
                        buf[4] = Convert.ToByte(sum >> 8);
                        buf[5] = Convert.ToByte(sum & 0xff);
                        serialPort1.Write(buf, 0, buf.Length);
                        readstate = 0;
                        timeout = 0;
                    }
                    finally
                    {
                        Listening = false;//我用完了，ui可以关闭串口了。
                    }
                    while (readstate != 1)//
                    {
                        if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环 
                        if (busy == 0)
                        {
                           timeout++;
                            if (timeout == 10)
                            {
                                timeout = 0;
                                buffer.Clear();
                                readstate = 2;
                            }
                            if (readstate == 2)
                            {
                                try
                                {
                                    Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                                    //重发
                                    buf[0] = 0xAA;
                                    buf[1] = (byte)( i >> 8 );

                                    buf[2] = (byte)( i & 0xff );
                                    buf[3] = 0xb4;

                                    int sum = buf[0] + buf[1] + buf[2] + buf[3];
                                    buf[4] = Convert.ToByte(sum >> 8);
                                    buf[5] = Convert.ToByte(sum & 0xff);
                                    serialPort1.Write(buf, 0, buf.Length);
                                    readstate = 0;
                                }
                                finally
                                {
                                    Listening = false;//我用完了，ui可以关闭串口了。
                                }
                            }
                        }
                        Thread.Sleep(10);
                    }
                }
                //路径获取
                Byte[] buf2 = new byte[6];
                for (int i = 1; i <= lujing_num; i++)
                {
                    if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环
                    try
                    {
                        Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                        buf2[0] = 0xBB;
                        buf2[1] = (byte)( i >> 8 );

                        buf2[2] = (byte)( i & 0xff );
                        buf2[3] = 0xb4;
                        int sum = buf2[0] + buf2[1] + buf2[2] + buf2[3];
                        buf2[4] = Convert.ToByte(sum >> 8);
                        buf2[5] = Convert.ToByte(sum & 0xff);
                        serialPort1.Write(buf2, 0, buf2.Length);
                        readstate = 0;
                        timeout = 0;
                    }
                    finally
                    {
                        Listening = false;//我用完了，ui可以关闭串口了。
                    }
                    while (readstate != 1)//
                    {
                        if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环 
                        if (busy == 0)
                        {
                           timeout++;
                            if (timeout == 10)
                            {
                                timeout = 0;
                                buffer.Clear();
                                readstate = 2;
                            }
                            if (readstate == 2)
                            {
                                try
                                {
                                    Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                                    //重发
                                    buf2[0] = 0xBB;
                                    buf2[1] = (byte)( i >> 8 );

                                    buf2[2] = (byte)( i & 0xff );
                                    buf2[3] = 0xb4;
                                    int sum = buf2[0] + buf2[1] + buf2[2] + buf2[3];
                                    buf2[4] = Convert.ToByte(sum >> 8);
                                    buf2[5] = Convert.ToByte(sum & 0xff);
                                    serialPort1.Write(buf2, 0, buf2.Length);
                                    readstate = 0;
                                }
                                finally
                                {
                                    Listening = false;//我用完了，ui可以关闭串口了。
                                }
                            }
                        }
                        Thread.Sleep(10);
                    }
                }
                //流程获取
                Byte[] buf1 = new byte[6];
                for (int i = 1; i <= liucheng_num; i++)
                {
                    if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环
                    try
                    {
                        Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                        buf1[0] = 0xEE;
                        buf1[1] = (byte)( i >> 8 );

                        buf1[2] = (byte)( i & 0xff );
                        buf1[3] = 0xb4;
                        int sum = buf1[0] + buf1[1] + buf1[2] + buf1[3];
                        buf1[4] = Convert.ToByte(sum >> 8);
                        buf1[5] = Convert.ToByte(sum & 0xff);
                        serialPort1.Write(buf1, 0, buf1.Length);
                        readstate = 0;
                        timeout = 0;
                    }
                    finally
                    {
                        Listening = false;//我用完了，ui可以关闭串口了。
                    }
                    while (readstate != 1)//
                    {
                        if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环 
                        if (busy == 0)
                        {
                            timeout++;
                            if (timeout == 10)
                            {
                                timeout = 0;
                                buffer.Clear();
                                readstate = 2;
                            }
                            if (readstate == 2)
                            {
                                try
                                {
                                    Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                                    //重发
                                    buf1[0] = 0xEE;
                                    buf1[1] = (byte)( i >> 8 );

                                    buf1[2] = (byte)( i & 0xff );
                                    buf1[3] = 0xb4;
                                    int sum = buf1[0] + buf1[1] + buf1[2] + buf1[3];
                                    buf1[4] = Convert.ToByte(sum >> 8);
                                    buf1[5] = Convert.ToByte(sum & 0xff);
                                    serialPort1.Write(buf1, 0, buf1.Length);
                                    readstate = 0;
                                }
                                finally
                                {
                                    Listening = false;//我用完了，ui可以关闭串口了。
                                }
                            }
                        }
                        Thread.Sleep(10);
                    }
                }
            }
            else
            {
                MessageBox.Show("请打开串口!", "错误提示");
            }
            timer1.Stop();
            thsyspara.Abort();
        }

        private void dataGridView1_RowPostPaint( object sender, DataGridViewRowPostPaintEventArgs e )
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                tvTaskList.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, ( e.RowIndex + 1 ).ToString(),
                tvTaskList.RowHeadersDefaultCellStyle.Font,
                rectangle,
                tvTaskList.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void button1_Click( object sender, EventArgs e )
        {
            //清空子节点
            node_parment.Nodes.Clear();
            node_lujingparment.Nodes.Clear();
            node_luchengparment.Nodes.Clear();
            countnum = 0;
            lujing_num = 0;
            liucheng_num = 0;
            label3.Text = "系  统：" + countnum.ToString();
            label4.Text = "路径数：" + lujing_num.ToString();
            label5.Text = "流程数：" + liucheng_num.ToString();
        }
        TreeGridNode node_parment_son, node_lujing_son,node_liucheng_son;
        private List<byte> buffer = new List<byte>(4096);

        private void timer1_Tick( object sender, EventArgs e )
        {
            numm++;
            textBox1.Text = numm.ToString();
        }

        int numm = 0;

        private void tvTaskList_CellClick( object sender, DataGridViewCellEventArgs e )
        {
            textBox1.Text = tvTaskList.CurrentRow.Cells[5].Value.ToString();
            try
            {
                if (tvTaskList.Rows[e.RowIndex].Cells[0].Value.ToString() != "")
                {
                    tvTaskList.Rows[e.RowIndex].ReadOnly = true;
                }
            }
            catch (Exception)
            {
            }

        }

        int a;
        private void tvTaskList_CellBeginEdit( object sender, DataGridViewCellCancelEventArgs e )
        {
            a = int.Parse(tvTaskList.CurrentRow.Cells[4].Value.ToString());
        }
        private void tvTaskList_CellEndEdit( object sender, DataGridViewCellEventArgs e )
        {
            int b = int.Parse(tvTaskList.CurrentRow.Cells[4].Value.ToString());
            if (a != b)
                tvTaskList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
        }

        private void button_savemiaoshu_Click(object sender, EventArgs e)
        {
            if (node_parment.Nodes.Count>0)
            {
                if (File.Exists(@System.Environment.CurrentDirectory + "\\" + "parment.xml"))
                {
                    File.SetAttributes(System.Environment.CurrentDirectory + "\\" + "parment.xml", FileAttributes.Normal);
                }
                XmlTextWriter writer = new XmlTextWriter(System.Environment.CurrentDirectory+"\\"+"parment.xml", null);
                //使用自动缩进便于阅读
                writer.Formatting = Formatting.Indented;
                File.SetAttributes(System.Environment.CurrentDirectory + "\\" + "parment.xml", FileAttributes.Hidden);
                //写入根元素
                writer.WriteStartElement("系统参数");
                //加入子元素
                for (int i = 0; i < node_parment.Nodes.Count; i++)
                {
                    writer.WriteStartElement("参数" + i.ToString());
                    writer.WriteElementString("参数号", node_parment.Nodes[i].Cells[1].Value.ToString());
                    writer.WriteElementString("参数名", node_parment.Nodes[i].Cells[2].Value.ToString());
                    writer.WriteElementString("描述", node_parment.Nodes[i].Cells[5].Value.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
            }
            else
            {
                MessageBox.Show("没有建立系统参数，请先建立！", "提示");
            }
            if (node_lujingparment.Nodes.Count > 0)
            {
                if (File.Exists(@System.Environment.CurrentDirectory + "\\" + "lujingparment.xml"))
                {
                    File.SetAttributes(System.Environment.CurrentDirectory + "\\" + "lujingparment.xml", FileAttributes.Normal);
                }
                XmlTextWriter writer = new XmlTextWriter(System.Environment.CurrentDirectory + "\\" + "lujingparment.xml", null);
                //使用自动缩进便于阅读
                writer.Formatting = Formatting.Indented;
                File.SetAttributes(System.Environment.CurrentDirectory + "\\" + "lujingparment.xml", FileAttributes.Hidden);
                //写入根元素
                writer.WriteStartElement("路径参数");
                //加入子元素
                for (int i = 0; i < node_lujingparment.Nodes[0].Nodes[0].Nodes.Count; i++)
                {
                    writer.WriteStartElement("参数" + i.ToString());
                    writer.WriteElementString("参数号", node_lujingparment.Nodes[0].Nodes[0].Nodes[i].Cells[1].Value.ToString());
                    writer.WriteElementString("参数名", node_lujingparment.Nodes[0].Nodes[0].Nodes[i].Cells[2].Value.ToString());
                    writer.WriteElementString("描述", node_lujingparment.Nodes[0].Nodes[0].Nodes[i].Cells[5].Value.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
            }
            else
            {
                MessageBox.Show("没有建立路径参数，请先建立！", "提示");
            }
            if (node_luchengparment.Nodes.Count > 0)
            {
                if (File.Exists(@System.Environment.CurrentDirectory + "\\" + "luchengparment.xml"))
                {
                    File.SetAttributes(System.Environment.CurrentDirectory + "\\" + "luchengparment.xml", FileAttributes.Normal);
                }
                XmlTextWriter writer = new XmlTextWriter(System.Environment.CurrentDirectory + "\\" + "luchengparment.xml", null);
                //使用自动缩进便于阅读
                writer.Formatting = Formatting.Indented;
                File.SetAttributes(System.Environment.CurrentDirectory + "\\" + "luchengparment.xml", FileAttributes.Hidden);
                //写入根元素
                writer.WriteStartElement("流程参数");
                //加入子元素
                for (int i = 0; i < node_luchengparment.Nodes[0].Nodes[0].Nodes.Count; i++)
                {
                    writer.WriteStartElement("参数" + i.ToString());
                    writer.WriteElementString("参数号", node_luchengparment.Nodes[0].Nodes[0].Nodes[i].Cells[1].Value.ToString());
                    writer.WriteElementString("参数名", node_luchengparment.Nodes[0].Nodes[0].Nodes[i].Cells[2].Value.ToString());
                    writer.WriteElementString("描述", node_luchengparment.Nodes[0].Nodes[0].Nodes[i].Cells[5].Value.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
            }
            else
            {
                MessageBox.Show("没有建立路径参数，请先建立！", "提示");
            }
        }

        private void button_readmiaoshu_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (node_parment.Nodes.Count > 0)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.Environment.CurrentDirectory + "\\" + "parment.xml");

                XmlNode xn = xmlDoc.SelectSingleNode("系统参数");

                XmlNodeList xnn = xn.ChildNodes;

                foreach (XmlNode xnk in xnn)
                {
                    XmlElement xe = (XmlElement)xnk;
                    XmlNodeList xnm = xe.ChildNodes;
                    node_parment.Nodes[i].Cells[0].Value = "";
                    node_parment.Nodes[i].Cells[2].Value = xnm.Item(1).InnerText.ToString();
                    node_parment.Nodes[i].Cells[5].Value = xnm.Item(2).InnerText.ToString();
                    i++;
                }
            }
            else
            {
                MessageBox.Show("没有建立系统参数，请先建立！","提示");
            }
            i = 0;
            if (node_lujingparment.Nodes.Count > 0)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.Environment.CurrentDirectory + "\\" + "lujingparment.xml");

                XmlNode xn = xmlDoc.SelectSingleNode("路径参数");

                XmlNodeList xnn = xn.ChildNodes;
                for (int j = 0; j < node_lujingparment.Nodes.Count; j++)
                {
                    for (int k = 0; k < node_lujingparment.Nodes[j].Nodes.Count; k++)
                    {
                        foreach (XmlNode xnk in xnn)
                        {
                            XmlElement xe = (XmlElement)xnk;
                            XmlNodeList xnm = xe.ChildNodes;
                            node_lujingparment.Nodes[j].Nodes[k].Nodes[i].Cells[2].Value = xnm.Item(1).InnerText.ToString();
                            node_lujingparment.Nodes[j].Nodes[k].Nodes[i].Cells[5].Value = xnm.Item(2).InnerText.ToString();
                            i++;
                        }
                        i = 0;
                    }
                }
            }
            else
            {
                MessageBox.Show("没有建立路径参数，请先建立！", "提示");
            }
            i = 0;
            if (node_luchengparment.Nodes.Count > 0)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.Environment.CurrentDirectory + "\\" + "luchengparment.xml");

                XmlNode xn = xmlDoc.SelectSingleNode("流程参数");

                XmlNodeList xnn = xn.ChildNodes;
                for (int j = 0; j < node_luchengparment.Nodes.Count; j++)
                {
                    for (int k = 0; k < node_luchengparment.Nodes[j].Nodes.Count; k++)
                    {
                        foreach (XmlNode xnk in xnn)
                        {
                            XmlElement xe = (XmlElement)xnk;
                            XmlNodeList xnm = xe.ChildNodes;
                            node_luchengparment.Nodes[j].Nodes[k].Nodes[i].Cells[2].Value = xnm.Item(1).InnerText.ToString();
                            node_luchengparment.Nodes[j].Nodes[k].Nodes[i].Cells[5].Value = xnm.Item(2).InnerText.ToString();
                            i++;
                        }
                        i = 0;
                    }
                }
            }
            else
            {
                MessageBox.Show("没有建立流程参数，请先建立！", "提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("软件版本：V3.1\n作者：TianGk\nQQ：931683637","关于");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            label6.Text = string.Format("{0:F}", dt);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }
        public class Info
        {
            public string Id { get; set; }
            public string Name { get; set; }

        }
        private void tvTaskList_CurrentCellChanged(object sender, EventArgs e)
        {
  //          try
  //          {
  //              this.comboBox1.Visible = false;
  //              this.comboBox1.Width = 0;
  //              this.comboBox1.Left = this.tvTaskList.GetCellDisplayRectangle(this.tvTaskList.CurrentCell.ColumnIndex, this.tvTaskList.CurrentCell.RowIndex, true).Left;
  //              this.comboBox1.Top = this.tvTaskList.GetCellDisplayRectangle(this.tvTaskList.CurrentCell.ColumnIndex, this.tvTaskList.CurrentCell.RowIndex, true).Top;
  //              this.comboBox1.Width = this.tvTaskList.GetCellDisplayRectangle(this.tvTaskList.CurrentCell.ColumnIndex, this.tvTaskList.CurrentCell.RowIndex, true).Width;
  //              if (this.tvTaskList.CurrentCell.ColumnIndex == 4)
  //              {
  //                  if (tvTaskList.CurrentRow.Cells[2].Value.ToString() == "远红外")
  //                  {
  //                      IList<Info> infoList = new List<Info>();
  //                      Info info1 = new Info() { Id = "0", Name = "打开" };
  //                      Info info2 = new Info() { Id = "1", Name = "关闭" };
  //                      infoList.Add(info1);
  //                      infoList.Add(info2);
  //                      comboBox1.DataSource = infoList;
  //                      comboBox1.ValueMember = "Id";
  //                      comboBox1.DisplayMember = "Name";
  //                      this.comboBox1.Visible = true;
  //                  }
  //                  else if (tvTaskList.CurrentRow.Cells[2].Value.ToString() == "近红外")
  //                  {
  //                      IList<Info> infoList = new List<Info>();
  //                      Info info1 = new Info() { Id = "0", Name = "打开" };
  //                      Info info2 = new Info() { Id = "1", Name = "关闭" };
  //                      infoList.Add(info1);
  //                      infoList.Add(info2);
  //                      comboBox1.DataSource = infoList;
  //                      comboBox1.ValueMember = "Id";
  //                      comboBox1.DisplayMember = "Name";
  //                      this.comboBox1.Visible = true;
  //                  }
  //                  else if (tvTaskList.CurrentRow.Cells[2].Value.ToString() == "分叉")
  //                  {
  //                      IList<Info> infoList = new List<Info>();
  //                      Info info1 = new Info() { Id = "0", Name = "左分叉" };
  //                      Info info2 = new Info() { Id = "1", Name = "右分叉" };
  //                      Info info3 = new Info() { Id = "2", Name = "保持" };

  //                      infoList.Add(info1);
  //                      infoList.Add(info2);
  //                      infoList.Add(info3);
  //                      comboBox1.DataSource = infoList;
  //                      comboBox1.ValueMember = "Id";
  //                      comboBox1.DisplayMember = "Name";
  //                      this.comboBox1.Visible = true;
  //                  }
  //                  else if (tvTaskList.CurrentRow.Cells[2].Value.ToString() == "叉    臂")
  //                  {
  //                      IList<Info> infoList = new List<Info>();
  //                      Info info1 = new Info() { Id = "0", Name = "上升" };
  //                      Info info2 = new Info() { Id = "1", Name = "下降" };
  //                      infoList.Add(info1);
  //                      infoList.Add(info2);
  //                      comboBox1.DataSource = infoList;
  //                      comboBox1.ValueMember = "Id";
  //                      comboBox1.DisplayMember = "Name";
  //                      this.comboBox1.Visible = true;
  //                  }
  //              }
  //              else
  //              {
  //                  this.comboBox1.Visible = false;
  //                  this.comboBox1.Width = 0;
  //              }
  //          }
  //          catch (Exception ex)
  //          {
  ////                MessageBox.Show(ex.Message); 
  //          }
        }

        private void tvTaskList_Scroll(object sender, ScrollEventArgs e)
        {
            this.comboBox1.Visible = false;
            this.comboBox1.Width = 0;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Info i = comboBox1.SelectedItem as Info;
            tvTaskList.CurrentCell.Value = i.Name;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.comboBox1.Visible = false;
            this.comboBox1.Width = 0;
        }

        int countnum = 0, lijingread_now = 0, liuchengread_now = 0;
        int rout_sum = 0, parment_flag = 0, lujing_flag = 0, liucheng_flag = 0, parmentdown_flag = 0, lujingdown_flag = 0, liuchengdown_flag = 0;
        int liucheng_sum = 0;
        int lujing_rewnum = 0, liucheng_rewnum = 0;
        int[] lujing_rewindex = new int[500];
        int[] liucheng_rewindex = new int[500];
        int busy = 1,zhandianxinxi_sum=0,xuhaoxinxi_sum=0;
        private void serialPort1_DataReceived( object sender, SerialDataReceivedEventArgs e )
        {

            if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环
            try
            {
                busy = 1;
                Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                int n = serialPort1.BytesToRead;
                byte[] buf1 = new byte[n];
                received_count += n;
                serialPort1.Read(buf1, 0, n);
                buffer.AddRange(buf1);

                this.Invoke((EventHandler)( delegate
                {
                while (buffer.Count >= 8)
                    {                        
                        if (( buffer[0] == 0xAA ) && ( buffer[1] == 0xcc ))//PC下载系统参数到Flash成功
                        {

                            int len = buffer[2] << 8 | buffer[3];
                            if (buffer.Count < len)
                            {
                                break;
                            }
                            byte[] buf4 = new byte[len];
                            buffer.CopyTo(0, buf4, 0, len);
                            int sum = buf4[0] + buf4[1] + buf4[2] + buf4[3] + buf4[4] + buf4[5]
                             + buf4[6] + buf4[7];
                            if (( buf4[8] == Convert.ToByte(sum >> 8) ) && ( buf4[9] == Convert.ToByte(sum & 0xff) ))
                            {
                                int parment_number = buf4[4]<<8| buf4[5]&0xff;
                                int parment_num = buf4[6] << 8 | buf4[7] & 0xff;
                                int num = int.Parse(node_parment.Nodes[parment_number].Cells[4].Value.ToString());
                                if (parment_num == num)                           
                                {
                                    parmentdown_flag = parment_number;
                                    parment_flag = 1;
                                }
                                else
                                {
                                    parment_flag = 2;
                                }
                                buffer.RemoveRange(0, len);
                            }
                            else
                            {
                                parment_flag = 2;
                                buffer.RemoveRange(0, len);
                            }
                        }
                        else if (( buffer[0] == 0xAA ) && ( buffer[1] == 0xdd ))//上位机下载路径参数到Flash成功
                        {
                            int len = buffer[2] << 8 | buffer[3];
                            if (buffer.Count < len)
                            {
                                break;
                            }
                            byte[] buf4 = new byte[len];
                            buffer.CopyTo(0, buf4, 0, len);
                            int sum = buf4[0] + buf4[1] + buf4[2] + buf4[3] + buf4[4] + buf4[5]
                             + buf4[6] + buf4[7] + buf4[8] + buf4[9];
                            if (( buf4[10] == Convert.ToByte(sum >> 8) ) && ( buf4[11] == Convert.ToByte(sum & 0xff) ))
                            {
                                int rout_number = buf4[4] << 8 | buf4[5];
                                int zhandian_sum = buf4[6] << 8 | buf4[7];
                                int zhandian_number = buf4[8] << 8 | buf4[9];
                                if (zhandian_number == ( zhandian_sum - 1 ))
                                {
                                    lujingdown_flag = lujingdown_index;
                                    label8.Text = lujingdown_flag.ToString();
                                    label9.Text = lujingdown_index.ToString();
                                }
                                lujing_flag = 1;
                            }
                            else
                            {
                                lujing_flag = 2;
                            }
                            buffer.RemoveRange(0, len);
                        }                        
                        else if (( buffer[0] == 0xAA ) && ( buffer[1] == 0x55 ))//上位机下载流程参数到Flash成功
                        {
                            int len = buffer[2] << 8 | buffer[3];
                            if (buffer.Count < len)
                            {
                                break;
                            }
                            byte[] buf4 = new byte[len];
                            buffer.CopyTo(0, buf4, 0, len);
                            int sum = buf4[0] + buf4[1] + buf4[2] + buf4[3] + buf4[4] + buf4[5]
                             + buf4[6] + buf4[7] + buf4[8] + buf4[9];
                            if (( buf4[10] == Convert.ToByte(sum >> 8) ) && ( buf4[11] == Convert.ToByte(sum & 0xff) ))
                            {
                                int liucheng_number = buf4[4] << 8 | buf4[5];
                                int xuhao_sum = buf4[6] << 8 | buf4[7];
                                int xuhao_number = buf4[8] << 8 | buf4[9];
                                if (xuhao_number == ( xuhao_sum - 1 ))
                                {
                                    liuchengdown_flag = liuchengdown_index;
                                    label8.Text = liuchengdown_flag.ToString();
                                    label9.Text = liuchengdown_index.ToString();
                                }
                                liucheng_flag = 1;
                            }
                            else
                            {
                                liucheng_flag = 2;
                            }
                            buffer.RemoveRange(0, len);
                        }
                        else if (( buffer[0] == 0xAA ) && ( buffer[1] == 0xAA ))//读取Flash系统参数数据
                        {
                            int len = buffer[2] << 8 | buffer[3];
                            if (buffer.Count < len)
                            {
                                break;
                            }
                            byte[] buf = new byte[10];
                            buffer.CopyTo(0, buf, 0, 10);
                            int sum = buf[0] + buf[1] + buf[2] + buf[3] + buf[4] + buf[5] + buf[6] + buf[7];

                            if (( buf[8] == Convert.ToByte(sum >> 8) ) && ( buf[9] == Convert.ToByte(sum & 0xff) ))
                            {
                                int parment_number = buf[4] << 8 | buf[5];
                                int parment_num = buf[6] << 8 | buf[7];

                                if (node_parment.Nodes.Count > parment_number)
                                {
                                    //存在更新
                                    node_parment.Nodes[parment_number].Cells[3].Value = parment_num;
                                    if (node_parment.Nodes[parment_number].Cells[4].Value == null)
                                    {
                                        node_parment.Nodes[parment_number].Cells[4].Value = parment_num;
                                    }
                                    else
                                    {
                                        if (node_parment.Nodes[parment_number].Cells[3].Value.ToString() != node_parment.Nodes[parment_number].Cells[4].Value.ToString())
                                        {
                                            node_parment.Nodes[parment_number].DefaultCellStyle.BackColor = Color.Red;
                                            MessageBox.Show("参数"+ parment_number.ToString()+"下载失败！", "提示");
                                        }
                                        else
                                        {
                                            node_parment.Nodes[parment_number].DefaultCellStyle.BackColor = Color.LightGray;
                                        }
                                    }
                                }
                                else
                                {
                                    //不存在，添加
                                    node_parment_son = node_parment.Nodes.Add(null, parment_number.ToString(), "", parment_num.ToString(), parment_num.ToString(), "");
                                    node_parment_son.ImageIndex = 1;
                                    node_parment.Nodes[parment_number].Parent.Nodes[parment_number].ImageIndex = 1;
                                }
                                countnum = parment_number;
                                label3.Text = "系  统：" + countnum.ToString();
                                if (countnum == 100)
                                {
                                    zhandianxinxi_sum = int.Parse(node_parment.Nodes[96].Cells[3].Value.ToString());
                                    xuhaoxinxi_sum = int.Parse(node_parment.Nodes[97].Cells[3].Value.ToString());
                                    lujing_num = int.Parse(node_parment.Nodes[99].Cells[3].Value.ToString());
                                    liucheng_num = int.Parse(node_parment.Nodes[98].Cells[3].Value.ToString());
                                    label4.Text = "路径数：" + lujing_num.ToString();
                                    label5.Text = "流程数：" + liucheng_num.ToString();
                                }
                                readstate = 1;
                                buffer.RemoveRange(0, len);
                            }
                            else
                            {
                                readstate = 2;
                                continue;
                            }
                        }
                        else if (( buffer[0] == 0xAA ) && ( buffer[1] == 0xBB ))//读取Flash路径参数数据
                        {
                            int len = buffer[2] << 8 | buffer[3];
                            if (buffer.Count < len)
                            {
                                break;
                            }
                            rout_sum = buffer[6] << 8 | buffer[7];
                            if (rout_sum == 0)
                            {
                                buffer.RemoveRange(0, len);
                                if (node_lujingparment.Nodes.Count > lujing_num)
                                {
                                    for (int k = node_lujingparment.Nodes.Count - 1; k >= lujing_num; k--)
                                    {
                                        node_lujingparment.Nodes.RemoveAt(k);
                                    }

                                }
                                readstate = 1;
                            }
                            else
                            {
                                if (buffer.Count < len * rout_sum)
                                {
                                    break;
                                }
                                int lujing_number = buffer[4] << 8 | buffer[5];
                                if (node_lujingparment.Nodes.Count > lujing_rewnum)
                                {
                                    node_lujingparment.Nodes[lujing_rewnum].Cells[0].Value = "路径" + (lujing_number).ToString();
                                }
                                else
                                {
                                    node_lujing = node_lujingparment.Nodes.Add("路径" + (lujing_number).ToString(), "", "", "", "", "");
                                    node_lujing.ImageIndex = 0;
                                }
                                byte[] buf3 = new byte[len];
                                for (int i = 0; i < rout_sum; i++)
                                {
                                    int sum2 = 0;
                                    buffer.CopyTo(0, buf3, 0, len);
                                    for (int k = 0; k < (len-2); k++)
                                    {
                                        sum2 += buf3[k];
                                    }
                                    if (( buf3[len-2] == Convert.ToByte(sum2 >> 8) ) && ( buf3[len-1] == Convert.ToByte(sum2 & 0xff) ))
                                    {
                                        if (node_lujingparment.Nodes[lujing_rewnum].Nodes.Count > rout_sum)
                                        {
                                            for (int k = node_lujingparment.Nodes[lujing_rewnum].Nodes.Count - 1; k >= rout_sum; k--)
                                            {
                                                node_lujingparment.Nodes[lujing_rewnum].Nodes.RemoveAt(k);
                                            }

                                        }
                                        if (node_lujingparment.Nodes[lujing_rewnum].Nodes.Count > i)
                                        {
                                            for (int g = 0; g < zhandianxinxi_sum; g++)
                                            {
                                                node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Nodes[g].Cells[3].Value = (buf3[8 + 2 * g] << 8 | buf3[9 + 2 * g] & 0xff).ToString();
                                                node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Nodes[g].Parent.Nodes[g].ImageIndex = 1;
                                            }

                                            if (node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Cells[4].Value == null)
                                            {
                                                for (int k = 0; k < zhandianxinxi_sum; k++)
                                                {
                                                    node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Nodes[k].Cells[4].Value = (buf3[8 + 2 * k] << 8 | buf3[9 + 2 * k] & 0xff).ToString();
                                                }
                                            }
                                            else
                                            {
                                                for (int nn = 0; nn < zhandianxinxi_sum; nn++)
                                                {
                                                    if (node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Nodes[nn].Cells[3].Value.ToString() != node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Nodes[nn].Cells[4].Value.ToString())
                                                    {
                                                        node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Nodes[nn].DefaultCellStyle.BackColor = Color.Red;
                                                        MessageBox.Show("路径" + lujing_number.ToString() + "站点"+ (i+1).ToString()+"下载失败！", "提示");
                                                    }
                                                    else
                                                    {
                                                        node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Nodes[nn].DefaultCellStyle.BackColor = Color.LightGray;                         
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            node_lujing_son = node_lujingparment.Nodes[lujing_rewnum].Nodes.Add("站点" + ( i + 1 ).ToString(), "", "", "", "", "");
                                            node_lujing_son.ImageIndex = 0;
                                            for (int m = 0; m < zhandianxinxi_sum; m++)
                                            {
                                                node_lujing_son.Nodes.Add("", m+1, "", (buf3[2*m+8] << 8 | buf3[2*m+9] & 0xff).ToString(), (buf3[2*m+8] << 8 | buf3[2*m+9] & 0xff).ToString(), "");
                                            }
                                            if (node_lujingparment.Nodes[lujing_rewnum].Nodes.Count != 0)
                                            {
                                                if (node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Nodes.Count != 0)
                                                {
                                                    for (int m = 0; m < zhandianxinxi_sum; m++)
                                                    {
                                                        node_lujingparment.Nodes[lujing_rewnum].Nodes[i].Nodes[m].Parent.Nodes[m].ImageIndex = 1;
                                                    }
                                                }
                                            }
                                        }
                                        buffer.RemoveRange(0, len);
                                    }
                                    else
                                    {
                                        readstate = 2;
                                        continue;
                                    }
                                }
                                lijingread_now++;
                                lujing_rewindex[lujing_rewnum] = lujing_number;
                                lujing_rewnum++;                                                              
                                readstate = 1;
                            }
                        }
                        else if (( buffer[0] == 0xAA ) && ( buffer[1] == 0xEE ))//读取Flash流程参数数据
                        {
                            int len = buffer[2] << 8 | buffer[3];
                            if (buffer.Count < len)
                            {
                                break;
                            }
                            liucheng_sum = buffer[6] << 8 | buffer[7];
                            if (liucheng_sum == 0)
                            {
                                buffer.RemoveRange(0, len);
                                if (node_luchengparment.Nodes.Count > liucheng_num)
                                {
                                    for (int k = node_luchengparment.Nodes.Count - 1; k >= liucheng_num; k--)
                                    {
                                        node_luchengparment.Nodes.RemoveAt(k);
                                    }
                                }
                                readstate = 1;
                            }
                            else
                            {
                                if (buffer.Count < len * liucheng_sum)
                                {
                                    break;
                                }
                                int lucheng_number = buffer[4] << 8 | buffer[5];
                                if (node_luchengparment.Nodes.Count > liucheng_rewnum)
                                {
                                    node_luchengparment.Nodes[liucheng_rewnum].Cells[0].Value = "流程" + (lucheng_number).ToString();
                                }
                                else
                                {
                                    node_liucheng = node_luchengparment.Nodes.Add("流程" + (lucheng_number).ToString(), "", "", "", "", "");
                                    node_liucheng.ImageIndex = 0;
                                }
                                byte[] buf3 = new byte[len];
                                for (int i = 0; i < liucheng_sum; i++)
                                {
                                    int sum2 = 0;
                                    buffer.CopyTo(0, buf3, 0, len);
                                    for (int k = 0; k < (len - 2); k++)
                                    {
                                        sum2 += buf3[k];
                                    }
                                    if (( buf3[len - 2] == Convert.ToByte(sum2 >> 8) ) && ( buf3[len - 1] == Convert.ToByte(sum2 & 0xff) ))
                                    {
                                        if (node_luchengparment.Nodes[liucheng_rewnum].Nodes.Count > liucheng_sum)
                                        {
                                            for (int k = node_luchengparment.Nodes[liucheng_rewnum].Nodes.Count - 1; k >= liucheng_sum; k--)
                                            {
                                                node_luchengparment.Nodes[liucheng_rewnum].Nodes.RemoveAt(k);
                                            }
                                        }
                                        if (node_luchengparment.Nodes[liucheng_rewnum].Nodes.Count > i)
                                        {
                                            for (int g = 0; g < xuhaoxinxi_sum; g++)
                                            {
                                                node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Nodes[g].Cells[3].Value = (buf3[8 + 2 * g] << 8 | buf3[9 + 2 * g] & 0xff).ToString();
                                                node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Nodes[g].Parent.Nodes[g].ImageIndex = 1;
                                            }

                                            if (node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Cells[4].Value == null)
                                            {
                                                for (int k = 0; k < zhandianxinxi_sum; k++)
                                                {
                                                    node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Nodes[k].Cells[4].Value = (buf3[8 + 2 * k] << 8 | buf3[9 + 2 * k] & 0xff).ToString();
                                                }
                                            }
                                            else
                                            {
                                                for (int nn = 0; nn < xuhaoxinxi_sum; nn++)
                                                {
                                                    if (node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Nodes[nn].Cells[3].Value.ToString() != node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Nodes[nn].Cells[4].Value.ToString())
                                                    {
                                                        node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Nodes[nn].DefaultCellStyle.BackColor = Color.Red;
                                                        MessageBox.Show("流程" + lucheng_number.ToString() + "序号" + (i + 1).ToString() + "下载失败！", "提示");
                                                    }
                                                    else
                                                    {
                                                        node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Nodes[nn].DefaultCellStyle.BackColor = Color.LightGray;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            node_liucheng_son =  node_luchengparment.Nodes[liucheng_rewnum].Nodes.Add("序号" + ( i + 1 ).ToString(), "", "", "", "", "");
                                            node_liucheng_son.ImageIndex = 0;
                                            for (int m = 0; m < xuhaoxinxi_sum; m++)
                                            {
                                                node_liucheng_son.Nodes.Add("", m+1, "", (buf3[2 * m + 8] << 8 | buf3[2 * m + 9] & 0xff).ToString(), (buf3[2 * m + 8] << 8 | buf3[2 * m + 9] & 0xff).ToString(), "");
                                            }
                                            if (node_luchengparment.Nodes[liucheng_rewnum].Nodes.Count != 0)
                                            {
                                                if (node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Nodes.Count != 0)
                                                {
                                                    for (int m = 0; m < xuhaoxinxi_sum; m++)
                                                    {
                                                        node_luchengparment.Nodes[liucheng_rewnum].Nodes[i].Nodes[m].Parent.Nodes[m].ImageIndex = 1;
                                                    }
                                                }
                                            }
                                        }
                                        buffer.RemoveRange(0, len);
                                    }
                                    else
                                    {
                                        readstate = 2;
                                        continue;
                                    }
                                }
                                liuchengread_now++;
                                liucheng_rewindex[liucheng_rewnum] = lucheng_number;
                                liucheng_rewnum++;
                                readstate = 1;
                            }
                        }
                        else
                        {
                            readstate = 2;
                            buffer.Clear();
                            break;
                        }                  
                    }
                } ));
            }
            finally
            {     
                Listening = false;//我用完了，ui可以关闭串口了。
            }
            busy = 0;
        }

    }
}
