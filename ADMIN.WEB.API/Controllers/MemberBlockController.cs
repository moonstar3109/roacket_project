using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
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
    public class MemberBlockController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 회원 차단설정
        /// 
        ///     {
        ///        "memberIdx" : 2,
        ///        "type" : 1, [ 1: 3일, 2 : 7일 , 3 : 30일 , 4 : 영구차단],
        ///        "start" : "2022-09-20",
        ///        "end" : "2022-09-23",
        ///        "memo" : ""
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("MemberBlock")]
        public object Post([FromBody] MemberBlock am)
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
            
            DataSet ds = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0;
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.EditMemberblock(am.memberIdx, am.type, am.memo, am.start, am.end, editMember);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }


            if (result != 0)
            {
                dic["success"] = false;
                dic["message"] = "db error";
                return dic;
            }

            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
        }

        /// <remarks>
        /// 차단 변경
        /// 
        ///     {
        ///        "memberIdx" : 2,
        ///        "type" : 1, [0 : 취소, 1: 3일, 2 : 7일 , 3 : 30일 , 4 : 영구차단],
        ///        "start" : "2022-09-20",
        ///        "end" : "2022-09-23",
        ///        "memo" : ""
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("MemberBlock")]
        public object Patch([FromBody] MemberBlock am)
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

            DataSet ds = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0;
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.editBlock(am.memberIdx, am.type, am.memo, am.start, am.end, editMember);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }


            if (result != 0)
            {
                dic["success"] = false;
                dic["message"] = "db error";
                return dic;
            }

            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
        }
    }
}
