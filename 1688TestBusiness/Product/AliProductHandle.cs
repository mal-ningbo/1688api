using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ali1688Business.Model.Product;

namespace Ali1688Business.Product
{
    public class AliProductHandle:AliPubHandle
    {
        public override void analyseBussdata(string userServer, string bussdata)
        {
            UserSrvType = userServer;
            BusData = bussdata;
            switch (userServer)
            {
                case "AddProduct":
                    //创建1688资源分类
                    //eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","ProductCode":"ZH001","ProductName":"商品砼","Unit":"件","Remark":"说明描述","Instruction":"副标题","Price":999999,"CategoryId":365776,"UserId":0,""Attribes":[{"name":"外置配件","unit":"件","values":["红色","黑色","绿色","蓝色"]},{"name":"附属装备","unit":"个","values":["黄色","白色","青灰色","紫色"]}]}
                    SetAddProductParams(bussdata);
                    break;
                case "ModifyProduct":
                    //修改1688物料
                    //eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","ProductId":4926466,"ProductCode":"ZH001","ProductName":"商品砼","Unit":"件","Remark":"说明描述","Instruction":"副标题","Price":999999,"CategoryId":365776,""Attribes":[{"name":"外置配件","unit":"件","values":["红色","黑色","绿色","蓝色"]},{"name":"附属装备","unit":"个","values":["黄色","白色","青灰色","紫色"]}]}
                    SetModifyProductParams(bussdata);
                    break;
                case "DelProduct":
                    //根据id删除1688物料
                    //eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","ProductId":4926443}
                    SetDelProductByIdParams(bussdata);
                    break;
                case "QueryProductById":
                    //根据1688主键查找物料
                    //eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","ProductId":4926443}
                    SetQueryProductByIdParams(bussdata);
                    break;
                case "QueryProduct":
                    //分页查询1688物料
                    //eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a"}
                    
                    break;
                //case "QueryProductByCodeList":
                //    //精确查询产品列表
                //    break;
                default:
                    UserSrvType = "";
                    throw new Exception(string.Format("{0}业务类型:{1},参数错误", "物料接口", userServer));
            }
        }

        private void SetAddProductParams(string busdata) {
            UserProduct product = JsonConvert.DeserializeObject<UserProduct>(busdata);
            Secret = product;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("productCode", product.ProductCode);
            UrlParams.Add("title", product.ProductName);
            UrlParams.Add("unit", product.Unit);
            UrlParams.Add("remark", product.Remark);
            UrlParams.Add("instruction", product.Instruction);
            UrlParams.Add("referencePrice", product.Price.ToString());
            UrlParams.Add("userCategoryId", product.CategoryId.ToString());
            if(product.UserId>0) UrlParams.Add("userId", product.UserId.ToString());
            if (product.Attribes != null)
            {
                JArray arr = new JArray(),attribes=null;
                JObject obj = null;
                foreach (ProductAttribe att in product.Attribes)
                {
                    if (string.IsNullOrWhiteSpace(att.Name)) continue;
                    obj = new JObject();
                    obj.Add("keyAttr", false);
                    obj.Add("name", att.Name);
                    obj.Add("unit", att.Unit);
                    if (att.Values != null)
                    {
                        attribes = new JArray();
                        foreach (string s in att.Values) {
                            attribes.Add(s);
                        }
                        obj.Add("values", attribes);
                    }
                    arr.Add(obj);
                }
                if (arr.Count > 0) UrlParams.Add("productAttributes",JsonConvert.SerializeObject(arr));
            }
            FunName = "cn.alibaba.open/caigou.api.product.addProduct";
        }

        private void SetModifyProductParams(string busdata) {
            UserProduct product = JsonConvert.DeserializeObject<UserProduct>(busdata);
            Secret = product;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("productId", product.ProductId.ToString());
            UrlParams.Add("productCode", product.ProductCode);
            UrlParams.Add("title", product.ProductName);
            UrlParams.Add("unit", product.Unit);
            UrlParams.Add("remark", product.Remark);
            UrlParams.Add("instruction", product.Instruction);
            UrlParams.Add("referencePrice", product.Price.ToString());
            UrlParams.Add("userCategoryId", product.CategoryId.ToString());
            if (product.Attribes != null)
            {
                JArray arr = new JArray(), attribes = null;
                JObject obj = null;
                foreach (ProductAttribe att in product.Attribes)
                {
                    if (string.IsNullOrWhiteSpace(att.Name)) continue;
                    obj = new JObject();
                    obj.Add("keyAttr", false);
                    obj.Add("name", att.Name);
                    obj.Add("unit", att.Unit);
                    if (att.Values != null)
                    {
                        attribes = new JArray();
                        foreach (string s in att.Values)
                        {
                            attribes.Add(s);
                        }
                        obj.Add("values", attribes);
                    }
                    arr.Add(obj);
                }
                if(arr.Count>0) UrlParams.Add("productAttributes", JsonConvert.SerializeObject(arr));
            }
            FunName = "cn.alibaba.open/caigou.api.product.modifyProduct";
        }

        private void SetQueryProductByIdParams(string busdata)
        {
            UserProduct product = JsonConvert.DeserializeObject<UserProduct>(busdata);
            Secret = product;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("productId", product.ProductId.ToString());           
            FunName = "cn.alibaba.open/caigou.api.product.queryProductById";
        }

        private void SetDelProductByIdParams(string busdata)
        {
            UserProduct product = JsonConvert.DeserializeObject<UserProduct>(busdata);
            Secret = product;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("productId", product.ProductId.ToString());
            FunName = "cn.alibaba.open/caigou.api.product.deleteProductById";
        }
    }
}
