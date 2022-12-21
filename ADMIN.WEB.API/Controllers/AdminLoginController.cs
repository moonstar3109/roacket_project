using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Statnco.FW.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AdminLoginController : ApiController
    {
        /// <remarks>
        /// 로그인
        /// 
        ///     {
        ///         "id" : "statnco",
        ///         "pw" : "a12345678"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AdminLogin")]
        public object Post([FromBody] Login li)
        {
            DataSet ds = null;

            string pw = Crypto.fnGetSHA512(li.pw);
            using (var biz = new Api_Biz())
            {
                ds = biz.Login(li.id, pw);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dic["success"] = true;
                dic["memberToken"] = clsJWT.createToken(
                                            Convert.ToInt32(ds.Tables[0].Rows[0]["memberIdx"].ToString()),
                                            ds.Tables[0].Rows[0]["memberId"].ToString(),
                                            ds.Tables[0].Rows[0]["memberName"].ToString()
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
