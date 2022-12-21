using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ManageController : ApiController
    {
        public static string jToken = string.Empty;
        /// <remarks>
        /// 관리목록
        /// </remarks>
        /// <returns></returns>
        [Route("Manage")]
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
            

            DataSet ds = null;

            try
            {
                using(var biz = new Api_Biz())
                {
                    ds = biz.GetManageList(memberIdx);
                }
            }
            catch (Exception)
            {

                throw;
            }

            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];

            Dictionary<string, object> dic = new Dictionary<string, object>();
            Dictionary<string, object> dic2 = new Dictionary<string, object>();
            List<object> list = new List<object>();

            if(dt1.Rows.Count == 0 && dt2.Rows.Count == 0)
            {
                dic["success"] = false;
                dic["message"] = "no data";
            }
            else
            {
                dic2["assAdmin"] = dt1;
                dic2["clubAdmin"] = dt2;

                list.Add(dic2);
                dic["success"] = true;
                dic["data"] = list;

            }

            return dic;

        }

        /// <remarks>
        /// 관리정보
        /// 
        ///     {
        ///         "type" : "ass", or "club"
        ///         "idx" : 1  idx번호
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("Manage")]
        public object Post([FromBody] Manage m)
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

            DataSet ds;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetManageDetail(memberIdx, m.type, m.idx);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }
            
            dic["success"] = true;
            dic["memberInfo"] = ds.Tables[0];

            // 구조 변경
            dic["manageInfo"] = ds.Tables[1];



            if(m.type == "club")
            {
                dic["count1"] = Convert.ToInt32(ds.Tables[2].Rows[0]["totalCnt"].ToString());
                dic["count2"] = Convert.ToInt32(ds.Tables[3].Rows[0]["unSignCnt"].ToString());
            }
            else
            {
                dic["count1"] = Convert.ToInt32(ds.Tables[2].Rows[0]["clubCnt"].ToString());
                dic["count2"] = Convert.ToInt32(ds.Tables[3].Rows[0]["memberCnt"].ToString());

            }
            return dic;
            
        }
    }
}
 