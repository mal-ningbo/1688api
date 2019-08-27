using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ali1688Business.Model.BulkSettlementImpl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Ali1688Business.BulkSettlementImpl
{
    public class AliBulkSettlementImplHandle:AliPubHandle
    {
        public override void analyseBussdata(string userServer, string bussdata)
        {
            UserSrvType = userServer;
            BusData=bussdata;
            switch (userServer)
            {
                case "CreateBulkSettlementImpl":
                    //根据收货单id创建结算单
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","ReceiveNodeIds":[1,2,3,4,5]}
                    setCreateBulkSettlementImplParams(bussdata);
                    break;
                case "CreateSettlementNote": //履约采购的结算
                    //bussdata eg:{ "AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","LoginId":"dongyl3333","Memo":"请及时开票2","Details":[{"RcvEntryId":332945,"TaxAmt":12,"Prc":12}]}
                    setCreateSettlementNoteParams(bussdata);
                    break;
                case "QuerySettlementNote"://履约采购查询结算单
                    //bussdata eg;{ "AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","Id":34632}
                    setQuerySettlementNoteParams(bussdata);
                    break;
                case "CreatePayNote"://履约采购通过结算创建付款单
                    //bussdata eg:{ "AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","Amt":8,"LoginId":"dongyl3333","PaymentMode":"transbybank","RequestNo":"1002003004005","Details":[{"Amt":8,"SettleId":34632}]}
                    setCreatePayNoteParams(bussdata);
                    break;
                default:
                    UserSrvType = "";
                    throw new Exception(string.Format("{0}业务类型:{1},参数错误", "结算接口", userServer));
            }
        }
        
        private void setCreateBulkSettlementImplParams(string bussdata)
        {
            AliBulkSettlementImpl settlement = JsonConvert.DeserializeObject<AliBulkSettlementImpl>(bussdata);
            Secret = settlement;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("receiveEntryNoteIds", JsonConvert.SerializeObject(settlement.ReceiveNodeIds));
            FunName = "com.alibaba.trade/alibaba.bulksettlement.OpCreateBulkSettlementImpl";
        }
        private void setCreateSettlementNoteParams(string bussdata) {
            AliBulkSettlementImpl settlement = JsonConvert.DeserializeObject<AliBulkSettlementImpl>(bussdata);
            Secret = settlement;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("scene", "accountPeriod");
            if(!string.IsNullOrWhiteSpace(settlement.LoginId))UrlParams.Add("operatorLoginId", settlement.LoginId);
            if(!string.IsNullOrWhiteSpace(settlement.Memo)) UrlParams.Add("buyerToSupplierMemo", settlement.Memo);
            JArray arr = new JArray();
            JObject obj = null;
            if (settlement.Details != null)
            {
                foreach (AliBulkSettlementDetail item in settlement.Details)
                {
                    obj = new JObject();
                    obj.Add("modifyPrice", item.ModifyPrice);
                    obj.Add("receiveEntryId", item.RcvEntryId);
                    obj.Add("rowAmount", item.TaxAmt);
                    obj.Add("settlePrice", item.Prc);
                    arr.Add(obj);
                }
            }
            UrlParams.Add("entryList",JsonConvert.SerializeObject(arr));
            FunName = "cn.alibaba.open/com.alibaba.procurement.BuyerSettlementService.createSettlementNote";
        }
        private void setQuerySettlementNoteParams(string bussdata) {
            AliBulkSettlementImpl settlement = JsonConvert.DeserializeObject<AliBulkSettlementImpl>(bussdata);
            Secret = settlement;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("settlementNoteId", settlement.Id.ToString());
            UrlParams.Add("needEntryExtendsData", "false");
            FunName = "cn.alibaba.open/com.alibaba.procurement.BuyerSettlementService.querySettlementNote";
        }

        private void setCreatePayNoteParams(string bussdata) {
            AliPayNote pay = JsonConvert.DeserializeObject<AliPayNote>(bussdata);
            Secret = pay;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            JObject param = new JObject();
            param.Add("amount", pay.Amt.ToString());
            param.Add("operatorLoginId", pay.LoginId);
            param.Add("requestNo", pay.RequestNo);
            JObject tmp = new JObject();
            if (!string.IsNullOrWhiteSpace(pay.PaymentMode))
            {
                tmp.Add(pay.PaymentMode, pay.Amt.ToString());
            }
            else {
                tmp.Add("transbybank", pay.Amt.ToString());
            }
            param.Add("payChannelMap", tmp);
            JArray arr = new JArray();
            if (pay.Details != null) {
                foreach (AliPayDetail item in pay.Details) {
                    tmp = new JObject();
                    tmp.Add("amount", item.Amt.ToString());
                    tmp.Add("bizId", item.SettleId.ToString());
                    tmp.Add("transferMode", "instant");
                    tmp.Add("type", "settle");
                    arr.Add(tmp);
                }
            }
            param.Add("details", arr);
            UrlParams.Add("param", JsonConvert.SerializeObject(param));
            FunName = "cn.alibaba.open/com.alibaba.procurement.BuyerPayService.createPayNote";
        }
    }
}
