using DispatchSystem.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace DispatchSystem.Set
{
    public partial class ModbusTcpConfigForm : Form
    {
        masterEntities db = new masterEntities();
        List<ModbusConfig> config = new List<ModbusConfig>();
        public ModbusTcpConfigForm()
        {
            InitializeComponent();
        }

        private void ModbusTcpConfigForm_Load(object sender, EventArgs e)
        {
            //1.加载所有记录,此处AsNoTracking是为了更新
            config = db.ModbusConfig.AsNoTracking().ToList();
            
            //2.如果找到记录则显示到界面
            if (config != null)
            {
                foreach (var item in config)
                {
                    switch (item.key)
                    {
                        case "ip":
                            textBoxIp.Text = item.value;
                            break;
                        case "port":
                            textBoxPort.Text = item.value;
                            break;
                        case "circle":
                            textBoxCircle.Text = item.value;
                            break;
                        default:
                            break;
                    }
                }
            }
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
                update("ip", textBoxIp.Text, "IP地址");
                update("port", textBoxPort.Text, "端口号");
                update("circle", textBoxCircle.Text, "查询周期");
                this.DialogResult = DialogResult.OK;
            }

            //重启modbustcp
            User.DataSync.SyncState.ModbusTcp = false;
        }

        private void update(string key, string value, string des = "")
        {
            var temp = config.FirstOrDefault(m => m.key == key);
            if (temp != null)
            {
                des = des == "" ? temp.des : des;
                ModbusConfig u = new ModbusConfig() { Id = temp.Id, value = value, key = temp.key, des = temp.des };
                db.Entry<ModbusConfig>(u).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                temp = new ModbusConfig
                {
                    key = key,
                    value = value,
                    des = des
                };
                db.ModbusConfig.Add(temp);
                db.SaveChanges();
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
