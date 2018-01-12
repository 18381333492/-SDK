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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using TenpaySdk;

namespace Web.Controllers
{

    public class DefaultController : Controller
    {
        //支付宝同样的私匙会产生不同的公钥
        private string merchant_private_key = "MIIEowIBAAKCAQEAyar5QMTDtAarKX9tcx0IluaSWezSc7GHqvcV8Yv+sEpFZCQZUbHsN7ssJXJXUFhsgCqvPTLDlmO0ns4PdNFSwMT2Q1JAT1dXSXEVvH/OorRA73L2oFEsrTKACN1xvrlsnF4OmH/z2/zCLjyQpAuvxXrfF6tU9uJyDB9iCCOuFJBgmD+N1ALosto6aALRKlMmF3ucL/XUVPjQY9bxqcDzGGkZnZxUeHLLF+9yP6JG814w+moy2PbSMvo+Vm4adDVi+aLVjbD3HOPhblxC4NXjc9MWxCThsCmRUDmkSVTcJNR8Q+DefR7604fzq8IoXLniDm0oO0Yi3qQnUd20PuMK8QIDAQABAoIBAHfYnoXqKS98Yw2nR8EIOQmMft7oCW1tzGVCr4y7mKDlknVfqphNN0creaHLYK5Dzj8gnsGswGVIXZeed7sBhr8+jecWI1fDXQEtLjC2d3Nj0c87L+u4Mee/wi0ChM1GXpBSqTPhnmdWv4NAxOhodY3TZm8nh7esfQBNSjHyGkrnMA3h9PtTEJd92KifPUYuvzHD2KOqzhHJboZ8Ih/lRh5uWCEp7366Mc3q2RafpLVxRjchPfGeU7PPidviFrnhN0BjaAsawPzPCpfn04zvS96dNbuJKLOZ0sMOHhxNLphWsk68nVUNytbmS8lpr2cKMPdg/QoiwGvhrNNOyxFTxAECgYEA6JTNlJBqfGmQkMtto09VJOhhO0r67nMd+fKdxBP+F5WVzNqoTBycH0HoIz6XYIsEubyAVMRNqZvQd9uvl7m3pgnGtEIj5TKB9WkpZtMe/k87APCeq+rZLjvt1bax5/Y9BQZwB3O0x2jAtVHZc2sO44ybQnKNAzTGvBvSbiQ8dEECgYEA3flSmfq/A4qTJ5gMSi60zFpn6v2UfDkv/UGwfOAWH+RrNKDq6wg2PVsRrkpOsS9oSkyiVtkWa4XVx9m34zo4APvOKoUyqfI3petTDtGV1zBhBvkOl6bCVAVxHUg6VN4wObaq86UR8FdfsauikTHxFscPgP+4mcr+DJyymDUEKrECgYEAqVgfT7rPLgMXFbZo/+21iwgAM9HmX1RGUUWMBcagzb9GsT/MJo72RfQQ+AiM4+iU6kAMGKxN997RrVOxyIGa7DRWD83QoQNjiLKnSI0UFgrOZWLNxVNcCsPr6h3573FlAJGtZF+lE0R8fAk6kUU0NA6exYTuk5UL1s9TKosL0YECgYB52cvGSydgQknViln0vv7wzxAMp3dDWgFF/TFs23ZJu5I+KbfLnY5oz/08t/3KtkOBxd+33SO5kpZwRsvzKJplr9TU8pmFQTnbEvtdPyAKKLyan02rYhd7GCGn+WZMAExo4iWl6g+W59/YIGf1XH0EC/Iu1jH3+r7LHZnMhA3tgQKBgAENUaxBNuWdPY/LMac1M6kEkxx377Vi41EBZOHpNHxiusN7q5CeW6/EvxodwMvBJaJXfSNSnIBth44PH06SmP6AGSp4cong7xpR3v3BGf3MI06Y4oNcgXv1gXg2nSgMH2y1g8PkE9lhVrdwKNGBusMV0yaDQCXfvv0Zaa3d1MWu";
        //qxnw@100bm.cn
        private string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAl4cdAY6ll1ZJyGLNUr/Yh+W2lZnPCzsGYzx9Qavg6PTrFK8BSZB6Pm9wNrhQcgMhOaQTqV8xMOPmtJMEEdcy3zKul7eVfJRCuQBwQ8i+DP2IMxa05+UjvBSyP5DMip5B/HMpqxl7Xs7cbqzH9G4/lrWo6nHvWhy7aO7ICu/+oxPPFadUj4vI0ZqA39falmlBFNk7lQDWXPxTt/gHkyPsL9zQkaYTMd6hPR4zsFJLqvm+Zl21fXWn0qyeOdlwRGBud8bdfif1jCOzoDlyZvWHw2eYzEgV99udBq6o7Iv+Z1f2upXYitLqv5ISOuicu9te/a7ePZOkLIbD7YCaSmQbBQIDAQAB";
        //
        private string publick_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkLMmX0RIi0jOwYR15VQ7NOjNQPtCt/NQRnhSCGtRMN7bMpWacLuU8IaqXJQRWrJZSybTKshvmEMPKlhcUvFAhk5YPOt5BJA8yaWHg33Dn4hVd74ds1A3ItHW87vRpt8VYj+myC1FLQJb7SxsKpNNcPR2idM94dTQ4f1P1ji5VT/cMsa3q5/aR+g6NHTG9BStyKFfrCL9XqXdfiZrR1kyTpGv7ws2xMKgb3XJHvA0HeqgniSgo8UAMP/2wc10HmQXBH/RWSTu30qIA0AQJFsOaSheHHFvJsCKMclY8D4hdK2ssiZ2c2UMvkL2sRWl4COTIztK+RcZu4/GdA0K+Sq7swIDAQAB";
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public void AliPayWapPay()
        {
            AlipayConfig config = new AlipayConfig();
            config.alipay_public_key = publick_key;
            config.merchant_private_key = merchant_private_key;
            config.app_id = "2016082201785041";
            var res = AlipayMode.WapPay(config, DateTime.Now.ToString("yyyyMMddHHmmssfff"), "测试订单", "0.01", null, null);
        }


        public void Trace()
        {
            //var logger=LoggerManager.Instance.GetLogger("Web");
            //try
            //{
            //    int p = 0;
            //    int s = 6 / p;
            //}
            //catch (Exception e)
            //{
            //    logger.Info(e.Message);
            //    logger.Fatal(e);
            //}
        }

        public void refund() { }



        public ActionResult Test()
        {
            //允许跨域
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return Content("success");
        }


        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPhoneCode(string sPhone)
        {
            var code = new Random().Next(10000, 99999);
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            Session[sPhone] = code;
            return Json(new { success=true,data= code,info="获取成功" });
        }


        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="sPhone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Login(string sPhone,string code)
        {
            var mm = Session[sPhone];
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            if (mm==null|| mm.ToString()!= code)
            {
                return Json(new { success = false, data = string.Empty, info ="验证码错误" });
            }
            else
            {
                return Json(new { success = true, data=string.Empty, info = "登录成功" });
            }

        }

        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetHomeInfo()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            JObject job = new JObject();
            job.Add(new JProperty("totalBalance", 15000.00));
            job.Add(new JProperty("cashBalance", 5800.00));
            job.Add(new JProperty("rebateBalance", 15000.00));
            job.Add(new JProperty("certification", true));
            return Json(new { success = true, data = job.ToString(), info = "获取数据成功" });
        }


        /// <summary>
        /// 获取消费记录
        /// </summary>
        /// <returns></returns>
        public ActionResult GetComsureListData(int page=1,int rows=6)
        {
            int[] s = new int[] { rows, rows, rows-2 };
            rows = s[page - 1];
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            List<object> list = new List<object>();
            for(var i=1;i<= rows; i++)
            {
                list.Add(new { money =-(new Random().Next(100,500)), address = "小草湖加油站", date = DateTime.Now.AddHours(new Random().Next(1,50)).ToString("yyyy-MM-dd HH:mm:ss") });
            }
            return Json(new { total = rows*3-2, list = list });
        }

        /// <summary>
        /// log4net日志插件测试
        /// </summary>
        public void Logs()
        {
            
        }


        /// <summary>
        /// 分类视图
        /// </summary>
        /// <returns></returns>
        public ActionResult type()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult car()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult person()
        {
            return View();
        }


        public void async()
        {

        }

        public void success()
        {

        }

        /// <summary>
        /// 支取请求
        /// </summary>
        public void PayRequest()
        {
            string version = "1.0";
            string merchantaccount = "HAO001";
            string orderno = "CC" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int amount = 100;
            string currency = "CNY";
            string membername = "a123456";
            string memberip = "127.0.0.1";
            string bankcode = "CN_ICBC";
            string ordertime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string serverreturnurl = "http://jialimall.vicp.io/Default/async";
            string memberreturnurl = "http://jialimall.vicp.io/Default/success";
            string key = "u&e2@UQsF&5Tfg";

            string signStr = string.Format(@"version={0}&merchantaccount={1}&orderno={2}&amount={3}&currency={4}&membername={5}&memberip={6}&bankcode={7}&ordertime={8}&serverreturnurl={9}&memberreturnurl={10}&key={11}"
                                             , version, merchantaccount, orderno, amount, currency
                                             , membername, memberip, bankcode, ordertime, serverreturnurl
                                             , memberreturnurl, key);


            string sign = SHA1(signStr);//签名
            //参数
            //dynamic obj= new System.Dynamic.ExpandoObject();
            //obj.version = version;
            //obj.merchantaccount = merchantaccount;
            //obj.orderno = orderno;
            //obj.amount = amount;
            //obj.currency = currency;
            //obj.membername = membername;
            //obj.memberip = memberip;
            //obj.bankcode = bankcode;
            //obj.ordertime = ordertime;
            //obj.serverreturnurl = serverreturnurl;
            //obj.memberreturnurl = memberreturnurl;
            //obj.digest = sign;
            //string parStr = JsonConvert.SerializeObject(obj);
            string parStr = string.Format(@"version={0}&merchantaccount={1}&orderno={2}&amount={3}&currency={4}&membername={5}&memberip={6}&bankcode={7}&ordertime={8}&serverreturnurl={9}&memberreturnurl={10}&digest={11}"
                                            , version, merchantaccount, orderno, amount, currency
                                            , membername, memberip, bankcode, ordertime, serverreturnurl
                                            , memberreturnurl, sign);
            string url ="https://www.grabbuy988.com/ddt.jsp";
            string result=TenpayHelp.HttpPost(url, parStr);
            Response.Write(result);
        }

        public  string SHA1(string input)
        {
            SHA1 shaHash = System.Security.Cryptography.SHA1.Create();
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().ToUpper();
        }


        public void httpTest()
        {
            var res=TenpayHelp.HttpPost("http://localhost:39251/User/TimeOut", string.Empty);
        }

    }
}