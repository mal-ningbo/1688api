using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.Data.Service;
using Ali1688Business.Model.Buyoffer;
using Ali1688Business.Model.Quotation;
using Ali1688Business.Model.Order;
using Ali1688Business.Model.OpBulkSettlement;
using Ali1688Business.Model.Supplier;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Ali1688Business
{
    public class AliDataAccess
    {
        private string _dbcon;
        public string Dbcon
        {
            get { return _dbcon; }
        }
        public AliDataAccess()
        {
            try
            {
                string dbtype;
                Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");
                ConnectionStringSettingsCollection consetting = config.ConnectionStrings.ConnectionStrings;
                if (consetting["1688BusinessConnection"] == null)
                {
                    _dbcon = consetting["DefaultConnection"].ConnectionString;
                    dbtype = consetting["DefaultConnection"].ProviderName;
                }
                else
                {
                    _dbcon = consetting["1688BusinessConnection"].ConnectionString;
                    dbtype = consetting["1688BusinessConnection"].ProviderName;
                }
                if (dbtype == "System.Data.SqlClient")
                {
                    _dbcon = "ConnectType=SqlClient;" + _dbcon;
                }
                else
                {
                    _dbcon = "ConnectType=ORACLEClient;" + _dbcon;
                    //Data Source=10.0.13.145:1521/oracleup.rd.ngsoft.com;User Id=NG0001;Password=123
                }

                //Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");
                //_dbcon = config.ConnectionStrings.ConnectionStrings["DefaultConnection"].ConnectionString;
                //string dbtype = config.ConnectionStrings.ConnectionStrings["DefaultConnection"].ProviderName;
                //if (dbtype == "System.Data.SqlClient")
                //{
                //    _dbcon = "ConnectType=SqlClient;" + _dbcon;
                //}
            }
            catch
            {
                _dbcon = "ConnectType=SqlClient;Server=127.0.0.1;Database=NG3CloudService;User ID=sa;Password=newgrand-2016";  //测试用，正式部署时再改为ConnectType=SqlClient;Server=127.0.0.1;Database=NG3CloudService;User ID=sa;Password=newgrand-2016
            }
        }
        //private string VarToSql(string val)
        //{
        //    if (val == null) return "NULL";
        //    return string.Format("'{0}'", val);
        //}
        //private string VarToSql(bool val)
        //{
        //    if (val) return "1";
        //    return "0";
        //}
        //private string VarToSql(DateTime val, bool isoracle = false)
        //{

        //    if (isoracle)
        //    {
        //        if (val.Equals(default(DateTime)))
        //        {
        //            return "NULL";
        //        }
        //        else
        //        {
        //            return string.Format("to_date('{0}','yyyy-MM-dd hh24:mi:ss')",
        //                val.ToString("yyyy-MM-dd HH:mm:ss"));
        //        }
        //    }
        //    else
        //    {
        //        if (val.Equals(default(DateTime)))
        //        {
        //            return "NULL";
        //        }
        //        else
        //        {
        //            return string.Format("'{0}'", val.ToString("yyyy-MM-dd HH:mm:ss"));
        //        }
        //    }
        //}
        ///// <summary>
        ///// 生成insert语句
        ///// </summary>
        ///// <param name="tablename">表名</param>
        ///// <param name="colvals">(列名-值)对</param>
        ///// <returns></returns>
        //private string CreateInsertSql(string tablename, Dictionary<string, string> colvals)
        //{
        //    if (colvals.Count == 0)
        //    {
        //        throw new Exception(string.Format("{0}缺少插入的列", tablename));
        //    }
        //    List<string> cols = new List<string>();
        //    List<string> vals = new List<string>();
        //    foreach (KeyValuePair<string, string> item in colvals)
        //    {
        //        cols.Add(item.Key);
        //        if (item.Value == null)
        //        {
        //            vals.Add("NULL");
        //        }
        //        else
        //        {
        //            vals.Add(item.Value);
        //        }
        //    }

        //    string sql = string.Format("insert into {0} ({1}) values({2})", tablename, string.Join(",", cols.ToArray()), string.Join(",", vals.ToArray()));
        //    return sql;

        //}
        private void ErrorLogForCloudDac(AliPubHandle handle, Exception msg)
        {
            try
            {
                string dir = Path.GetDirectoryName(new Uri(this.GetType().Assembly.CodeBase).LocalPath);
                string file = Path.Combine(dir, handle.Secret.AppKey + "_" + DateTime.Today.ToShortDateString().Replace('/', '_') + "Dac.txt");
                FileInfo f = new FileInfo(file);
                using (StreamWriter sw = f.Exists ? f.AppendText() : f.CreateText())
                {
                    sw.WriteLine(string.Format("{0}\r\n{1} {2} {3} error:{4}\r\nbusata:{5}\r\n", DateTime.Now, handle.Secret.AppKey, handle.Secret.AppSecret, handle.Secret.TokenKey, msg, handle.BusData));
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
                //任何异常不处理，不抛出
            }
        }

        public void SaveBusData(AliPubHandle handle, string result)
        {
            try
            {
                switch (handle.UserSrvType)
                {
                    case "GetSubAccountBindingList"://取1688账号与i6p账号绑定清单
                        break;
                    case "BindSubAccount"://绑定或解绑i6p账号
                        break;
                    case "GetMemberIdsByLoginIds"://通过列表loginIds查询对应的memberId,批量最大数不要超过110个
                        break;
                    case "GetLoginIdsByMemberIds": //通过列表memberId查询对应的loginIds,批量最大数不要超过110个
                        break;
                    case "CreateSubAccount":
                        break;
                    case "GetSupplier"://取1688供应商信息
                        //供应商对照及根据询价id获取询价单的报价信息时会用到
                        SaveGetSupplier(handle,result);
                        break;
                    case "PostBuyoffer"://发布询价单
                        //发布成功后在使用GetBuyoffer方法获取询价单详情，留存此详情信息
                        SavePostBuyoffer(handle, result);
                        break;
                    case "CloseBuyOffer"://关闭询价单
                        break;
                    case "GetBuyoffer"://取1688询价单信息
                        //在获取询价单的报价信息前夕会调用，时询价单的最终信息
                        SaveGetBuyoffer(handle, result);
                        break;
                    case "GetQuotationListByBuyOfferId"://根据询价id获取询价单的报价信息
                        SaveGetQuotationListByBuyOfferId(handle, result);
                        break;
                    case "GetOrder"://根据订单id获取订单的信息
                        //此方法用于服务端轮询，获取订单状态，不必留存数据，否则数据库访问过于频繁
                        break;
                    case "GetOrder2":
                        //此方法用于创建收货单前夕获取订单详细信息，是订单的最终信息
                        SaveGetOrder2(handle, result);
                        break;
                    case "CreateOrder"://创建询价流程采购单  需要报价单主键
                        //发布成功后在使用GetOrder2方法获取订单详情，留存此详情信息
                        SaveCreateOrder(handle, result);
                        break;
                    case "CreateOrderPayment"://根据订单创建付款单
                        break;
                    case "CancelOrder"://取消订单
                        break;
                    case "ReceiveOrder"://确认订单生成收货单
                        break;
                    case "GetOpBulkSettlement"://获取收货单信息
                        //根据收货单创建结算单时调用
                        SaveGetOpBulkSettlement(handle, result);
                        break;
                    case "CreateBulkSettlementImpl"://根据收货单子单id创建结算单               
                        break;
                    default:
                        return;
                }
            }
            catch (Exception e)
            {
                //记录异常 但不抛出
                ErrorLogForCloudDac(handle, e);
            }
        }
        /// <summary>
        /// 保存推送1688询价单的数据
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="result">1688返回主键</param>
        private void SavePostBuyoffer(AliPubHandle handle, string result)
        {
            ///调用getbuyoffer接口获取刚发布成功的报价单信息，存入数据库
            JObject ret = JsonConvert.DeserializeObject<JObject>(result);
            string offerid = ret.Value<string>("id");
            if (string.IsNullOrWhiteSpace(offerid)) return; //没有发布成功
            AliBuyoffer buyoffer = (AliBuyoffer)handle.Secret;
            JObject obj = new JObject();
            obj.Add("AppKey", buyoffer.AppKey);
            obj.Add("AppSecret", buyoffer.AppSecret);
            obj.Add("TokenKey", buyoffer.TokenKey);
            obj.Add("offerid", offerid);
            handle.analyseBussdata("GetBuyoffer", JsonConvert.SerializeObject(obj));
            string content = handle.doPost();
            //测试数据"{\"buyOffer\":{\"processTemplateCode\":\"accountPeriod\",\"attachments\":[],\"contactInfo\":{\"phone\":\"18305811642\",\"contact\":\"小张\"},\"allowPartQuote\":false,\"gmtQuotationExpire\":\"20190331015209000+0800\",\"prId\":\"xj82\",\"isVisibleAfterEndQuote\":false,\"title\":\"330ht(20190330-0001-XJCG)\",\"gmtCreate\":\"20190330135359000+0800\",\"quoteHasPostFee\":true,\"purchaseNoteId\":297650670727,\"includeTax\":true,\"sourceMethodType\":\"selectedmysupplier\",\"subBizType\":\"singlepurchase\",\"onlineShoppingAttachList\":[],\"transToolType\":\"3\",\"openToPortal\":false,\"purchaseNoteItems\":[{\"attachments\":[],\"purchaseCount\":10,\"subject\":\"苹果\",\"purchaseId\":297650670727,\"purchaseNoteItemId\":1132333900728,\"prItemId\":\"10087\",\"brandName\":\"\",\"purchaseAmount\":10,\"productFeature\":\"\",\"purchaseItemSign\":{\"sign\":0,\"addedIntoProductedDeport\":false},\"unit\":\"台\",\"productCode\":\"006001\",\"lineNum\":\"1\",\"modelNumber\":\"\",\"category\":{\"topCategoryId\":130822002,\"topCategoryName\":\"餐饮生鲜\",\"secondCategoryName\":\"水果\",\"thirdCategoryName\":\"苹果\",\"leafCategoryId\":1036642,\"secondCategoryId\":10037,\"categorylist\":[130822002,10037,1036642],\"thirdCategoryId\":1036642,\"leafCategoryName\":\"苹果\"},\"categoryId\":1036642},{\"attachments\":[],\"purchaseCount\":20,\"subject\":\"菠萝\",\"purchaseId\":297650670727,\"purchaseNoteItemId\":1132333910727,\"prItemId\":\"10088\",\"brandName\":\"\",\"purchaseAmount\":50,\"productFeature\":\"\",\"purchaseItemSign\":{\"sign\":0,\"addedIntoProductedDeport\":false},\"unit\":\"台\",\"productCode\":\"006002\",\"lineNum\":\"2\",\"modelNumber\":\"\",\"category\":{\"topCategoryId\":130822002,\"topCategoryName\":\"餐饮生鲜\",\"secondCategoryName\":\"水果\",\"thirdCategoryName\":\"菠萝/凤梨\",\"leafCategoryId\":125062004,\"secondCategoryId\":10037,\"categorylist\":[130822002,10037,125062004],\"thirdCategoryId\":125062004,\"leafCategoryName\":\"菠萝/凤梨\"},\"categoryId\":125062004}],\"invoiceType\":\"vat\",\"subUserId\":2200593250128,\"status\":\"sent\"}}";
            SaveGetBuyoffer(handle, content);

        }
        /// <summary>
        /// 保存获取的1688询价单的数据
        /// 修改,删除，新增
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="result">1688返回的询价单内容</param>
        /// result格式:
        ///         {
        ///  "buyOffer": {
        ///    "processTemplateCode": "accountPeriod",
        ///    "attachments": [],
        ///    "contactInfo": {
        ///      "phone": "18305811642",
        ///      "contact": "小张"
        ///    },
        ///    "allowPartQuote": false,
        ///    "gmtQuotationExpire": "20190331015209000+0800",
        ///    "prId": "xj82",
        ///    "isVisibleAfterEndQuote": false,
        ///    "title": "330ht(20190330-0001-XJCG)",
        ///    "gmtCreate": "20190330135359000+0800",
        ///    "quoteHasPostFee": true,
        ///    "purchaseNoteId": 297650670727,
        ///    "includeTax": true,
        ///    "sourceMethodType": "selectedmysupplier",
        ///    "subBizType": "singlepurchase",
        ///    "onlineShoppingAttachList": [],
        ///    "transToolType": "3",
        ///    "openToPortal": false,
        ///    "purchaseNoteItems": [
        ///      {
        ///        "attachments": [],
        ///        "purchaseCount": 10,
        ///        "subject": "苹果",
        ///        "purchaseId": 297650670727,
        ///        "purchaseNoteItemId": 1132333900727,
        ///        "prItemId": "10087",
        ///        "brandName": "",
        ///        "purchaseAmount": 10,
        ///        "productFeature": " ",
        ///        "purchaseItemSign": {
        ///          "sign": 0,
        ///          "addedIntoProductedDeport": false
        ///        },
        ///        "unit": "台",
        ///        "productCode": "006001",
        ///        "lineNum": "1",
        ///        "modelNumber": "",
        ///        "category": {
        ///          "topCategoryId": 130822002,
        ///          "topCategoryName": "餐饮生鲜",
        ///          "secondCategoryName": "水果",
        ///          "thirdCategoryName": "苹果",
        ///          "leafCategoryId": 1036642,
        ///          "secondCategoryId": 10037,
        ///          "categorylist": [
        ///            130822002,
        ///            10037,
        ///            1036642
        ///          ],
        ///          "thirdCategoryId": 1036642,
        ///          "leafCategoryName": "苹果"
        ///        },
        ///        "categoryId": 1036642
        ///      },
        ///      {
        ///        "attachments": [],
        ///        "purchaseCount": 20,
        ///        "subject": "菠萝",
        ///        "purchaseId": 297650670727,  //表头主键
        ///        "purchaseNoteItemId": 1132333910727,  //明细主键
        ///        "prItemId": "10088",
        ///        "brandName": "",
        ///        "purchaseAmount": 20,
        ///        "productFeature": " ",
        ///        "purchaseItemSign": {
        ///          "sign": 0,
        ///          "addedIntoProductedDeport": false
        ///        },
        ///        "unit": "台",
        ///        "productCode": "006002",
        ///        "lineNum": "2",
        ///        "modelNumber": "",
        ///        "category": {
        ///          "topCategoryId": 130822002,
        ///          "topCategoryName": "餐饮生鲜",
        ///          "secondCategoryName": "水果",
        ///          "thirdCategoryName": "菠萝/凤梨",
        ///          "leafCategoryId": 125062004,
        ///          "secondCategoryId": 10037,
        ///          "categorylist": [
        ///            130822002,
        ///            10037,
        ///            125062004
        ///          ],
        ///          "thirdCategoryId": 125062004,
        ///          "leafCategoryName": "菠萝/凤梨"
        ///        },
        ///        "categoryId": 125062004
        ///      }
        ///    ],
        ///    "invoiceType": "vat",
        ///    "subUserId": 2200593250128,
        ///    "status": "sent"
        ///  }
        ///}
        private void SaveGetBuyoffer(AliPubHandle handle, string result)
        {
            //返回数据
            JObject offer = JsonConvert.DeserializeObject<JObject>(result);
            offer = offer.Value<JObject>("buyOffer");
            if (offer == null) return;//接口返回错误
            AliBuyoffer buyoffer = (AliBuyoffer)handle.Secret;
            DataTable mst = DbHelper.GetDataTable(_dbcon, string.Format("select * from alibuyoffermst where appkey='{0}' and appsecret='{1}' and apptoken='{2}' and offerid='{3}'", buyoffer.AppKey, buyoffer.AppSecret, buyoffer.TokenKey, buyoffer.offerid));
            DataTable dtls = DbHelper.GetDataTable(_dbcon, string.Format("select * from alibuyofferdtl where appkey='{0}' and appsecret='{1}' and apptoken='{2}' and offerid='{3}'", buyoffer.AppKey, buyoffer.AppSecret, buyoffer.TokenKey, buyoffer.offerid));
            DataRow dr;
            JObject tmpobj;

            JArray items = offer.Value<JArray>("purchaseNoteItems");//明细部分

            //处理表头
            if (mst.Rows.Count == 0)
            {
                //不存在则新增
                dr = mst.NewRow();
                dr["appkey"] = buyoffer.AppKey;
                dr["appsecret"] = buyoffer.AppSecret;
                dr["apptoken"] = buyoffer.TokenKey;
                dr["offerid"] = buyoffer.offerid;
            }
            else
            {
                dr = mst.Rows[0];
            }
            dr["subject"] = offer.Value<string>("title");
            dr["description"] = offer.Value<string>("description"); //1688接口说明中有此属性
            tmpobj = offer.Value<JObject>("contactInfo");
            if (tmpobj != null)
            {
                dr["contact"] = tmpobj.Value<string>("contact");
                dr["phone"] = tmpobj.Value<string>("phone");
            }
            dr["gmtquotationexpire"] = DateTime.ParseExact(offer.Value<string>("gmtQuotationExpire"), "yyyyMMddHHmmssfffzz00", null);
            dr["subbizType"] = offer.Value<string>("subBizType");
            dr["processtemplatecode"] = offer.Value<string>("processTemplateCode");
            dr["transtooltype"] = offer.Value<string>("transToolType");
            dr["invoicerequirement"] = offer.Value<string>("invoiceType");
            dr["visibleafterendquote"] = offer.Value<bool>("isVisibleAfterEndQuote") ? 1 : 0;
            dr["sourcemethodtype"] = offer.Value<string>("sourceMethodType");
            dr["processtemplatecode"] = offer.Value<string>("processTemplateCode");
            dr["includetax"] = offer.Value<bool>("includeTax") ? 1 : 0;
            dr["quotehaspostfee"] = offer.Value<bool>("quoteHasPostFee") ? 1 : 0;
            dr["allowpartoffer"] = offer.Value<bool>("allowPartQuote") ? 1 : 0;
            dr["subuserid"] = offer.Value<string>("subUserId");
            dr["ngid"] = offer.Value<string>("prId");
            dr["status"] = offer.Value<string>("status");
            dr["receivestreetaddress"] = offer.Value<string>("receiveStreetAddress");//1688接口说明中有此属性  
            dr["gmtcreate"] = DateTime.ParseExact(offer.Value<string>("gmtCreate"), "yyyyMMddHHmmssfffzz00", null);
            if (mst.Rows.Count == 0)
            {
                mst.Rows.Add(dr);
            }
            //处理表体
            //List<string> delSqls = new List<string>();
            bool exists = false;
            foreach (DataRow r in dtls.Rows)
            {
                //datatable中的数据若存在于返回内容中则修改
                //若不存在则删除
                exists = false;
                foreach (JToken item in items)
                {
                    if (item.Value<string>("purchaseNoteItemId") == r.Field<string>("purchasenoteitemid"))
                    {
                        exists = true;
                        r["pritemid"] = item.Value<string>("prItemId");
                        r["productcode"] = item.Value<string>("productCode");
                        r["purchaseamount"] = item.Value<decimal>("purchaseAmount");
                        r["subject"] = item.Value<string>("subject");
                        r["descript"] = item.Value<string>("productFeature");
                        r["unit"] = item.Value<string>("unit");
                        r["brandname"] = item.Value<string>("brandName");
                        r["modelnumber"] = item.Value<string>("modelNumber");
                        break;
                    }
                }
                if (!exists)
                {
                    r.Delete();
                }
            }
            foreach (JToken item in items)
            {
                //若返回数据不存在于datatable则新增
                if (dtls.Select(string.Format("purchasenoteitemid='{0}'", item.Value<string>("purchaseNoteItemId"))).Length == 0)
                {
                    dr = dtls.NewRow();
                    dr["appkey"] = buyoffer.AppKey;
                    dr["appsecret"] = buyoffer.AppSecret;
                    dr["apptoken"] = buyoffer.TokenKey;
                    dr["offerid"] = buyoffer.offerid;
                    dr["purchasenoteitemid"] = item.Value<string>("purchaseNoteItemId");
                    dr["pritemid"] = item.Value<string>("prItemId");
                    dr["productcode"] = item.Value<string>("productCode");
                    dr["purchaseamount"] = item.Value<decimal>("purchaseAmount");
                    dr["subject"] = item.Value<string>("subject");
                    dr["descript"] = item.Value<string>("productFeature");
                    dr["unit"] = item.Value<string>("unit");
                    dr["brandname"] = item.Value<string>("brandName");
                    dr["modelnumber"] = item.Value<string>("modelNumber");
                    dtls.Rows.Add(dr);
                }
            }

            //更新数据
            DbHelper.Open(_dbcon);
            try
            {
                DbHelper.BeginTran(_dbcon);
                if (DbHelper.Update(_dbcon, mst, "select * from alibuyoffermst") > 0)
                {
                    if (DbHelper.Update(_dbcon, dtls, "select * from alibuyofferdtl") > 0)
                    {
                        DbHelper.CommitTran(_dbcon);
                    }
                    else
                    {
                        throw new Exception("明细alibuyofferdtl更新失败");
                    }
                }
                else
                {
                    throw new Exception("表头alibuyoffermst更新失败");
                }
            }
            catch (Exception e)
            {
                DbHelper.RollbackTran(_dbcon);
                throw e;
            }
            finally
            {
                DbHelper.Close(_dbcon);
            }
        }
        /// <summary>
        /// 保存1688报价信息
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="result"></param>
        /// result格式如下
        /// {
        /// "quotationList": [
        /// {
        ///   "expireDate": "20190531235959000+0800",
        /// "buyOfferId": 297650670727,
        /// "attachments": [],
        ///"supplierMemberId": "b2b-2248624086",
        /// "contactInfo": {
        ///        "contact": "张工",
        ///"mobile": "18305811673"
        ///},
        ///"invoiceType": "common",
        ///"freight": 0,
        ///"totalPrice": 5000,
        ///"supplyNoteItems": [
        ///{
        ///"subject": "苹果",
        ///"amount": 10,
        ///"unit": "台",
        ///"price": 100,
        ///"prItemId": "10087",
        ///"id": 1495899150727, //明细主键
        ///"productCode": "006001",
        ///"taxRate": "3",
        ///"itemCount": 10.0,
        ///"brandName": "",
        ///"modelNumber": ""
        ///},
        ///{
        ///"subject": "菠萝",
        ///"amount": 20,
        ///"unit": "台",
        ///"price": 200,
        ///"prItemId": "10088",
        ///"id": 1495899160727,
        ///"productCode": "006002",
        ///"taxRate": "3",
        ///"itemCount": 20.0,
        ///"brandName": "",
        ///"modelNumber": ""
        ///}
        ///],
        ///"payRequirementInfo": {
        ///"type": "payperiod"
        ///},
        ///"gmtCreate": "20190330135621000+0800",
        ///"prId": "xj82",
        ///"id": 1165882280727,  //单据主键
        ///"status": "sent"
        ///}
        ///]
        ///}
        private void SaveGetQuotationListByBuyOfferId(AliPubHandle handle, string result)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(result);
            JObject tmpobj;
            JArray quotaArr = obj.Value<JArray>("quotationList");
            if (quotaArr == null) return;//接口返回错误
            if (quotaArr.Count == 0) return; //无报价信息            
            AliQuotation quote = (AliQuotation)handle.Secret;
            DataTable msts = DbHelper.GetDataTable(_dbcon, string.Format("select * from quotation where appkey='{0}' and appsecret='{1}' and apptoken='{2}' and buyofferid='{3}'", quote.AppKey, quote.AppSecret, quote.TokenKey, quote.OfferId));
            DataTable dtls = DbHelper.GetDataTable(_dbcon, string.Format("select * from quotationdtl where exists(select 1 from quotation where quotation.quoteid=quotationdtl.quoteid and" +
                " quotation.appkey = quotationdtl.appkey and quotation.appsecret = quotationdtl.appsecret and quotation.apptoken = quotationdtl.apptoken and quotation.buyofferid='{3}')" +
                " and appkey='{0}' and appsecret='{1}' and apptoken='{2}'", quote.AppKey, quote.AppSecret, quote.TokenKey, quote.OfferId));

            bool exists = false, dexists = false;
            JArray quotaItems;
            DataRow[] details;
            DataRow newrow;
            foreach (DataRow dr in msts.Rows)
            {
                //主表数据若存在于返回内容中则修改
                //若不存在则删除
                exists = false;
                foreach (JToken item in quotaArr)
                {
                    if (item.Value<string>("id") == dr.Field<string>("quoteid"))
                    {
                        exists = true;
                        dr["ngid"] = item.Value<string>("prId");
                        dr["status"] = item.Value<string>("status");
                        dr["gmtcreate"] = DateTime.ParseExact(item.Value<string>("gmtCreate"), "yyyyMMddHHmmssfffzz00", null);
                        dr["expiredate"] = DateTime.ParseExact(item.Value<string>("expireDate"), "yyyyMMddHHmmssfffzz00", null);
                        dr["buyofferid"] = item.Value<string>("buyOfferId");
                        dr["suppliermemberid"] = item.Value<string>("supplierMemberId");
                        tmpobj = item.Value<JObject>("contactInfo");
                        if (tmpobj != null)
                        {
                            dr["contact"] = tmpobj.Value<string>("contact");
                            dr["mobile"] = tmpobj.Value<string>("mobile");
                        }

                        dr["invoicetype"] = item.Value<string>("invoiceType");
                        dr["freight"] = item.Value<long>("freight");
                        dr["totalprice"] = item.Value<long>("totalPrice");
                        tmpobj = item.Value<JObject>("payRequirementInfo");
                        if (tmpobj != null)
                        {
                            dr["paytype"] = tmpobj.Value<string>("type");
                        }
                        //处理明细
                        quotaItems = item.Value<JArray>("supplyNoteItems");
                        details = dtls.Select(string.Format("quoteid='{0}'", dr.Field<string>("quoteid")));
                        foreach (DataRow r in details)
                        {
                            //datatable中的数据若存在于返回内容中则修改
                            //若不存在则删除
                            dexists = false;
                            foreach (JToken d in quotaItems)
                            {
                                if (d.Value<string>("id") == r.Field<string>("quotedid"))
                                {
                                    dexists = true;
                                    r["pritemid"] = d.Value<string>("prItemId");
                                    r["productcode"] = d.Value<string>("productCode");
                                    r["amount"] = d.Value<int>("amount");
                                    r["subject"] = d.Value<string>("subject");
                                    r["price"] = d.Value<long>("price");
                                    r["unit"] = d.Value<string>("unit");
                                    r["itemcount"] = d.Value<decimal>("itemCount");
                                    r["brandname"] = d.Value<string>("brandName");
                                    r["modelnumber"] = d.Value<string>("modelNumber");
                                    r["taxrate"] = d.Value<string>("taxRate");
                                    break;
                                }
                            }
                            if (!dexists)
                            {
                                r.Delete();
                            }
                        }
                        //返回的内容不存在于datatable中
                        foreach (JToken d in quotaItems)
                        {
                            if (dtls.Select(string.Format("quoteid='{0}' and quotedid='{1}'", dr.Field<string>("quoteid"), d.Value<string>("id"))).Length == 0)
                            {
                                newrow = dtls.NewRow();
                                newrow["appkey"] = quote.AppKey;
                                newrow["appsecret"] = quote.AppSecret;
                                newrow["apptoken"] = quote.TokenKey;
                                newrow["quoteid"] = dr.Field<string>("quoteid");
                                newrow["quotedid"] = d.Value<string>("id");
                                newrow["pritemid"] = d.Value<string>("prItemId");
                                newrow["productcode"] = d.Value<string>("productCode");
                                newrow["amount"] = d.Value<int>("amount");
                                newrow["subject"] = d.Value<string>("subject");
                                newrow["price"] = d.Value<long>("price");
                                newrow["unit"] = d.Value<string>("unit");
                                newrow["itemcount"] = d.Value<decimal>("itemCount");
                                newrow["brandname"] = d.Value<string>("brandName");
                                newrow["modelnumber"] = d.Value<string>("modelNumber");
                                newrow["taxrate"] = d.Value<string>("taxRate");
                                dtls.Rows.Add(newrow);
                            }
                        }
                        break;
                    }
                }
                if (!exists)
                {
                    //删除明细
                    details = dtls.Select(string.Format("quoteid='{0}'", dr.Field<string>("quoteid")));
                    foreach (DataRow r in details)
                    {
                        r.Delete();
                    }
                    //删除表头
                    dr.Delete();
                }
            }

            //返回的内容不存在于datatable中
            foreach (JToken item in quotaArr)
            {
                if (msts.Select(string.Format("quoteid='{0}'", item.Value<string>("id"))).Length == 0)
                {
                    newrow = msts.NewRow();
                    newrow["appkey"] = quote.AppKey;
                    newrow["appsecret"] = quote.AppSecret;
                    newrow["apptoken"] = quote.TokenKey;
                    newrow["quoteid"] = item.Value<string>("id");
                    newrow["ngid"] = item.Value<string>("prId");
                    newrow["status"] = item.Value<string>("status");
                    newrow["gmtcreate"] = DateTime.ParseExact(item.Value<string>("gmtCreate"), "yyyyMMddHHmmssfffzz00", null);
                    newrow["expiredate"] = DateTime.ParseExact(item.Value<string>("expireDate"), "yyyyMMddHHmmssfffzz00", null);
                    newrow["buyofferid"] = item.Value<string>("buyOfferId");
                    newrow["suppliermemberid"] = item.Value<string>("supplierMemberId");
                    tmpobj = item.Value<JObject>("contactInfo");
                    if (tmpobj != null)
                    {
                        newrow["contact"] = tmpobj.Value<string>("contact");
                        newrow["mobile"] = tmpobj.Value<string>("mobile");
                    }

                    newrow["invoicetype"] = item.Value<string>("invoiceType");
                    newrow["freight"] = item.Value<long>("freight");
                    newrow["totalprice"] = item.Value<long>("totalPrice");
                    tmpobj = item.Value<JObject>("payRequirementInfo");
                    if (tmpobj != null)
                    {
                        newrow["paytype"] = tmpobj.Value<string>("type");
                    }
                    msts.Rows.Add(newrow);
                    //加明细
                    quotaItems = item.Value<JArray>("supplyNoteItems");
                    foreach (JToken d in quotaItems)
                    {
                        if (dtls.Select(string.Format("quoteid='{0}' and quotedid='{1}'", item.Value<string>("id"), d.Value<string>("id"))).Length == 0)
                        {
                            newrow = dtls.NewRow();
                            newrow["appkey"] = quote.AppKey;
                            newrow["appsecret"] = quote.AppSecret;
                            newrow["apptoken"] = quote.TokenKey;
                            newrow["quoteid"] = item.Value<string>("id");
                            newrow["quotedid"] = d.Value<string>("id");
                            newrow["pritemid"] = d.Value<string>("prItemId");
                            newrow["productcode"] = d.Value<string>("productCode");
                            newrow["amount"] = d.Value<int>("amount");
                            newrow["subject"] = d.Value<string>("subject");
                            newrow["price"] = d.Value<long>("price");
                            newrow["unit"] = d.Value<string>("unit");
                            newrow["itemcount"] = d.Value<decimal>("itemCount");
                            newrow["brandname"] = d.Value<string>("brandName");
                            newrow["modelnumber"] = d.Value<string>("modelNumber");
                            newrow["taxrate"] = d.Value<string>("taxRate");
                            dtls.Rows.Add(newrow);
                        }
                    }
                }
            }

            //更新数据
            DbHelper.Open(_dbcon);
            try
            {
                DbHelper.BeginTran(_dbcon);
                if (DbHelper.Update(_dbcon, msts, "select * from quotation") > 0)
                {
                    if (DbHelper.Update(_dbcon, dtls, "select * from quotationdtl") > 0)
                    {
                        DbHelper.CommitTran(_dbcon);
                    }
                    else
                    {
                        throw new Exception("明细quotationdtl更新失败");
                    }
                }
                else
                {
                    throw new Exception("表头quotation更新失败");
                }
            }
            catch (Exception e)
            {
                DbHelper.RollbackTran(_dbcon);
                throw e;
            }
            finally
            {
                DbHelper.Close(_dbcon);
            }

        }
        /// <summary>
        /// 保存1688订单信息
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="result"></param>
        /// result格式如下
        //        {
        //  "orderModel": {
        //    "certificateSumPayment": 5000,
        //    "codActualFee": 5000,
        //    "gmtSign": "20190330140316000+0800",
        //    "statTags": [
        //      "hy0"
        //    ],
        //    "orderPromotionsFee": 0,
        //    "orderRiskInfo": {
        //      "makeOrderRiskInfo": {
        //        "operatorIp": "101.37.31.231"
        //      }
        //    },
        //    "buyerFlag": "0",
        //    "buyerSex": "M",
        //    "tbId": 271523654409255893,
        //    "newStepOrderList": [
        //      {
        //        "onlyAlink": false,
        //        "lastStep": true,
        //        "gmtModified": "20190330140357000+0800",
        //        "discountFee": 0,
        //        "gmtPay": "20190330140316000+0800",
        //        "hadMoreOrLess": false,
        //        "gmtShip": "20190330140357000+0800",
        //        "hasDelivery": true,
        //        "confirmGoodsWithDisburse": true,
        //        "bALink": false,
        //        "processTemplateStep": {
        //          "onlyAlink": false,
        //          "delivery": true,
        //          "supportMoreOrLess": false,
        //          "fundRules": [
        //            {
        //              "scale": 10000,
        //              "time": 0,
        //              "type": "byConfirmReceiveGoods",
        //              "steps": [
        //                1
        //              ]
        //            }
        //          ],
        //          "bALink": false,
        //          "body": {
        //            "apdt": "5",
        //            "apd": "30",
        //            "apt": "-1"
        //          },
        //          "processTemplateId": 66,
        //          "deliveryScale": 10000,
        //          "sortType": "AB",
        //          "stepName": "全款",
        //          "confirmGoodsTimeout": 864000,
        //          "aBLink": true,
        //          "payScale": 10000,
        //          "attributes": {},
        //          "id": 93,
        //          "payTimeout": 7776000,
        //          "allowInstant": false,
        //          "payTimeoutType": 0
        //        },
        //        "shipAmount": {
        //          "271523654411255893": 20000,
        //          "271523654410255893": 10000
        //        },
        //        "buyerId": 983259358,
        //        "startedShip": true,
        //        "stepOrderId": 4505670352255893,
        //        "sellerId": 2248624086,
        //        "payFee": 5000,
        //        "stepName": "全款",
        //        "paidPostFee": 0,
        //        "aBLink": true,
        //        "activeEnable": true,
        //        "goodsFee": 5000,
        //        "adjustFee": 0,
        //        "stepNo": 1,
        //        "postFee": 0,
        //        "gmtCreate": "20190330140232000+0800",
        //        "paidFee": 5000,
        //        "gmtStart": "20190330140233000+0800",
        //        "firstStep": true,
        //        "activeStatus": 1,
        //        "bizOrderId": 271523654409255893,
        //        "bizOrderIdStr": "271523654409255893",
        //        "shipedAmount": {
        //          "271523654411255893": 20000,
        //          "271523654410255893": 10000
        //        },
        //        "attributes": {
        //          "started_ship": "1"
        //        },
        //        "payStatus": 2,
        //        "logisticsStatus": 2
        //      }
        //    ],
        //    "buyerCompanyName": "阿里巴巴(测试)网络技术有限公司",
        //    "codSellerFee": 0,
        //    "toDivisionCode": "330108",
        //    "kjpayOrder": false,
        //    "couponFee": 0,
        //    "sheGouPayOrder": false,
        //    "overSold": false,
        //    "gmtModified": "20190330140357000+0800",
        //    "sumPayment": 5000,
        //    "bizTypeEnum": "CN",
        //    "carriage": 0,
        //    "statusStr": "waitbuyerreceive",
        //    "sellerName": "乔的石",
        //    "orderPaymentSign": {
        //      "alipayAssure": true,
        //      "bankInstant": false,
        //      "paymentSign": 1
        //    },
        //    "supportDae": false,
        //    "trustDeceit": false,
        //    "labelDisplayModel": {
        //      "sellerDisplayText": {
        //        "wirelessText": "待收货",
        //        "pcBrowserText": "等待买家确认收货"
        //      },
        //      "defaultDisplayText": {
        //        "wirelessText": "待收货",
        //        "pcBrowserText": "等待买家确认收货"
        //      },
        //      "buyerDisplayText": {
        //        "wirelessText": "待收货",
        //        "pcBrowserText": "等待买家确认收货"
        //      },
        //      "label": "waitbuyerreceive"
        //    },
        //    "tradeType": "UNKNOW",
        //    "paidCarriage": 0,
        //    "needSellerAcceptOrder": false,
        //    "subBiz": "buyoffer",
        //    "sellerUserId": 2248624086,
        //    "tocSign": "1",
        //    "orderSign": {
        //      "couponConsume": false,
        //      "goodsReadyToShip": true,
        //      "freezePayed": false,
        //      "couponUnFreezed": false,
        //      "assurePayed": false,
        //      "couponFreezed": false,
        //      "sellerSeeLogistics": false,
        //      "suspected": false,
        //      "sendGoodsArranged": false,
        //      "sign": 32768,
        //      "ensureReleased": false,
        //      "buyerSeeLogistics": false,
        //      "supportInvoice": false,
        //      "sellerNotifyLogistics": false,
        //      "transfer": false,
        //      "buyerNotifyLogistics": false,
        //      "stepTradeHuopinOffer": false
        //    },
        //    "singleStepForNewStep": true,
        //    "alipayTradeId": "5994244005",
        //    "sumProductPayment": 5000,
        //    "isVisual": 0,
        //    "belonging": 0,
        //    "shipping": 2,
        //    "sellerPhone": "86-0571-3555555555",
        //    "codStatus": 0,
        //    "payDetailModelList": [
        //      {
        //        "cancel": false,
        //        "myBankEFTChannel": false,
        //        "payDetailFee": 5000,
        //        "gmtModified": "20190330140357000+0800",
        //        "payOrderId": 271523654409255893,
        //        "payTime": "20190330140315000+0800",
        //        "accountPeriodChannel": true,
        //        "buyerId": 983259358,
        //        "realChannelKeysInPayment": [],
        //        "paymentAlipayChannel": false,
        //        "payDetailId": 5994244005,
        //        "buyerAccountId": "2088031666286208",
        //        "stepOrderId": 4505670352255893,
        //        "sellerId": 2248624086,
        //        "payOrderIdStr": "271523654409255893",
        //        "toBuyerFee": 0,
        //        "jinPiaoChannel": false,
        //        "realChannelInPayment": 7,
        //        "absentChannel": false,
        //        "mergeChannel": false,
        //        "payDetailChannel": 7,
        //        "electronicAcceptanceBillChannel": false,
        //        "payDetailTypePay": true,
        //        "outPayDetailId": "5994244005",
        //        "gmtCreate": "20190330140315000+0800",
        //        "sellerAccountId": "2088611490712805",
        //        "noOutPayOrder": false,
        //        "payDetailTypeRepayment": false,
        //        "paymentChannel": false,
        //        "payDetailStatus": 2,
        //        "piaoJuSubsidy": false,
        //        "outTradeId": "5994244005",
        //        "paidNotEnd": true,
        //        "paid": true,
        //        "lstShegouChannel": false,
        //        "inPay": false,
        //        "waitPay": false,
        //        "attributes": {
        //          "src_channel": "WEB",
        //          "src_terminal": "PC"
        //        },
        //        "payDetailType": 1,
        //        "toSellerFee": 0,
        //        "sheGouChannel": false,
        //        "alipayChannel": false
        //      }
        //    ],
        //    "buyerRateStatus": 5,
        //    "orderFrom": "tb",
        //    "myBankPEFTPayOrder": false,
        //    "buyerUserId": 983259358,
        //    "paidPaymentCouponFee": 0,
        //    "refundPayment": 0,
        //    "virtual": false,
        //    "bizType": "cn",
        //    "gmtPayment": "20190330140316000+0800",
        //    "wirelessOrder": false,
        //    "idStr": "271523654409255893",
        //    "gmtGoodsSend": "20190330140357000+0800",
        //    "codBuyerFee": 0,
        //    "onlyAlipayPayChannel": false,
        //    "accountPeriodDateType": 5,
        //    "aliloanSecurityFee": 0,
        //    "tocManagedTimeout": true,
        //    "alipayPayOrder": false,
        //    "ladderGroup": false,
        //    "discount": 0,
        //    "paidSumPayment": 5000,
        //    "productName": "苹果 等多件",
        //    "buyerLoginId": "dongyl3333",
        //    "codAudit": false,
        //    "id": 271523654409255893,
        //    "accountPeriodExtendCount": 0,
        //    "buyerMemberId": "dongyl3333",
        //    "dataFromType": "CHINASITE_VERSION_TWO",
        //    "sourceTypeStr": "pq",
        //    "payOnline": false,
        //    "status": "WAIT_BUYER_RECEIVE",
        //    "accountPeriodDate": 30,
        //    "sellerCompanyName": "阿里巴巴（中国）网络技术有限公司",
        //    "orderEntries": [
        //      {
        //        "refundFee": 0,
        //        "entryDiscount": 0,
        //        "orderId": 271523654409255893,
        //        "discount": 1.0,
        //        "surplusFee": 1000,
        //        "sourceIdStr": "1495899150727",
        //        "productName": "苹果",
        //        "waitSellerConfirmQuantity": {
        //          "realAmount": 0.0,
        //          "realAmountStr": "0",
        //          "amountFactor": 1000.0,
        //          "calAmount": 0
        //        },
        //        "entryStatus": "WAIT_BUYER_RECEIVE",
        //        "totalReceiveAmountString": "0.000",
        //        "price": 100,
        //        "id": 271523654410255893,
        //        "amountFactor": 1000.0,
        //        "tbId": 271523654410255893,
        //        "unitPrice": 100,
        //        "actualRefundProductFee": 0,
        //        "totalReceiveGoodsQuantity": {
        //          "realAmount": 0.0,
        //          "realAmountStr": "0",
        //          "amountFactor": 1000.0,
        //          "calAmount": 0
        //        },
        //        "fromOffer": false,
        //        "priceAfterPromotion": 100,
        //        "currentAmountModel": {
        //          "realAmount": 10.0,
        //          "realAmountStr": "10",
        //          "amountFactor": 1000.0,
        //          "calAmount": 10000
        //        },
        //        "actualPayFee": 1000,
        //        "sevenDayFlag": false,
        //        "overSold": false,
        //        "gmtModified": "20190330140357000+0800",
        //        "actualPaidFee": 1000,
        //        "actualPaidUnitPrice": 100,
        //        "crossPromotionFee": 0,
        //        "complaintStatus": {
        //          "afterSalesComplaintDoing": false,
        //          "valid": true,
        //          "refundComplaintDoing": false
        //        },
        //        "refundGoodsAmountModel": {
        //          "realAmount": 0.0,
        //          "realAmountStr": "0",
        //          "amountFactor": 1000.0,
        //          "calAmount": 0
        //        },
        //        "confirmGoodsFeeForBatchSettlement": 0,
        //        "bizSign": {
        //          "freePostage": false,
        //          "salePromotion": false,
        //          "commonSample": false,
        //          "jiCaiGoods": false,
        //          "yiFenSample": false,
        //          "sign": 0,
        //          "mix": false,
        //          "binaryString": "0",
        //          "virtualGoods": false
        //        },
        //        "detailItemToSellerFee": 0,
        //        "tbEntryRefundStatus": 9,
        //        "logisticsOrderId": -1,
        //        "snapshotId": "o:271523654410255893_1",
        //        "productPic": "pqimage//cms/upload/2012/138/334/433831_61694261.png",
        //        "sellerUserId": 2248624086,
        //        "entryStatusStr": "waitbuyerreceive",
        //        "gmtCreate": "20190330140233000+0800",
        //        "buyerSecuritySupport": false,
        //        "platformPromotionFee": 0,
        //        "orderSourceType": "pq",
        //        "attributes": {
        //          "ods_ump_outid": "792550308124",
        //          "b_ra": "10.0",
        //          "bizCode": "com.1688.business",
        //          "oup": "100",
        //          "b_ci": "1036642",
        //          "pSubOutId": "792550308124",
        //          "steppd": "1",
        //          "b_pr": "1000",
        //          "b_seq": "1",
        //          "paf": "1000",
        //          "b_pc": "QUOTA1688",
        //          "ada": "10000",
        //          "b_mss": "1",
        //          "b_bs": "1",
        //          "af": "1000",
        //          "b_bsmol": "1",
        //          "b_wd": "1.0",
        //          "subFlowId": "74",
        //          "b_unit": "台",
        //          "oba": "10000.0",
        //          "b_ss": "0",
        //          "b_cc": "CNY",
        //          "b_up": "100",
        //          "sendgt": "1553925837014",
        //          "b_pmc": "006001",
        //          "first_pay_time": "2019-03-30 14:03:15",
        //          "b_of": "pq",
        //          "b_amt": "1000",
        //          "tp3create": "1",
        //          "b_soc": "1"
        //        },
        //        "productMaterialCode": "006001",
        //        "currencyCode": "CNY",
        //        "sourceId": 1495899150727,
        //        "allCommissionSceneType": [
        //          "cyd"
        //        ],
        //        "quantityModel": {
        //          "realAmount": 10.0,
        //          "realAmountStr": "10",
        //          "amountFactor": 1000.0,
        //          "calAmount": 10000
        //        },
        //        "guaranteeSupport": false,
        //        "orderSharedPromotionFee": 0,
        //        "activityId": 0,
        //        "snapshotImages": [
        //          {
        //            "midImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //            "imageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //            "summImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png"
        //          }
        //        ],
        //        "actualDeliveryAmountModel": {
        //          "realAmount": 10.0,
        //          "realAmountStr": "10",
        //          "amountFactor": 1000.0,
        //          "calAmount": 10000
        //        },
        //        "promotionsFee": 0,
        //        "isMaliciousOrder": false,
        //        "mainImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //        "mainSummImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //        "refundGoodsAmount": 0.0,
        //        "productPics": [
        //          "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png"
        //        ],
        //        "actualDeliveryAmount": 10.0,
        //        "sellerRateStatus": 5,
        //        "orderIdStr": "271523654409255893",
        //        "commissionModel": {
        //          "validCommissionSceneType": false
        //        },
        //        "codStatus": 0,
        //        "unit": "台",
        //        "canRefundFee": 1000,
        //        "buyerRateStatus": 5,
        //        "orderFrom": "tb",
        //        "buyerUserId": 983259358,
        //        "surplusAmount": 10.0,
        //        "pmtARCouponFee": 0,
        //        "discountPrice": 100,
        //        "mainMidImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //        "maliciousOrder": false,
        //        "actualConfirmProductFee": 0,
        //        "confirmGoodsAmountModel": {
        //          "realAmount": 0.0,
        //          "realAmountStr": "0",
        //          "amountFactor": 1000.0,
        //          "calAmount": 0
        //        },
        //        "idStr": "271523654410255893",
        //        "amountWithDiscountAndPromotion": 1000,
        //        "amount": 1000,
        //        "quantity": 10.0,
        //        "pmtRCouponFee": 0,
        //        "entryPayStatus": 2,
        //        "surplusAmountModel": {
        //          "realAmount": 10.0,
        //          "realAmountStr": "10",
        //          "amountFactor": 1000.0,
        //          "calAmount": 10000
        //        },
        //        "confirmGoodsAmount": 0.0,
        //        "logisticsStatus": 2,
        //        "categoryId": 1036642
        //      },
        //      {
        //        "refundFee": 0,
        //        "entryDiscount": 0,
        //        "orderId": 271523654409255893,
        //        "discount": 1.0,
        //        "surplusFee": 4000,
        //        "sourceIdStr": "1495899160727",
        //        "productName": "菠萝",
        //        "waitSellerConfirmQuantity": {
        //          "realAmount": 0.0,
        //          "realAmountStr": "0",
        //          "amountFactor": 1000.0,
        //          "calAmount": 0
        //        },
        //        "entryStatus": "WAIT_BUYER_RECEIVE",
        //        "totalReceiveAmountString": "0.000",
        //        "price": 200,
        //        "id": 271523654411255893,
        //        "amountFactor": 1000.0,
        //        "tbId": 271523654411255893,
        //        "unitPrice": 200,
        //        "actualRefundProductFee": 0,
        //        "totalReceiveGoodsQuantity": {
        //          "realAmount": 0.0,
        //          "realAmountStr": "0",
        //          "amountFactor": 1000.0,
        //          "calAmount": 0
        //        },
        //        "fromOffer": false,
        //        "priceAfterPromotion": 200,
        //        "currentAmountModel": {
        //          "realAmount": 20.0,
        //          "realAmountStr": "20",
        //          "amountFactor": 1000.0,
        //          "calAmount": 20000
        //        },
        //        "actualPayFee": 4000,
        //        "sevenDayFlag": false,
        //        "overSold": false,
        //        "gmtModified": "20190330140357000+0800",
        //        "actualPaidFee": 4000,
        //        "actualPaidUnitPrice": 200,
        //        "crossPromotionFee": 0,
        //        "complaintStatus": {
        //          "afterSalesComplaintDoing": false,
        //          "valid": true,
        //          "refundComplaintDoing": false
        //        },
        //        "refundGoodsAmountModel": {
        //          "realAmount": 0.0,
        //          "realAmountStr": "0",
        //          "amountFactor": 1000.0,
        //          "calAmount": 0
        //        },
        //        "confirmGoodsFeeForBatchSettlement": 0,
        //        "bizSign": {
        //          "freePostage": false,
        //          "salePromotion": false,
        //          "commonSample": false,
        //          "jiCaiGoods": false,
        //          "yiFenSample": false,
        //          "sign": 0,
        //          "mix": false,
        //          "binaryString": "0",
        //          "virtualGoods": false
        //        },
        //        "detailItemToSellerFee": 0,
        //        "tbEntryRefundStatus": 9,
        //        "logisticsOrderId": -1,
        //        "snapshotId": "o:271523654411255893_1",
        //        "productPic": "pqimage//cms/upload/2012/138/334/433831_61694261.png",
        //        "sellerUserId": 2248624086,
        //        "entryStatusStr": "waitbuyerreceive",
        //        "gmtCreate": "20190330140233000+0800",
        //        "buyerSecuritySupport": false,
        //        "platformPromotionFee": 0,
        //        "orderSourceType": "pq",
        //        "attributes": {
        //          "ods_ump_outid": "792550308125",
        //          "b_ra": "20.0",
        //          "bizCode": "com.1688.business",
        //          "oup": "200",
        //          "b_ci": "125062004",
        //          "pSubOutId": "792550308125",
        //          "steppd": "1",
        //          "b_pr": "1000",
        //          "b_seq": "2",
        //          "paf": "4000",
        //          "b_pc": "QUOTA1688",
        //          "ada": "20000",
        //          "b_mss": "1",
        //          "b_bs": "1",
        //          "af": "4000",
        //          "b_bsmol": "1",
        //          "b_wd": "1.0",
        //          "subFlowId": "74",
        //          "b_unit": "台",
        //          "oba": "20000.0",
        //          "b_ss": "0",
        //          "b_cc": "CNY",
        //          "b_up": "200",
        //          "sendgt": "1553925837014",
        //          "b_pmc": "006002",
        //          "first_pay_time": "2019-03-30 14:03:15",
        //          "b_of": "pq",
        //          "b_amt": "4000",
        //          "tp3create": "1",
        //          "b_soc": "1"
        //        },
        //        "productMaterialCode": "006002",
        //        "currencyCode": "CNY",
        //        "sourceId": 1495899160727,
        //        "allCommissionSceneType": [
        //          "cyd"
        //        ],
        //        "quantityModel": {
        //          "realAmount": 20.0,
        //          "realAmountStr": "20",
        //          "amountFactor": 1000.0,
        //          "calAmount": 20000
        //        },
        //        "guaranteeSupport": false,
        //        "orderSharedPromotionFee": 0,
        //        "activityId": 0,
        //        "snapshotImages": [
        //          {
        //            "midImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //            "imageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //            "summImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png"
        //          }
        //        ],
        //        "actualDeliveryAmountModel": {
        //          "realAmount": 20.0,
        //          "realAmountStr": "20",
        //          "amountFactor": 1000.0,
        //          "calAmount": 20000
        //        },
        //        "promotionsFee": 0,
        //        "isMaliciousOrder": false,
        //        "mainImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //        "mainSummImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //        "refundGoodsAmount": 0.0,
        //        "productPics": [
        //          "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png"
        //        ],
        //        "actualDeliveryAmount": 20.0,
        //        "sellerRateStatus": 5,
        //        "orderIdStr": "271523654409255893",
        //        "commissionModel": {
        //          "validCommissionSceneType": false
        //        },
        //        "codStatus": 0,
        //        "unit": "台",
        //        "canRefundFee": 4000,
        //        "buyerRateStatus": 5,
        //        "orderFrom": "tb",
        //        "buyerUserId": 983259358,
        //        "surplusAmount": 20.0,
        //        "pmtARCouponFee": 0,
        //        "discountPrice": 200,
        //        "mainMidImageUrl": "https://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //        "maliciousOrder": false,
        //        "actualConfirmProductFee": 0,
        //        "confirmGoodsAmountModel": {
        //          "realAmount": 0.0,
        //          "realAmountStr": "0",
        //          "amountFactor": 1000.0,
        //          "calAmount": 0
        //        },
        //        "idStr": "271523654411255893",
        //        "amountWithDiscountAndPromotion": 4000,
        //        "amount": 4000,
        //        "quantity": 20.0,
        //        "pmtRCouponFee": 0,
        //        "entryPayStatus": 2,
        //        "surplusAmountModel": {
        //          "realAmount": 20.0,
        //          "realAmountStr": "20",
        //          "amountFactor": 1000.0,
        //          "calAmount": 20000
        //        },
        //        "confirmGoodsAmount": 0.0,
        //        "logisticsStatus": 2,
        //        "categoryId": 125062004
        //      }
        //    ],
        //    "refundPostFee": 0,
        //    "crossPromotionFee": 0,
        //    "myBankEFTPayOrder": false,
        //    "payOrder": {
        //      "couponFee": 0,
        //      "refundFee": 0,
        //      "ppCheckout": false,
        //      "gmtModified": "20190330140357000+0800",
        //      "payOrderId": 271523654409255893,
        //      "discountFee": 0,
        //      "payTime": "20190330140316000+0800",
        //      "partConfirmFee": 0,
        //      "gmtCreate": "20190330140232000+0800",
        //      "buyerId": 983259358,
        //      "actualTotalFee": 5000,
        //      "alipaySellerId": "2088611490712805",
        //      "sellerId": 2248624086,
        //      "totalFee": 5000,
        //      "outPayId": "5994244005",
        //      "alipayBuyerId": "2088031666286208",
        //      "attributes": {
        //        "outPaySystem": "NOOUTPAY",
        //        "payMode": "B_SECURED"
        //      },
        //      "payStatus": 2,
        //      "adjustFee": 0
        //    },
        //    "sellerFlag": "0",
        //    "codBuyerInitFee": 0,
        //    "stepPayAll": false,
        //    "gmtCreate": "20190330140232000+0800",
        //    "buyerName": "托狼",
        //    "buyerAlipayId": "2088031666286208",
        //    "payidByMybankPEFT": false,
        //    "platformPromotionFee": 0,
        //    "toFullName": "小张",
        //    "toMobile": "18305811642",
        //    "accountPeriodPayOrder": true,
        //    "attributes": {
        //      "b_isbb": "1",
        //      "t_code": "accountPeriodBusinessBuy",
        //      "b_dft": "2",
        //      "flowTemplateId": "66",
        //      "tp3_lock": "1553925837015",
        //      "bizCode": "com.1688.business",
        //      "b_ms": "32768",
        //      "steppd": "1",
        //      "b_receivename": "小张",
        //      "b_nrd": "1",
        //      "b_ods_v": "1",
        //      "b_bc": "阿里巴巴(测试)网络技术有限公司",
        //      "shipping": "2",
        //      "b_rm": "18305811642",
        //      "newDO": "1",
        //      "pOutId": "792759238863",
        //      "balanceFee": "5000",
        //      "b_b_no": "1553925752316798671",
        //      "b_pc": "QUOTA1688",
        //      "b_sc": "阿里巴巴（中国）网络技术有限公司",
        //      "otf": "5000",
        //      "b_ltpt": "2019-03-30 14:03:15",
        //      "b_bs": "1",
        //      "opf": "0",
        //      "b_bsmol": "1",
        //      "subFlowId": "74",
        //      "b_bl": "L1",
        //      "b_nao": "0",
        //      "b_buyername": "dongyl3333",
        //      "b_obs": "quotation",
        //      "tf": "5000",
        //      "b_st": "hy0",
        //      "first_pay_time": "2019-03-30 14:03:15",
        //      "b_ltpsf": "5000",
        //      "tp3create": "1",
        //      "b_soc": "1",
        //      "b_osn": "1"
        //    },
        //    "paymentCouponFee": 0,
        //    "toArea": "浙江省 杭州市 滨江区 **",
        //    "flowTemplateId": 66,
        //    "buyerEmail": "dongyl3333@163.com",
        //    "tradeTypeStr": "50060",
        //    "flowTemplateCode": "accountPeriodBusinessBuy",
        //    "daifaOrder": false,
        //    "toPost": "",
        //    "sumProductPaymentWithCoupon": 5000,
        //    "newStepOrderMap": {
        //      "4505670352255893": {
        //        "onlyAlink": false,
        //        "lastStep": true,
        //        "gmtModified": "20190330140357000+0800",
        //        "discountFee": 0,
        //        "gmtPay": "20190330140316000+0800",
        //        "hadMoreOrLess": false,
        //        "gmtShip": "20190330140357000+0800",
        //        "hasDelivery": true,
        //        "confirmGoodsWithDisburse": true,
        //        "bALink": false,
        //        "processTemplateStep": {
        //          "onlyAlink": false,
        //          "delivery": true,
        //          "supportMoreOrLess": false,
        //          "fundRules": [
        //            {
        //              "scale": 10000,
        //              "time": 0,
        //              "type": "byConfirmReceiveGoods",
        //              "steps": [
        //                1
        //              ]
        //            }
        //          ],
        //          "bALink": false,
        //          "body": {
        //            "apdt": "5",
        //            "apd": "30",
        //            "apt": "-1"
        //          },
        //          "processTemplateId": 66,
        //          "deliveryScale": 10000,
        //          "sortType": "AB",
        //          "stepName": "全款",
        //          "confirmGoodsTimeout": 864000,
        //          "aBLink": true,
        //          "payScale": 10000,
        //          "attributes": {},
        //          "id": 93,
        //          "payTimeout": 7776000,
        //          "allowInstant": false,
        //          "payTimeoutType": 0
        //        },
        //        "shipAmount": {
        //          "271523654411255893": 20000,
        //          "271523654410255893": 10000
        //        },
        //        "buyerId": 983259358,
        //        "startedShip": true,
        //        "stepOrderId": 4505670352255893,
        //        "sellerId": 2248624086,
        //        "payFee": 5000,
        //        "stepName": "全款",
        //        "paidPostFee": 0,
        //        "aBLink": true,
        //        "activeEnable": true,
        //        "goodsFee": 5000,
        //        "adjustFee": 0,
        //        "stepNo": 1,
        //        "postFee": 0,
        //        "gmtCreate": "20190330140232000+0800",
        //        "paidFee": 5000,
        //        "gmtStart": "20190330140233000+0800",
        //        "firstStep": true,
        //        "activeStatus": 1,
        //        "bizOrderId": 271523654409255893,
        //        "bizOrderIdStr": "271523654409255893",
        //        "shipedAmount": {
        //          "271523654411255893": 20000,
        //          "271523654410255893": 10000
        //        },
        //        "attributes": {
        //          "started_ship": "1"
        //        },
        //        "payStatus": 2,
        //        "logisticsStatus": 2
        //      }
        //    },
        //    "promotionsFee": 0,
        //    "sellerAlipayId": "2088611490712805",
        //    "sellerSex": "M",
        //    "currentNewStepOrder": {
        //      "onlyAlink": false,
        //      "lastStep": true,
        //      "gmtModified": "20190330140357000+0800",
        //      "discountFee": 0,
        //      "gmtPay": "20190330140316000+0800",
        //      "hadMoreOrLess": false,
        //      "gmtShip": "20190330140357000+0800",
        //      "hasDelivery": true,
        //      "confirmGoodsWithDisburse": true,
        //      "bALink": false,
        //      "processTemplateStep": {
        //        "onlyAlink": false,
        //        "delivery": true,
        //        "supportMoreOrLess": false,
        //        "fundRules": [
        //          {
        //            "scale": 10000,
        //            "time": 0,
        //            "type": "byConfirmReceiveGoods",
        //            "steps": [
        //              1
        //            ]
        //          }
        //        ],
        //        "bALink": false,
        //        "body": {
        //          "apdt": "5",
        //          "apd": "30",
        //          "apt": "-1"
        //        },
        //        "processTemplateId": 66,
        //        "deliveryScale": 10000,
        //        "sortType": "AB",
        //        "stepName": "全款",
        //        "confirmGoodsTimeout": 864000,
        //        "aBLink": true,
        //        "payScale": 10000,
        //        "attributes": {},
        //        "id": 93,
        //        "payTimeout": 7776000,
        //        "allowInstant": false,
        //        "payTimeoutType": 0
        //      },
        //      "shipAmount": {
        //        "271523654411255893": 20000,
        //        "271523654410255893": 10000
        //      },
        //      "buyerId": 983259358,
        //      "startedShip": true,
        //      "stepOrderId": 4505670352255893,
        //      "sellerId": 2248624086,
        //      "payFee": 5000,
        //      "stepName": "全款",
        //      "paidPostFee": 0,
        //      "aBLink": true,
        //      "activeEnable": true,
        //      "goodsFee": 5000,
        //      "adjustFee": 0,
        //      "stepNo": 1,
        //      "postFee": 0,
        //      "gmtCreate": "20190330140232000+0800",
        //      "paidFee": 5000,
        //      "gmtStart": "20190330140233000+0800",
        //      "firstStep": true,
        //      "activeStatus": 1,
        //      "bizOrderId": 271523654409255893,
        //      "bizOrderIdStr": "271523654409255893",
        //      "shipedAmount": {
        //        "271523654411255893": 20000,
        //        "271523654410255893": 10000
        //      },
        //      "attributes": {
        //        "started_ship": "1"
        //      },
        //      "payStatus": 2,
        //      "logisticsStatus": 2
        //    },
        //    "sumConfirmPaidPayment": 0,
        //    "allPromotionsFee": 0,
        //    "sellerLoginId": "b测试账号003",
        //    "sellerMemberId": "b2b-2248624086",
        //    "jinPiaoPayOrder": false,
        //    "sellerRateStatus": 5,
        //    "buyerPhone": "86-0571-85551452",
        //    "buyerMobile": "13777467803",
        //    "sLSJOrder": false,
        //    "stepId2PayDetailMap": {
        //      "4505670352255893": {
        //        "cancel": false,
        //        "myBankEFTChannel": false,
        //        "payDetailFee": 5000,
        //        "gmtModified": "20190330140357000+0800",
        //        "payOrderId": 271523654409255893,
        //        "payTime": "20190330140315000+0800",
        //        "accountPeriodChannel": true,
        //        "buyerId": 983259358,
        //        "realChannelKeysInPayment": [],
        //        "paymentAlipayChannel": false,
        //        "payDetailId": 5994244005,
        //        "buyerAccountId": "2088031666286208",
        //        "stepOrderId": 4505670352255893,
        //        "sellerId": 2248624086,
        //        "payOrderIdStr": "271523654409255893",
        //        "toBuyerFee": 0,
        //        "jinPiaoChannel": false,
        //        "realChannelInPayment": 7,
        //        "absentChannel": false,
        //        "mergeChannel": false,
        //        "payDetailChannel": 7,
        //        "electronicAcceptanceBillChannel": false,
        //        "payDetailTypePay": true,
        //        "outPayDetailId": "5994244005",
        //        "gmtCreate": "20190330140315000+0800",
        //        "sellerAccountId": "2088611490712805",
        //        "noOutPayOrder": false,
        //        "payDetailTypeRepayment": false,
        //        "paymentChannel": false,
        //        "payDetailStatus": 2,
        //        "piaoJuSubsidy": false,
        //        "outTradeId": "5994244005",
        //        "paidNotEnd": true,
        //        "paid": true,
        //        "lstShegouChannel": false,
        //        "inPay": false,
        //        "waitPay": false,
        //        "attributes": {
        //          "src_channel": "WEB",
        //          "src_terminal": "PC"
        //        },
        //        "payDetailType": 1,
        //        "toSellerFee": 0,
        //        "sheGouChannel": false,
        //        "alipayChannel": false
        //      }
        //    },
        //    "codFee": 0,
        //    "payChannels": [
        //      "account_period"
        //    ],
        //    "toPhone": "",
        //    "agentOrder": false,
        //    "needApproveOrder": false,
        //    "succSumPayment": 0,
        //    "creditCard": false,
        //    "payStatus": 2,
        //    "logisticsStatus": 2,
        //    "newComplaintStatus": {
        //      "afterSalesComplaintDoing": false,
        //      "valid": true,
        //      "refundComplaintDoing": false
        //    }
        //  }
        //}
        private void SaveGetOrder2(AliPubHandle handle, string result)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(result);
            JObject bill = obj.Value<JObject>("orderModel");
            if (bill == null) return;//接口返回错误
            AliOrder order = (AliOrder)handle.Secret;
            DataTable mst = DbHelper.GetDataTable(_dbcon, string.Format("select * from aliordermst where appkey='{0}' and appsecret='{1}' and apptoken='{2}' and orderid='{3}'", order.AppKey, order.AppSecret, order.TokenKey, order.OrderId));
            DataTable dtls = DbHelper.GetDataTable(_dbcon, string.Format("select * from aliorderdtl where appkey='{0}' and appsecret='{1}' and apptoken='{2}' and orderid='{3}'", order.AppKey, order.AppSecret, order.TokenKey, order.OrderId));
            DataRow dr;
            JArray items = bill.Value<JArray>("orderEntries");//明细部分
            //处理表头
            if (mst.Rows.Count == 0)
            {
                //不存在则新增
                dr = mst.NewRow();
                dr["appkey"] = order.AppKey;
                dr["appsecret"] = order.AppSecret;
                dr["apptoken"] = order.TokenKey;
                dr["orderid"] = order.OrderId;
            }
            else
            {
                dr = mst.Rows[0];
            }
            dr["status"] = bill.Value<string>("status");
            dr["subbiz"] = bill.Value<string>("subBiz"); //1688接口说明中有此属性
            dr["gmtcreate"] = DateTime.ParseExact(bill.Value<string>("gmtCreate"), "yyyyMMddHHmmssfffzz00", null);
            dr["subbiz"] = bill.Value<string>("subBiz");
            dr["discount"] = bill.Value<long>("discount");
            dr["carriage"] = bill.Value<long>("carriage");
            dr["refundpayment"] = bill.Value<long>("refundPayment");
            dr["sumpayment"] = bill.Value<long>("sumPayment");
            dr["sumproductpayment"] = bill.Value<long>("sumProductPayment");
            dr["sellercompanyname"] = bill.Value<string>("sellerCompanyName");
            dr["selleremail"] = bill.Value<string>("sellerEmail");
            dr["sellermemberid"] = bill.Value<string>("sellerMemberId");
            dr["selleruserid"] = bill.Value<long>("sellerUserId");
            dr["selleralipayid"] = bill.Value<string>("sellerAlipayId");
            dr["sellerloginid"] = bill.Value<string>("sellerLoginId");
            dr["buyercompanyname"] = bill.Value<string>("buyerCompanyName");
            dr["buyeremail"] = bill.Value<string>("buyerEmail");
            dr["buyermemberid"] = bill.Value<string>("buyerMemberId");
            dr["buyeruserid"] = bill.Value<long>("buyerUserId");
            dr["buyeralipayid"] = bill.Value<string>("buyerAlipayId");
            dr["buyerloginid"] = bill.Value<string>("buyerLoginId");
            dr["sellername"] = bill.Value<string>("sellerName");
            dr["buyername"] = bill.Value<string>("buyerName");
            if (mst.Rows.Count == 0)
            {
                mst.Rows.Add(dr);
            }
            //处理表体           
            bool exists = false;
            foreach (DataRow r in dtls.Rows)
            {
                //datatable中的数据若存在于返回内容中则修改
                //若不存在则删除
                exists = false;
                foreach (JToken item in items)
                {
                    if (item.Value<string>("id") == r.Field<string>("orderdtlid"))
                    {
                        exists = true;
                        r["productname"] = item.Value<string>("productName");
                        r["unit"] = item.Value<string>("unit");
                        r["qty"] = item.Value<decimal>("quantity");
                        r["price"] = item.Value<long>("price");
                        r["amt"] = item.Value<long>("amount");
                        r["refundfee"] = item.Value<long>("refundFee");
                        r["discount"] = item.Value<decimal>("discount");
                        r["sourceid"] = item.Value<string>("sourceId");
                        r["status"] = item.Value<string>("entryStatus");
                        r["actualpayfee"] = item.Value<long>("actualPayFee");
                        break;
                    }
                }
                if (!exists)
                {
                    r.Delete();
                }
            }
            foreach (JToken item in items)
            {
                //若返回数据不存在于datatable则新增
                if (dtls.Select(string.Format("orderdtlid='{0}'", item.Value<string>("id"))).Length == 0)
                {
                    dr = dtls.NewRow();
                    dr["appkey"] = order.AppKey;
                    dr["appsecret"] = order.AppSecret;
                    dr["apptoken"] = order.TokenKey;
                    dr["orderid"] = order.OrderId;
                    dr["orderdtlid"] = item.Value<string>("id");
                    dr["productname"] = item.Value<string>("productName");
                    dr["unit"] = item.Value<string>("unit");
                    dr["qty"] = item.Value<decimal>("quantity");
                    dr["price"] = item.Value<long>("price");
                    dr["amt"] = item.Value<long>("amount");
                    dr["refundfee"] = item.Value<long>("refundFee");
                    dr["discount"] = item.Value<decimal>("discount");
                    dr["sourceid"] = item.Value<string>("sourceId");
                    dr["status"] = item.Value<string>("entryStatus");
                    dr["actualpayfee"] = item.Value<long>("actualPayFee");
                    dtls.Rows.Add(dr);
                }
            }

            //更新数据
            DbHelper.Open(_dbcon);
            try
            {
                DbHelper.BeginTran(_dbcon);
                if (DbHelper.Update(_dbcon, mst, "select * from aliordermst") > 0)
                {
                    if (DbHelper.Update(_dbcon, dtls, "select * from aliorderdtl") > 0)
                    {
                        DbHelper.CommitTran(_dbcon);
                    }
                    else
                    {
                        throw new Exception("明细aliorderdtl更新失败");
                    }
                }
                else
                {
                    throw new Exception("表头aliordermst更新失败");
                }
            }
            catch (Exception e)
            {
                DbHelper.RollbackTran(_dbcon);
                throw e;
            }
            finally
            {
                DbHelper.Close(_dbcon);
            }

        }

        /// <summary>
        /// 保存刚发布的1688订单信息
        /// 返回数据信息不够，调用getorder2获取详情
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="result"></param>
        /// result格式:
        //        {
        //  "orderResult": {
        //    "failOrderMap": [],
        //    "success": true,
        //    "mutilOrderPublicData": {
        //      "isSupportFreeCarriage": false,
        //      "supportInvoice": false
        //    },
        //    "commitResults": [
        //      {
        //        "orderAmmount": 20000,
        //        "orderId": 271525574350255893,
        //        "success": true,
        //        "customOffer": false,
        //        "orderModel": {
        //          "buyerMemberId": "dongyl3333",
        //          "sellerAlipayId": "2088611490712805",
        //          "tradeTypeStr": "50060",
        //          "sellerUserId": 2248624086,
        //          "sellerMemberId": "b2b-2248624086",
        //          "succSumPayment": 20000,
        //          "buyerAlipayId": "2088031666286208",
        //          "orderEntryModel": [
        //            {
        //              "succSumPayment": 0,
        //              "outOrderId": "792552900841",
        //              "id": 271525574351255893,
        //              "attr": [
        //                {
        //                  "value": "792552900841",
        //                  "key": "ods_ump_outid"
        //                },
        //                {
        //                  "value": "20.0",
        //                  "key": "b_ra"
        //                },
        //                {
        //                  "value": "4000",
        //                  "key": "af"
        //                },
        //                {
        //                  "value": "com.1688.business",
        //                  "key": "bizCode"
        //                },
        //                {
        //                  "value": "1.0",
        //                  "key": "b_wd"
        //                },
        //                {
        //                  "value": "74",
        //                  "key": "subFlowId"
        //                },
        //                {
        //                  "value": "200",
        //                  "key": "oup"
        //                },
        //                {
        //                  "value": "台",
        //                  "key": "b_unit"
        //                },
        //                {
        //                  "value": "126484005",
        //                  "key": "b_ci"
        //                },
        //                {
        //                  "value": "20000.0",
        //                  "key": "oba"
        //                },
        //                {
        //                  "value": "792552900841",
        //                  "key": "pSubOutId"
        //                },
        //                {
        //                  "value": "0",
        //                  "key": "b_ss"
        //                },
        //                {
        //                  "value": "CNY",
        //                  "key": "b_cc"
        //                },
        //                {
        //                  "value": "1000",
        //                  "key": "b_pr"
        //                },
        //                {
        //                  "value": "1",
        //                  "key": "b_seq"
        //                },
        //                {
        //                  "value": "200",
        //                  "key": "b_up"
        //                },
        //                {
        //                  "value": "0060006",
        //                  "key": "b_pmc"
        //                },
        //                {
        //                  "value": "pq",
        //                  "key": "b_of"
        //                },
        //                {
        //                  "value": "4000",
        //                  "key": "b_amt"
        //                },
        //                {
        //                  "value": "1",
        //                  "key": "tp3create"
        //                },
        //                {
        //                  "value": "1",
        //                  "key": "b_soc"
        //                }
        //              ],
        //              "tbId": 271525574351255893
        //            },
        //            {
        //              "succSumPayment": 0,
        //              "outOrderId": "792552900842",
        //              "id": 271525574352255893,
        //              "attr": [
        //                {
        //                  "value": "792552900842",
        //                  "key": "ods_ump_outid"
        //                },
        //                {
        //                  "value": "40.0",
        //                  "key": "b_ra"
        //                },
        //                {
        //                  "value": "16000",
        //                  "key": "af"
        //                },
        //                {
        //                  "value": "com.1688.business",
        //                  "key": "bizCode"
        //                },
        //                {
        //                  "value": "1.0",
        //                  "key": "b_wd"
        //                },
        //                {
        //                  "value": "74",
        //                  "key": "subFlowId"
        //                },
        //                {
        //                  "value": "400",
        //                  "key": "oup"
        //                },
        //                {
        //                  "value": "台",
        //                  "key": "b_unit"
        //                },
        //                {
        //                  "value": "124734044",
        //                  "key": "b_ci"
        //                },
        //                {
        //                  "value": "40000.0",
        //                  "key": "oba"
        //                },
        //                {
        //                  "value": "792552900842",
        //                  "key": "pSubOutId"
        //                },
        //                {
        //                  "value": "0",
        //                  "key": "b_ss"
        //                },
        //                {
        //                  "value": "CNY",
        //                  "key": "b_cc"
        //                },
        //                {
        //                  "value": "1000",
        //                  "key": "b_pr"
        //                },
        //                {
        //                  "value": "2",
        //                  "key": "b_seq"
        //                },
        //                {
        //                  "value": "400",
        //                  "key": "b_up"
        //                },
        //                {
        //                  "value": "006003",
        //                  "key": "b_pmc"
        //                },
        //                {
        //                  "value": "pq",
        //                  "key": "b_of"
        //                },
        //                {
        //                  "value": "16000",
        //                  "key": "b_amt"
        //                },
        //                {
        //                  "value": "1",
        //                  "key": "tp3create"
        //                },
        //                {
        //                  "value": "1",
        //                  "key": "b_soc"
        //                }
        //              ],
        //              "tbId": 271525574352255893
        //            }
        //          ],
        //          "outOrderId": "792552836889",
        //          "buyerUserId": 983259358,
        //          "id": 271525574350255893
        //        }
        //      }
        //    ],
        //    "successOrderMap": [
        //      {
        //        "value": {
        //          "orderAmmount": 20000,
        //          "orderId": 271525574350255893,
        //          "success": true,
        //          "customOffer": false,
        //          "orderModel": {
        //            "buyerMemberId": "dongyl3333",
        //            "sellerAlipayId": "2088611490712805",
        //            "tradeTypeStr": "50060",
        //            "sellerUserId": 2248624086,
        //            "sellerMemberId": "b2b-2248624086",
        //            "succSumPayment": 20000,
        //            "buyerAlipayId": "2088031666286208",
        //            "orderEntryModel": [
        //              {
        //                "succSumPayment": 0,
        //                "outOrderId": "792552900841",
        //                "id": 271525574351255893,
        //                "attr": [
        //                  {
        //                    "value": "792552900841",
        //                    "key": "ods_ump_outid"
        //                  },
        //                  {
        //                    "value": "20.0",
        //                    "key": "b_ra"
        //                  },
        //                  {
        //                    "value": "4000",
        //                    "key": "af"
        //                  },
        //                  {
        //                    "value": "com.1688.business",
        //                    "key": "bizCode"
        //                  },
        //                  {
        //                    "value": "1.0",
        //                    "key": "b_wd"
        //                  },
        //                  {
        //                    "value": "74",
        //                    "key": "subFlowId"
        //                  },
        //                  {
        //                    "value": "200",
        //                    "key": "oup"
        //                  },
        //                  {
        //                    "value": "台",
        //                    "key": "b_unit"
        //                  },
        //                  {
        //                    "value": "126484005",
        //                    "key": "b_ci"
        //                  },
        //                  {
        //                    "value": "20000.0",
        //                    "key": "oba"
        //                  },
        //                  {
        //                    "value": "792552900841",
        //                    "key": "pSubOutId"
        //                  },
        //                  {
        //                    "value": "0",
        //                    "key": "b_ss"
        //                  },
        //                  {
        //                    "value": "CNY",
        //                    "key": "b_cc"
        //                  },
        //                  {
        //                    "value": "1000",
        //                    "key": "b_pr"
        //                  },
        //                  {
        //                    "value": "1",
        //                    "key": "b_seq"
        //                  },
        //                  {
        //                    "value": "200",
        //                    "key": "b_up"
        //                  },
        //                  {
        //                    "value": "0060006",
        //                    "key": "b_pmc"
        //                  },
        //                  {
        //                    "value": "pq",
        //                    "key": "b_of"
        //                  },
        //                  {
        //                    "value": "4000",
        //                    "key": "b_amt"
        //                  },
        //                  {
        //                    "value": "1",
        //                    "key": "tp3create"
        //                  },
        //                  {
        //                    "value": "1",
        //                    "key": "b_soc"
        //                  }
        //                ],
        //                "tbId": 271525574351255893
        //              },
        //              {
        //                "succSumPayment": 0,
        //                "outOrderId": "792552900842",
        //                "id": 271525574352255893,
        //                "attr": [
        //                  {
        //                    "value": "792552900842",
        //                    "key": "ods_ump_outid"
        //                  },
        //                  {
        //                    "value": "40.0",
        //                    "key": "b_ra"
        //                  },
        //                  {
        //                    "value": "16000",
        //                    "key": "af"
        //                  },
        //                  {
        //                    "value": "com.1688.business",
        //                    "key": "bizCode"
        //                  },
        //                  {
        //                    "value": "1.0",
        //                    "key": "b_wd"
        //                  },
        //                  {
        //                    "value": "74",
        //                    "key": "subFlowId"
        //                  },
        //                  {
        //                    "value": "400",
        //                    "key": "oup"
        //                  },
        //                  {
        //                    "value": "台",
        //                    "key": "b_unit"
        //                  },
        //                  {
        //                    "value": "124734044",
        //                    "key": "b_ci"
        //                  },
        //                  {
        //                    "value": "40000.0",
        //                    "key": "oba"
        //                  },
        //                  {
        //                    "value": "792552900842",
        //                    "key": "pSubOutId"
        //                  },
        //                  {
        //                    "value": "0",
        //                    "key": "b_ss"
        //                  },
        //                  {
        //                    "value": "CNY",
        //                    "key": "b_cc"
        //                  },
        //                  {
        //                    "value": "1000",
        //                    "key": "b_pr"
        //                  },
        //                  {
        //                    "value": "2",
        //                    "key": "b_seq"
        //                  },
        //                  {
        //                    "value": "400",
        //                    "key": "b_up"
        //                  },
        //                  {
        //                    "value": "006003",
        //                    "key": "b_pmc"
        //                  },
        //                  {
        //                    "value": "pq",
        //                    "key": "b_of"
        //                  },
        //                  {
        //                    "value": "16000",
        //                    "key": "b_amt"
        //                  },
        //                  {
        //                    "value": "1",
        //                    "key": "tp3create"
        //                  },
        //                  {
        //                    "value": "1",
        //                    "key": "b_soc"
        //                  }
        //                ],
        //                "tbId": 271525574352255893
        //              }
        //            ],
        //            "outOrderId": "792552836889",
        //            "buyerUserId": 983259358,
        //            "id": 271525574350255893
        //          }
        //        },
        //        "key": "quotation_0"
        //      }
        //    ]
        //  }
        //}
        private void SaveCreateOrder(AliPubHandle handle, string result)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(result);
            obj = obj.Value<JObject>("orderResult");
            if (obj == null) return;
            if (!obj.Value<bool>("success")) return;
            JArray orders = obj.Value<JArray>("commitResults");
            if (orders.Count == 0) return;
            string orderid = orders[0].Value<string>("orderId");
            obj = new JObject();
            obj.Add("AppKey", handle.Secret.AppKey);
            obj.Add("AppSecret", handle.Secret.AppSecret);
            obj.Add("TokenKey", handle.Secret.TokenKey);
            obj.Add("OrderId", orderid);
            handle.analyseBussdata("GetOrder2", JsonConvert.SerializeObject(obj));
            string content = handle.doPost();
            SaveGetOrder2(handle, content);
        }

        /// <summary>
        /// 留存1688收货单数据
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="result"></param>
        /// result格式:
        //        {
        //  "result": {
        //    "totalCount": 0,
        //    "realPrePageSize": 0,
        //    "modelList": [
        //      {
        //        "id": 199012010,
        //        "statusInfo": "wait_settle",
        //        "actualPayFee": 5000,
        //        "isSuccess": 1,
        //        "orderId": 271523654409255893,
        //        "gmtReceive": "20190330140438000+0800",
        //        "gmtCreate": "20190330140438000+0800",
        //        "gmtModified": "20190330140438000+0800",
        //        "sellerUserId": 2248624086,
        //        "sellerCompanyName": "阿里巴巴（中国）网络技术有限公司",
        //        "operatorUsername": "CN06REQ02",
        //        "buyerUserId": 983259358,
        //        "operatorUserId": 2200593250128,
        //        "buyerName": "dongyl3333",
        //        "receiveEntrylist": [
        //          {
        //            "id": 200454016,
        //            "receiveId": 199012010,
        //            "statusInfo": "wait_settle",
        //            "gmtCreate": "20190330140438000+0800",
        //            "gmtModified": "20190330140438000+0800",
        //            "orderId": 271523654409255893,
        //            "orderEntryId": 271523654410255893,
        //            "orderSourceType": "pq",
        //            "price": 100,
        //            "originalPrice": 100,
        //            "postFee": 0,
        //            "actualPayFee": 1000,
        //            "productName": "苹果",
        //            "sourceId": 1495899150727,
        //            "summImageUrl": "http://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //            "productMaterialCode": "006001",
        //            "attributes": ";originalPrice:100;snapshotImages:http#3B//cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png;productMaterialCode:006001;productName:苹果;b_ci:1036642;",
        //            "amount": 10000,
        //            "specItems": [],
        //            "quantityModel": {
        //              "realAmount": 10.0,
        //              "amountFactor": 1000.0,
        //              "calAmount": 10000,
        //              "realAmountStr": "10"
        //            }
        //          },
        //          {
        //            "id": 200454017,
        //            "receiveId": 199012010,
        //            "statusInfo": "wait_settle",
        //            "gmtCreate": "20190330140438000+0800",
        //            "gmtModified": "20190330140438000+0800",
        //            "orderId": 271523654409255893,
        //            "orderEntryId": 271523654411255893,
        //            "orderSourceType": "pq",
        //            "price": 200,
        //            "originalPrice": 200,
        //            "actualPayFee": 4000,
        //            "productName": "菠萝",
        //            "sourceId": 1495899160727,
        //            "summImageUrl": "http://cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png",
        //            "productMaterialCode": "006002",
        //            "attributes": ";originalPrice:200;snapshotImages:http#3B//cbu01.alicdn.com/cms/upload/2012/138/334/433831_61694261.png;productMaterialCode:006002;productName:菠萝;b_ci:125062004;",
        //            "amount": 20000,
        //            "specItems": [],
        //            "quantityModel": {
        //              "realAmount": 20.0,
        //              "amountFactor": 1000.0,
        //              "calAmount": 20000,
        //              "realAmountStr": "20"
        //            }
        //          }
        //        ]
        //      }
        //    ]
        //  }
        //}
        private void SaveGetOpBulkSettlement(AliPubHandle handle, string result)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(result);
            obj = obj.Value<JObject>("result");
            if (obj == null) return;
            JArray bills = obj.Value<JArray>("modelList");
            if (bills.Count == 0) return; //根据主键查询得到的结果，数组中至多1条数据
            AliOpBulkSettlement opbulk = (AliOpBulkSettlement)handle.Secret;
            DataTable mst = DbHelper.GetDataTable(_dbcon, string.Format("select * from aliopbulksettlement where appkey='{0}' and appsecret='{1}' and apptoken='{2}' and receiveid='{3}'", opbulk.AppKey, opbulk.AppSecret, opbulk.TokenKey, opbulk.RcvId));
            DataTable dtls = DbHelper.GetDataTable(_dbcon, string.Format("select * from aliopbulksettlementdtl where appkey='{0}' and appsecret='{1}' and apptoken='{2}' and receiveid='{3}'", opbulk.AppKey, opbulk.AppSecret, opbulk.TokenKey, opbulk.RcvId));
            DataRow dr;
            JArray items = bills[0].Value<JArray>("receiveEntrylist");//明细部分
            //处理表头
            if (mst.Rows.Count == 0)
            {
                //不存在则新增
                dr = mst.NewRow();
                dr["appkey"] = opbulk.AppKey;
                dr["appsecret"] = opbulk.AppSecret;
                dr["apptoken"] = opbulk.TokenKey;
                dr["receiveid"] = opbulk.RcvId;
            }
            else
            {
                dr = mst.Rows[0];
            }
            dr["status"] = bills[0].Value<string>("statusInfo");
            dr["actualpayfee"] = bills[0].Value<long>("actualPayFee"); //1688接口说明中有此属性
            dr["gmtcreate"] = DateTime.ParseExact(bills[0].Value<string>("gmtCreate"), "yyyyMMddHHmmssfffzz00", null);
            dr["issuccess"] = bills[0].Value<int>("isSuccess");
            dr["orderid"] = bills[0].Value<string>("orderId");
            dr["gmtreceive"] = DateTime.ParseExact(bills[0].Value<string>("gmtReceive"), "yyyyMMddHHmmssfffzz00", null);
            dr["selleruserid"] = bills[0].Value<long>("sellerUserId");
            dr["sellercompanyname"] = bills[0].Value<string>("sellerCompanyName");
            dr["operatorusername"] = bills[0].Value<string>("operatorUsername");
            dr["buyeruserid"] = bills[0].Value<long>("buyerUserId");
            dr["operatoruserid"] = bills[0].Value<long>("operatorUserId");
            dr["buyername"] = bills[0].Value<string>("buyerName");
            if (mst.Rows.Count == 0)
            {
                mst.Rows.Add(dr);
            }
            //处理表体    

            bool exists = false;
            JObject tmpobj;
            foreach (DataRow r in dtls.Rows)
            {
                //datatable中的数据若存在于返回内容中则修改
                //若不存在则删除
                exists = false;
                foreach (JToken item in items)
                {
                    if (item.Value<string>("id") == r.Field<string>("id"))
                    {
                        exists = true;
                        r["productcode"] = item.Value<string>("productMaterialCode");
                        r["productname"] = item.Value<string>("productName");
                        r["status"] = item.Value<string>("statusInfo");
                        r["orderid"] = item.Value<string>("orderId");
                        r["orderentryid"] = item.Value<string>("orderEntryId");
                        r["ordersourcetype"] = item.Value<string>("orderSourceType");
                        r["price"] = item.Value<long>("price");
                        r["originalprice"] = item.Value<long>("originalPrice");
                        r["postfee"] = item.Value<long>("postFee");
                        r["actualpayfee"] = item.Value<long>("actualPayFee");
                        r["sourceid"] = item.Value<string>("sourceId");
                        tmpobj = item.Value<JObject>("quantityModel");
                        if (tmpobj != null)
                        {
                            r["qty"] = tmpobj.Value<decimal>("realAmount");
                        }
                        break;
                    }
                }
                if (!exists)
                {
                    r.Delete();
                }
            }
            foreach (JToken item in items)
            {
                //若返回数据不存在于datatable则新增
                if (dtls.Select(string.Format("id='{0}'", item.Value<string>("id"))).Length == 0)
                {
                    dr = dtls.NewRow();
                    dr["appkey"] = opbulk.AppKey;
                    dr["appsecret"] = opbulk.AppSecret;
                    dr["apptoken"] = opbulk.TokenKey;
                    dr["receiveid"] = opbulk.RcvId;
                    dr["id"] = item.Value<string>("id");
                    dr["productcode"] = item.Value<string>("productMaterialCode");
                    dr["productname"] = item.Value<string>("productName");
                    dr["status"] = item.Value<string>("statusInfo");
                    dr["orderid"] = item.Value<string>("orderId");
                    dr["orderentryid"] = item.Value<string>("orderEntryId");
                    dr["ordersourcetype"] = item.Value<string>("orderSourceType");
                    dr["price"] = item.Value<long>("price");
                    dr["originalprice"] = item.Value<long>("originalPrice");
                    dr["postfee"] = item.Value<long>("postFee");
                    dr["actualpayfee"] = item.Value<long>("actualPayFee");
                    dr["sourceid"] = item.Value<string>("sourceId");
                    tmpobj = item.Value<JObject>("quantityModel");
                    if (tmpobj != null)
                    {
                        dr["qty"] = tmpobj.Value<decimal>("realAmount");
                    }
                    dtls.Rows.Add(dr);
                }
            }


            //更新数据
            DbHelper.Open(_dbcon);
            try
            {
                DbHelper.BeginTran(_dbcon);
                if (DbHelper.Update(_dbcon, mst, "select * from aliopbulksettlement") > 0)
                {
                    if (DbHelper.Update(_dbcon, dtls, "select * from aliopbulksettlementdtl") > 0)
                    {
                        DbHelper.CommitTran(_dbcon);
                    }
                    else
                    {
                        throw new Exception("明细aliopbulksettlementdtl更新失败");
                    }
                }
                else
                {
                    throw new Exception("表头aliopbulksettlement更新失败");
                }
            }
            catch (Exception e)
            {
                DbHelper.RollbackTran(_dbcon);
                throw e;
            }
            finally
            {
                DbHelper.Close(_dbcon);
            }

        }
        /// <summary>
        /// 留存供应商信息
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="result"></param>
        ///result格式:
        //{
        //  "supplier": {
        //    "loginId": "b测试账号003",
        //    "contactInfo": {
        //      "gender": "M",
        //      "phone": "86-0571-3555555555",
        //      "name": "乔的石",
        //      "contactAddress": "中国-广州-广州市白云区-网商路699号",
        //      "fax": "86-0571-22211111"
        //    },
        //    "companyInfo": {
        //      "icrInfo": {
        //        "bankAccount": "",
        //        "principal": "aaa",
        //        "bank": "",
        //        "registeredCapital": "500万元",
        //        "companyAddress": "浙江/杭州",
        //        "registrationId": "111111111111111111",
        //        "dateOfEstablishment": "1999",
        //        "name": "阿里巴巴（中国）网络技术有限公司",
        //        "businessScope": "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        //        "businessTerm": "Thu Sep 09 00:00:00 CST 1999-Sat Sep 08 00:00:00 CST 2040",
        //        "redistrationAuthority": "杭州市高新区（滨江）市场监督管理局",
        //        "enterpriseType": "有限责任公司(自然人投资或控股)"
        //      },
        //      "homepageUrl": [
        //        "http://www.google.com"
        //      ],
        //      "companySummary": "测试一下啦啦啦啦啦啦啦啦啦测试一下啦啦啦啦啦啦啦啦啦测试一下啦啦啦啦啦啦啦啦啦测试一下啦啦啦啦啦啦啦啦啦",
        //      "governmentInvitedSupplier": false,
        //      "tp": true
        //    },
        //    "businessAbility": {
        //      "buyerProtected": true,
        //      "employeesCount": "51 - 100 人",
        //      "hasTaobaoSeller": false,
        //      "strokeBuyerCount90days": 0,
        //      "hasMingQiBuyer": true,
        //      "bindAlipay": true,
        //      "vatInvoice": true,
        //      "strokeCountIn90days": 0,
        //      "alipayType": "个人支付宝"
        //    },
        //    "companyCredit": {
        //      "truthfulDescription": 0,
        //      "serviceAttitude": 43,
        //      "buyersCommentsIn180days": 0,
        //      "complaintRateIn90days": 0,
        //      "refundRateIn90days": 0,
        //      "arrivalRate": 41,
        //      "supplierLevel": 2,
        //      "averageDeliverySpeedIn90days": 0,
        //      "authType": "av"
        //    },
        //    "certifications": {
        //      "icpLicence": [
        //        {
        //          "expiryDate": "2020-01-16 00:00:00",
        //          "code": "54545454545454",
        //          "certificatePicture": [
        //            "img/ibank/2016/352/221/3013122253.jpg"
        //          ],
        //          "certifyingAuthority": "gdgghgf",
        //          "certificateName": "食品生产许可证（酒）",
        //          "effectiveDate": "2016-05-04 00:00:00"
        //        },
        //        {
        //          "expiryDate": "2020-01-08 00:00:00",
        //          "code": "564564565465464565465",
        //          "certificatePicture": [
        //            "img/ibank/2016/713/641/3013146317.jpg"
        //          ],
        //          "certifyingAuthority": "法规范和法国和法国队和",
        //          "certificateName": "食品流通许可证（酒）",
        //          "effectiveDate": "2016-05-04 00:00:00"
        //        },
        //        {
        //          "expiryDate": "2033-01-12 00:00:00",
        //          "code": "343434",
        //          "certificatePicture": [
        //            "img/ibank/2018/855/790/9545097558.jpg"
        //          ],
        //          "certifyingAuthority": "送福利是放假了",
        //          "certificateName": "食品流通许可证（保健食品）",
        //          "effectiveDate": "2018-10-01 00:00:00"
        //        }
        //      ],
        //      "taxRegistration": [
        //        {
        //          "expiryDate": "",
        //          "code": "324",
        //          "certificatePicture": [
        //            "img/ibank/2018/677/601/9438106776.jpg"
        //          ],
        //          "certifyingAuthority": "25",
        //          "certificateName": "三/五证合一",
        //          "effectiveDate": "2018-09-01 00:00:00"
        //        }
        //      ]
        //    },
        //    "businessInfo": {
        //      "productionService": [
        //        "成衣",
        //        "布料",
        //        "阿斯顿发大水",
        //        "阿斯顿f",
        //        "阿斯顿f",
        //        "%……￥"
        //      ],
        //      "mainIndustries": "田径用品、篮球用品、二极管、人造毛皮、男装",
        //      "businessModel": "生产加工",
        //      "businessAddress": "浙江 杭州市上城区",
        //      "enterpriseType": "有限责任公司(自然人投资或控股)"
        //    }
        //  }
        //}
        private void SaveGetSupplier(AliPubHandle handle, string result)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(result);
            obj = obj.Value<JObject>("supplier");
            if (obj == null) return;
            AliSupplierInfo supply = (AliSupplierInfo)handle.Secret;
            DataTable mst = DbHelper.GetDataTable(_dbcon, string.Format("select * from alisupplyer where appkey='{0}' and appsecret='{1}' and apptoken='{2}' and memberid='{3}'", supply.AppKey, supply.AppSecret, supply.TokenKey, supply.MemberId));
            DataTable dtls = DbHelper.GetDataTable(_dbcon, string.Format("select * from alicertification where appkey='{0}' and appsecret='{1}' and apptoken='{2}' and memberid='{3}'", supply.AppKey, supply.AppSecret, supply.TokenKey, supply.MemberId));
            DataRow dr;
            JObject tmpobj;
            if (mst.Rows.Count == 0)
            {
                //不存在则新增
                dr = mst.NewRow();
                dr["appkey"] = supply.AppKey;
                dr["appsecret"] = supply.AppSecret;
                dr["apptoken"] = supply.TokenKey;
                dr["memberid"] = supply.MemberId;
            }
            else
            {
                dr = mst.Rows[0];
            }
            dr["loginid"] = obj.Value<string>("loginId");
            tmpobj = obj.Value<JObject>("contactInfo");
            if (tmpobj != null)
            {
                dr["contact_gender"] = tmpobj.Value<string>("gender");
                dr["contact_phone"] = tmpobj.Value<string>("phone");
                dr["contact_name"] = tmpobj.Value<string>("name");
                dr["contact_address"] = tmpobj.Value<string>("contactAddress");
                dr["contact_fax"] = tmpobj.Value<string>("fax");
            }
            tmpobj = obj.Value<JObject>("companyInfo");
            if (tmpobj != null)
            {
                dr["companysummary"] = tmpobj.Value<string>("companySummary");
                dr["governmentinvitedsupplier"] = tmpobj.Value<bool>("governmentInvitedSupplier") ? 1 : 0;
                tmpobj = tmpobj.Value<JObject>("icrInfo");
                if (tmpobj != null)
                {
                    dr["bankaccount"] = tmpobj.Value<string>("bankAccount");
                    dr["principal"] = tmpobj.Value<string>("principal");
                    dr["bank"] = tmpobj.Value<string>("bank");
                    dr["registeredcapital"] = tmpobj.Value<string>("registeredCapital");
                    dr["companyaddress"] = tmpobj.Value<string>("companyAddress");
                    dr["registrationid"] = tmpobj.Value<string>("registrationId");
                    dr["dateofestablishment"] = tmpobj.Value<string>("dateOfEstablishment");
                    dr["companyname"] = tmpobj.Value<string>("name");
                    dr["businessscope"] = tmpobj.Value<string>("businessScope");
                    dr["businessterm"] = tmpobj.Value<string>("businessTerm");
                    dr["redistrationauthority"] = tmpobj.Value<string>("redistrationAuthority");
                    dr["enterprisetype"] = tmpobj.Value<string>("enterpriseType");
                }
            }
            tmpobj = obj.Value<JObject>("businessAbility");
            if (tmpobj != null)
            {
                dr["employeescount"] = tmpobj.Value<string>("employeesCount");
                dr["bindalipay"] = tmpobj.Value<bool>("bindAlipay") ? 1 : 0;
                dr["vatinvoice"] = tmpobj.Value<bool>("vatInvoice") ? 1 : 0;
                dr["alipaytype"] = tmpobj.Value<string>("alipayType");
            }
            tmpobj = obj.Value<JObject>("businessInfo");
            if (tmpobj != null)
            {
                JArray arr = tmpobj.Value<JArray>("productionService");
                dr["productionservice"] =arr==null?Convert.DBNull:arr.ToString();
                dr["mainIndustries"] = tmpobj.Value<string>("mainIndustries");
                dr["businessModel"] = tmpobj.Value<string>("businessModel");
                dr["businessAddress"] = tmpobj.Value<string>("businessAddress");
            }
            if (mst.Rows.Count == 0)
            {
                mst.Rows.Add(dr);
            }

            //处理明细
            tmpobj = obj.Value<JObject>("certifications");
            if (tmpobj != null)
            {
                JArray arr = tmpobj.Value<JArray>("icpLicence");
                bool exists = false;
                DateTime dt;
                foreach (DataRow r in dtls.Rows)
                {
                    //datatable中的数据若存在于返回内容中则修改
                    //若不存在则删除
                    exists = false;
                    if (arr != null)
                    {
                        foreach (JToken item in arr)
                        {
                            if (item.Value<string>("code") == r.Field<string>("code"))
                            {
                                exists = true;
                                if (DateTime.TryParse(item.Value<string>("expiryDate"), out dt))
                                {
                                    r["expirydate"] = dt;
                                }
                                if (DateTime.TryParse(item.Value<string>("effectiveDate"), out dt))
                                {
                                    r["effectivedate"] = dt;
                                }
                                r["authority"] = item.Value<string>("certifyingAuthority");
                                r["name"] = item.Value<string>("certificateName");
                            }
                        }
                    }
                    if (!exists)
                    {
                        r.Delete();
                    }
                }
                if (arr != null)
                {
                    foreach (JToken item in arr)
                    {
                        //若返回数据不存在于datatable则新增
                        if (dtls.Select(string.Format("code='{0}'", item.Value<string>("code"))).Length == 0)
                        {
                            dr = dtls.NewRow();
                            dr["appkey"] = supply.AppKey;
                            dr["appsecret"] = supply.AppSecret;
                            dr["apptoken"] = supply.TokenKey;
                            dr["memberid"] = supply.MemberId;
                            dr["code"] = item.Value<string>("code");
                            dr["authority"] = item.Value<string>("certifyingAuthority");
                            dr["name"] = item.Value<string>("certificateName");
                            if (DateTime.TryParse(item.Value<string>("expiryDate"), out dt))
                            {
                                dr["expirydate"] = dt;
                            }
                            if (DateTime.TryParse(item.Value<string>("effectiveDate"), out dt))
                            {
                                dr["effectivedate"] = dt;
                            }

                            dtls.Rows.Add(dr);
                        }
                    }
                }

            }
            //更新数据
            DbHelper.Open(_dbcon);
            try
            {
                DbHelper.BeginTran(_dbcon);
                if (DbHelper.Update(_dbcon, mst, "select * from alisupplyer") > 0)
                {
                    if (DbHelper.Update(_dbcon, dtls, "select * from alicertification") > 0)
                    {
                        DbHelper.CommitTran(_dbcon);
                    }
                    else
                    {
                        throw new Exception("明细alicertification更新失败");
                    }
                }
                else
                {
                    throw new Exception("表头alisupplyer更新失败");
                }
            }
            catch (Exception e)
            {
                DbHelper.RollbackTran(_dbcon);
                throw e;
            }
            finally
            {
                DbHelper.Close(_dbcon);
            }



        }
    }
}
