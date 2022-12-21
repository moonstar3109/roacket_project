using ADMIN.API.DA;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class BelongController : ApiController
    {
        /// <remarks>
        /// 협회소속 지역리스트 조회
        /// 
        ///     {   
        ///           "assIdx" : 1,
        ///           "regionCode" : 12
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Belongs")]
        public object Get(string option)
        {
            JObject root = JObject.Parse(option);

            int assIdx = Convert.ToInt32(root["assIdx"].ToString());
            int regionCode = Convert.ToInt32(root["regionCode"].ToString());

            DataTable dt = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetAssAreaList(assIdx, regionCode);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt;

            return dic;
        }
    }
}
