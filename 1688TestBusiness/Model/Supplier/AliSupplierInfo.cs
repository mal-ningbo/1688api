using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Supplier
{
    /// <summary>
    /// 供应商
    /// </summary>
    public class AliSupplierInfo : AliSecret
    {
        /// <summary>
        /// i8编码
        /// </summary>
        public string SupplierCode { get; set; }
        public string MemberId { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 社会信用代码
        /// </summary>
        public string CreditCode { get; set; }
        /// <summary>
        /// 工商注册号
        /// </summary>
        public string RegNo { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        /// <summary>
        /// i8主键
        /// </summary>
        public string NgId { get; set; }
    }

    public class AliSuppliersImport : AliSecret {
        public List<AliSupplierInfo> Suppliers { get; set; }
    }
}
