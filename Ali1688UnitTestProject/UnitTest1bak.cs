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
            AliAccountHandle account = new AliAccountHandle();
            //"{ \"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"ac9f41e0-1eaf-4899-be1e-1eb273df5358\"}"  0a54c693-6ca2-4765-abdf-c52b768dafbe  b2b-1623492085
            //"{ \"AppKey\":\"485676\",\"AppSecret\":\"4WSJAzHzqMc\",\"TokenKey\":\"256e2be5-e1f8-4565-8d35-325fdde69acc\"}"
            account.analyseBussdata("GetSubAccountBindingList", "{ \"AppKey\":\"485676\",\"AppSecret\":\"4WSJAzHzqMc\",\"TokenKey\":\"256e2be5-e1f8-4565-8d35-325fdde69acc\"}");
            string ret = account.doPost();
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsNotNull(obj, "返回值空对象引用");
            JArray bandingList = obj.Value<JArray>("subAccountBindingList");
            Assert.IsNotNull(bandingList, "AccountBindingList空对象引用");
            Assert.IsTrue(bandingList.Count > 0, "未获取账户列表");

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
            string data = "{\"AppKey\":\"334362\",\"AppSecret\":\"w5v9Xu2sK2y5\",\"TokenKey\":\"0a54c693-6ca2-4765-abdf-c52b768dafbe\",\"subject\":\"test地址4\",\"phone\":\"13978766543\",\"contact\":\"联系人\",\"description\":\"描述1\",\"gmtQuotationExpire\":\"2018-12-12 12:12:12\",\"subBizType\":\"singlepurchase\",\"open\":true,\"openToPortal\":true,\"processTemplateCode\":\"assureTradeBusinessBuy\",\"transToolType\":\"3\",\"invoiceRequirement\":\"vat\",\"visibleAfterEndQuote\":\"false\",\"sourceMethodType\":\"open\",\"ngid\":\"\",\"receiveStreetAddress\":\"古林镇\",\"includeTax\":\"true\",\"quoteHasPostFee\":\"false\",\"allowPartOffer\":\"false\",\"certificateIds\":[\"542\",\"1\",\"2\",\"3\",\"400\",\"548\",\"549\"],\"otherCertificateNames\":[\"合格证\",\"检测报告\"],items:[{\"prItemId\":\"1001\",\"productCode\":\"a001\",\"purchaseAmount\":100,\"subject\":\"资源名称\",\"desc\":\"资源描述\",\"unit\":\"千克\",\"brandName\":\"品牌\",\"modelNumber\":\"型号1\"}]}"; //\"supplierMemberIds\":[\"331010\",\"678908\"],
            buyoffer.analyseBussdata("PostBuyoffer", data);
            string ret =buyoffer.doPost();// "{\"id\":123456789}"; //
            //AliDataAccess ada = new AliDataAccess();
            //ada.SaveBusData(buyoffer, ret);
            JObject obj = JsonConvert.DeserializeObject<JObject>(ret);
            Assert.IsNotNull(obj, "返回值空对象引用");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(obj.Value<string>("id")), "发布失败");
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
        [TestMethod]
        public void SimpleTest()
        {
            //JObject obj = new JObject();
            //obj.Add("id",302040320727);
            //object s = obj.ToString();
            //obj=JsonConvert.DeserializeObject<JObject>(s.ToString());
            string dt = "20190331015209000+0800";
            DateTime.ParseExact(dt, "yyyyMMddHHmmssfffzz00", null);
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

