using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model
{
    /// <summary>
    /// 订单model
    /// </summary>
    [Serializable]
    public class Order
    {
        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterielCode { get; set; }
        /// <summary>
        /// 加个
        /// </summary>
        public decimal Price { get; set; }
    }
}
