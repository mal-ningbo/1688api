using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Buyoffer
{
    public class AliBuyofferDetail
    {
        //{\"prItemId\":\"1001\",\"productCode\":\"a001\",\"purchaseAmount\":100,\"subject\":\"资源名称\",\"desc\":\"资源描述\",\"unit\":\"千克\",\"brandName\":\"品牌\"}
        public string prItemId { get; set; }
        public string productCode { get; set; }
        public decimal purchaseAmount { get; set; }
        public string subject { get; set; }
        public string desc { get; set; }
        public string unit { get; set; }
        public string brandName { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public string modelNumber { get; set; }
    }
}
