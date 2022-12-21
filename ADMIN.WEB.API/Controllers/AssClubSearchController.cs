using ADMIN.API.DA;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AssClubSearchController : ApiController
    {
        /// <remarks>
        /// 클럽검색
        /// 
        ///     {
        ///         "regionCode" : 1,
        ///         "option" : 1, [0 : 전체, 1:클럽명, 2:클럽회장, 3:연락처]
        ///         "search" : "",
        ///         "areaCode" : 0  [ 0 : 전체, 1 > 각 지역번호]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssClubSearch")]
        public object Get(string filter) 
        {
            JObject root = JObject.Parse(filter);
            
            int regionCode = Convert.ToInt32(root["regionCode"].ToString());
            int option = Convert.ToInt32(root["option"].ToString());
            int areaCode = Convert.ToInt32(root["areaCode"].ToString());
            string search = root["search"].ToString();


            DataSet ds = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAssClubSearch(regionCode, areaCode, option, search);
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
                dic["clubCnt"] = dt2.Rows[0]["clubCnt"];
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt1;
            dic["clubCnt"] = dt2.Rows[0]["clubCnt"];
            return dic;
        }
    }
}
