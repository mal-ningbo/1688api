using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ali1688Business.Model;
using Ali1688Business.Model.Quotation;
using Newtonsoft.Json;

namespace Ali1688Business.Quotation
{
    public class AliQuotationHandle:AliPubHandle
    {
        public override void analyseBussdata(string userServer, string bussdata)
        {
            UserSrvType = userServer;
            BusData=bussdata;
            switch (userServer)
            {
                case "GetQuotationListByBuyOfferId":
                    //根据询价id获取询价单的报价信息
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","OfferId":"123"}                  
                    setGetQuotationListByBuyOfferIdParams(bussdata);
                    break;
                default:
                    UserSrvType = "";
                    throw new Exception(string.Format("{0}业务类型:{1},参数错误", "报价接口", userServer));
            }
        }

        private void setGetQuotationListByBuyOfferIdParams(string bussdata)
        {
            AliQuotation quota = JsonConvert.DeserializeObject<AliQuotation>(bussdata);
            Secret = quota;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("buyofferId", quota.OfferId);
            FunName = "cn.alibaba.open/caigou.api.quotation.buyerGetQuotationListByBuyOfferId";
        }
    }
}
