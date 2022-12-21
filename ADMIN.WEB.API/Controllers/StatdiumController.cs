using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
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
    public class StatdiumController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 체육관 조회
        /// 
        ///     {
        ///        "regionCode" : 1,
        ///        "areaCode" : 1,
        ///        "search" : "부산"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Statdium")]
        public object Get(string filter)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }

            JObject root = JObject.Parse(filter);

            int regionCode = Convert.ToInt32(root["regionCode"].ToString());
            int areaCode = Convert.ToInt32(root["areaCode"].ToString());
            string search = root["search"].ToString() == "" ? string.Empty : root["search"].ToString();

            DataTable dt = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetStadium(regionCode, areaCode, search);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }
            if(dt.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["message"] = "data empty";
                return dic;
            }
            dic["success"] = true;
            dic["data"] = dt;
            return dic;
        }

        /// <remarks>
        /// 체육관 요청
        /// 
        ///     {
        ///        "regionCode" : 1,
        ///        "areaCode" : 1,
        ///        "stadiumName" : "원피스",
        ///        "clubIdx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Stadium")]
        public object Post([FromBody] Stadium st)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }

            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();

            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0;

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.SetStadiumRquest(memberIdx, st.regionCode, st.areaCode, st.stadiumName, st.clubIdx);
                }
            }
            catch (Exception e )
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if(result != 0)
            {
                dic["success"] = false;
                dic["message"] = "database error";
                return dic;
            }

            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";

            return dic;
        }
    }
}
