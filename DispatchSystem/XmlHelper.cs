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
        //文件路径-Debug
        public static string FilePath_Debug = DirectoryPath + "\\Debug.xml";
        //文件路径-Dbus
        public static string FilePath_Dbus = DirectoryPath + "\\Dbus.xml";

        public static List<Debug> DebugList = new List<Debug>();

        //SAVE -> XML 更新xml
        public static void UpdataDebug()
        {
            SaveXml(FilePath_Debug, DebugList);
        }
        public static void UpdataDbus()
        {
            //  SaveXml(FilePath_Dbus, dbus);
        }

        /// <summary>
        /// 初始化xml文件
        /// </summary>
        /// <param name="Reset">复位xml参数到默认状态</param>
        public static void InitDebug(bool Reset = false)
        {
            //创建文件夹
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            //创建文件
            if (!File.Exists(FilePath_Debug) || Reset)
            {
                Debug debug = new Debug();
                debug.Name = "心跳帧";
                debug.Value = true;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "操作帧";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "响应帧";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "实时帧";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "读单个寄存器";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "写单个寄存器";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "读多个寄存器";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "写多个寄存器";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "无效帧";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "收到数据";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "单包数据有效帧数量";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "发送数据";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "错误";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "系统消息";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "服务器";
                debug.Value = false;
                DebugList.Add(debug);

                debug = new Debug();
                debug.Name = "debug";
                debug.Value = false;
                DebugList.Add(debug);
                SaveXml(FilePath_Debug, DebugList);
            }
            else
            {
                //加载数据
                object result = LoadXml(FilePath_Debug, typeof(List<Debug>));
                DebugList = result as List<Debug>;
            }
        }

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
        
        [Serializable]
        public class Debug
        {
            [XmlElement("名称")]
            public string Name
            {
                get;
                set;
            }
            [XmlElement("数值")]
            public bool Value
            {
                get;
                set;
            }
        }
    }
}