using ADMIN.API.DA;
using Newtonsoft.Json.Linq;
using Statnco.FW.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AssociationNameController : ApiController
    {

        public static string jToken = string.Empty;

        /// <remarks>
        /// 협회명 중복체크
        /// 
        ///     {
        ///         "region" : 1,
        ///         "area" : 2,
        ///         "assName" : "부산시 협회"
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssociationName")]
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
            string assName = root["assName"].ToString();


            int result = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();


            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.AssNameOverlap(region, area, assName);
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




        /// <remarks>
        /// 협회명 반환
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///        
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssociationName/{id}")]
        public object Get(int id)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            DataTable dt = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetAssName(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }


            dic["success"] = true;
            dic["success"] = dt.Rows[0].DataRowToJson();

            return dic;
        }

    }
}
