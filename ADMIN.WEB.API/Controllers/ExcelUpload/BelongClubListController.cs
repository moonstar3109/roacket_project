using ADMIN.API.DA;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers.ExcelUpload
{
    public class BelongClubListController : ApiController
    {
        /// <remarks>
        /// 협회소속 클럽 목록
        /// 
        ///     {   
        ///           "region" : 1,
        ///           "area" : 2,
        ///           "assIdx" : 2
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("BelongClubList")]
        public object Get(string filter)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            JObject root = JObject.Parse(filter);

            int region = Convert.ToInt32(root["region"].ToString());
            int area = Convert.ToInt32(root["area"].ToString());
            int assIdx = Convert.ToInt32(root["assIdx"].ToString());

            DataTable dt = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetBelongClubList(region, area, assIdx);
                }
            }
            catch (Exception e) 
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }
            
            if(dt.Rows.Count > 0)
            {
                dic["success"] = true;
                dic["data"] = dt;
            }
            else
            {
                dic["success"] = true;
                dic["data"] = new DataTable();
            }

            return dic;
        }
    }
}
