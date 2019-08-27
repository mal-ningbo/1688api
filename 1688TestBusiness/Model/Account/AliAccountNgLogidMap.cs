using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Account
{
    /// <summary>
    /// 1699子账户与i6p账号映射关系
    /// </summary>
    public class AliAccountNgLogidMap:AliSecret
    {
        /// <summary>
        /// 1688子账号
        /// </summary>
        public string SubAccount { get; set; }
        /// <summary>
        /// i6p登陆账号
        /// </summary>
        public string NgLogid { get; set; }
    }
}
