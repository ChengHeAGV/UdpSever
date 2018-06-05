using DispatchSystem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DispatchSystem.Class
{
    class ExTimeOut
    {
        protected int _timeout;
        protected bool connected;
        protected Exception exception;
        protected masterEntities db = new masterEntities();
        protected List<ModbusConfig> modbusConfig = new List<ModbusConfig>();
        public ExTimeOut(int timeout)
        {
            _timeout = timeout;
        }
        public List<ModbusConfig> Connect()
        {
            connected = false;
            exception = null;
            Thread thread = new Thread(new ThreadStart(BeginConnect));
            thread.IsBackground = true; // 作为后台线程处理
                                        // 不会占用机器太长的时间
            thread.Start();

            // 等待如下的时间
            thread.Join(_timeout);

            if (connected == true)
            {
                // 如果成功就返回TcpClient对象
                thread.Abort();
                return modbusConfig;
            }
            if (exception != null)
            {
                // 如果失败就抛出错误
                thread.Abort();
                return null;
            }
            else
            {
                // 同样地抛出错误
                thread.Abort();
                return null;
            }
        }

        protected void BeginConnect()
        {
            try
            {
               //var set = db as System.Data.Entity.DbSet<ModbusConfig>;
                modbusConfig = db.ModbusConfig.AsNoTracking().ToList();
                // 标记成功，返回调用者
                connected = true;
            }
            catch (Exception ex)
            {
                // 标记失败
                exception = ex;
            }
        }
    }
}
