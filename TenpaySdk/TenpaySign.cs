using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenpaySdk
{
    // <summary>
    /// 微信支付签名类
    /// </summary>
    public class TenpaySign
    {
        /*******************微信支付的请求和接收数据均需要校验签名，详细方法请参考安全规范-签名算法********************/
        /*    
        签名算法
        签名生成的通用步骤如下：
         第一步，设所有发送或者接收到的数据为集合M，将集合M内非空参数值的参数按照参数名ASCII码从小到大排序（字典序），
         使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串stringA。
         特别注意以下重要规则：
         ◆ 参数名ASCII码从小到大排序（字典序）；
         ◆ 如果参数的值为空不参与签名；
         ◆ 参数名区分大小写；
         ◆ 验证调用返回或微信主动通知签名时，传送的sign参数不参与签名，将生成的签名与该sign值作校验。
         ◆ 微信接口可能增加字段，验证签名时必须支持增加的扩展字段
         第二步，在stringA最后拼接上key得到stringSignTemp字符串，并对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为大写，得到sign值signValue。
         key设置路径：微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
         */


        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static string CreateSign(Dictionary<string, string> Parameters, string ApiKey)
        {
            StringBuilder Sign = new StringBuilder();
            var Keys = new ArrayList(Parameters.Keys);
            Keys.Sort();//字典排序
            foreach (string key in Keys)
            {
                if (!string.IsNullOrEmpty(Parameters[key]))
                {//拼接成键值对字符串
                    Sign.Append(key + "=" + Parameters[key] + "&");
                }
            }
            Sign.Append("key=" + ApiKey);
            string sign = TenpayHelp.MD5(Sign.ToString()).ToUpper();
            return sign;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="Parameters">字典集合参数</param>
        /// <returns></returns>
        public static bool CheckSign(Dictionary<string, string> Parameters, string ApiKey)
        {
            bool bResult = false;
            if (Parameters.ContainsKey("sign"))
            {
                string old_sign = Parameters["sign"];
                Parameters.Remove("sign");
                string sign = CreateSign(Parameters, ApiKey);
                if (old_sign == sign)//验证成功
                    bResult = true;
            }
            return bResult;
        }
    }
}
