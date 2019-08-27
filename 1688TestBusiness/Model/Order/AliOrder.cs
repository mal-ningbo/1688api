using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Order
{
    public class AliOrder : AliSecret
    {
        public string OrderId { get; set; }
        public string SubBiz { get; set; }
        /// <summary>
        /// 报价单id
        /// </summary>
        public string SupplyNoteId { get; set; }
        public AliOrderAddress ReceiveAddressGroup { get; set; }
        /// <summary>
        /// 下单报价项集合
        /// </summary>
        public List<AliOrderDetail> AliOrderDetails { get; set; }
        /// <summary>
        /// 取消交易原因
        /// </summary>
        public string cancelReason { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
    public class AliOrderDetail
    {
        /// <summary>
        /// 报价项id
        /// </summary>
        public long QuoteItemId { get; set; }
        /// <summary>
        /// 下单数量
        /// </summary>
        public double ItemCount { get; set; }
    }
}
