using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlipaySdk;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Repository;

namespace Web.Controllers
{

    public class DefaultController : Controller
    {
        private string merchant_private_key = "MIIEowIBAAKCAQEAyar5QMTDtAarKX9tcx0IluaSWezSc7GHqvcV8Yv+sEpFZCQZUbHsN7ssJXJXUFhsgCqvPTLDlmO0ns4PdNFSwMT2Q1JAT1dXSXEVvH/OorRA73L2oFEsrTKACN1xvrlsnF4OmH/z2/zCLjyQpAuvxXrfF6tU9uJyDB9iCCOuFJBgmD+N1ALosto6aALRKlMmF3ucL/XUVPjQY9bxqcDzGGkZnZxUeHLLF+9yP6JG814w+moy2PbSMvo+Vm4adDVi+aLVjbD3HOPhblxC4NXjc9MWxCThsCmRUDmkSVTcJNR8Q+DefR7604fzq8IoXLniDm0oO0Yi3qQnUd20PuMK8QIDAQABAoIBAHfYnoXqKS98Yw2nR8EIOQmMft7oCW1tzGVCr4y7mKDlknVfqphNN0creaHLYK5Dzj8gnsGswGVIXZeed7sBhr8+jecWI1fDXQEtLjC2d3Nj0c87L+u4Mee/wi0ChM1GXpBSqTPhnmdWv4NAxOhodY3TZm8nh7esfQBNSjHyGkrnMA3h9PtTEJd92KifPUYuvzHD2KOqzhHJboZ8Ih/lRh5uWCEp7366Mc3q2RafpLVxRjchPfGeU7PPidviFrnhN0BjaAsawPzPCpfn04zvS96dNbuJKLOZ0sMOHhxNLphWsk68nVUNytbmS8lpr2cKMPdg/QoiwGvhrNNOyxFTxAECgYEA6JTNlJBqfGmQkMtto09VJOhhO0r67nMd+fKdxBP+F5WVzNqoTBycH0HoIz6XYIsEubyAVMRNqZvQd9uvl7m3pgnGtEIj5TKB9WkpZtMe/k87APCeq+rZLjvt1bax5/Y9BQZwB3O0x2jAtVHZc2sO44ybQnKNAzTGvBvSbiQ8dEECgYEA3flSmfq/A4qTJ5gMSi60zFpn6v2UfDkv/UGwfOAWH+RrNKDq6wg2PVsRrkpOsS9oSkyiVtkWa4XVx9m34zo4APvOKoUyqfI3petTDtGV1zBhBvkOl6bCVAVxHUg6VN4wObaq86UR8FdfsauikTHxFscPgP+4mcr+DJyymDUEKrECgYEAqVgfT7rPLgMXFbZo/+21iwgAM9HmX1RGUUWMBcagzb9GsT/MJo72RfQQ+AiM4+iU6kAMGKxN997RrVOxyIGa7DRWD83QoQNjiLKnSI0UFgrOZWLNxVNcCsPr6h3573FlAJGtZF+lE0R8fAk6kUU0NA6exYTuk5UL1s9TKosL0YECgYB52cvGSydgQknViln0vv7wzxAMp3dDWgFF/TFs23ZJu5I+KbfLnY5oz/08t/3KtkOBxd+33SO5kpZwRsvzKJplr9TU8pmFQTnbEvtdPyAKKLyan02rYhd7GCGn+WZMAExo4iWl6g+W59/YIGf1XH0EC/Iu1jH3+r7LHZnMhA3tgQKBgAENUaxBNuWdPY/LMac1M6kEkxx377Vi41EBZOHpNHxiusN7q5CeW6/EvxodwMvBJaJXfSNSnIBth44PH06SmP6AGSp4cong7xpR3v3BGf3MI06Y4oNcgXv1gXg2nSgMH2y1g8PkE9lhVrdwKNGBusMV0yaDQCXfvv0Zaa3d1MWu";
        private string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAl4cdAY6ll1ZJyGLNUr/Yh+W2lZnPCzsGYzx9Qavg6PTrFK8BSZB6Pm9wNrhQcgMhOaQTqV8xMOPmtJMEEdcy3zKul7eVfJRCuQBwQ8i+DP2IMxa05+UjvBSyP5DMip5B/HMpqxl7Xs7cbqzH9G4/lrWo6nHvWhy7aO7ICu/+oxPPFadUj4vI0ZqA39falmlBFNk7lQDWXPxTt/gHkyPsL9zQkaYTMd6hPR4zsFJLqvm+Zl21fXWn0qyeOdlwRGBud8bdfif1jCOzoDlyZvWHw2eYzEgV99udBq6o7Iv+Z1f2upXYitLqv5ISOuicu9te/a7ePZOkLIbD7YCaSmQbBQIDAQAB";
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public void AliPayWapPay()
        {
            //AlipayConfig config = new AlipayConfig();
            //config.alipay_public_key = alipay_public_key;
            //config.merchant_private_key = merchant_private_key;
            //config.app_id = "2017081108144704";
            //var res=AlipayMode.WapPay(config, DateTime.Now.ToString("yyyyMMddHHmmssfff"), "测试订单", "0.01", null, null);
        }


        public ActionResult Test()
        {
            //允许跨域
            Response.AppendHeader("Access-Control-Allow-Origin", "http://localhost:13165");
            return Content("success");
        }

        /// <summary>
        /// log4net日志插件测试
        /// </summary>
        public void Logs()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "log4net.config";
            LogsHelper.init(path);
            var logger = LogsHelper.Instance.GetLogger("InfoAppender");
            logger.Info("fsdfsfdfs");
        }
    }


    public class LogsHelper
    {

        private static ILoggerRepository storedPath= null;
        private static IAppender[] appenders = null;


        private RollingFileAppender Appender = null;//当前日志的Appender
        private ILog logger=null;

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
        public static void init(string sPath=null)
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
                Appender= appenders.FirstOrDefault(m => m.Name == name) as RollingFileAppender
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
                this.Appender.File = string.Format("{0}{1}\\{2}\\{3}\\", AppDomain.CurrentDomain.BaseDirectory, "Logs", name,TypeName);
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