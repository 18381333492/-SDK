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
    /// 支付宝支付相关统一的API接口
    /// </summary>
    public class AlipayApi
    {

        /// <summary>
        /// 获取AOP请求
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <returns></returns>
        public static IAopClient GetIAopClient(AlipayConfig config)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do",
                               config.app_id,//支付宝分配给开发者的应用ID
                               config.merchant_private_key,
                               "json", //仅支持JSON
                               "1.0",  //调用的接口版本，固定为：1.0
                               "RSA2", //商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
                               config.alipay_public_key,
                              "utf-8",//编码
                               false);

            return client;
        }

        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="out_trade_no">商户订单号</param>
        public static AlipayMessage QueryOrderPayState(AlipayConfig config, string out_trade_no)
        {

            var result = new AlipayMessage();
            try
            {
                IAopClient client = GetIAopClient(config);
                AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
                AlipayTradeQueryModel model = new AlipayTradeQueryModel();
                model.OutTradeNo = out_trade_no;
                request.SetBizModel(model);
                AlipayTradeQueryResponse response = client.Execute(request);
                //获取响应返回的数据
                result.returnData = response.Body;
                if (!response.IsError)
                {
                    if (response.TradeStatus == "TRADE_SUCCESS")
                    {
                        result.state = true;
                        result.error = "交易支付成功";
                    }
                    else
                    {
                        if (response.TradeStatus == "WAIT_BUYER_PAY")
                            result.error = "交易创建，等待买家付款";
                        if (response.TradeStatus == "TRADE_CLOSED")
                            result.error = "未付款交易超时关闭，或支付完成后全额退款";
                        if (response.TradeStatus == "TRADE_FINISHED")
                            result.error = "交易结束，不可退款";
                    }
                }
                else
                {//错误信息
                    result.error =string.Format("[{0}]{1}", response.Code, response.SubMsg) ;
                }
            }
            catch (Exception e)
            {
                result.error = string.Format("查询订单支付状态出现异常:{0}", e.Message);
            }
            return result;
        }


        /// <summary>
        /// 查询订单退款状态
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="out_request_no">退款订单号</param>
        /// <returns></returns>
        public static AlipayMessage QueryOrderRefundState(AlipayConfig config, string out_trade_no,string out_request_no)
        {
            var result = new AlipayMessage();
            try
            {
                IAopClient client = GetIAopClient(config);
                AlipayTradeFastpayRefundQueryRequest request = new AlipayTradeFastpayRefundQueryRequest();
                AlipayTradeFastpayRefundQueryModel model = new AlipayTradeFastpayRefundQueryModel();
                model.OutTradeNo = out_trade_no;
                model.OutRequestNo = out_request_no;
                request.SetBizModel(model);
                AlipayTradeFastpayRefundQueryResponse response = client.Execute(request);
                //获取响应返回的数据
                result.returnData = response.Body;
                if (!response.IsError)
                {//响应正确
                    result.state = true;
                }
                else
                {
                    //错误信息
                    result.error = string.Format("[{0}]{1}", response.Code, response.SubMsg);
                }
            }
            catch (Exception e)
            {
                result.error = string.Format("查询订单退款状态出现异常:{0}", e.Message);
            }
            return result;
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="out_request_no">退款订单号</param>
        /// <param name="refund_amount">退款</param>
        /// <param name="refund_reason"></param>
        /// <returns></returns>
        public static AlipayMessage Refund(AlipayConfig config,string out_trade_no, string out_request_no,string refund_amount,string refund_reason=null)
        {
            var result = new AlipayMessage();
            try
            {
                IAopClient client = GetIAopClient(config);
                AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
                AlipayTradeRefundModel model = new AlipayTradeRefundModel();
                model.OutTradeNo = out_trade_no;
                model.OutRequestNo = out_request_no;
                model.RefundAmount = refund_amount;
                if (!string.IsNullOrEmpty(refund_reason))
                    model.RefundReason = refund_reason;
                request.SetBizModel(model);
                AlipayTradeRefundResponse response = client.Execute(request);
                if (!response.IsError)
                {
                    if (response.FundChange == "Y")
                    {
                        result.state = true;
                        result.error = "退款成功";
                    }
                    else
                    {
                        result.error = "订单已退款";
                    }
                }
                else
                {//响应错误
                    result.error = string.Format("[{0}]{1}", response.Code, response.SubMsg);
                }
            }
            catch (Exception e)
            {
                result.error = string.Format("退款出现异常:{0}", e.Message);
            }
            return result;
        }
    }
}
