using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ExpulsionController : ApiController
    {
        public static string jToken = string.Empty;
        /// <remarks>
        /// 제명 신청
        /// 
        ///     {   
        ///         "type" : "club", 
        ///         "idx" : 1,
        ///         "memberIdx" : 1,
        ///         "srtDate" : "2022-07-28",
        ///         "endDate" : "2022-07-28",
        ///         "expReason" : "제명의 이유는 제4조 5항과 같다"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Expulsion")]
        public object Post([FromBody] Expulsion e)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }

            int result = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();
            int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.SetExpulsion(e.type, e.idx, e.memberIdx, 
                                              e.srtDate, 
                                              e.endDate,
                                              e.expReason,
                                              editMember);
                }
            }
            catch (Exception ex)
            {
                dic["success"] = false;
                dic["message"] = ex.ToString();
            }

            if(result != 0)
            {
                dic["success"] = false;
                dic["message"] = "처리중 오류";
                return dic;
            }
            dic["success"] = true;
            dic["message"] = "정상처리";
            return dic;
        }

        /// <remarks>
        /// 제명 내용 조회
        /// 
        ///     {   
        ///         "type" : 'club', 
        ///         "idx" : 1,
        ///         "memberIdx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Expulsion")]
        public object Get(string search)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();

            JObject root = JObject.Parse(search);

            string type = root["type"].ToString();
            int idx = Convert.ToInt32(root["idx"].ToString());
            int memberIdx = Convert.ToInt32(root["memberIdx"].ToString());

            DataTable dt = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetExpulsion(type, idx, memberIdx);
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
            }
            dic["success"] = true;
            dic["data"] = dt;
            return dic;

        }


        /// <remarks>
        /// 제명 수정
        /// 
        ///     {   
        ///         "expIdx" : 1, 
        ///         "srtDate" : "2022-07-28",
        ///         "endDate" : "2022-07-28",
        ///         "isExp" : true,
        ///         "expReason" : "제명의 이유는 제4조 5항과 같다"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Expulsion")]
        public object Put([FromBody] Expulsion e )
        {
            int rtn = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();
            int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());
            try
            {
                using (var biz = new Api_Biz())
                {
                    rtn = biz.EditExpulsion(e.expIdx, e.srtDate, e.endDate, e.isExp, e.expReason, editMember);
                }
            }
            catch (Exception ex)
            {
                dic["success"] = false;
                dic["message"] = ex.ToString();
                return dic;
            }

            if (rtn != 0)
            {
                dic["success"] = false;
                dic["message"] = "처리중 오류";
                return dic;
            }
            dic["success"] = true;
            dic["data"] = rtn;
            return dic;
        }


        /// <remarks>
        /// 제명 취소
        /// 
        ///     {   
        ///         "expIdx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Expulsion")]
        public object Delete([FromBody] Expulsion e)
        {
            int rtn = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    rtn = biz.CancelExpulsion(e.expIdx);
                }
            }
            catch (Exception ex)
            {
                dic["success"] = false;
                dic["message"] = ex.ToString();
            }

            if (rtn != 0)
            {
                dic["success"] = false;
                dic["message"] = "처리중 오류";
                return dic;
            }
            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
        }
    }
}
