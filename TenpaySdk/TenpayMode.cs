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
    }
}
