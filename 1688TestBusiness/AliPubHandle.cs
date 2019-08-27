using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enterprise3.WebApi.Client.Enums;
using Enterprise3.WebApi.Client.Models;
using Enterprise3.WebApi.Client;
using Ali1688Business.Model;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Configuration;

namespace Ali1688Business
{
    /// <summary>
    /// 定义了与1688接口的公共操作，如加密，获取webapiclient等
    /// </summary>
    public abstract class AliPubHandle
    {
        private bool _UseWebApiClient;
        public AliPubHandle()
        {
            try
            {
                Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");
                if (config.AppSettings.Settings.AllKeys.Count(delegate (string item) { return item == "1688useWebApiClient"; }) == 0)
                {
                    _UseWebApiClient = true;
                }
                else
                {
                    string tmp = config.AppSettings.Settings["1688useWebApiClient"].Value;
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        _UseWebApiClient = true;
                    }
                    else
                    {
                        _UseWebApiClient = tmp == "1";
                    }
                }
            }
            catch
            {
                _UseWebApiClient = false;
            }
        }
        /// <summary>
        /// api参数集合
        /// </summary>
        protected Dictionary<string, string> UrlParams { get; set; }
        /// <summary>
        /// 授权信息
        /// </summary>
        public AliSecret Secret { get; set; }
        /// <summary>
        /// 方法名eg:cn.alibaba.open/caigou.api.account.listSubAcccounts
        /// </summary>
        public string FunName { get; set; }
        /// <summary>
        /// 服务名
        /// </summary>
        public string UserSrvType { get; set; }
        /// <summary>
        /// 业务数据json内容
        /// </summary>
        public string BusData { get;set;}
        /// <summary>
        /// 解析业务数据，获取api参数集合、授权信息、方法名
        /// </summary>
        /// <param name="userServer"></param>
        /// <param name="bussdata"></param>
        public virtual void analyseBussdata(string userServer, string bussdata) { }
        /// <summary>
        /// 调用1688接口方法
        /// </summary>
        /// <param name="secret">授权信息</param>
        /// <param name="funName">方法名eg:cn.alibaba.open/caigou.api.account.listSubAcccounts</param>
        /// <param name="urlParams">api参数集合</param>
        /// <returns></returns>
        public string doPost()
        {
            if (!_UseWebApiClient) return Post();
            JObject ret;

            //获取签名
            string sign = GetSignature();
            //拼装api参数
            ParameterCollection ps = GetUrlParmCollection(sign);
            WebApiClient client = GetWebApiClient();
            WebApiResponse resp = client.Post(FunName + "/" + Secret.AppKey, null, ps);
            if (resp.IsError)
            {
                throw new Exception(string.Format("调用1688接口{0}错误:{1}",FunName,JsonConvert.SerializeObject(UrlParams)));
            }
            ret = JsonConvert.DeserializeObject<JObject>(resp.Content);
            if (ret == null)
            {
                throw new Exception(string.Format("调用1688接口{0}返回空对象{1},返回内容{2}", FunName, JsonConvert.SerializeObject(UrlParams), JsonConvert.SerializeObject(resp)));
            }
            return ret.ToString();
        }
        /// <summary>
        /// 不使用enterprise3.WebApi.Client.dll的调用方式
        /// </summary>
        /// <returns></returns>
        public string Post() {
            string ret;
            try {
                string sign = GetSignature();
                StringBuilder bussdata = new StringBuilder();
                foreach(KeyValuePair<string,string> pair in UrlParams) {
                    if(bussdata.Length>0)bussdata.Append('&');
                    bussdata.AppendFormat("{0}={1}",pair.Key,HttpUtility.UrlEncode(pair.Value));
                }
                bussdata.AppendFormat("&_aop_signature={0}",HttpUtility.UrlEncode(sign));
                using (HttpClient client = new HttpClient()) {
                    HttpContent httpContent;
                    httpContent = new StringContent(bussdata.ToString(),Encoding.UTF8);
                    httpContent.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-WWW-form-urlencoded");
                    client.Timeout=new TimeSpan(TimeSpan.TicksPerSecond*10);//超时10秒
                    var result = client.PostAsync(Secret.ApiUrl+FunName+"/"+Secret.AppKey,httpContent).Result.Content.ReadAsByteArrayAsync().Result;
                    ret=System.Text.Encoding.UTF8.GetString(result);
                }
                if (string.IsNullOrWhiteSpace(ret)) {
                    throw new Exception("调用接口返回值为空");
                }
                JObject retObj = JsonConvert.DeserializeObject<JObject>(ret);
                string errmsg = retObj.Value<string>("error_message");
                if (!string.IsNullOrWhiteSpace(errmsg)) {
                    throw new Exception(errmsg);
                }
                errmsg = retObj.Value<string>("exception");
                if (!string.IsNullOrWhiteSpace(errmsg))
                {
                    throw new Exception(errmsg);
                }
            }
            catch (Exception e ) {
                throw new Exception(string.Format("调用1688接口{0}错误{1},返回内容{2}", FunName, JsonConvert.SerializeObject(UrlParams),e.Message));
            }
            return ret;
        }
        ///// <summary>
        ///// 日期格式转换
        ///// 输入格式为yyyyMMddHHmmssfffzz00
        ///// </summary>
        ///// <param name="albaba_dt"></param>
        ///// <returns></returns>
        //public DateTime FormatDateTime(string albaba_dt)
        //{
        //    int year = Int32.Parse(albaba_dt.Substring(0, 4));
        //    int mon = Int32.Parse(albaba_dt.Substring(4, 2));
        //    int day = Int32.Parse(albaba_dt.Substring(6, 2));
        //    int hour = Int32.Parse(albaba_dt.Substring(8, 2));
        //    int min = Int32.Parse(albaba_dt.Substring(10, 2));
        //    int sec = Int32.Parse(albaba_dt.Substring(12, 2));
        //    DateTime dt = new DateTime(year, mon, day, hour, min, sec, DateTimeKind.Local);
        //    return dt;
        //}

        #region 私用函数
        /// <summary>
        /// 创建webapiclient
        /// </summary>
        /// <param name="infor"></param>
        /// <returns></returns>
        private WebApiClient GetWebApiClient()
        {
            AppInfoBase appinfo = new AppInfoBase();
            appinfo.AppKey = Secret.AppKey;
            appinfo.AppSecret = Secret.AppSecret;
            WebApiClient client = new WebApiClient(Secret.ApiUrl, appinfo, EnumDataFormat.Json);
            return client;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="infor"></param>
        /// <param name="funname"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        private string GetSignature()
        {
            string p1 = Secret.ApiParmUrl + FunName + "/" + Secret.AppKey;
            List<string> p2 = new List<string>();
            Dictionary<string, string>.Enumerator itemator = UrlParams.GetEnumerator();
            while (itemator.MoveNext())
            {
                p2.Add(itemator.Current.Key + itemator.Current.Value);
            }
            //排序
            p2.Sort(delegate (string a, string b)
            {
                int i;
                for (i = 0; i < a.Length; i++)
                {
                    if (i >= b.Length) return 1;//a比b长
                    if (a[i] < b[i]) return -1;
                    if (a[i] > b[i]) return 1;
                }
                if (i < b.Length)
                { //a比b短
                    return -1;
                }
                return 0;

            });

            p2.ForEach(delegate (string item) { p1 += item; });

            byte[] signatureKey = Encoding.UTF8.GetBytes(Secret.AppSecret);
            HMACSHA1 hmacsha1 = new HMACSHA1(signatureKey);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(p1));
            byte[] hash = hmacsha1.Hash;
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();

        }


        /// <summary>
        /// 拼装api参数
        /// </summary>
        /// <param name="urlParanms">url参数</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        private ParameterCollection GetUrlParmCollection(string sign)
        {
            ParameterCollection ps = new ParameterCollection();
            Dictionary<string, string>.Enumerator i = UrlParams.GetEnumerator();
            while (i.MoveNext())
            {
                ps.Add(i.Current.Key, i.Current.Value);
            }
            if (!string.IsNullOrEmpty(sign))
            {
                ps.Add("_aop_signature", sign);
            }
            return ps;
        }
        #endregion
    }
}
