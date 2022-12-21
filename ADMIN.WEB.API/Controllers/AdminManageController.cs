using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
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
    public class AdminManageController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 어드민 관리정보
        /// 
        ///     {
        ///         
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AdminManage")]
        public object Get()
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
            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();
            string memberId = clsJWT.isValidToken(jToken).Payload["memberId"].ToString();
            string memberName = clsJWT.isValidToken(jToken).Payload["memberName"].ToString();
            DataSet ds = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAdminManageDetail();
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }


            dic["success"] = true;
            dic["memberInfo"] = new
            {
                memberId = memberId,
                memberName = memberName
            };

            dic["clubCnt"] = Convert.ToInt32(ds.Tables[0].Rows[0]["clubCnt"].ToString());
            dic["memberCnt"] = Convert.ToInt32(ds.Tables[1].Rows[0]["memberCnt"].ToString());
            return dic;
        }
    }
}
