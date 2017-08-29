using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenpaySdk
{
    /// <summary>
    /// 微信公众号支付(JSSDK支付)
    /// </summary>
    public partial class TenpayMode
    {

        /// <summary>
        /// 微信公众号支付
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="body">订单描述</param>
        /// <param name="sOrderNo">订单编号</param>
        /// <param name="total_fee">订单金额(单位分)</param>
        /// <param name="notify_url">异步通知地址</param>
        /// <param name="spbill_create_ip">网页支付提交用户端ip</param>
        /// <param name="openid">微信用户在商户对应appid下的唯一标识</param>
        /// <param name="time_expire">订单失效时间</param>
        /// <returns></returns>
        public static Message JsApiPay(TenpayConfig config, string body, string sOrderNo, int total_fee, string notify_url,  string spbill_create_ip,string openid, string time_expire)
        {
            var result = UniteOrderByJsApi(config, body, sOrderNo, total_fee.ToString(), notify_url, spbill_create_ip, openid, time_expire);
            return result;
        }

        /// <summary>
        ///  获取微信公众号支付的相关参数
        /// </summary>
        /// <param name="config"></param>
        /// <param name="prepay_id"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPayParams(TenpayConfig config,string prepay_id)
        {
            var param = new Dictionary<string, string>();
            param.Add("appId", config.appid);
            param.Add("timeStamp", TenpayConfig.time_stamp());
            param.Add("nonceStr", TenpayConfig.nonce_str());
            param.Add("package", string.Format("prepay_id={0}", prepay_id));
            param.Add("signType", "MD5");
            //创建签名
            string paySign = TenpaySign.CreateSign(param, config.key);
            param.Add("paySign", paySign);
            return param;
        }

        /// <summary>
        /// JSSDK支付统一下单接口
        /// </summary>
        /// <param name="config">公共参数</param>
        /// <param name="body">描述</param>
        /// <param name="out_trade_no">商户系统内部的订单编号</param>
        /// <param name="total_fee">订单总金额，单位为分</param>
        /// <param name="notify_url">接收微信支付异步通知回调地址,不能携带参数</param>
        /// <param name="spbill_create_ip">APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP.</param>
        /// <param name="time_expire">订单失效时间(格式为yyyyMMddHHmmss)</param>
        /// <returns></returns>
        public static Message UniteOrderByJsApi(TenpayConfig config, string body, string out_trade_no, string total_fee,
                                                                    string notify_url, string spbill_create_ip,string openid, string time_expire)
        {
            var result = new Message();
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
                requestParams.Add("openid", openid);
                requestParams.Add("notify_url", notify_url);
                requestParams.Add("sign_type", "MD5");
                if (string.IsNullOrEmpty(time_expire))//默认48小时
                    time_expire = DateTime.Now.AddHours(48).ToString("yyyyMMddHHmmss");
                requestParams.Add("time_expire", time_expire);
                requestParams.Add("trade_type", PayMode.JSAPI.ToString());//JSAPI支付 
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
                            result.data = responeParameters["prepay_id"];//获取预支付id
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
