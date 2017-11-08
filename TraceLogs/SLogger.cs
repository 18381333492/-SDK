using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceLogs
{
    public class SLogger:ILogger
    {

        private string m_fileName;

        private string basePath;

        public string dir;

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="dir"></param>
        public SLogger(string dir)
        {
            this.dir = dir;
            string root = ConfigurationManager.AppSettings["logger"];
            if (string.IsNullOrEmpty(root)) root = "logs";
            this.basePath = string.Format("{0}{1}\\{2}", AppDomain.CurrentDomain.BaseDirectory, root, dir);
        }

        public void Info(object msg)
        {
            string message = Format(msg, LoggerLevel.Info.ToString());
            File.AppendAllText(m_fileName, message);
        }

        public void Warn(object msg)
        {
            string message = Format(msg, LoggerLevel.Warn.ToString());
            File.AppendAllText(m_fileName, message);
        }

        public void Error(object msg)
        {
            string message = Format(msg, LoggerLevel.Error.ToString());
            File.AppendAllText(m_fileName, message);
        }

        public void Fatal(object msg)
        {
            string message = Format(msg, LoggerLevel.Fatal.ToString());
            File.AppendAllText(m_fileName, message);
        }

        /// <summary>
        /// Format
        /// </summary>
        private string Format(object obj, string category)
        {
            string sPath = this.basePath + "\\" + category + "\\";
            if (!Directory.Exists(sPath))
                Directory.CreateDirectory(sPath);
            this.m_fileName = sPath + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            //需要写入的日志
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("[{0}] ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.AppendFormat("[{0}] ", dir);
            if (obj is Exception)
            {
                var ex = (Exception)obj;
                builder.Append(ex.Message + "\r\n");
                builder.Append("   错误源:" + ex.Source + "\r\n");
                builder.Append("   异常函数:" + ex.TargetSite + "\r\n");
                builder.Append("   异常类型:" + ex.GetType() + "\r\n");
                builder.Append(ex.StackTrace + "\r\n");
            }
            else
            {
                builder.Append(obj.ToString() + "\r\n");
            }
            return builder.ToString();
        }
    }
}
