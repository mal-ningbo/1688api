using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Category
{
    public class UserCategory: AliSecret
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 本级编码
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 上级编码
        /// </summary>
        public string ParentId { get; set; }

    }

    public class CategoryList : AliSecret
    {
        public List<UserCategory> Categories { get; set; }
        public List<long> DelIds { get; set; }
    }
}
