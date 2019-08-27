using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model
{
    /// <summary>
    /// 1688授权信息,使用前需向1688申请
    /// 测试数据:
    ///private string _appkey = "334362";
    ///private string _appsecret = "w5v9Xu2sK2y5";
    ///private string _tokenkey = "aa3cbd3b-de0c-4779-8596-6724864d7a13";
    /// </summary>
    public class AliSecret
    {
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string TokenKey { get; set; }
        /// <summary>
        /// 1688接口地址,现为
        /// "https://gw.open.1688.com/openapi/param2/1/"
        /// </summary>
        private string _apiBaseUrl = "https://gw.open.1688.com/openapi/";
        private string _apiParmUrl = "param2/1/";
        public string ApiBaseUrl { get { return _apiBaseUrl; } }
        public string ApiParmUrl { get { return _apiParmUrl; } }

        public string ApiUrl {
            get { return _apiBaseUrl + _apiParmUrl; }
        }

    }
}
