using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem
{
    class MyConsole
    {
        public class ConsoleClass
        {
            public string Msg;
            public Color color = Color.Black;
            public int Size = 14;
        }

        public static List<ConsoleClass> ConsoleList = new List<ConsoleClass>();
        public static void Add(string msg, int size = 14)
        {
            ConsoleClass dd = new ConsoleClass();
            dd.Msg = msg;
            dd.Size = size;
            ConsoleList.Add(dd);
        }
        public static void Add(string msg, Color color, int size = 14)
        {
            ConsoleClass dd = new ConsoleClass();
            dd.Msg = msg;
            dd.color = color;
            dd.Size = size;
            ConsoleList.Add(dd);
        }
    }
}
