using log4net;
using log4net.Appender;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web
{
    public class LogsHelper
    {

        private static ILoggerRepository storedPath = null;
        private static IAppender[] appenders = null;


        private RollingFileAppender Appender = null;//当前日志的Appender
        private ILog logger = null;

        private static LogsHelper instance = null;

        public static LogsHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new LogsHelper();
                return instance;
            }
        }

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        public static void init(string sPath = null)
        {
            if (string.IsNullOrEmpty(sPath))
                log4net.Config.XmlConfigurator.Configure();
            else
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(sPath));
            storedPath = LogManager.GetRepository();
            appenders = storedPath.GetAppenders();
        }


        /// <summary>
        /// 获取日志logger
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Logger GetLogger(string name)
        {
            return new Logger()
            {
                name = name,
                logger = LogManager.GetLogger(name),
                Appender = appenders.FirstOrDefault(m => m.Name == name) as RollingFileAppender
            };
        }
    }

    /// <summary>
    /// 日志记录类
    /// </summary>
    public class Logger
    {

        public string name
        {
            get;
            set;
        }

        public RollingFileAppender Appender
        {
            get;
            set;
        }

        public ILog logger
        {
            get;
            set;
        }

        /// <summary>
        /// 动态修改日志输出路径
        /// </summary>
        private void UpdateFolder(string TypeName)
        {
            if (this.Appender != null)
            {//修改目录
                this.Appender.File = string.Format("{0}{1}\\{2}\\{3}\\", AppDomain.CurrentDomain.BaseDirectory, "Logs", name, TypeName);
                this.Appender.ActivateOptions();
            }
        }

        /// <summary>
        /// 记录Info日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Info(string message, Exception ex = null)
        {
            UpdateFolder("Info");
            if (ex == null)
            {
                logger.Info(message);
            }
            else
            {
                logger.Info(message, ex);
            }
        }

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Fatal(string message, Exception ex = null)
        {
            UpdateFolder("Fatal");
            if (ex == null)
            {
                logger.Fatal(message);
            }
            else
            {
                logger.Fatal(message, ex);
            }
        }

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Error(string message, Exception ex = null)
        {
            UpdateFolder("Error");
            if (ex == null)
            {
                logger.Error(message);
            }
            else
            {
                logger.Error(message, ex);
            }
        }
    }
}