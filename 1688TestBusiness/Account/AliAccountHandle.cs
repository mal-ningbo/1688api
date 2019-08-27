using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ali1688Business.Model;
using Ali1688Business.Model.Account;
using Newtonsoft.Json;
namespace Ali1688Business.Account
{
    /// <summary>
    /// 与1688账户相关的操作
    /// </summary>
    public class AliAccountHandle:AliPubHandle
    {
        public override void analyseBussdata(string userServer, string bussdata)
        {
            UserSrvType = userServer;
            BusData=bussdata;
           switch (userServer){
                case "GetSubAccountBindingList":
                    //取1688账号与i6p账号绑定清单
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13"}
                    SetGetSubAccountBindingListParams(bussdata);          
                    break;
                case "BindSubAccount":
                    //绑定或解绑i6p账号
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","SubAccount":"1688子账号","NgLogid":"9999"}
                    SetBindSubAccountParams(bussdata);
                    break;
                case "GetMemberIdsByLoginIds":
                    //通过列表loginIds查询对应的memberId,批量最大数不要超过110个
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","LoginIds":["loginid1","loginid2"]}
                    //api返回格式{
                    // "良和建设": "b2b-284590656421eaa",
                    // "启盛新材料": "b2b-3461502808b3bc2"
                    //}
                    SetGetMemberIdsByLoginIdsParams(bussdata);
                    break;
                case "GetLoginIdsByMemberIds":
                    //通过列表memberId查询对应的loginIds,批量最大数不要超过110个
                    //bussdata,eg:{"AppKey":"334362","AppSecret":"w5v9Xu2sK2y5","TokenKey":"aa3cbd3b-de0c-4779-8596-6724864d7a13","MemberIds":["memberId1","memberId2"]}
                    // api返回格式{
                    //"loginIdMap": {
                    // "b2b-3461502808b3bc2": "启盛新材料",
                    //  "b2b-284590656421eaa": "良和建设"
                    // }
                    //}
                    SetGetLoginIdsByMemberIdsParams(bussdata);
                    break;
                case "CreateSubAccount":
                    SetCreateSubAccountParams(bussdata);
                    break;
                default:
                    UserSrvType = "";
                    throw new Exception(string.Format("{0}业务类型:{1},参数错误","账号接口", userServer));            
            }
        }

        #region 私有函数，设置不同业务类型的参数
        /// <summary>
        /// 取1688账号与i6p账号绑定清单，设置参数
        /// </summary>
        /// <param name="bussdata"></param>
        private void SetGetSubAccountBindingListParams(string bussdata)
        {
            Secret = JsonConvert.DeserializeObject<AliSecret>(bussdata);
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            FunName = "cn.alibaba.open/caigou.api.account.listSubAcccounts";
        }
        /// <summary>
        /// 将i6p账号与1688子帐号进行绑定/解绑 设置参数
        /// </summary>
        /// <param name="bussdata"></param>
        private void SetBindSubAccountParams(string bussdata)
        {
            AliAccountNgLogidMap map =JsonConvert.DeserializeObject<AliAccountNgLogidMap>(bussdata);
            Secret = map;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("subAccountloginId", map.SubAccount);
            if (string.IsNullOrEmpty(map.NgLogid))
            {
                UrlParams.Add("employeeId", string.Format("#{0}#", map.SubAccount));
            }
            else
            {
                UrlParams.Add("employeeId", map.NgLogid);
            }
            FunName = "cn.alibaba.open/caigou.api.account.bindAccount";
        }

        private void SetGetMemberIdsByLoginIdsParams(string bussdata)
        {
            AliMemberidLogidMap map = JsonConvert.DeserializeObject<AliMemberidLogidMap>(bussdata);
            Secret = map;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("loginIds", JsonConvert.SerializeObject(map.LoginIds));
            //StringBuilder sb = new StringBuilder();
            //sb.Append("[");
            //for(int i = 0; i < map.LoginIds.Count; i++)
            //{
            //    sb.AppendFormat("\"{0}\"", map.LoginIds[i]);
            //    if(i< map.LoginIds.Count - 1)
            //    {
            //        sb.Append(",");
            //    }
            //}
            //sb.Append("]");
            //UrlParams.Add("loginIds", sb.ToString());
            FunName = "cn.alibaba.open/convertMemberIdsByLoginIds";
        }

        private void SetGetLoginIdsByMemberIdsParams(string bussdata)
        {
            AliMemberidLogidMap map = JsonConvert.DeserializeObject<AliMemberidLogidMap>(bussdata);
            Secret = map;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("access_token", Secret.TokenKey);
            UrlParams.Add("memberIds", JsonConvert.SerializeObject(map.MemberIds));
            //StringBuilder sb = new StringBuilder();
            //sb.Append("[");
            //for (int i = 0; i < map.MemberIds.Count; i++)
            //{
            //    sb.AppendFormat("\"{0}\"", map.MemberIds[i]);
            //    if (i < map.MemberIds.Count - 1)
            //    {
            //        sb.Append(",");
            //    }
            //}
            //sb.Append("]");
            //UrlParams.Add("memberIds", sb.ToString());
            FunName = "cn.alibaba.open/convertLoginIdsByMemberIds";
        }
        /// <summary>
        /// 创建1688子账号设置参数
        /// </summary>
        /// <param name="bussdata"></param>
        private void SetCreateSubAccountParams(string bussdata)
        {
            AliEmpAccount emp = JsonConvert.DeserializeObject<AliEmpAccount>(bussdata);
            Secret = emp;
            UrlParams = new Dictionary<string, string>();
            UrlParams.Add("employeeId", emp.employeeId);
            UrlParams.Add("name", emp.name);
            if (!string.IsNullOrWhiteSpace(emp.mobileNo)) UrlParams.Add("mobileNo", emp.mobileNo);
            if (!string.IsNullOrWhiteSpace(emp.email)) UrlParams.Add("email", emp.email);
            UrlParams.Add("sex", emp.sex);
            if (!string.IsNullOrWhiteSpace(emp.department)) UrlParams.Add("department", emp.department);
            if (!string.IsNullOrWhiteSpace(emp.role)) UrlParams.Add("role", emp.role);
            UrlParams.Add("access_token", Secret.TokenKey);
            FunName = "cn.alibaba.open/caigou.api.account.createSubAccount";
        }
        #endregion
    }
}
