using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlipaySdk
{

    /// <summary>
    /// 支付宝支付模式
    /// </summary>
    public class AlipayMode
    {

        /// <summary>
        /// 手机模式支付
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="out_trade_no"></param>
        /// <param name="subject"></param>
        /// <param name="total_amount"></param>
        /// <param name="notify_url"></param>
        /// <param name="return_url"></param>
        /// <param name="quit_url"></param>
        /// <param name="timeout_express">该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天
        /// （1c-当天的情况下，无论交易何时创建，都在0点关闭）。 该参数数值不接受小数点， 如 1.5h，可转换为 90m。</param>
        /// <returns></returns>
        public static AlipayMessage WapPay(AlipayConfig config, string out_trade_no, string subject, string total_amount, string notify_url, string return_url, 
                                                                                                      string quit_url = null, string timeout_express = null)
        {
            var result = new AlipayMessage();
            try
            {
                IAopClient client = AlipayApi.GetIAopClient(config);
                AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
                AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
                model.Subject = subject;//订单描述
                model.OutTradeNo = out_trade_no;
                model.TotalAmount = total_amount;//订单支付金额
                model.ProductCode = "QUICK_WAP_WAY";
                if (!string.IsNullOrEmpty(timeout_express))
                    model.TimeoutExpress = timeout_express;
                if (!string.IsNullOrEmpty(quit_url))
                    model.QuitUrl = quit_url;
                request.SetBizModel(model);
                request.SetNotifyUrl(notify_url);//设置异步跳转地址
                request.SetReturnUrl(return_url);//设置同步跳转地址
                AlipayTradeWapPayResponse response = client.pageExecute(request);
                if (!response.IsError)
                {
                    result.state = true;
                    result.data = response.Body;
                }
                else
                {
                    //响应错误
                    result.error = string.Format("[{0}]{1}", response.Code, response.SubMsg);
                }
            }
            catch (Exception e)
            {
                result.error = string.Format("手机网站支付出现异常:{0}", e.Message);
            }
            return result;
        }
    }
}
