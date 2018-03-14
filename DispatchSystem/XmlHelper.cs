using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DispatchSystem
{

    //调用方法

    //初始化
    //XmlHelper.Init();

    //读取参数
    //object result = xml.SerializeXml.LoadXml("C:\\x.xml", typeof(List<xml.Root>));
    //List<xml.Root> dd = result as List<xml.Root>;

    //保存参数
    //Updata();



    /// <summary>  
    /// <remarks>Xml序列化与反序列化</remarks>  
    /// <creator>zhangdapeng</creator>  
    /// </summary>  
    public class XmlHelper
    {

        //文件夹连接
        public static string DirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "Config";
        //文件路径
        public static string FilePath = DirectoryPath + "\\config.xml";

        public static Root Config;//系统参数

        // OBJECT -> XML  
        public static void SaveXml(string filePath, object obj)
        {
            SaveXml(filePath, obj, obj.GetType());
        }
        public static void SaveXml(string filePath, object obj, System.Type type)
        {
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

        //SAVE -> XML 更新xml
        public static void Updata()
        {
            SaveXml(FilePath, Config);
        }


        /// <summary>
        /// 初始化xml文件
        /// </summary>
        /// <param name="Reset">复位xml参数到默认状态</param>
        public static void Init(bool Reset = false)
        {
            //创建文件夹
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            //创建文件
            if (!File.Exists(FilePath) || Reset)
            {
                //序列化
                Root config = new Root();

                config.dbus = new Dbus();
                config.debug = new Debug();


                #region Dbus参数设置
                config.dbus.Port = 18666;
                config.dbus.DeviceNum = 20;
                #endregion

                #region Debug 参数配置

                ////心跳帧
                //config.debug.HeartFrame = true;
                ////操作帧
                //config.debug.OperationFrame = true;
                ////响应帧
                //config.debug.ResponseFrame = true;
                ////实时帧
                //config.debug.RealTimeFrame = true;

                ////读单个寄存器
                //config.debug.ReadRegister = true;
                ////写单个寄存器
                //config.debug.WriteRegister = true;
                ////读多个寄存器
                //config.debug.ReadMuliteRegister = true;
                ////写多个寄存器
                //config.debug.WriteMuliteRegister = true;

                ////无效帧
                //config.debug.ValidFrames = true;
                ////收到数据
                //config.debug.ReciveData = true;
                ////单包数据有效帧数量
                //config.debug.VaildFramesNum = true;
                ////发送数据
                //config.debug.SendData = true;
                ////错误
                //config.debug.Error = true;
                ////系统消息
                //config.debug.SystemMsg = true;
                ////服务器
                //config.debug.Sever = true;
                ////debug
                //config.debug.debug = true;
                #endregion

                ////<Root> cityList = new List<Root>();
                //cityList.Add(config);
                SaveXml(FilePath, config);

            }

            //加载数据
            object result = LoadXml(FilePath, typeof(Root));
            Config = result as Root;

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
}