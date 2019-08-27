using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Order
{
    /// <summary>
    /// 订单收货信息
    /// </summary>
    public class AliOrderAddress
    {
        //{"address":"'+address+'","areaCode":"330108","fullName":"'+ls_rec+'","mobile":"'+ls_phone+'","phone":"'+ls_tel+'","postCode":"'+ls_post+'"}
        public string address { get; set; }
        public string areaCode { get; set; }
        public string fullName { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string postCode { get; set; }
    }
}
