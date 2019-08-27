using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Quotation
{
    /// <summary>
    /// 报价单
    /// </summary>
    public class AliQuotation: AliSecret
    {
        public string OfferId { get; set; } //1688询价单主键
    }
}
