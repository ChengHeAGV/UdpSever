using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DispatchSystem
{
    /// <summary>  
    /// <remarks>Xml序列化与反序列化</remarks>  
    /// <creator>zhangdapeng</creator>  
    /// </summary>  
    public class XmlHelper
    {
        // OBJECT -> XML  
        public static void SaveXml(string filePath, object obj) { SaveXml(filePath, obj, obj.GetType()); }
        public static void SaveXml(string filePath, object obj, System.Type type)
        {
            //XmlSerializer ser = new XmlSerializer(type);
            //var c = File.Create(filePath);
            //ser.Serialize(c, obj);
            //c.Close();
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);
                xs.Serialize(writer, obj);
                writer.Close();
            }
        }
        // XML -> OBJECT  
        public static object LoadXml(string filePath, System.Type type)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);
                object obj = xs.Deserialize(reader);
                reader.Close();
                return obj;
            }
        }

        /// <summary>
        /// 初始化xml文件
        /// </summary>
        /// <param name="Reset">复位xml参数到默认状态</param>
        public static void Init(bool Reset = false)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Config";
            string logPath = filePath + "\\config.xml";
            //创建文件夹
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            //创建文件
            if (!Directory.Exists(logPath) || Reset)
            {
                //序列化
                Root root = new Root();
                root.dbus = new Dbus();
                root.debug = new Debug();

                #region Debug 参数配置
                root.debug.HeartFrame = true;
                root.debug.SendData = false;
                #endregion


                //Dbus 参数配置
                root.dbus.DeviceNum = 11;
                root.dbus.Port = 30;

                List<Root> cityList = new List<Root>();
                cityList.Add(root);

                SaveXml(logPath, cityList);
            }
        }
    }

    [Serializable]
    [XmlRoot("root")]
    public class Root
    {
        [XmlElement("Debug")]
        public Debug debug
        {
            get;
            set;
        }

        [XmlElement("Dbus")]
        public Dbus dbus
        {
            get;
            set;
        }
    }
    public class Dbus
    {
        [XmlElement("端口")]
        public int Port
        {
            get;
            set;
        }

        [XmlElement("设备数量")]
        public int DeviceNum
        {
            get;
            set;
        }
    }
    public class Debug
    {
        [XmlElement("心跳帧")]
        public bool HeartFrame
        {
            get;
            set;
        }

        [XmlElement("操作帧")]
        public bool OperationFrame
        {
            get;
            set;
        }

        [XmlElement("响应帧")]
        public bool ResponseFrame
        {
            get;
            set;
        }

        [XmlElement("实时帧")]
        public bool RealTimeFrame
        {
            get;
            set;
        }

        [XmlElement("读单个寄存器")]
        public bool ReadRegister
        {
            get;
            set;
        }

        [XmlElement("写单个寄存器")]
        public bool WriteRegister
        {
            get;
            set;
        }

        [XmlElement("读多个寄存器")]
        public bool ReadMuliteRegister
        {
            get;
            set;
        }

        [XmlElement("写多个寄存器")]
        public bool WriteMuliteRegister
        {
            get;
            set;
        }

        [XmlElement("无效帧")]
        public bool ValidFrames
        {
            get;
            set;
        }

        [XmlElement("收到数据")]
        public bool ReciveData
        {
            get;
            set;
        }

        [XmlElement("单包数据有效帧数量")]
        public bool VaildFramesNum
        {
            get;
            set;
        }

        [XmlElement("发送数据")]
        public bool SendData
        {
            get;
            set;
        }

        [XmlElement("错误")]
        public bool Error
        {
            get;
            set;
        }

        [XmlElement("系统消息")]
        public bool SystemMsg
        {
            get;
            set;
        }

        [XmlElement("服务器")]
        public bool Sever
        {
            get;
            set;
        }

        [XmlElement("debug")]
        public bool debug
        {
            get;
            set;
        }
    }
}
