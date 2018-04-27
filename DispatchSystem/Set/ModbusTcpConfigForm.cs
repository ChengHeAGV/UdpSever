using DispatchSystem.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem.Set
{
    public partial class ModbusTcpConfigForm : Form
    {
        public ModbusTcpConfigForm()
        {
            InitializeComponent();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            //判断ip
            if (IP_YN(textBoxIp.Text) == false)
            {
                MessageBox.Show("您输入的IP号为空或格式不正确，请改正后重试！");
            }
            else
            //判断端口
            if (PORT_YN(textBoxPort.Text) == false)
            {
                MessageBox.Show("您输入的端口号为空或格式不正确，请改正后重试！");
            }
            else
            //判断周期
            if (Circle_YN(textBoxCircle.Text) == false)
            {
                MessageBox.Show("您输入的周期为空或格式不正确，请改正后重试！");
            }
            else
            {
                //存储到数据库
                DatabaseEntities db = new DatabaseEntities();
                //1.找到对象
                var config = db.ModbusConfig.FirstOrDefault(m => m.key == "IP");
                //2.更新对象
                if (config!=null)
                {
                    config.value = textBoxIp.Text;
                }
                else
                {
                    config = new ModbusConfig
                    {
                        key = "ip",
                        value = "textBoxIp.Text",
                        des = "IP地址"
                    };
                    db.ModbusConfig.Add(config);
                }
                //3.保存修改
                db.SaveChanges();
                //var config = new ModbusConfig
                //{
                //    key = "bomo",
                //    value = 21,
                //    des = "male"
                //};
                //db.DbusSever.Add(user);
                //db.SaveChanges();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// ip的有效性
        /// </summary>
        /// <param name="ip">IP</param>
        /// <returns></returns>
        public bool IP_YN(String ip)
        {
            bool YN = false;
            if (System.Text.RegularExpressions.Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {

                string[] ips = ip.Split('.');
                if (ips.Length == 4)
                    if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256
                          && System.Int32.Parse(ips[2]) < 256 && System.Int32.Parse(ips[3]) < 256)
                    {
                        YN = true;
                    }
            }

            return YN;
        }
        /// <summary>
        /// port有效性
        /// </summary>
        /// <param name="str">端口</param>
        /// <returns></returns>
        public bool PORT_YN(String str)
        {
            bool YN = true;
            if (str.Length == 0)
            {
                YN = false;
            }
            else//没有非数字字符
            {
                for (int i = 0; i < str.Length; i++)
                    if (!char.IsNumber(str, i))
                        YN = false;
            }
            if (YN)//介于0-65535之间
            {
                if ((int.Parse(str) > 0) && (int.Parse(str) < 65535))
                {
                    YN = true;
                }
            }
            return YN;
        }

        /// <summary>
        /// 周期有效性检查
        /// </summary>
        /// <param name="circle">周期值</param>
        /// <returns></returns>
        public bool Circle_YN(string circle)
        {
            if (circle.Length == 0)
                return false;
            circle = circle.Trim();
            try
            {
                var i = int.Parse(circle);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
