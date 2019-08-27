using Ali1688Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business
{
    /// <summary>
    /// 订单处理
    /// </summary>
    public class OrderHandle
    {
        public string AddOrder(Order oneOrder)
        {
            return "AddOrderOK";
        }
        public string DelOrder(long id)
        {
            return "DelOrderOk";
        }
    }
}
