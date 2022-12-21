using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Data;
using ADMIN.API.DA;
using Statnco.FW.Util;

namespace ADMIN.WEB.API.Controllers
{
    public class ClubInfoController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 관리목록
        /// 
        ///     {
        ///         "id" : 1384
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClubInfo/{id}")]
        public object Get(int id)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }
            ////token 정보 조회
            //jToken = Request.Headers.Authorization.ToString();

            //int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            DataSet ds = null;

            try
            {
                using(var biz = new Api_Biz())
                {
                    ds = biz.GetClubInfo(id);
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            DataTable dt3 = ds.Tables[2];

            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic["success"] = true;
            dic["clubInfo"] = dt3.Rows[0].DataRowToJson();
            dic["memberCnt"] = dt1.Rows[0]["cnt"];
            dic["unSignCnt"] = dt2.Rows[0]["cnt"];

            return dic;
        }
    }
}
