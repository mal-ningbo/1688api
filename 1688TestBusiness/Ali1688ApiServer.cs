using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.CloudService.Bus.Interface;
using NG3.CloudService.Bus.Models;
using Ali1688Business.Account;
using Ali1688Business.Supplier;
using Ali1688Business.Buyoffer;
using Ali1688Business.Quotation;
using Ali1688Business.Order;
using Ali1688Business.OpBulkSettlement;
using Ali1688Business.BulkSettlementImpl;
using Ali1688Business.Category;
using Ali1688Business.Product;
namespace Ali1688Business
{
    public class Ali1688ApiServer : IBusinessInter
    {
        public bool BusTypeCheck(string busType)
        {
            if (busType == "1688")
                return true;
            return false;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public object DoBusinessWork(string useServer, object busData, CloudBaseData cloudData)
        {
            object result = null;
            AliPubHandle handle = null;
            switch (useServer)
            {
                case "GetSubAccountBindingList"://取1688账号与i6p账号绑定清单
                case "BindSubAccount"://绑定或解绑i6p账号
                case "GetMemberIdsByLoginIds"://通过列表loginIds查询对应的memberId,批量最大数不要超过110个
                case "GetLoginIdsByMemberIds": //通过列表memberId查询对应的loginIds,批量最大数不要超过110个
                case "CreateSubAccount":
                    handle = new AliAccountHandle();
                    break;
                case "GetSupplier"://取1688供应商信息
                case "UpdateSupplierCode"://更新供应商外部编码,用于将i8系统的编码同步到1688
                case "ImportSuppliers"://批量导入1688供应商 添加后供应商需完成账号注册并与公司名绑定才能与买家进行线上采购合作
                    handle = new AliSupplierHandle();
                    break;
                case "PostBuyoffer"://发布询价单
                case "CloseBuyOffer"://关闭询价单
                case "GetBuyoffer"://取1688询价单信息
                    handle = new AliBuyofferHandle();
                    break;
                case "GetQuotationListByBuyOfferId"://根据询价id获取询价单的报价信息
                    handle = new AliQuotationHandle();
                    break;
                case "GetOrder"://根据订单id获取订单的信息
                case "GetOrder2":
                case "QueryOrderDetail": //查询采购订单详情
                case "CreateOrder"://创建询价流程采购单(交易)  需要报价单主键
                case "CreateOrderPayment"://根据订单创建付款单
                case "CreateProcurementOrder"://创建履约订单
                case "ApproveOrder"://采购订单审批
                case "CancelOrder"://取消订单
                case "CreateOrderBySourceId"://根据来源单据创建订单
                    handle = new AliOrderHandle();
                    break;
                case "ReceiveOrder"://确认订单生成收货单
                case "GetOpBulkSettlement"://获取收货单信息
                case "ReceiveGoods"://履约采购订单创建收货单
                case "QueryReceiveGoods":///查询履约采购的收货单
                    handle = new AliOpBulkSettlementHandle();
                    break;
                case "CreateBulkSettlementImpl"://根据收货单子单id创建结算单
                case "CreateSettlementNote": //履约采购的结算
                case "QuerySettlementNote"://履约采购查询结算单
                case "CreatePayNote"://履约采购通过结算创建付款单
                    handle = new AliBulkSettlementImplHandle();
                    break;
                case "AddCategory"://创建1688资源分类
                case "ModifyCategory"://修改1688资源分类
                case "DelCategory"://删除1688资源分类
                case "GetCategoryById"://通过1688主键查找单个资源分类
                case "QueryAllCategory"://查询所有1688资源分类
                    handle = new AliCategoryHandle();
                    break;
                case "AddProduct"://创建1688物料
                case "DelProduct"://根据id删除1688物料
                case "ModifyProduct"://修改1688物料
                case "QueryProduct"://分页查询1688物料  
                case "QueryProductById"://根据1688主键查找物料
                case "QueryProductByCodeList"://精确查询产品列表
                    handle = new AliProductHandle();
                    break;
                case "TestCouldService":
                    return string.Format("云服务通畅;AppKey:{0},Book:{1},IP:{2},Loginer:{3},Rule:{4},TimeStamp:{5}", cloudData.AppKey, cloudData.Book, cloudData.IP, cloudData.Loginer, cloudData.Rule, cloudData.TimeStamp);
                default:
                    throw new Exception(string.Format("'{0}' is invalid", useServer));
            }
            string data = busData.ToString();
            AliDataLog adl = new AliDataLog();
            AliDataAccess ada = new AliDataAccess();
            try
            {

                handle.analyseBussdata(useServer, data);
                result = handle.doPost();
                adl.LogForAliApi(handle, result.ToString(), true);
                ada.SaveBusData(handle, result.ToString());
            }
            catch (Exception e)
            {
                adl.LogForAliApi(handle, "", false, e);
                throw e;
            }

            return result;
        }

    }
}
