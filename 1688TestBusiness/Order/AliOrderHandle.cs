using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ali1688Business.Model;
using Ali1688Business.Model.Order;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Ali1688Business.Order
{
    public class AliOrderHandle : AliPubHandle
    {
        public override void analyseBussdata(string userServer, string bussdata)
        {
            UserSrvType = userServer;
            BusData = bussdata;
            switch (userServer)
            {
                case "GetOrder":
                    //根据订单id获取订单的信息
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","OrderId":"123"}                  
                    setGetOrderParams(bussdata);
                    break;
                case "CreateOrder":
                    //创建询价流程采购单  需要报价单主键
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","SubBiz":"buyoffer","SupplyNoteId":"1688报价单主键",ReceiveAddressGroup:{"address":"地址","areaCode":"330108","fullName":"收货人","mobile":"手机","phone":"办公电话","postCode":"邮编"},"AliOrderDetails":[{"QuoteItemId":报价项id,"ItemCount":9.88},...]}                  
                    setCreateOrderParams(bussdata);
                    break;
                case "CreateOrderPayment":
                    //根据订单创建付款单
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","OrderId":"123"}    
                    setCreateOrderPaymentParams(bussdata);
                    break;
                case "GetOrder2":
                    //根据订单id获取订单的信息
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","OrderId":"123"}                  
                    setGetOrder2Params(bussdata);
                    break;
                case "CancelOrder":
                    //取消订单
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","OrderId":"123","cancelReason":"取消原因","remark":"备注"}  
                    setCancelOrderParams(bussdata);
                    break;
                case "QueryOrderDetail":
                    //查询采购订单详情接口
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","OrderId":"88875164"}                  
                    setQueryOrderDetailParams(bussdata);
                    break;
                case "CreateProcurementOrder":
                    //创建履约订单
                    //bussdata eg:{"CompanyCode":"1000","CompanyName":null,"CreatorName":"托狼","CreatorUserId":0,"Address":{"Address":"浙江省杭州市滨江区网商路699号","MobilePhone":"13786543897","Phone":null,"Post":"315151","ReceiverName":"李四"},"OperatorLoginId":"dongyl3333","SupplierCode":"sh0001","SupplierName":null,"PaymentMode":"transbybank","TradeMode":"alipay","NgPurId":"2001","NgPurNo":"billno001","Details":[{"NgPurDetailId":"1001","ResCode":"85000480","Description":"资源描述,备注杂七杂八的全放这","UnitName":"kg","Qty":10.0,"UntaxPrc":10.0,"TaxPrc":12.0,"TaxRate":13.0,"TaxCode":"J13","DeliveryDate":"2019-08-03 07:41:15"},{"NgPurDetailId":"1002","ResCode":"85000481","Description":"资源描述,备注杂七杂八的全放这","UnitName":"g","Qty":10.0,"UntaxPrc":10.0,"TaxPrc":17.0,"TaxRate":17.0,"TaxCode":"J13","DeliveryDate":"2019-08-03 07:41:15"},{"NgPurDetailId":"1003","ResCode":"85000482","Description":"资源描述,备注杂七杂八的全放这","UnitName":"mg","Qty":100.0,"UntaxPrc":100.0,"TaxPrc":127.0,"TaxRate":17.0,"TaxCode":"J13","DeliveryDate":"2019-08-03 07:41:15"}],"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a"}
                    setCreateProcurementOrderParams(bussdata);
                    break;
                case "ApproveOrder":
                    //采购订单审批
                    SetApproveOrderParams(bussdata);
                    break;
                    //根据来源单据创建订单
                case "CreateOrderBySourceId":
                    SetCreateOrderBySourceIdParams(bussdata);
                    break;
                default:
                    UserSrvType = "";
                    throw new Exception(string.Format("{0}业务类型:{1},参数错误", "订单接口", userServer));
            }
        }

        private void setGetOrderParams(string bussdata)
        {
            AliOrder order = JsonConvert.DeserializeObject<AliOrder>(bussdata);
            Secret = order;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("orderId", order.OrderId);
            FunName = "cn.alibaba.open/trade.order.orderDetail.get";
        }

        private void setGetOrder2Params(string bussdata)
        {
            AliOrder order = JsonConvert.DeserializeObject<AliOrder>(bussdata);
            Secret = order;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("id", order.OrderId);
            UrlParams.Add("needInvoiceInfo", "false");
            UrlParams.Add("needOrderMemoList", "false");
            UrlParams.Add("needLogisticsOrderList", "false");
            UrlParams.Add("needOrderEntries", "true");
            FunName = "cn.alibaba.open/trade.order.detail.get";
        }
        private void setCreateOrderParams(string bussdata)
        {
            AliOrder order = JsonConvert.DeserializeObject<AliOrder>(bussdata);
            Secret = order;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("subBiz", order.SubBiz);
            JObject obj = new JObject();
            obj.Add("supplyNoteId", order.SupplyNoteId);
            if (order.AliOrderDetails != null)
            {
                if (order.AliOrderDetails.Count > 0)
                {
                    JArray ids = new JArray();
                    JArray details = new JArray();
                    JObject itemobj;
                    foreach (AliOrderDetail item in order.AliOrderDetails)
                    {
                        itemobj = new JObject();
                        itemobj.Add("quoteItemId", item.QuoteItemId);
                        itemobj.Add("itemCount", item.ItemCount);
                        ids.Add(item.QuoteItemId);
                        details.Add(itemobj);
                    }
                    obj.Add("quoteItemIds", ids);
                    obj.Add("skuAmountList", details);
                }
            }
            UrlParams.Add("quotationInfo", JsonConvert.SerializeObject(obj));
            obj = new JObject();
            obj.Add("receiveAddressGroup", JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(order.ReceiveAddressGroup)));
            UrlParams.Add("makeSingleOrderGroup", JsonConvert.SerializeObject(obj));
            FunName = "com.alibaba.trade/alibaba.trade.quotationOrder.create";
        }

        private void setCreateOrderPaymentParams(string bussdata)
        {
            AliOrder order = JsonConvert.DeserializeObject<AliOrder>(bussdata);
            Secret = order;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("orderId", order.OrderId);
            FunName = "com.alibaba.trade/alibaba.payment.order.bank.create";
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="bussdata"></param>
        private void setCancelOrderParams(string bussdata)
        {
            AliOrder order = JsonConvert.DeserializeObject<AliOrder>(bussdata);
            Secret = order;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("webSite", "1688");
            UrlParams.Add("tradeID", order.OrderId);
            UrlParams.Add("cancelReason", order.cancelReason);
            UrlParams.Add("remark", order.remark);
            FunName = "com.alibaba.trade/alibaba.trade.cancel";
        }

        private void setQueryOrderDetailParams(string busdata)
        {
            ProcurementOrder order = JsonConvert.DeserializeObject<ProcurementOrder>(busdata);
            Secret = order;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("procurementOrderId", order.OrderId);
            FunName = "cn.alibaba.open/alibaba.caigou.procurement.order.queryProcurementOrderDetail";
        }
        private void setCreateProcurementOrderParams(string busdata)
        {
            ProcurementOrder order = JsonConvert.DeserializeObject<ProcurementOrder>(busdata);
            Secret = order;
            UrlParams = new Dictionary<string, string>();
            JObject createOrderParam = new JObject(), tmpObj = new JObject();
            createOrderParam.Add("bizScene", order.BizScene);
            createOrderParam.Add("orderType", "standard");
            createOrderParam.Add("erpId", order.NgPurId);
            createOrderParam.Add("needSupplierConfirm", order.NeedSupplierConfirm);
            createOrderParam.Add("operatorLoginId", order.OperatorLoginId);

            tmpObj = new JObject();
            tmpObj.Add("companyCode", order.CompanyCode);
            tmpObj.Add("companyName", order.CompanyName);
            createOrderParam.Add("buyerInfo", tmpObj);
            if (order.Address != null)
            {
                tmpObj = new JObject();
                tmpObj.Add("address", order.Address.Address);
                tmpObj.Add("mobilePhone", order.Address.MobilePhone);
                tmpObj.Add("phone", order.Address.Phone);
                tmpObj.Add("post", order.Address.Post);
                tmpObj.Add("receiverName", order.Address.ReceiverName);
                JObject logisticsInfo = new JObject();
                logisticsInfo.Add("address", tmpObj);
                createOrderParam.Add("logisticsInfo", logisticsInfo);
            }

            tmpObj = new JObject();
            tmpObj.Add("supplierCode", order.SupplierCode);
            tmpObj.Add("supplierName", order.SupplierName);
            createOrderParam.Add("supplierInfo", tmpObj);

            tmpObj = new JObject();
            JArray payModes = new JArray();
            tmpObj.Add("tradeMode", order.TradeMode);
            payModes.Add(order.PaymentMode);
            tmpObj.Add("paymentModes", payModes);
            createOrderParam.Add("tradeTermInfo", tmpObj);

            JArray entries = new JArray();
            JObject entry = null;
            foreach (ProcurementOrderDetail d in order.Details)
            {
                entry = new JObject();
                entry.Add("amount", d.Qty);
                entry.Add("precisionPea", d.UntaxPrc);
                entry.Add("precisionPia", d.TaxPrc);
                entry.Add("taxRate", d.TaxRate);
                entry.Add("taxCode", d.TaxCode);
                entry.Add("unitName", d.UnitName);
                if (d.DeliveryDate.HasValue)
                {
                    tmpObj = new JObject();
                    tmpObj.Add("deliveryDate", d.DeliveryDate.Value);
                    entry.Add("deliveryInfo", tmpObj);
                }
                tmpObj = new JObject();
                tmpObj.Add("bizType", order.BizScene);
                tmpObj.Add("creatorName", order.CreatorName);
                tmpObj.Add("creatorUserId", order.CreatorUserId);
                tmpObj.Add("documentCode", order.NgPurNo);
                tmpObj.Add("documentId", order.NgPurId);
                tmpObj.Add("outerDocumentCode", order.NgPurId);//purchasingApplicationNumber
                tmpObj.Add("outerPrimaryId", d.NgPurDetailId);//purchasingApplicationRowId
                tmpObj.Add("outerRowId", d.NgPurDetailId);//purchasingApplicationRowId
                tmpObj.Add("primaryId", d.QuoteDetialId);//sourceId
                tmpObj.Add("rowId", d.QuoteDetialId);//sourceId
                entry.Add("sourceInfo", tmpObj);

                tmpObj = new JObject();
                tmpObj.Add("code", d.ResCode);
                tmpObj.Add("description", d.Description);
                tmpObj.Add("type", "offer");
                entry.Add("subjectInfo", tmpObj);

                entries.Add(entry);
            }
            createOrderParam.Add("orderEntryList", entries);
            UrlParams.Add("access_token", Secret.TokenKey);
            JsonSerializerSettings set = new JsonSerializerSettings();
            set.DateFormatString = "yyyy-MM-dd hh:mm:ss";
            UrlParams.Add("createOrderParam", JsonConvert.SerializeObject(createOrderParam, set));
            FunName = "cn.alibaba.open/alibaba.caigou.procurement.order.createProcurementOrder";
        }

        private void SetApproveOrderParams(string busdata)
        {
            ProcurementOrder order = JsonConvert.DeserializeObject<ProcurementOrder>(busdata);
            Secret = order;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("procurementOrderId", order.OrderId);
            UrlParams.Add("erpId", order.NgPurId);
            UrlParams.Add("status", order.Status);
            FunName = "cn.alibaba.open/alibaba.caigou.procurement.order.approveOrder";
        }

        private void SetCreateOrderBySourceIdParams(string busdata)
        {
            ProcurementOrderFromNg order = JsonConvert.DeserializeObject<ProcurementOrderFromNg>(busdata);
            Secret = order;
            UrlParams = new Dictionary<string, string>();
            JObject createOrderParam = new JObject(), tmpObj = new JObject();
            //createOrderParam.Add("bizScene", "outersystem");
            createOrderParam.Add("companyCode", order.CompanyCode);
            createOrderParam.Add("erpId", order.NgPurId);
            createOrderParam.Add("needDefaultInvoiceTemplate", order.NeedInvoice);
            createOrderParam.Add("needSupplierConfirm", order.NeedSupplierConfirm);
            createOrderParam.Add("operatorLoginId", order.OperatorLoginId);
            if (string.IsNullOrWhiteSpace(order.OrderType))
            {
                createOrderParam.Add("orderType", "standard");
            }
            else {
                createOrderParam.Add("orderType", order.OrderType);
            }
            createOrderParam.Add("sourceId", order.QuoteId);
            if (string.IsNullOrWhiteSpace(order.SourceType))
            {
                createOrderParam.Add("sourceType", "quotation");
            }
            else
            {
                createOrderParam.Add("sourceType", order.SourceType);
            }
            if (order.Address != null)
            {
                tmpObj = new JObject();
                tmpObj.Add("address", order.Address.Address);
                tmpObj.Add("mobilePhone", order.Address.MobilePhone);
                tmpObj.Add("phone", order.Address.Phone);
                tmpObj.Add("post", order.Address.Post);
                tmpObj.Add("receiverName", order.Address.ReceiverName);
                JObject logisticsInfo = new JObject();
                logisticsInfo.Add("address", tmpObj);
                createOrderParam.Add("logisticsInfo", logisticsInfo);
            }
            JArray entries = new JArray();
            JObject entry = null;
            foreach (ProcurementOrderDetail d in order.Details)
            {
                entry = new JObject();
                entry.Add("amount", d.Qty);
                if (d.DeliveryDate.HasValue)
                {
                    tmpObj = new JObject();
                    tmpObj.Add("deliveryDate", d.DeliveryDate.Value.ToString("yyyyMMddHHmmssfffzz00"));
                    entry.Add("deliveryInfo", tmpObj);
                }
                entry.Add("sourceRowId", d.QuoteDetialId);
                entries.Add(entry);
            }
            createOrderParam.Add("orderEntryList", entries);
            UrlParams.Add("access_token", Secret.TokenKey);
            //JsonSerializerSettings set = new JsonSerializerSettings();
            //set.DateFormatString = "yyyy-MM-dd hh:mm:ss";
            UrlParams.Add("param", JsonConvert.SerializeObject(createOrderParam));
            FunName = "cn.alibaba.open/com.alibaba.procurement.BuyerOrderService.createOrderBySourceId";           
        }
    }
}
