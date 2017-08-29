using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenpaySdk
{

    /// <summary>
    /// 微信支付相关统一的API接口
    /// </summary>
    public partial class TenpayApi
    {

        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <returns></returns>
        public static Message QueryOrderPayState(TenpayConfig config, string out_trade_no)
        {
            var result = new Message();
            try
            {
                var requestParams = new Dictionary<string, string>();
                requestParams.Add("appid", config.appid);
                requestParams.Add("mch_id", config.mch_id);
                requestParams.Add("nonce_str", TenpayConfig.nonce_str());
                requestParams.Add("sign_type", "MD5");
                requestParams.Add("out_trade_no", out_trade_no);
                //创建签名
                string sign = TenpaySign.CreateSign(requestParams, config.key);
                requestParams.Add("sign", sign);

                //组装数据
                string RequestData = TenpayHelp.InstallXml(requestParams);

                //查询订单
                string sUrl = "https://api.mch.weixin.qq.com/pay/orderquery";
                string sResponeResult = TenpayHelp.HttpPost(sUrl, RequestData);//调用接口

                //解析数据
                var responeParameters = TenpayHelp.GetDictionaryFromCDATAXml(sResponeResult);
                result.returnData = responeParameters;//返回响应的参数
                if (responeParameters["return_code"] == "SUCCESS")
                {//验证签名
                    if (TenpaySign.CheckSign(responeParameters, config.key))
                    {
                        if (responeParameters["result_code"] == "SUCCESS")
                        {
                            if (responeParameters["trade_state"] == "SUCCESS")
                            {//订单支付成功
                                result.state = true;
                                result.error = responeParameters["trade_state_desc"];
                            }
                            else
                            {
                                result.error = responeParameters["trade_state_desc"];
                            }
                        }
                        else
                        {
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
                result.error = string.Format("出现异常:{0}",e.Message.ToString());
            }
            return result;
        }

        /// <summary>
        /// 查询退款订单状态
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="out_refund_no">商户退款订单号</param>
        /// <returns></returns>
        public static Message QueryOrderRefundState(TenpayConfig config, string out_trade_no = null, string out_refund_no = null)
        {
            var result = new Message();
            try
            {
                var requestParams = new Dictionary<string, string>();
                requestParams.Add("appid", config.appid);
                requestParams.Add("mch_id", config.mch_id);
                requestParams.Add("nonce_str", TenpayConfig.nonce_str());
                requestParams.Add("sign_type", "MD5");
                requestParams.Add("out_trade_no", out_trade_no);
                requestParams.Add("out_refund_no", out_refund_no);
                //创建签名
                string sign = TenpaySign.CreateSign(requestParams, config.key);
                requestParams.Add("sign", sign);

                //组装数据
                string RequestData = TenpayHelp.InstallXml(requestParams);

                //退款查询
                string sUrl = "https://api.mch.weixin.qq.com/pay/refundquery";
                string sResponeResult = TenpayHelp.HttpPost(sUrl, RequestData);//调用接口

                //解析数据
                var responeParameters = TenpayHelp.GetDictionaryFromCDATAXml(sResponeResult);
                result.returnData = responeParameters;//返回响应的参数
                if (responeParameters["return_code"] == "SUCCESS")
                {//验证签名
                    if (TenpaySign.CheckSign(responeParameters, config.key))
                    {
                        if (responeParameters["result_code"] == "SUCCESS")
                        {
                            if (responeParameters["refund_status_0"] == "SUCCESS")
                            {//退款成功
                                result.state = true;
                                result.error = "退款成功";
                            }
                            else
                            {
                                if (responeParameters["refund_status_0"] == "REFUNDCLOSE")
                                    result.error = "退款关闭";
                                if (responeParameters["refund_status_0"] == "PROCESSING")
                                    result.error = "退款处理中";
                                if (responeParameters["refund_status_0"] == "CHANGE")
                                    result.error = "退款异常";
                            }
                        }
                        else
                        {
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


        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="out_trade_no">订单号</param>
        /// <param name="out_refund_no">退款订单号</param>
        /// <param name="total_fee">支付金额</param>
        /// <param name="refund_fee">退款金额</param>
        /// <param name="refund_desc">退款原因</param>
        /// <returns></returns>
        public static Message ApplyRefund(TenpayConfig config, string out_trade_no, string out_refund_no,int total_fee,int refund_fee,string refund_desc)
        {
            var result = new Message();
            try
            {
                var requestParams = new Dictionary<string, string>();
                requestParams.Add("appid", config.appid);
                requestParams.Add("mch_id", config.mch_id);
                requestParams.Add("nonce_str", TenpayConfig.nonce_str());
                requestParams.Add("sign_type", "MD5");
                requestParams.Add("out_trade_no", out_trade_no);
                requestParams.Add("out_trade_no", out_refund_no);
                requestParams.Add("total_fee", total_fee.ToString());
                requestParams.Add("refund_fee", refund_fee.ToString());
                if (!string.IsNullOrEmpty(refund_desc))
                    requestParams.Add("refund_desc", refund_desc);
                //创建签名
                string sign = TenpaySign.CreateSign(requestParams, config.key);
                requestParams.Add("sign", sign);

                //组装数据
                string RequestData = TenpayHelp.InstallXml(requestParams);

                //申请退款
                string sUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";
                //申请退款需要证书
                string sResponeResult = TenpayHelp.HttpPost(sUrl, RequestData,true, config);//调用接口

                //解析数据
                var responeParameters = TenpayHelp.GetDictionaryFromCDATAXml(sResponeResult);
                result.returnData = responeParameters;//返回响应的参数
                if (responeParameters["return_code"] == "SUCCESS")
                {//验证签名
                    if (TenpaySign.CheckSign(responeParameters, config.key))
                    {
                        if (responeParameters["result_code"] == "SUCCESS")
                        {//退款申请成功
                            result.state = true;
                            result.error = "退款申请成功";
                        }
                        else
                        {
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
                throw;
            }
            return result;
        }

    }
}
