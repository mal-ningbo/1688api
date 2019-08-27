using System;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Ali1688Business
{
    public class AliDataLog
    {

        private bool _logenabled;//日志开关
        public AliDataLog()
        {
            try
            {
                Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");
                string dbtype = config.ConnectionStrings.ConnectionStrings["DefaultConnection"].ProviderName;
                if (config.AppSettings.Settings.AllKeys.Count(delegate (string item) { return item == "1688logenabled"; }) == 0)
                {
                    _logenabled = false;
                }
                else
                {
                    string tmp = config.AppSettings.Settings["1688logenabled"].Value;
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        _logenabled = false;
                    }
                    else
                    {
                        _logenabled = tmp == "1";
                    }
                }
            }
            catch
            {
                _logenabled = false;
            }
        }

        /// <summary>
        /// 记录调用1688接口的信息
        /// </summary>
        /// <param name="handle">调用详情（appkey,appsecret,apptoken,调用方法，json格式的业务数据,usersrvtype）</param>
        /// <param name="result">1688接口返回结果</param>
        /// <param name="success">调用是否成功</param>
        /// <param name="errMsg">云端执行时的异常信息</param>      
        public void LogForAliApi(AliPubHandle handle, string result, bool success, Exception errMsg = null)
        {
            if (!_logenabled) return;
            string dir = Path.GetDirectoryName(new Uri(this.GetType().Assembly.CodeBase).LocalPath);
            try
            {
                string file = Path.Combine(dir, handle.Secret.AppKey + "_" + DateTime.Today.ToShortDateString().Replace('/', '_') + ".txt");
                FileInfo f = new FileInfo(file);
                using (StreamWriter sw = f.Exists ? f.AppendText() : f.CreateText())
                {
                    sw.WriteLine(string.Format("{0}\r\n{1} {2} {3}\r\nUserType:{4}\r\nFunName:{5}\r\nBusData:{6}\r\nresult:{7}\r\nsuccess:{8}\r\nerror:{9}\r\n",
                        DateTime.Now,
                        handle.Secret.AppKey,
                        handle.Secret.AppSecret,
                        handle.Secret.TokenKey,
                        handle.UserSrvType, handle.FunName,
                        handle.BusData, result, success, errMsg == null ? "" : errMsg.ToString()));
                    sw.Flush();
                    sw.Close();
                }
            }
            catch //(Exception e)
            {
                //任何异常，不抛出  
            }

        }
    }
}
