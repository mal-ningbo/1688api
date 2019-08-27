using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ali1688Business.Model;
using Ali1688Business.Model.Supplier;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ali1688Business.Supplier
{
    public class AliSupplierHandle: AliPubHandle
    {
        public override void analyseBussdata(string userServer, string bussdata)
        {
            UserSrvType = userServer;
            BusData=bussdata;
            switch (userServer)
            {
                case "GetSupplier":
                    //取1688供应商信息
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","MemberId":"1688供应商memberid"}
                    SetGetSupplierParams(bussdata);
                    break;
                case "UpdateSupplierCode":
                    //更新供应商外部编码,用于将i8系统的编码同步到1688
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","SupplierName": "湖南凡木家具有限公司","CreditCode": "91430121MA4PFPBX71","SupplierCode": "100909090","RegNo": "430121000244766"}
                    SetUpdateSupplierCodeParams(bussdata);
                    break;
                case "ImportSuppliers":
                    //批量导入1688供应商 添加后供应商需完成账号注册并与公司名绑定才能与买家进行线上采购合作
                    SetImportSuppliersParams(bussdata);
                    break;
                default:
                    UserSrvType = "";
                    throw new Exception(string.Format("{0}业务类型:{1},参数错误", "供应商接口", userServer));
            }
        }

        private void SetGetSupplierParams(string bussdata)
        {
            AliSupplierInfo supplier = JsonConvert.DeserializeObject<AliSupplierInfo>(bussdata);
            Secret = supplier;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("memberId", supplier.MemberId);
            FunName = "cn.alibaba.open/caigou.api.supplier.getSupplier";
        }
        private void SetUpdateSupplierCodeParams(string bussdata) {
            AliSupplierInfo supplier = JsonConvert.DeserializeObject<AliSupplierInfo>(bussdata);
            Secret = supplier;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            JObject param = new JObject();
            if (!string.IsNullOrWhiteSpace(supplier.SupplierName)) param.Add("companyName", supplier.SupplierName);
            if (!string.IsNullOrWhiteSpace(supplier.CreditCode)) param.Add("creditCode", supplier.CreditCode);
            if (!string.IsNullOrWhiteSpace(supplier.RegNo)) param.Add("regNo", supplier.RegNo);
            param.Add("outCode", supplier.SupplierCode);
            UrlParams.Add("updateSupplierBuyerParam", JsonConvert.SerializeObject(param));
            FunName = "cn.alibaba.open/caigou.supplier.ocean.updateSupplierOutCode";
        }
        private void SetImportSuppliersParams(string bussdata) {
            AliSuppliersImport suppliers = JsonConvert.DeserializeObject<AliSuppliersImport>(bussdata);
            Secret = suppliers;
            JArray arr = new JArray();
            if (suppliers.Suppliers != null) {
                JObject obj = null;
                foreach (AliSupplierInfo item in suppliers.Suppliers) {
                    obj = new JObject();
                    if (!string.IsNullOrWhiteSpace(item.MemberId)) obj.Add("supplierMemberId", item.MemberId);
                    if (!string.IsNullOrWhiteSpace(item.SupplierName)) obj.Add("supplierCompanyName", item.SupplierName);
                    if (!string.IsNullOrWhiteSpace(item.Email)) obj.Add("supplierEmail", item.Email);
                    if (!string.IsNullOrWhiteSpace(item.Mobile)) obj.Add("supplierMobile", item.Mobile);
                    if (!string.IsNullOrWhiteSpace(item.Phone)) obj.Add("supplierPhone", item.Phone);
                    if (!string.IsNullOrWhiteSpace(item.NgId))  obj.Add("externalId", item.NgId);
                    arr.Add(obj);
                }
            }
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("list", JsonConvert.SerializeObject(arr));
            FunName = "cn.alibaba.open/caigou.api.supplier.import";
        }
    }
}
