using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Ali1688Business.Model.Buyoffer
{
    /// <summary>
    /// 询价单
    /// </summary>
    public class AliBuyoffer : AliSecret
    {
        //{"subject":"test","phone":"13978766543","contact":"联系人","description":"描述1","gmtQuotationExpire":"2017-12-12 12:12:12","subBizType":"singlepurchase","processTemplateCode":"assureTradeBussinessBuy","transToolType":"3","invoiceRequirement":"vat","visibleAfterEndQuote":"false","sourceMethodType":"selectedmysupplier","supplierMemberIds":["331010","678908"],"includeTax":"true","quoteHasPostFee":"false","allowPartOffer":"false","certificateIds":["542","1","2","3","400","548","549"],"otherCertificateNames":["合格证","检测报告"],items:[{"prItemId":"1001","productCode":"a001","purchaseAmount":100,"subject":"资源名称","desc":"资源描述","unit":"千克","brandName":"品牌"}]}
        public string offerid { get; set; }
        public string subject { get; set; }
        public string phone { get; set; }
        public string contact { get; set; }
        public string description { get; set; }
        public DateTime gmtQuotationExpire { get; set; }
        public string subBizType { get; set; }
        public string processTemplateCode { get; set; }
        public string transToolType { get; set; }
        public string invoiceRequirement { get; set; }
        public bool visibleAfterEndQuote { get; set; }
        public string sourceMethodType { get; set; }
        public List<string> supplierMemberIds { get; set; }
        /// <summary>
        /// 供应商登陆账号
        /// </summary>
        public List<string> supplierLoginIds { get; set; }
        public bool includeTax { get; set; }
        public bool quoteHasPostFee { get; set; }

        public bool allowPartOffer { get; set; }

        public List<string> certificateIds { get; set; }
        public List<string> otherCertificateNames { get; set; }

        public string subuserid { get; set; }
        public string ngid { get; set; }
        public string closereason { get; set; }
        public string closedesc { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string receiveStreetAddress { get; set; }
        //明细数据
        public List<AliBuyofferDetail> items { get; set; }

        public bool open { get; set; }
        public bool openToPortal { get; set; }

        public string receiveAddressProvince { get; set; }
        public string receiveAddressCity { get; set; }
    }
}
