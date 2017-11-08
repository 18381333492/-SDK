using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceLogs
{
    /// <summary>
    /// 轻量型的文本日志记录
    /// 适合接口和一些小的程序
    /// </summary>
    public class LoggerManager
    {
        private static LoggerManager instance;

        private static Logger logger;

        public static LoggerManager Instance
        {
            get
            {
                if (instance == null)
                    instance =new LoggerManager();
                return instance;
            }
        }

        /// <summary>
        /// 获取logger
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="cache">是否缓存</param>
        /// <returns></returns>
        public ILogger GetLogger()
        {
            if (logger == null)
                logger = new Logger();
            return logger;
        }
    }
}
