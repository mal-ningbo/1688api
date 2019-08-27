using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.BulkSettlementImpl
{
    public class AliBulkSettlementImpl : AliSecret
    {
        /// <summary>
        /// 1688主键
        /// </summary>
        public long Id { get; set; } 
        public List<long> ReceiveNodeIds { get; set; }//1688收货单明细主键
        /// <summary>
        /// 1688登录账号
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// 附言
        /// </summary>
        public string Memo { get; set; }
        public List<AliBulkSettlementDetail> Details { get; set; }
    }
    public class AliBulkSettlementDetail {
        /// <summary>
        /// 是否修改单价
        /// </summary>
        public bool ModifyPrice { get; set; }
        /// <summary>
        /// 收货单明细主键
        /// </summary>
        public long RcvEntryId { get; set; }
        /// <summary>
        /// 含税金额
        /// </summary>
        public decimal TaxAmt { get; set; }
        /// <summary>
        /// 结算单价
        /// </summary>
        public decimal Prc { get; set; }
    }

    public class AliPayNote : AliSecret {
        public long id { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amt { get; set; }
        /// <summary>
        /// 1688登录账号
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// 付款方式 默认transbybank
        /// </summary>
        public string PaymentMode { get; set; }
        /// <summary>
        ///唯一请求号,i8主键即可
        /// </summary>
        public string RequestNo { get; set; }
        public List<AliPayDetail> Details { get; set; }
    }
    public class AliPayDetail {
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amt { get; set; }
        public long SettleId { get; set; } //结算单主键
    }
}
