using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.OpBulkSettlement
{
    /// <summary>
    /// 收货单
    /// </summary>
    public class AliOpBulkSettlement:AliSecret
    {
        public string OrderId { get; set; } //1688订单主键
        public string RcvId { get; set; }//收货单主键
        public long Id { get; set; } //履约收货单主键
        public List<AliOpBulkSettlementDetail> ReceivedDetails { get; set; }

        public string SubAccount { get; set; }      
        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime? ReceiveDate { get; set; }
        /// <summary>
        /// 对内备注
        /// </summary>
        public string InsideRemark { get; set; }
        /// <summary>
        /// 对外备注
        /// </summary>
        public string OutsideRemark { get; set; }
    }
}
