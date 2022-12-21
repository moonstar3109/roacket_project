using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AdminSocialLoginController : ApiController
    {
        /// <remarks>
        /// 로그인
        /// 
        ///     {
        ///         "id" : "stadmin",
        ///         "pw" : "a12345678"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AdminSocialLogin")]
        public object Post([FromBody] AdminSocialLogin asl)
        {
            DataTable dt = null;

            using (var biz = new Api_Biz())
            {
                dt = biz.GetAdminSocialLogin(asl.id, asl.type);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (dt.Rows.Count > 0)
            {
                dic["success"] = true;
                dic["memberToken"] = clsJWT.createToken(
                                            Convert.ToInt32(dt.Rows[0]["memberIdx"].ToString()),
                                            dt.Rows[0]["memberId"].ToString(),
                                            dt.Rows[0]["memberName"].ToString()

                                            );
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "no data";
            }

            return dic;


        }
    }
}
