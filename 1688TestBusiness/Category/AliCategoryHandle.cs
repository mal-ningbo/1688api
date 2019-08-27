using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ali1688Business.Model;
using Ali1688Business.Model.Category;
namespace Ali1688Business.Category
{
    public class AliCategoryHandle : AliPubHandle
    {
        public override void analyseBussdata(string userServer, string bussdata)
        {
            UserSrvType = userServer;
            BusData = bussdata;
            switch (userServer)
            {
                case "AddCategory":
                    //创建1688资源分类
                    //eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","Categories":[{CategoryName:"钢材",CategoryId:"ng001"},{ CategoryName: "槽钢",CategoryId: "ng00101",ParentId: "ng001" },{ CategoryName: "角钢",CategoryId: "ng00102",ParentId: "ng001" }]}
                    setAddCategoryParams(bussdata);
                    break;
                case "ModifyCategory":
                    //修改1688资源分类
                    //eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","Categories":[{CategoryName:"钢材",CategoryId:"365769"},{ CategoryName: "槽钢",CategoryId: "365770",ParentId: "ng001" },{ CategoryName: "角钢",CategoryId: "365771",ParentId: "ng001" }]}
                    //ps:此时CategoryId 为1688的主键
                    setModifyCategoryParams(bussdata);
                    break;
                case "DelCategory":
                    //删除1688资源分类
                    setDelCategoryParams(bussdata);
                    break;
                case "GetCategoryById":
                    //通过1688主键查找单个资源分类
                    //eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a","CategoryId":"365770"}
                    setGetCategoryByIdParams(bussdata);
                    break;
                case "QueryAllCategory":
                    //查询所有1688资源分类
                    //eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"9ee0ac93-0aae-45f5-a671-ca7564280a3a"}
                    setQueryAllCategoryParams(bussdata);
                    break;
                default:
                    UserSrvType = "";
                    throw new Exception(string.Format("{0}业务类型:{1},参数错误", "类目接口", userServer));
            }
        }

        private void setAddCategoryParams(string busdata)
        {
            CategoryList list = JsonConvert.DeserializeObject<CategoryList>(busdata);
            Secret = list;
            if (list.Categories == null) {
                throw new Exception("参数Categories为空");
            }
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            JArray arr = new JArray();
            JObject category = null, att = new JObject();
            att.Add("isExternalSupplier", true);
            foreach (UserCategory item in list.Categories) {
                category = new JObject();
                if (!string.IsNullOrWhiteSpace(item.CategoryName)) category.Add("categoryName", item.CategoryName);
                if (!string.IsNullOrWhiteSpace(item.CategoryId)) category.Add("categoryId", item.CategoryId);
                if (!string.IsNullOrWhiteSpace(item.ParentId)) category.Add("parentId", item.ParentId);
                category.Add("attribute", att);
                arr.Add(category);
            }
            UrlParams.Add("categoryList", JsonConvert.SerializeObject(arr));
            FunName = "cn.alibaba.open/caigou.api.category.addUserCategory";
        }
        private void setModifyCategoryParams(string busdata)
        {
            CategoryList list = JsonConvert.DeserializeObject<CategoryList>(busdata);
            Secret = list;
            if (list.Categories == null)
            {
                throw new Exception("参数Categories为空");
            }
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            JArray arr = new JArray();
            JObject category = null, att = new JObject();
            att.Add("isExternalSupplier", false);
            foreach (UserCategory item in list.Categories)
            {
                category = new JObject();
                if (!string.IsNullOrWhiteSpace(item.CategoryName)) category.Add("categoryName", item.CategoryName);
                if (!string.IsNullOrWhiteSpace(item.CategoryId)) category.Add("categoryId", item.CategoryId);
                if (!string.IsNullOrWhiteSpace(item.ParentId)) category.Add("parentId", item.ParentId);
                category.Add("attribute", att);
                arr.Add(category);
            }
            UrlParams.Add("categoryList", JsonConvert.SerializeObject(arr));
            FunName = "cn.alibaba.open/caigou.api.category.modifyUserCategory";
        }
        private void setQueryAllCategoryParams(string busdata) {
            Secret = JsonConvert.DeserializeObject<AliSecret>(busdata);
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            FunName = "cn.alibaba.open/caigou.api.category.queryAll";
        }
        private void setGetCategoryByIdParams(string busdata)
        {
            UserCategory category= JsonConvert.DeserializeObject<UserCategory>(busdata);
            Secret = category;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("categoryId",category.CategoryId);
            FunName = "cn.alibaba.open/caigou.api.category.getById";
        }
        private void setDelCategoryParams(string busdata) {
            CategoryList list = JsonConvert.DeserializeObject<CategoryList>(busdata);
            Secret = list;
            if (list.DelIds == null)
            {
                throw new Exception("参数DelIds为空");
            }
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("categoryIds",JsonConvert.SerializeObject( list.DelIds ));
            FunName = "cn.alibaba.open/caigou.api.category.deleteUserCategory";
        }
    }
}
