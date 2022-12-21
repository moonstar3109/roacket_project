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
    public class AssClubWaitController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 가입대기 클럽 조회
        /// 
        ///     {   
        ///           "assIdx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssClubWait/{id}")]
        public object Get(int id)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataTable dt = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetWaitClubList(id);
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
                dic["data"] = new DataTable();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt;
            return dic;
        }


        /// <remarks>
        /// 가입대기 클럽 상태 변경
        /// 
        ///     {   
        ///           "assIdx" : 1,
        ///           "clubIdx" : 2,
        ///           "isOk" : 1,  [1: 승인, -1 : 거절]
        ///           "reason" : "거절사유"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssClubWait")]
        public object Post([FromBody] AssClubWait acw)
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
            int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0;
            
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.SetAssClubWait(acw.assIdx, acw.clubIdx, acw.isOk, editMember, acw.reason);
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
