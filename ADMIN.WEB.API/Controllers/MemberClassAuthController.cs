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
    public class MemberClassAuthController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 회원 급수 인증
        /// 
        ///     {   
        ///          "assIdx" : 1,
        ///          "classList" : [
        ///             {
        ///                 "clubIdx" : 3,
        ///                 "memberIdx" : 5
        ///             }
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("MemberClassAuth")]
        public object Post([FromBody] MemberClassAuth mca)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

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
            int result = 0;

            DataTable classList = new DataTable();
            classList.Columns.Add("clubIdx", typeof(int));
            classList.Columns.Add("memberIdx", typeof(int));

            for(int i = 0; i < mca.classList.Length; i++)
            {
                DataRow formatRow = classList.NewRow();

                formatRow["clubIdx"] = mca.classList[i].clubIdx;
                formatRow["memberIdx"] = mca.classList[i].memberIdx;

                classList.Rows.Add(formatRow);
            }

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.SetMemberClassAuth(mca.assIdx, classList, editMember);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if(result == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상처리";
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "DB error";
            }

            return dic;
        }
    }
}
