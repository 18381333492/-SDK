using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Configuration;

namespace TraceLogs
{
    public class LogerTraceListener : TraceListener
    {
        /// <summary>
        /// FileName
        /// </summary>
        private string m_fileName;

        /// <summary>
        /// 日志文件根目录
        /// </summary>
        private string basePath;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogerTraceListener()
        {
            string root = ConfigurationManager.AppSettings["logger"];
            if (string.IsNullOrEmpty(root)) root = "logs";
            this.basePath = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, root);
        }

        /// <summary>
        /// 抽象函数必须继承
        /// </summary>
        public override void Write(string message)
        {
           
        }

        /// <summary>
        /// 抽象函数必须继承
        /// </summary>
        public override void WriteLine(string message)
        {

        }

        /// <summary>
        /// WriteLine
        /// </summary>
        public override void WriteLine(object obj, string category)
        {
            string message = Format(obj, category);
            File.AppendAllText(m_fileName, message);
        }

        /// <summary>
        /// Format
        /// </summary>
        private string Format(object obj, string category)
        {
            string sPath= this.basePath + "\\" + category+"\\";
            if (!Directory.Exists(sPath))
                Directory.CreateDirectory(sPath);
            this.m_fileName = sPath + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            //需要写入的日志
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("[{0}] ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.AppendFormat("[{0}] ", category);
            if (obj is Exception)
            {
                var ex = (Exception)obj;
                builder.Append(ex.Message + "\r\n");
                builder.Append("   错误源:"+ex.Source + "\r\n");
                builder.Append("   异常函数:"+ex.TargetSite + "\r\n");
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
