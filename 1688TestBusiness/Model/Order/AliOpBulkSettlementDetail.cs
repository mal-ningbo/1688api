using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.OpBulkSettlement
{
    /// <summary>
    /// 收货单确认订单明细
    /// </summary>
    public class AliOpBulkSettlementDetail
    {
        //{"orderEntryId": "88549252424587071","quantity":1,"realQuantity":1}
        public string orderEntryId { get; set; } //1688订单明细主键
        public long EntryId { get; set; }//1688订单明细主键
        public int quantity { get; set; }
        public decimal realQuantity { get; set; }
    }
}
