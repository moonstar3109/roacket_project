using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AdminCheckController : ApiController
    {
        public static string jToken = string.Empty;

        [Route("AdminCheck")]
        public object Get()
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

            DataTable dt = null;
            using(var biz = new Api_Biz())
            {
                dt = biz.GetAdminCheck(memberIdx);
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["success"] = true;
            dic["assAdmin"] =  Convert.ToInt32(dt.Rows[0]["assAdmin"].ToString()) > 0 ? true : false;
            dic["clubAdmin"] =  Convert.ToInt32(dt.Rows[0]["clubAdmin"].ToString()) > 0  ? true : false;

            return dic;
        }
    }
}
