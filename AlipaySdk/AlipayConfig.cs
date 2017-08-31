using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlipaySdk
{
    public class AlipayConfig
    {

        /// <summary>
        /// 支付宝分配给开发者的应用ID
        /// </summary>
        public string app_id
        {
            get;
            set;
        }

        /// <summary>
        /// 商户私钥
        /// </summary>
        public string merchant_private_key
        {
            get;
            set;
        }

        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string alipay_public_key
        {
            get;
            set;
        }
    }

    public class AlipayMessage
    {
        public bool state = false;
        public string data;
        public string returnData;
        public string error;
    }
}
