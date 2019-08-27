using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Product
{
    public class UserProduct:AliSecret
    {
        /// <summary>
        /// 1688上的主键
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string Instruction { get; set; }
        /// <summary>
        /// 参考单价(以分为单位,long型)
        /// </summary>
        public long Price { get; set; }
        /// <summary>
        /// 用户在1688的自定义类目id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 操作员在1688上的子帐号ID(可不传)
        /// </summary>
        public long UserId { get; set; }
        public List<ProductAttribe> Attribes { get; set; }
    }

    /// <summary>
    /// 自定义属性
    /// </summary>
    public class ProductAttribe {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public List<string> Values { get; set; }
    }
}
