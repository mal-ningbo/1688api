using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ali1688Business.Model;
using Ali1688Business.Model.Buyoffer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Ali1688Business.Buyoffer
{
    public class AliBuyofferHandle : AliPubHandle
    {
        public override void analyseBussdata(string userServer, string bussdata)
        {
            UserSrvType = userServer;
            BusData=bussdata;
            switch (userServer)
            {
                case "PostBuyoffer":
                    //发布询价单
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","subject":"test","phone":"13978766543","contact":"联系人","description":"描述1","gmtQuotationExpire":"2017-12-12 12:12:12","subBizType":"singlepurchase","processTemplateCode":"assureTradeBussinessBuy","transToolType":"3","invoiceRequirement":"vat","visibleAfterEndQuote":"false","sourceMethodType":"selectedmysupplier","supplierMemberIds":["331010","678908"],"supplierLoginIds":["登陆账号1","登陆账号2"],"includeTax":"true","quoteHasPostFee":"false","allowPartOffer":"false","certificateIds":["542","1","2","3","400","548","549"],"otherCertificateNames":["合格证","检测报告"],"ngid":"123","receiveStreetAddress":"hangzhou",items:[{"prItemId":"1001","productCode":"a001","purchaseAmount":100,"subject":"资源名称","desc":"资源描述","unit":"千克","brandName":"品牌"}]}
                    setPostBuyofferParams(bussdata);
                    break;
                case "CloseBuyOffer":
                    //关闭询价单
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","offerid":"1688单据主键","closereason":"关闭理由","closedesc":"关闭描述"}
                    setCloseBuyOfferParams(bussdata);
                    break;
                case "GetBuyoffer":
                    //取1688询价单信息
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","offerid":"1688单据主键"}
                    setGetBuyofferParams(bussdata);
                    break;

                default:
                    UserSrvType = "";
                    throw new Exception(string.Format("{0}业务类型:{1},参数错误", "询价接口", userServer));
            }
        }

        private void setPostBuyofferParams(string bussdata)
        {
            AliBuyoffer buyoffer = JsonConvert.DeserializeObject<AliBuyoffer>(bussdata);
            Secret = buyoffer;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            if (buyoffer.items == null)
            {
                throw new Exception("缺少询价单明细");
            }
            if (buyoffer.items.Count == 0)
            {
                throw new Exception("询价单明细行数为0");
            }
            UrlParams.Add("items", JsonConvert.SerializeObject(buyoffer.items));
            UrlParams.Add("prId", buyoffer.ngid);
            UrlParams.Add("open", buyoffer.open.ToString());
            UrlParams.Add("openToPortal", buyoffer.openToPortal.ToString());
            UrlParams.Add("allmysupplier","true");
            UrlParams.Add("needSignAgreement", "false");
            if (buyoffer.subuserid != "0" && !string.IsNullOrEmpty(buyoffer.subuserid))
            {
                UrlParams.Add("subUserId", buyoffer.subuserid);
            }
            UrlParams.Add("gmtQuotationExpire", buyoffer.gmtQuotationExpire.ToString("yyyyMMddHHmmssfffzz00"));
            UrlParams.Add("subBizType", buyoffer.subBizType);
            UrlParams.Add("processTemplateCode", buyoffer.processTemplateCode);
            UrlParams.Add("transToolType", buyoffer.transToolType);
            UrlParams.Add("invoiceRequirement", buyoffer.invoiceRequirement);
            UrlParams.Add("visibleAfterEndQuote", buyoffer.visibleAfterEndQuote.ToString());
            UrlParams.Add("sourceMethodType", buyoffer.sourceMethodType);
            if (buyoffer.sourceMethodType == "selectedmysupplier")
            {
                if (buyoffer.supplierLoginIds != null)
                {
                    if (buyoffer.supplierLoginIds.Count > 0)
                    {
                        UrlParams.Add("supplierLoginIds", JsonConvert.SerializeObject(buyoffer.supplierLoginIds));
                        UrlParams.Add("selectedmysupplier", "true");
                    }
                }
                else if (buyoffer.supplierMemberIds != null)
                {
                    if (buyoffer.supplierMemberIds.Count > 0)
                    {
                        UrlParams.Add("supplierMemberIds", JsonConvert.SerializeObject(buyoffer.supplierMemberIds));
                        UrlParams.Add("selectedmysupplier","true");
                    }
                }

            }
            UrlParams.Add("includeTax", buyoffer.includeTax.ToString());
            UrlParams.Add("quoteHasPostFee", buyoffer.quoteHasPostFee.ToString());
            UrlParams.Add("allowPartOffer", buyoffer.allowPartOffer.ToString());
            if (buyoffer.certificateIds != null)
            {
                if (buyoffer.certificateIds.Count > 0)
                {
                    UrlParams.Add("certificateIds", JsonConvert.SerializeObject(buyoffer.certificateIds));
                }
            }
            if (buyoffer.otherCertificateNames != null)
            {
                if (buyoffer.otherCertificateNames.Count > 0)
                {
                    UrlParams.Add("otherCertificateNames", JsonConvert.SerializeObject(buyoffer.otherCertificateNames));
                }
            }
            UrlParams.Add("description", buyoffer.description);
            UrlParams.Add("contact", buyoffer.contact);
            UrlParams.Add("phone", buyoffer.phone);
            UrlParams.Add("subject", buyoffer.subject);
            //UrlParams.Add("receiveAddressProvince", "3478");
            //UrlParams.Add("receiveAddressCity", "3486");
            //UrlParams.Add("receiveAddressCounty", "3487");

            if (!string.IsNullOrWhiteSpace(buyoffer.receiveStreetAddress))
            {
                UrlParams.Add("receiveStreetAddress", buyoffer.receiveStreetAddress);
            }
            if (!string.IsNullOrWhiteSpace(buyoffer.receiveAddressProvince)) {
                UrlParams.Add("receiveAddressProvince", buyoffer.receiveAddressProvince);
            }
            if (!string.IsNullOrWhiteSpace(buyoffer.receiveAddressCity)) {
                UrlParams.Add("receiveAddressCity", buyoffer.receiveAddressCity);
            }
            FunName = "cn.alibaba.open/caigou.api.buyoffer.postBuyoffer";

        }
        private void setCloseBuyOfferParams(string bussdata)
        {
            AliBuyoffer buyoffer = JsonConvert.DeserializeObject<AliBuyoffer>(bussdata);
            Secret = buyoffer;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("buyOfferId", buyoffer.offerid);
            UrlParams.Add("closeReason", buyoffer.closereason);
            UrlParams.Add("closeDesc", buyoffer.closedesc);
            FunName = "cn.alibaba.open/caigou.api.buyoffer.closeBuyOffer";
        }
        private void setGetBuyofferParams(string bussdata)
        {
            AliBuyoffer buyoffer = JsonConvert.DeserializeObject<AliBuyoffer>(bussdata);
            Secret = buyoffer;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("buyOfferId", buyoffer.offerid);
            FunName = "cn.alibaba.open/caigou.api.buyOffer.getBuyOfferById";
        }
    }
}
