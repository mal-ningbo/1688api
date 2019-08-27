using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ali1688Business;
using Ali1688Business.Account;
using Ali1688Business.Supplier;
using Ali1688Business.Buyoffer;
using Ali1688Business.Quotation;
using Ali1688Business.Order;
using Ali1688Business.OpBulkSettlement;
using Ali1688Business.Category;
using Ali1688Business.Product;
using Ali1688Business.Model.Order;
using Ali1688Business.BulkSettlementImpl;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ali1688UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        //app_kapp_key=485676;app_secret=4WSJAzHzqMc;access_token=256e2be5-e1f8-4565-8d35-325fdde69acc
        //485676
        #region 账号相关函数测试
        [TestMethod]
        public void TestGetSubAccountBindingList()
        {
            //try
            //{
            AliAccountHandle account = new AliAccountHandle();
            //"{ \"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"ac9f41e0-1eaf-4899-be1e-1eb273df5358\"}"  0a54c693-6ca2-4765-abdf-c52b768dafbe  b2b-1623492085
            //"{ \"AppKey\":\"485676\",\"AppSecret\":\"4WSJAzHzqMc\",\"TokenKey\":\"256e2be5-e1f8-4565-8d35-325fdde69acc\"}"
            account.analyseBussdata("GetSubAccountBindingList", "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\"}");
            string ret = account.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            //Assert.IsNotNull(obj, "返回值空对象引用");
            //JArray bandingList = obj.Value<JArray>("subAccountBindingList");
            //Assert.IsNotNull(bandingList, "AccountBindingList空对象引用");
            //Assert.IsTrue(bandingList.Count > 0, "未获取账户列表");
            //}
            //catch (Exception e) {
            //    Assert.IsTrue(true, e.ToString());
            //}

        }
        [TestMethod]
        public void TestGetMemberIdsByLoginIds()
        {
            AliAccountHandle account = new AliAccountHandle();
            account.analyseBussdata("GetMemberIdsByLoginIds", "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"ac9f41e0-1eaf-4899-be1e-1eb273df5358\",\"LoginIds\":[\"alitestforisv04\",\"alitestforisv01\"]}");
            string ret = account.doPost();
            Dictionary<string, string> map = JsonConvert.DeserializeObject<Dictionary<string, string>>(ret);
            Assert.IsNotNull(map, "返回值空对象引用");
            Assert.IsTrue(map.Count > 0, "未获取对应关系");
        }
        [TestMethod]
        public void TestGetLoginIdsByMemberIds()
        {
            AliAccountHandle account = new AliAccountHandle();
            account.analyseBussdata("GetLoginIdsByMemberIds", "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"aa3cbd3b-de0c-4779-8596-6724864d7a13\",\"MemberIds\":[\"b2b-1624747073\",\"b2b-1623492085\"]}");
            string ret = account.doPost();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsNotNull(obj, "返回值空对象引用");
            obj = obj.Value<JObject>("loginIdMap");
            Assert.IsNotNull(obj, "未获取对应关系1");
            Assert.IsTrue(obj.Count > 0, "未获取对应关系2");
        }

        [TestMethod]
        public void TestBindSubAccount()
        {

            //注意每个nglogid只能绑定一次,此为1688接口逻辑
            AliDataLog adl = new AliDataLog();
            AliAccountHandle account = new AliAccountHandle();
            try
            {

                account.analyseBussdata("BindSubAccount", "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"aa3cbd3b-de0c-4779-8596-6724864d7a13\",\"SubAccount\":\"alitestforisv04:you\", \"NgLogid\":\"福泽忍\"}");
                string ret = "";// account.doPost();
                adl.LogForAliApi(account, ret.ToString(), true);
                JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
                Assert.IsNotNull(obj, "返回值空对象引用");
                Assert.IsTrue(obj.Value<bool>("result"), "绑定失败");
            }
            catch (Exception e)
            {
                adl.LogForAliApi(account, "", false, e);
                Assert.IsFalse(true, e.Message);
            }

        }
        #endregion
        [TestMethod]
        public void TestPostBuyoffer()
        {
            AliBuyofferHandle buyoffer = new AliBuyofferHandle();
            //string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"0a54c693-6ca2-4765-abdf-c52b768dafbe\",\"subject\":\"test地址4\",\"phone\":\"13978766543\",\"contact\":\"联系人\",\"description\":\"描述1\",\"gmtQuotationExpire\":\"2018-12-12 12:12:12\",\"subBizType\":\"singlepurchase\",\"open\":true,\"openToPortal\":true,\"processTemplateCode\":\"assureTradeBusinessBuy\",\"transToolType\":\"3\",\"invoiceRequirement\":\"vat\",\"visibleAfterEndQuote\":\"false\",\"sourceMethodType\":\"open\",\"ngid\":\"\",\"receiveStreetAddress\":\"古林镇\",\"includeTax\":\"true\",\"quoteHasPostFee\":\"false\",\"allowPartOffer\":\"false\",\"certificateIds\":[\"542\",\"1\",\"2\",\"3\",\"400\",\"548\",\"549\"],\"otherCertificateNames\":[\"合格证\",\"检测报告\"],items:[{\"prItemId\":\"1001\",\"productCode\":\"a001\",\"purchaseAmount\":100,\"subject\":\"资源名称\",\"desc\":\"资源描述\",\"unit\":\"千克\",\"brandName\":\"品牌\",\"modelNumber\":\"型号1\"}]}"; //\"supplierMemberIds\":[\"331010\",\"678908\"],
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"subject\":\"625询价采购测试(201906250001)\",\"description\":\"\",\"contact\":\"唐小云\",\"phone\":\"18371077417\",\"gmtQuotationExpire\":\"2019-06-27 10:59:04\",\"subBizType\":\"singlepurchase\",\"processTemplateCode\":\"accountPeriod\",\"transToolType\":\"3\",\"invoiceRequirement\":\"vat\",\"visibleAfterEndQuote\":\"false\",\"sourceMethodType\":\"open\",\"includeTax\":\"true\",\"quoteHasPostFee\":\"true\",\"allowPartOffer\":\"false\",\"certificateIds\":[\"1\",\"2\"],\"otherCertificateNames\":[\"合格证\",\"检测报告\"],\"ngid\":\"xj23\",\"open\":\"true\",\"openToPortal\":\"true\",\"needSignAgreement\":\"false\",\"subuserId\":\"2201286932668\",\"receiveStreetAddress\":null,\"items\":[{\"prItemId\":\"10013\",\"productCode\":\"20190424-0001\",\"purchaseAmount\":66,\"subject\":\"大砂石\",\"desc\":\" \",\"unit\":\"台\",\"brandName\":\"\",\"modelNumber\":\"\"}]}";
            buyoffer.analyseBussdata("PostBuyoffer", data);
            string ret = buyoffer.doPost();// "{\"id\":123456789}"; //
            //AliDataAccess ada = new AliDataAccess();
            //ada.SaveBusData(buyoffer, ret);
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsNotNull(obj, "返回值空对象引用");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(obj.Value<string>("id")), "发布失败");
        }
        [TestMethod]
        public void TestGetQuotationListByBuyOfferId() {
            AliQuotationHandle handle = new AliQuotationHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"OfferId\":\"319444090727\"}";
            handle.analyseBussdata("GetQuotationListByBuyOfferId", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsTrue(string.IsNullOrWhiteSpace(obj.Value<string>("message")), obj.Value<string>("message"));
        }
        ////{"Appkey":"999922","BusType":"1688","UseServer":"CreateOrder","BusData":"{\"AppKey\":\"485676\",\"AppSecret\":\"4WSJAzHzqMc\",\"TokenKey\":\"256e2be5-e1f8-4565-8d35-325fdde69acc\",\"SubBiz\":\"subBiz\",\"SupplyNoteId\":\"1116408851886\",\"ReceiveAddressGroup\":{\"address\":\"宁波市象山县石浦镇昌国盐桥一桥\",\"areaCode\":\"330108\",\"fullName\":\"\",\"mobile\":\"\",\"phone\":\"\",\"postCode\":\"\"}}"}
        //[TestMethod]
        //public void TestCreateOrder()
        //{
        //    AliOrderHandle order = new AliOrderHandle();
        //    string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"66ff3a91-9580-4baa-b626-6002aabe766c\",\"SubBiz\":\"buyoffer\",\"SupplyNoteId\":\"1113861922028\",\"ReceiveAddressGroup\":{\"address\":\"宁波市象山县石浦镇昌国盐桥一桥\",\"areaCode\":\"330108\",\"fullName\":\"测试\",\"mobile\":\"17283948576\",\"phone\":\"\",\"postCode\":\"\"},\"AliOrderDetails\":[{\"QuoteItemId\":1388074822028,\"ItemCount\":9}]}";
        //    order.analyseBussdata("CreateOrder", data);
        //    string ret = order.doPost();
        //    JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
        //    //Assert.IsNotNull(order.ToString());
        //    Assert.IsNotNull(obj, "返回值空对象引用");
        //    //Assert.IsTrue(!string.IsNullOrWhiteSpace(obj.Value<string>("id")), "发布失败");
        //    obj = obj.Value<JObject>("orderResult");
        //    if (!obj.Value<bool>("success"))
        //    {
        //        throw new Exception("订单创建失败");
        //    }
        //    JArray secbills = obj.Value<JArray>("commitResults");

        //    Assert.IsTrue(!string.IsNullOrWhiteSpace(secbills[0].Value<string>("orderId")), "发布失败");

        //}

        //[TestMethod]
        //public void TestCancelOrder()
        //{
        //    AliOrderHandle order = new AliOrderHandle();
        //    string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"0a54c693-6ca2-4765-abdf-c52b768dafbe\",\"OrderId\":206680598605498520,\"cancelReason\":\"i6p取消核准\",\"remark\":\"i6p取消核准\"}";
        //    order.analyseBussdata("CancelOrder", data);
        //    string ret = order.doPost();
        //    JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
        //    Assert.IsNotNull(obj, "返回值空对象引用");
        //    Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("errorMessage"));

        //}
        #region 履约订单
        [TestMethod]
        public void TestQueryOrderDetail()
        {
            AliOrderHandle order = new AliOrderHandle();
            string data = "{ \"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"OrderId\":\"89275019\"}"; //89139200
            order.analyseBussdata("QueryOrderDetail", data);
            string ret = order.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        private string CreateTestOrder()
        {
            ProcurementOrder order = new ProcurementOrder();
            order.AppKey = "334362";
            order.AppSecret = "w5v9Xu2sK2y5";
            order.TokenKey = "9ee0ac93-0aae-45f5-a671-ca7564280a3a";
            order.CompanyCode = "1000";
            order.OperatorLoginId = "dongyl3333";
            order.SupplierCode = "1000006";
            order.NgPurId = DateTime.Now.Ticks.ToString();
            order.NgPurNo = DateTime.Now.ToLongDateString();
            order.TradeMode = "payperiod";
            order.PaymentMode = "transbybank";
            order.CreatorName = "托狼";
            order.Address = new ProcurementOrderAddress();
            order.Address.Address = "浙江省杭州市滨江区网商路699号";
            order.Address.ReceiverName = "李四";
            order.Address.Post = "315151";
            order.Address.MobilePhone = "13786543897";
            order.NeedSupplierConfirm = false;
            order.Details = new List<ProcurementOrderDetail>();
            order.Details.Add(new ProcurementOrderDetail()
            {
                DeliveryDate = DateTime.Now,
                NgPurDetailId = "3001",
                QuoteDetialId = 1571067420727,
                ResCode = "85000480",
                Description = "资源描述,备注杂七杂八的全放这",
                UnitName = "kg",
                Qty = 10,
                UntaxPrc = 10,
                TaxPrc = 12,
                TaxRate = 13,
                TaxCode = "CN15"
            });
            order.Details.Add(new ProcurementOrderDetail()
            {
                DeliveryDate = DateTime.Now,
                NgPurDetailId = "3002",
                QuoteDetialId = 1571067420728,
                ResCode = "85000481",
                Description = "资源描述,备注杂七杂八的全放这",
                UnitName = "g",
                Qty = 10,
                UntaxPrc = 10,
                TaxPrc = 17,
                TaxRate = 17,
                TaxCode = "CN15"
            });
            order.Details.Add(new ProcurementOrderDetail()
            {
                DeliveryDate = DateTime.Now,
                NgPurDetailId = "3003",
                QuoteDetialId = 1571067420729,
                ResCode = "85000482",
                Description = "资源描述,备注杂七杂八的全放这",
                UnitName = "mg",
                Qty = 100,
                UntaxPrc = 100,
                TaxPrc = 127,
                TaxRate = 17,
                TaxCode = "CN15"
            });
            JsonSerializerSettings set = new JsonSerializerSettings();
            set.DateFormatString = "yyyy-MM-dd hh:mm:ss";
            return JsonConvert.SerializeObject(order, set);
        }
        private string CreateTestOrderFromNg()
        {
            ProcurementOrderFromNg order = new ProcurementOrderFromNg();
            order.AppKey = "334362";
            order.AppSecret = "w5v9Xu2sK2y5";
            order.TokenKey = "9ee0ac93-0aae-45f5-a671-ca7564280a3a";
            order.CompanyCode = "1000";
            order.OperatorLoginId = "dongyl3333";
            order.NgPurId = DateTime.Now.Ticks.ToString();
            order.Address = new ProcurementOrderAddress();
            order.Address.Address = "浙江省杭州市滨江区网商路699号";
            order.Address.ReceiverName = "李四";
            order.Address.Post = "315151";
            order.Address.MobilePhone = "13786543897";
            order.NeedSupplierConfirm = false;
            order.NeedInvoice = false;
            order.QuoteId = 1190909900727;
            order.Details = new List<ProcurementOrderDetail>();
            order.Details.Add(new ProcurementOrderDetail()
            {
                DeliveryDate = DateTime.Now.AddDays(10),
                QuoteDetialId = 1575894180727,
                Qty = 15
            });
            order.Details.Add(new ProcurementOrderDetail()
            {
                DeliveryDate = DateTime.Now.AddDays(10),
                QuoteDetialId = 1575894190727,
                Qty = 15
            });           
            JsonSerializerSettings set = new JsonSerializerSettings();
            set.DateFormatString = "yyyy-MM-dd hh:mm:ss";
            return JsonConvert.SerializeObject(order, set);
        }
        [TestMethod]
        public void TestCreateProcurementOrder()
        {
            AliOrderHandle order = new AliOrderHandle();
            string data = CreateTestOrder();
            order.analyseBussdata("CreateProcurementOrder", data);
            string ret = order.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            obj = obj.Value<JObject>("result");
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        [TestMethod]
        public void TestCreateOrderBySourceId() {
            AliOrderHandle order = new AliOrderHandle();
            string data = CreateTestOrderFromNg();// "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"CompanyCode\":\"1000\",\"NgPurId\":\"318190821000001\",\"NeedSupplierConfirm\":false,\"NeedInvoice\":false,\"OperatorLoginId\":\"dongyl3333:cg000556\",\"QuoteId\":\"1178451390727\",\"Address\":{\"Address\":\"杭州运河\",\"MobilePhone\":\"15478965258\",\"Phone\":\"\",\"Post\":\"\",\"ReceiverName\":\"123\"},\"Details\":[{\"QuoteDetialId\":\"1580108480727\",\"Qty\":12.0,\"DeliveryDate\":null},{\"QuoteDetialId\":\"1580108490727\",\"Qty\":15.0,\"DeliveryDate\":null}]}";// 
            order.analyseBussdata("CreateOrderBySourceId", data);
            string ret = order.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            //obj = obj.Value<JObject>("result");
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        [TestMethod]
        public void TestApproveOrder()
        {
            AliOrderHandle order = new AliOrderHandle();
            string data = "{ \"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"OrderId\":\"88766926\",\"NgPurId\":\"637018103985963096\",\"Status\":\"dismissed\"}";
            order.analyseBussdata("ApproveOrder", data);
            string ret = order.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        #endregion

        #region 收货
        [TestMethod]
        public void TestQueryReceiveGoods() {
            AliOpBulkSettlementHandle handle = new AliOpBulkSettlementHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"Id\":75142}";
            handle.analyseBussdata("QueryReceiveGoods", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        [TestMethod]
        public void TestReceiveGoods() {
            AliOpBulkSettlementHandle handle = new AliOpBulkSettlementHandle();
            string data = "{ \"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"SubAccount\":\"dongyl3333\",\"ReceiveDate\":\"2019-08-28\",\"InsideRemark\":\"memo1\",\"OutsideRemark\":\"memo2\",\"ReceivedDetails\":[{\"EntryId\":89275021,\"realQuantity\":3},{\"EntryId\":89275020,\"realQuantity\":5}]}";
            handle.analyseBussdata("ReceiveGoods", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        #endregion
        #region 结算付款
        /// <summary>
        /// 付款
        /// </summary>
        [TestMethod]
        public void TestCreatePayNote() {
            AliBulkSettlementImplHandle handle = new AliBulkSettlementImplHandle();
            string data = "{ \"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"Amt\":5725,\"LoginId\":\"dongyl3333\",\"PaymentMode\":\"transbybank\",\"RequestNo\":\"zh000001\",\"Details\":[{\"Amt\":5725,\"SettleId\":36521}]}";
            handle.analyseBussdata("CreatePayNote", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        [TestMethod]
        public void TestQuerySettlementNote() {
            AliBulkSettlementImplHandle handle = new AliBulkSettlementImplHandle();
            string data = "{ \"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"Id\":36521}";
            handle.analyseBussdata("QuerySettlementNote", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        [TestMethod]
        public void TestCreateSettlementNote() {
            AliBulkSettlementImplHandle handle = new AliBulkSettlementImplHandle();
            string data = "{ \"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"LoginId\":\"dongyl3333\",\"Memo\":\"请及时开票2\",\"Details\":[{\"RcvEntryId\":346844,\"TaxAmt\":5100,\"Prc\":1700},{\"RcvEntryId\":346845,\"TaxAmt\":625,\"Prc\":125}]}";
            handle.analyseBussdata("CreateSettlementNote", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        #endregion

        #region 类目
        [TestMethod]
        public void TestAddCategory()
        {
            AliCategoryHandle handle = new AliCategoryHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"Categories\":[{\"CategoryName\":\"钢材\",\"CategoryId\":\"ng001\"},{\"CategoryName\":\"槽钢\",\"CategoryId\":\"ng00101\",\"ParentId\":\"ng001\"},{\"CategoryName\":\"角钢\",\"CategoryId\":\"ng00102\",\"ParentId\":\"ng001\"}]}";
            handle.analyseBussdata("AddCategory", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        [TestMethod]
        public void TestQueryAllCategory()
        {
            AliCategoryHandle handle = new AliCategoryHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\"}";
            handle.analyseBussdata("QueryAllCategory", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        [TestMethod]
        public void TestGetCategoryById()
        {
            AliCategoryHandle handle = new AliCategoryHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"CategoryId\":\"365770\"}";
            handle.analyseBussdata("GetCategoryById", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        [TestMethod]
        public void TestDelCategory()
        {
            AliCategoryHandle handle = new AliCategoryHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"DelIds\":[365775,365774]}";
            handle.analyseBussdata("DelCategory", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        [TestMethod]
        public void TestModifyCategory()
        {
            AliCategoryHandle handle = new AliCategoryHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"Categories\":[{\"CategoryName\":\"钢材\",\"CategoryId\":\"365773\"},{\"CategoryName\":\"角钢材\",\"CategoryId\":\"365776\",\"ParentId\":\"365773\"}]}";
            handle.analyseBussdata("ModifyCategory", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        #endregion

        #region 物料
        [TestMethod]
        public void TestAddProduct()
        {
            AliProductHandle handle = new AliProductHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"ProductCode\":\"ZH001\",\"ProductName\":\"商品砼\",\"Unit\":\"件\",\"Remark\":\"说明描述\",\"Instruction\":\"副标题\",\"Price\":999999,\"CategoryId\":365776,\"Attribes\":[{\"name\":\"外置配件\",\"unit\":\"件\",\"values\":[\"红色\",\"黑色\",\"绿色\",\"蓝色\"]},{\"name\":\"附属装备\",\"unit\":\"个\",\"values\":[\"黄色\",\"白色\",\"青灰色\",\"紫色\"]}]}";
            //data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"ProductCode\":\"ZH002\",\"ProductName\":\"商品砼\",\"Unit\":\"件\",\"CategoryId\":365776}";
            handle.analyseBussdata("AddProduct", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        [TestMethod]
        public void TestQueryProductById()
        {
            AliProductHandle handle = new AliProductHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"ProductId\":4926443}";

            handle.analyseBussdata("QueryProductById", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        [TestMethod]
        public void TestDelProductById()
        {
            AliProductHandle handle = new AliProductHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"ProductId\":4926443}";

            handle.analyseBussdata("DelProduct", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        [TestMethod]
        public void TestModifyProduct()
        {
            AliProductHandle handle = new AliProductHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"ProductId\":4926466,\"ProductCode\":\"ZH002\",\"ProductName\":\"商品砼3\",\"Unit\":\"吨\",\"Instruction\":\"副标题2\",\"Price\":888888,\"CategoryId\":365776,\"Attribes\":[{\"name\":\"外置配件\",\"unit\":\"件\",\"values\":[\"红色\",\"黑色\",\"绿色\",\"蓝色\"]},{\"name\":\"附属装备\",\"unit\":\"个\",\"values\":[\"黄色1\",\"白色2\",\"青灰色3\",\"紫色4\"]}]}";
            //data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"ProductCode\":\"ZH002\",\"ProductName\":\"商品砼\",\"Unit\":\"件\",\"CategoryId\":365776}";
            handle.analyseBussdata("ModifyProduct", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        #endregion

        #region 供应商
        [TestMethod]
        public void TestUpdateSupplierCode() {
            AliSupplierHandle handle = new AliSupplierHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"CreditCode\": \"91430121MA4PFPBX71\",\"SupplierCode\": \"100909090\"}";
            handle.analyseBussdata("UpdateSupplierCode", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsTrue(obj.Value<bool>("success"), obj.Value<string>("message"));
        }
        [TestMethod]
        public void TestImportSuppliers() {
            AliSupplierHandle handle = new AliSupplierHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"Suppliers\": [{\"SupplierName\": \"供应商公司999\",\"Email\": \"3264612820@qq.com\",\"Mobile\": \"13000123456\",\"Phone\": \"114\",\"NgId\":\"NGExternalId001\"},{\"SupplierName\": \"供应商公司998\",\"Email\": \"3264612821@qq.com\",\"Mobile\": \"13000123453\",\"Phone\": \"120\",\"NgId\":\"NGExternalId002\"}]}";
            handle.analyseBussdata("ImportSuppliers", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            JArray arr = obj.Value<JArray>("list");
            Assert.IsTrue(arr.Count==0,ret);
        }
        #endregion
        [TestMethod]
        public void TestConfirmGoods()
        {
            AliOpBulkSettlementHandle handle = new AliOpBulkSettlementHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"Code\":\"1001001005\",\"Detials\":[{\"NgPurId\":\"637008587391704596\",\"NgPurDetailId\":\"3001\",\"Qty\":7,\"UntaxPrc\":10,\"UntaxAmt\":70,\"TaxPrc\":12,\"TaxAmt\":84,\"End\":false}]}";
            handle.analyseBussdata("ConfirmGoods", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        [TestMethod]
        public void TestReceiveOrder()
        {
            AliOpBulkSettlementHandle handle = new AliOpBulkSettlementHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"9ee0ac93-0aae-45f5-a671-ca7564280a3a\",\"OrderId\":\"88786898\",\"ReceivedDetails\":[{\"orderEntryId\":\"88786900\",\"quantity\":1,\"realQuantity\":1}]}";
            handle.analyseBussdata("ReceiveOrder", data);
            string ret = handle.Post();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            string error = obj.Value<string>("error_message");
            Assert.IsFalse(!string.IsNullOrWhiteSpace(error), error);
        }
        [TestMethod]
        public void SimpleTest()
        {
            //string dt = "20190331015209000+0800";
            //DateTime dt = DateTime.Now;
            //dt.ToString("yyyyMMddHHmmssfffzz00");
            //DateTime.ParseExact(dt, "yyyyMMddHHmmssfffzz00", null);


        }
        #region 云端存储测试
        [TestMethod]
        public void TestSaveBusData_GetBuyoffer()
        {
            AliBuyofferHandle buyoffer = new AliBuyofferHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"aa3cbd3b-de0c-4779-8596-6724864d7a13\",\"offerid\":\"297650670727\"}";
            buyoffer.analyseBussdata("GetBuyoffer", data);
            string ret = "{\"buyOffer\":{\"processTemplateCode\":\"accountPeriod\",\"attachments\":[],\"contactInfo\":{\"phone\":\"18305811642\",\"contact\":\"小张\"},\"allowPartQuote\":false,\"gmtQuotationExpire\":\"20190331015209000+0800\",\"prId\":\"xj82\",\"isVisibleAfterEndQuote\":false,\"title\":\"330ht(20190330-0001-XJCG)\",\"gmtCreate\":\"20190330135359000+0800\",\"quoteHasPostFee\":true,\"purchaseNoteId\":297650670727,\"includeTax\":true,\"sourceMethodType\":\"selectedmysupplier\",\"subBizType\":\"singlepurchase\",\"onlineShoppingAttachList\":[],\"transToolType\":\"3\",\"openToPortal\":false,\"purchaseNoteItems\":[{\"attachments\":[],\"purchaseCount\":10,\"subject\":\"苹果\",\"purchaseId\":297650670727,\"purchaseNoteItemId\":1132333900728,\"prItemId\":\"10087\",\"brandName\":\"\",\"purchaseAmount\":10,\"productFeature\":\"\",\"purchaseItemSign\":{\"sign\":0,\"addedIntoProductedDeport\":false},\"unit\":\"台\",\"productCode\":\"006001\",\"lineNum\":\"1\",\"modelNumber\":\"\",\"category\":{\"topCategoryId\":130822002,\"topCategoryName\":\"餐饮生鲜\",\"secondCategoryName\":\"水果\",\"thirdCategoryName\":\"苹果\",\"leafCategoryId\":1036642,\"secondCategoryId\":10037,\"categorylist\":[130822002,10037,1036642],\"thirdCategoryId\":1036642,\"leafCategoryName\":\"苹果\"},\"categoryId\":1036642},{\"attachments\":[],\"purchaseCount\":20,\"subject\":\"菠萝\",\"purchaseId\":297650670727,\"purchaseNoteItemId\":1132333910727,\"prItemId\":\"10088\",\"brandName\":\"\",\"purchaseAmount\":50,\"productFeature\":\"\",\"purchaseItemSign\":{\"sign\":0,\"addedIntoProductedDeport\":false},\"unit\":\"台\",\"productCode\":\"006002\",\"lineNum\":\"2\",\"modelNumber\":\"\",\"category\":{\"topCategoryId\":130822002,\"topCategoryName\":\"餐饮生鲜\",\"secondCategoryName\":\"水果\",\"thirdCategoryName\":\"菠萝/凤梨\",\"leafCategoryId\":125062004,\"secondCategoryId\":10037,\"categorylist\":[130822002,10037,125062004],\"thirdCategoryId\":125062004,\"leafCategoryName\":\"菠萝/凤梨\"},\"categoryId\":125062004}],\"invoiceType\":\"vat\",\"subUserId\":2200593250128,\"status\":\"sent\"}}";
            AliDataAccess ada = new AliDataAccess();
            ada.SaveBusData(buyoffer, ret);
        }
        [TestMethod]
        public void TestSaveBusData_PostBuyoffer()
        {
            AliBuyofferHandle buyoffer = new AliBuyofferHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"0a54c693-6ca2-4765-abdf-c52b768dafbe\",\"subject\":\"test地址4\",\"phone\":\"13978766543\",\"contact\":\"联系人\",\"description\":\"描述1\",\"gmtQuotationExpire\":\"2018-12-12 12:12:12\",\"subBizType\":\"singlepurchase\",\"processTemplateCode\":\"assureTradeBusinessBuy\",\"transToolType\":\"3\",\"invoiceRequirement\":\"vat\",\"visibleAfterEndQuote\":\"false\",\"sourceMethodType\":\"open\",\"ngid\":\"\",\"receiveStreetAddress\":\"古林镇\",\"includeTax\":\"true\",\"quoteHasPostFee\":\"false\",\"allowPartOffer\":\"false\",\"certificateIds\":[\"542\",\"1\",\"2\",\"3\",\"400\",\"548\",\"549\"],\"otherCertificateNames\":[\"合格证\",\"检测报告\"],items:[{\"prItemId\":\"1001\",\"productCode\":\"a001\",\"purchaseAmount\":100,\"subject\":\"资源名称\",\"desc\":\"资源描述\",\"unit\":\"千克\",\"brandName\":\"品牌\",\"modelNumber\":\"型号1\"}]}";
            buyoffer.analyseBussdata("PostBuyoffer", data);
            string ret = "{\"id\":297650670727}";//buyoffer.doPost();
            AliDataAccess ada = new AliDataAccess();
            ada.SaveBusData(buyoffer, ret);
        }
        [TestMethod]
        public void TestSaveBusData_GetQuotationListByBuyOfferId()
        {
            AliQuotationHandle quote = new AliQuotationHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"aa3cbd3b-de0c-4779-8596-6724864d7a13\",\"OfferId\":\"297650670727\"}";
            quote.analyseBussdata("GetQuotationListByBuyOfferId", data);
            string ret = "{\"quotationList\":[{\"expireDate\":\"20190531235959000+0800\",\"buyOfferId\":297650670727,\"attachments\":[],\"supplierMemberId\":\"b2b-2248624086\",\"contactInfo\":{\"contact\":\"李工\",\"mobile\":\"18305811673\"},\"invoiceType\":\"common\",\"freight\":0,\"totalPrice\":5000,\"supplyNoteItems\":[{\"subject\":\"苹果\",\"amount\":10,\"unit\":\"台\",\"price\":100,\"prItemId\":\"10087\",\"id\":1495899150729,\"productCode\":\"006001\",\"taxRate\":\"3\",\"itemCount\":10.0,\"brandName\":\"\",\"modelNumber\":\"\"},{\"subject\":\"菠萝蜜\",\"amount\":20,\"unit\":\"台\",\"price\":200,\"prItemId\":\"10088\",\"id\":1495899160727,\"productCode\":\"006002\",\"taxRate\":\"3\",\"itemCount\":20.0,\"brandName\":\"\",\"modelNumber\":\"\"}],\"payRequirementInfo\":{\"type\":\"payperiod\"},\"gmtCreate\":\"20190330135621000+0800\",\"prId\":\"xj82\",\"id\":1165882280727,\"status\":\"sent\"}]}";
            AliDataAccess ada = new AliDataAccess();
            ada.SaveBusData(quote, ret);
        }
        [TestMethod]
        public void TestSaveBusData_GetOrder2()
        {
            AliOrderHandle order = new AliOrderHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"aa3cbd3b-de0c-4779-8596-6724864d7a13\",\"OrderId\":\"271523654409255893\"}";
            order.analyseBussdata("GetOrder2", data);
            string ret = "";
            FileInfo f = new FileInfo(@"E:\1688云端\json1.json");
            using (FileStream fs = f.OpenRead())
            {
                int n;
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] cach = new byte[255];
                    while ((n = fs.Read(cach, 0, cach.Length)) > 0)
                    {
                        ms.Write(cach, 0, n);

                    }
                    ret = System.Text.Encoding.Default.GetString(ms.ToArray());
                    ms.Close();
                }

                fs.Close();
            }

            AliDataAccess ada = new AliDataAccess();
            ada.SaveBusData(order, ret);
        }
        [TestMethod]
        public void TestSaveBusData_GetOpBulkSettlement()
        {
            AliOpBulkSettlementHandle rcv = new AliOpBulkSettlementHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"aa3cbd3b-de0c-4779-8596-6724864d7a13\",\"RcvId\":\"199012010\"}";
            rcv.analyseBussdata("GetOpBulkSettlement", data);
            string ret = "";
            FileInfo f = new FileInfo(@"E:\1688云端\json2.json");
            using (FileStream fs = f.OpenRead())
            {
                int n;
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] cach = new byte[255];
                    while ((n = fs.Read(cach, 0, cach.Length)) > 0)
                    {
                        ms.Write(cach, 0, n);

                    }
                    ret = System.Text.Encoding.Default.GetString(ms.ToArray());
                    ms.Close();
                }

                fs.Close();
            }
            AliDataAccess ada = new AliDataAccess();
            ada.SaveBusData(rcv, ret);
        }
        [TestMethod]
        public void TestSaveBusData_GetSupplier()
        {
            AliSupplierHandle supplier = new AliSupplierHandle();
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"aa3cbd3b-de0c-4779-8596-6724864d7a13\",\"MemberId\":\"b2b-2248624086\"}";
            supplier.analyseBussdata("GetSupplier", data);
            string ret = "";
            FileInfo f = new FileInfo(@"E:\1688云端\json3.json");
            using (FileStream fs = f.OpenRead())
            {
                int n;
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] cach = new byte[255];
                    while ((n = fs.Read(cach, 0, cach.Length)) > 0)
                    {
                        ms.Write(cach, 0, n);

                    }
                    ret = System.Text.Encoding.Default.GetString(ms.ToArray());
                    ms.Close();
                }

                fs.Close();
            }
            AliDataAccess ada = new AliDataAccess();
            ada.SaveBusData(supplier, ret);
        }
        #endregion 
    }
}

