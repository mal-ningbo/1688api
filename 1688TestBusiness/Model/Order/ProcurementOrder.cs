using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Order
{
    /// <summary>
    /// cn.alibaba.open:alibaba.caigou.procurement.order.createProcurementOrder 用这个
    /// </summary>
    public class ProcurementOrder : AliSecret
    {
        private string _bizScene;
        /// <summary>
        /// 业务场景 默认外部系统接入(outersystem),询报价(quotation),内部商城(mall_requisition)
        /// </summary>
        public string BizScene {
            set { _bizScene = value; }
            get { return string.IsNullOrWhiteSpace(_bizScene) ? "outersystem" : _bizScene; }
        }
        /// <summary>
        /// 1688订单主键
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 买家公司编码,1688上先维护
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 买家公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 来源创建者名称
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 来源创建者USER ID,不知道就用0
        /// </summary>
        public long CreatorUserId { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public ProcurementOrderAddress Address { get; set; }
        /// <summary>
        /// 操作者登陆账号，可以是主账号或者子账号 eg:dongyl3333
        /// </summary>
        public string OperatorLoginId { get; set; }
        /// <summary>
        /// ERP系统供应商编码，需要在供应商库维护编码
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// ERP系统供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 支付方式 alipay（支付宝）,transbybank（银行转账）,mybanketicket（电子承兑汇票）
        /// </summary>
        public string PaymentMode { get; set; }
        /// <summary>
        /// 交易方式 instant（即时到账）,alipay（担保交易）,steppay（分阶段支付）,payperiod（账期支付）
        /// </summary>
        public string TradeMode { get; set; }
        /// <summary>
        /// i8订单主键
        /// </summary>
        public string NgPurId { get; set; }
        /// <summary>
        /// i8订单单据号
        /// </summary>
        public string NgPurNo { get; set; }
        /// <summary>
        /// 是否需要供应商确认
        /// </summary>
        public bool NeedSupplierConfirm { get; set; }
        /// <summary>
        /// 审批结果	同意：approved 拒绝：dismissed 草稿：draft
        /// </summary>
        public string Status { get; set; }

        public List<ProcurementOrderDetail> Details { get; set; }
    }
    /// <summary>
    /// cn.alibaba.open:com.alibaba.procurement.BuyerOrderService.createOrderBySourceId 用这个
    /// </summary>
    public class ProcurementOrderFromNg : AliSecret {
        /// <summary>
        /// 买家公司编码,1688上先维护
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// i8订单主键
        /// </summary>
        public string NgPurId { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public ProcurementOrderAddress Address { get; set; }
        /// <summary>
        /// 是否需要供应商确认
        /// </summary>
        public bool NeedSupplierConfirm { get; set; }
        /// <summary>
        /// 是否需要默认发票
        /// </summary>
        public bool NeedInvoice { get; set; }
        /// <summary>
        /// 操作者登陆账号，可以是主账号或者子账号 eg:dongyl3333
        /// </summary>
        public string OperatorLoginId { get; set; }
        /// <summary>
        /// 订单类型 默认standard
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 来源主键id 1688报价单主键
        /// </summary>
        public long QuoteId { get; set; }
        /// <summary>
        /// 来源类型 默认:询报价
        /// </summary>
        public string SourceType { get; set; }
        /// <summary>
        /// 审批结果	同意：approved 拒绝：dismissed 草稿：draft
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public List<ProcurementOrderDetail> Details { get; set; }
    }

    public class ProcurementOrderAddress
    {
        /// <summary>
        /// 收货地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string Post { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }
    }

    public class ProcurementOrderDetail
    {
        /// <summary>
        /// i8订单明细主键
        /// </summary>
        public string NgPurDetailId { get; set; }
        /// <summary>
        /// 1688报价明细主键
        /// </summary>
        public long QuoteDetialId { get; set; }
        /// <summary>
        /// 物料编码,需要维护1688主数据
        /// </summary>
        public string ResCode { get; set; }
        /// <summary>
        /// 物料描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 计量单位名称
        /// </summary>
        public string UnitName { get; set; }

        public decimal Qty { get; set; }
        public decimal UntaxPrc { get; set; }
        public decimal TaxPrc { get; set; }
        public decimal TaxRate { get; set; }
        /// <summary>
        /// 税码，需要维护主数据
        /// </summary>
        public string TaxCode { get; set; }
        /// <summary>
        /// 交付日期交付日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
    }
}
