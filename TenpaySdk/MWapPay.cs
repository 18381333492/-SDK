using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenpaySdk
{
    
    /// <summary>
    /// 微信支付H5模式(WapPay支付)
    /// </summary>
    public partial class TenpayMode
    {

        /// <summary>
        ///  微信H5支付
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="body">订单描述</param>
        /// <param name="sOrderNo">订单号</param>
        /// <param name="total_fee">订单金额(单位分)</param>
        /// <param name="notify_url">异步通知地址</param>
        /// <param name="return_url">同步通知地址</param>
        /// <param name="sClientRealIp">客户端真实IP</param>
        /// <param name="time_expire">订单失效时间</param>
        /// <returns></returns>
        public static TenpayMessage WapPay(TenpayConfig config, string body, string sOrderNo, int total_fee, string notify_url, string return_url, string sClientRealIp, string time_expire)
        {
            var result = UniteOrderByWapPay(config, body, sOrderNo, total_fee.ToString(), notify_url, sClientRealIp, time_expire);
            if (result.state)
            {
                if (!string.IsNullOrEmpty(return_url))
                {
                    return_url = TenpayHelp.UrlEncode(return_url);
                    result.data = string.Format("{0}&redirect_url={1}", result.data, return_url);
                }
            }
            return result;
        }

        /// <summary>
        /// WapPay支付统一下单接口
        /// </summary>
        /// <param name="config">公共参数</param>
        /// <param name="body">描述</param>
        /// <param name="out_trade_no">商户系统内部的订单编号</param>
        /// <param name="total_fee">订单总金额，单位为分</param>
        /// <param name="notify_url">接收微信支付异步通知回调地址,不能携带参数</param>
        /// <param name="spbill_create_ip">APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP.</param>
        /// <param name="time_expire">订单失效时间(格式为yyyyMMddHHmmss)</param>
        /// <returns></returns>
        public static TenpayMessage UniteOrderByWapPay(TenpayConfig config, string body, string out_trade_no, string total_fee,string notify_url, string spbill_create_ip, string time_expire)
        {
            var result = new TenpayMessage();
            try
            {
                var requestParams = new Dictionary<string, string>();
                requestParams.Add("appid", config.appid);
                requestParams.Add("mch_id", config.mch_id);
                requestParams.Add("nonce_str", TenpayConfig.nonce_str());
                requestParams.Add("body", body);
                requestParams.Add("out_trade_no", out_trade_no);
                requestParams.Add("total_fee", total_fee);
                requestParams.Add("spbill_create_ip", spbill_create_ip);
                requestParams.Add("notify_url", notify_url);
                requestParams.Add("sign_type", "MD5");
                if (string.IsNullOrEmpty(time_expire))//默认48小时
                    time_expire = DateTime.Now.AddHours(48).ToString("yyyyMMddHHmmss");
                requestParams.Add("time_expire", time_expire);
                requestParams.Add("trade_type", PayMode.MWEB.ToString());//H5支付 
                                                                         //创建签名
                string sign = TenpaySign.CreateSign(requestParams, config.key);
                requestParams.Add("sign", sign);

                //组装数据
                string RequestData = TenpayHelp.InstallXml(requestParams);

                //请求统一下单支付API
                string sUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                string sResponeResult = TenpayHelp.HttpPost(sUrl, RequestData);//调用接口

                //解析数据
                var responeParameters = TenpayHelp.GetDictionaryFromCDATAXml(sResponeResult);
                result.returnData = responeParameters;
                if (responeParameters["return_code"] == "SUCCESS")
                {//验证签名
                    if (TenpaySign.CheckSign(responeParameters, config.key))
                    {
                        if (responeParameters["result_code"] == "SUCCESS")
                        {//统一下单成功
                            result.state = true;
                            result.data = responeParameters["mweb_url"];
                        }
                        else
                        {//统一下单失败
                            result.error = responeParameters["err_code_des"];//错误信息描述
                        }
                    }
                    else
                    {
                        result.error = string.Format("签名验证失败,验证key={0}", config.key);
                    }
                }
                else
                {
                    result.error = responeParameters["return_msg"];
                }
            }
            catch (Exception e)
            {
                result.error = string.Format("出现异常:{0}", e.Message.ToString());
            }
            return result;
        }
    }
}
