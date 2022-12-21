using ADMIN.API.DA;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AllMemberController : ApiController
    {
        /// <remarks>
        /// 전체회원 검색
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "option" : 1,[ 1 : 이름, 2 : 핸드폰]
        ///         "search" : ""
        ///     }
        ///      
        /// </remarks> 
        /// <returns></returns>
        [Route("AllMember")]
        public object Get(string filter)
        {
            JObject root = JObject.Parse(filter);
            
            int assIdx = Convert.ToInt32(root["assIdx"].ToString());
            int option = Convert.ToInt32(root["option"].ToString());
            string search = root["search"].ToString() == "" ? string.Empty : root["search"].ToString();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            DataSet ds = null;

            try
            {            
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAllMember(assIdx, option, search);
                }
            } 
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];

            if (dt1.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["data"] = new DataTable();
                return dic;
            }
          
            dic["success"] = true;
            dic["data"] = dt1;
            dic["memberCnt"] = dt2.Rows[0]["memberCnt"];
            return dic;
        }
    }
}
