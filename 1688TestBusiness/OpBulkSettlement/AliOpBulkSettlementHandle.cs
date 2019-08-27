using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ali1688Business.Model;
using Ali1688Business.Model.OpBulkSettlement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ali1688Business.OpBulkSettlement
{
    public class AliOpBulkSettlementHandle:AliPubHandle
    {
        public override void analyseBussdata(string userServer, string bussdata)
        {
            UserSrvType = userServer;
            BusData=bussdata;
            switch (userServer)
            {
                case "ReceiveOrder":
                    //确认订单生成收货单
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","OrderId":"123","SubAccount":"XXXXX",ReceivedDetails:[{"orderEntryId": "88549252424587071","quantity":1,"realQuantity":1},{"orderEntryId": "88549252424587071","quantity":2,"realQuantity":2},{"orderEntryId":"88549252426587071","quantity":2,"realQuantity":2}]}                  
                    setReceiveOrderParams(bussdata);
                    break;
                case "GetOpBulkSettlement":
                    //获取收货单信息
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","RcvId":"123"}
                    setGetOpBulkSettlementParams(bussdata);
                    break;
                case "ReceiveGoods": //履约采购创建收货单
                    //bussdata eg:{ "AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","SubAccount":"dongyl3333","ReceiveDate":"2019-08-20","InsideRemark":"memo1","OutsideRemark":"memo2","ReceivedDetails":[{"EntryId":88786899,"realQuantity":1}]}
                    setReceiveGoodsParams(bussdata);
                    break;
                case "QueryReceiveGoods"://查询履约采购收货单
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","Id":123}
                    setQueryReceiveGoodsParams(bussdata);
                    break;
                //case "ConfirmGoods":
                //    //履约采购订单确认收货
                //    setConfirmGoodsParams(bussdata);
                //    break;
                default:
                    UserSrvType = "";
                    throw new Exception(string.Format("{0}业务类型:{1},参数错误", "到货接口", userServer));
            }
        }

        private void setReceiveOrderParams(string bussdata)
        {
            AliOpBulkSettlement settlement = JsonConvert.DeserializeObject<AliOpBulkSettlement>(bussdata);
            Secret = settlement;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("orderId", settlement.OrderId);
            if (!string.IsNullOrEmpty(settlement.SubAccount))
            {
                UrlParams.Add("subUserLoginId", settlement.SubAccount);
            }
            UrlParams.Add("receivedQuantity", JsonConvert.SerializeObject(settlement.ReceivedDetails));
            //UrlParams.Add("refundIdReceivedQuantity", "[]");
            FunName = "com.alibaba.logistics/alibaba.bulksettlement.OpCreateBulkSettlementReceiveNote";
        }

        private void setGetOpBulkSettlementParams(string bussdata)
        {
            AliOpBulkSettlement settlement = JsonConvert.DeserializeObject<AliOpBulkSettlement>(bussdata);
            Secret = settlement;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("receiveNodeIds", "["+settlement.RcvId+"]");
            FunName = "com.alibaba.logistics/alibaba.bulksettlement.OpBulkSettlementQueryReceiveNoteListByIds";
        }
        private void setReceiveGoodsParams(string bussdata) {
            AliOpBulkSettlement settlement = JsonConvert.DeserializeObject<AliOpBulkSettlement>(bussdata);
            Secret = settlement;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            if (!string.IsNullOrWhiteSpace(settlement.InsideRemark)) {
                UrlParams.Add("buyerInsideRemark", settlement.InsideRemark);
            }
            if (!string.IsNullOrWhiteSpace(settlement.OutsideRemark))
            {
                UrlParams.Add("buyerOutsideRemark", settlement.OutsideRemark);
            }
            if (settlement.ReceiveDate.HasValue) {
                UrlParams.Add("receiveDate", settlement.ReceiveDate.Value.ToString("yyyyMMddHHmmssfffzz00"));
            }
            if (!string.IsNullOrWhiteSpace(settlement.SubAccount)) {
                UrlParams.Add("subUserLoginId", settlement.SubAccount);
            }
            UrlParams.Add("type", "order_entry");
            JArray arr = new JArray();
            JObject obj = null;
            foreach (AliOpBulkSettlementDetail item in settlement.ReceivedDetails) {
                obj = new JObject();
                obj.Add("entryId", item.EntryId);
                obj.Add("quantity", item.realQuantity);
                arr.Add(obj);
            }
            UrlParams.Add("createReceiveGoodsNoteEntryParams", JsonConvert.SerializeObject(arr));
            FunName = "cn.alibaba.open/com.alibaba.procurement.BuyerReceiveGoodsService.createReceiveGoodsNote";
        }

        private void setQueryReceiveGoodsParams(string bussdata) {
            AliOpBulkSettlement settlement = JsonConvert.DeserializeObject<AliOpBulkSettlement>(bussdata);
            Secret = settlement;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("id", settlement.Id.ToString());
            FunName = "cn.alibaba.open/com.alibaba.procurement.BuyerReceiveGoodsService.queryReceiveGoodsNote";
        }
        //private void setConfirmGoodsParams(string busdata) {
        //    Receipt rec = JsonConvert.DeserializeObject<Receipt>(busdata);
        //    Secret = rec;
        //    UrlParams = new Dictionary<string, string>();
        //    UrlParams.Add("access_token", Secret.TokenKey);
        //    UrlParams.Add("confirmCode", rec.Code);
        //    JArray detials = new JArray();
        //    JObject dobj = null;
        //    if (rec.Detials != null) {
        //        foreach (ReceiptDetial item in rec.Detials) {
        //            dobj = new JObject();
        //            dobj.Add("erpId", item.NgPurId);
        //            dobj.Add("rowId", item.NgPurDetailId);
        //            dobj.Add("amount", item.Qty.ToString());
        //            dobj.Add("taxPrice", item.TaxPrc.ToString());
        //            dobj.Add("taxTotalPrice", item.TaxAmt.ToString());
        //            dobj.Add("noTaxPrice", item.UntaxPrc.ToString());
        //            dobj.Add("noTaxTotalPrice", item.UntaxAmt.ToString());
        //            dobj.Add("end", item.End);
        //            detials.Add(dobj);
        //        }
        //        UrlParams.Add("confirmGoodsInfos", JsonConvert.SerializeObject(detials));
        //    }
        //    FunName = "cn.alibaba.open/alibaba.caigou.procurement.order.confirmGoods";
        //}
    }
    
}
