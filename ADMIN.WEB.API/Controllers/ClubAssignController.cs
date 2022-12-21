using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ClubAssignController : ApiController
    {

        public static string jToken = string.Empty;

        /// <remarks>
        /// 클럽장 양도
        /// 
        ///     {
        ///        "clubIdx" : 1,
        ///        "memberIdx" : 3
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("ClubAssign")]
        public object Post([FromBody] ClubAssign ca)
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
            int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int result = 0;

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.AssignClubMaster(ca.clubIdx, ca.memberIdx, editMember);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }


            if (result == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상처리 되었습니다";
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "db error";
            }

            return dic;
        }
    }
}
