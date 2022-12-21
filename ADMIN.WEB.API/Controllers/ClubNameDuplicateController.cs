using ADMIN.API.DA;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ClubNameDuplicateController : ApiController
    {
        /// <remarks>
        /// 클럽명 중복체크
        /// 
        ///     {
        ///         "region" : 1,
        ///         "area" : 2,
        ///         "clubName" : "센텀파워"
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("ClubNameDuplicate")]
        public object Get(string filter)
        {
            string server = ConfigurationManager.AppSettings.Get("server");
            Dictionary<string, object> check = new Dictionary<string, object>();

            //개발서버 토큰 필수제외
            if (server != "dev")
            {
                if (Request.Headers.Authorization == null)
                {
                    check["success"] = false;
                    check["message"] = "토큰이 없습니다";
                    return check;
                }

            }
            JObject root = JObject.Parse(filter);

            int region = Convert.ToInt32(root["region"].ToString());
            int area = Convert.ToInt32(root["area"].ToString());
            string clubName = root["clubName"].ToString();


            int result = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();


            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.ClubNameOverlap(region, area, clubName);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if (result == 1)
            {
                dic["success"] = true;
                dic["message"] = "중복";
            }
            else if (result == 0)
            {
                dic["success"] = true;
                dic["message"] = "사용 가능한 이름";
            }

            return dic;
        }


    }
}
