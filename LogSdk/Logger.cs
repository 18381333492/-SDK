using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSdk
{
    public class Logger:ILogger
    {

        public string dir;
        /// <summary>
        /// 初始化构造函数移除默认的监听
        /// </summary>
        /// <param name="dir"></param>
        public Logger(string dir)
        {
            this.dir = dir;
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new LogerTraceListener(dir));
        }

        public void Info(object msg)
        {
            Trace.WriteLine(msg, LoggerLevel.Info.ToString());
        }

        public void Warn(object msg)
        {
            Trace.WriteLine(msg, LoggerLevel.Warn.ToString());
        }

        public void Error(object msg)
        {
            Trace.WriteLine(msg, LoggerLevel.Error.ToString());
        }

        public void Fatal(object msg)
        {
            Trace.WriteLine(msg, LoggerLevel.Fatal.ToString());
        }
    }
}
