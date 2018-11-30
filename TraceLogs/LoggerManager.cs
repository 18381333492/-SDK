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
        //缓存
        private static List<SLogger> CacheList = new List<SLogger>();

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
        /// 获取slogger
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="cache">是否缓存</param>
        /// <returns></returns>
        public ILogger GetSLogger(string dir)
        {
            return Get(dir);
        }


        /// <summary>
        /// 获取slogger
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private SLogger Get(string dir)
        {
            if (CacheList.Any(m => m.dir == dir))
            {//存在直接返回
                return CacheList.FirstOrDefault(m => m.dir == dir);
            }
            else
            {//不存在
                SLogger logger = new SLogger(dir);
                CacheList.Add(logger);//添加到缓存
                return logger;
            }
        }


    }
}
