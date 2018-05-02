using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DispatchSystem.Class
{
    class ExThread
    {
        public AutoResetEvent exitEvent;
        private Thread thread;
        //定义一个委托
        public delegate void delegateFun();

        public ExThread(delegateFun fun)
        {
            exitEvent = new AutoResetEvent(false);
            thread = new Thread(new ThreadStart(fun));
        }
        public void Start()
        {
            thread.Start();
        }
        public void Stop()
        {
            exitEvent.Set();
            thread.Join();
        }
    }
}

///////////////
//函数体使用方法
//ThreadClass udpThread = new ThreadClass(UdpFunc);
//udpThread.Run();
//udpThread.Stop();
//private void Process()
//{
//    while (true)
//    {
//        Console.WriteLine("do some thing");
//        if (exitEvent.WaitOne(100))//延时100ms
//        {
//            break;
//        }
//    }
//}