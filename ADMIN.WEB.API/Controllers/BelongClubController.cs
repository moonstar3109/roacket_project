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
    public class BelongClubController : ApiController
    {
        /// <remarks>
        /// 등록을 위한 클럽 조회(협회소속)
        /// 
        ///     {   
        ///           "assIdx" : 1,
        ///           "regionCode" : 2,
        ///           "option" : 1, [0 : 전체, 1:클럽명, 2:클럽회장, 3:연락처]
        ///           "search" : "",
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("BelongClub")]
        public object Get(string filter)
        {
            JObject root = JObject.Parse(filter);

            int assIdx = Convert.ToInt32(root["assIdx"].ToString());
            int regionCode = Convert.ToInt32(root["regionCode"].ToString());
            int option = Convert.ToInt32(root["option"].ToString());
            string search = root["search"].ToString();


            DataSet ds = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAllClub(assIdx,regionCode, option, search);
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
                dic["totalCnt"] = dt2.Rows[0]["totalCnt"];
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt1;
            dic["totalCnt"] = dt2.Rows[0]["totalCnt"];
            return dic;
        }
    }
}
