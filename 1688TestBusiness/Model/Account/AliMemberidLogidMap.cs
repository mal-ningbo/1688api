using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Account
{
    /// <summary>
    /// 1688memberid和logid映射关系
    /// 
    /// </summary>
    public class AliMemberidLogidMap:AliSecret
    {
        public List<string> LoginIds { get; set; }
        public List<string> MemberIds { get; set; }
    }
}
