using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenpaySdk
{

    /// <summary>
    /// 微信支付的模式
    /// </summary>
    public partial class TenpayMode
    {

        /// <summary>
        /// 微信支付的模式
        /// </summary>
        public enum PayMode
        {
            MWEB=1,//H5支付
            JSAPI=2,//微信公众号支付
            NATIVE=3//原生支付(扫码支付)
        }

        /************************* 调用微信统一下单API所必需的参数******************************************/
        //公众账号ID      appid                  微信分配的公众账号ID（企业号corpid即为此appId）
        //商户号          mch_id                 微信支付分配的商户号
        //随机字符串      nonce_str              随机字符串，不长于32位。推荐随机数生成算法
        //签名            sign                   签名，详见签名生成算法
        //商品描述        body                   商品或支付单简要描述
        //商户订单号      out_trade_no           商户系统内部的订单号,32个字符内、可包含字母, 其他说明见商户订单号
        //总金额          total_fee              订单总金额，单位为分，详见支付金额
        //终端IP          spbill_create_ip       APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。
        //通知地址        notify_url             接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数。
        //交易类型        trade_type             取值如下：JSAPI，NATIVE，APP，详细说明见参数规定
        //商品ID          product_id             trade_type=NATIVE，此参数必传。此id为二维码中包含的商户定义的商品id或者订单号，商户自行定义。
        /************************* 调用微信统一下单API所必需的参数******************************************/

        /// <summary>
        /// 统一下单支付接口
        /// </summary>
        /// <param name="config">公共参数</param>
        /// <param name="payMode">支付模式</param>
        /// <param name="body">描述</param>
        /// <param name="out_trade_no">商户系统内部的订单编号</param>
        /// <param name="total_fee">订单总金额，单位为分</param>
        /// <param name="notify_url">接收微信支付异步通知回调地址,不能携带参数</param>
        /// <param name="return_url">微信支付同步通知回调地址,不能携带参数</param>
        /// <param name="spbill_create_ip">APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP.</param>
        /// <param name="openid">trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识</param>
        /// <returns></returns>
        public static Message UniteOrder(TenpayConfig config,
                                          PayMode payMode,
                                          string body,
                                          string out_trade_no, 
                                          string total_fee,
                                          string notify_url,
                                          string return_url,
                                          string spbill_create_ip,
                                          string openid)
        {
            /************************* 调用微信统一支付API所必需的参数******************************************/
            //公众账号ID      appid                  微信分配的公众账号ID（企业号corpid即为此appId）
            //商户号          mch_id                 微信支付分配的商户号
            //随机字符串      nonce_str              随机字符串，不长于32位。推荐随机数生成算法
            //签名            sign                   签名，详见签名生成算法
            //商品描述        body                   商品或支付单简要描述
            //商户订单号      out_trade_no           商户系统内部的订单号,32个字符内、可包含字母, 其他说明见商户订单号
            //总金额          total_fee              订单总金额，单位为分，详见支付金额
            //终端IP          spbill_create_ip       APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。
            //通知地址        notify_url             接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数。
            //交易类型        trade_type             取值如下：JSAPI，NATIVE，APP，详细说明见参数规定
            //商品ID          product_id             trade_type=NATIVE，此参数必传。此id为二维码中包含的商户定义的商品id或者订单号，商户自行定义。
            /************************* 调用微信统一支付API所必需的参数******************************************/
            var requestParams = new Dictionary<string, string>();
            requestParams.Add("appid", config.appid);
            requestParams.Add("mch_id", config.mch_id);
            requestParams.Add("nonce_str", TenpayConfig.nonce_str());
            requestParams.Add("body", body);
            requestParams.Add("out_trade_no", out_trade_no);
            requestParams.Add("total_fee", total_fee);
            requestParams.Add("spbill_create_ip", spbill_create_ip);
            requestParams.Add("notify_url", notify_url);
            requestParams.Add("time_expire", DateTime.Now.AddDays(2).ToString("yyyyMMddHHmmss"));//订单的失效时间默认2天
            switch (payMode)
            {
                case PayMode.MWEB://H5支付
                    requestParams.Add("trade_type", PayMode.MWEB.ToString());
                    break;
                case PayMode.JSAPI://H5支付
                    requestParams.Add("trade_type", PayMode.JSAPI.ToString());
                    break;
                case PayMode.NATIVE://H5支付
                    requestParams.Add("trade_type", PayMode.NATIVE.ToString());
                    break;
            }





            requestParams.Add("trade_type", "MWEB");//新h5模式
            requestParams.Add("time_expire", DateTime.Now.AddDays(2).ToString("yyyyMMddHHmmss"));//订单的失效时间默认2天
            //创建签名
            string sign = TenpaySign.CreateSign(requestParams, config.key);
            requestParams.Add("sign", sign);

            string RequestData = TenpayHelp.InstallXml(requestParams);

            //请求统一下单支付API
            string sUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string sResult = TenpayHelp.HttpPost(sUrl, RequestData);//调用接口

            var Parameters = TenpayHelp.GetDictionaryFromCDATAXml(sResult);

            Message Msg = new Message();
            if (TenpaySign.CheckSign(Parameters, config.key))
            {//验证签名
                if (Parameters["return_code"] == "SUCCESS")
                {
                    if (Parameters["result_code"] == "SUCCESS")
                    {//统一下单成功
                        Msg.state = true;
                        Msg.data = Parameters["mweb_url"];
                    }
                    else
                    {//统一下单失败
                        Msg.error = Parameters["err_code_des"];//错误信息描述
                    }
                }
                else
                {
                    Msg.error = Parameters["return_msg"];
                }
            }
            else
            {
                Msg.error = sResult;
            }

            return Msg;
        }

    }
}
