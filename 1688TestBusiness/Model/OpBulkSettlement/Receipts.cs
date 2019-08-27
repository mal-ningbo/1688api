using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.OpBulkSettlement
{
    public class Receipt:AliSecret
    {
        /// <summary>
        /// 收货单编码
        /// </summary>
        public string Code { get; set; }
        public List<ReceiptDetial> Detials { get; set; }
    }
    public class ReceiptDetial {
        /// <summary>
        /// i8订单主键
        /// </summary>
        public string NgPurId { get; set; }
        /// <summary>
        /// i8订单明细主键
        /// </summary>
        public string NgPurDetailId { get; set; }
        public decimal Qty { get; set; }
        public decimal UntaxPrc { get; set; }
        public decimal TaxPrc { get; set; }
        public decimal UntaxAmt { get; set; }
        public decimal TaxAmt { get; set; }
        /// <summary>
        /// 是否最后一笔
        /// </summary>
        public bool End { get; set; }
    }
}
