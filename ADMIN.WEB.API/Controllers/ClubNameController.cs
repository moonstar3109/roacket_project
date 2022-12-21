using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;
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
    public class ClubNameController : ApiController
    {

        public static string jToken = string.Empty;

        /// <remarks>
        /// 클럽명 변경 상세정보
        /// 
        ///     {
        ///        "editIdx" : 1
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("ClubName/{id}")]
        public object Get(int id)
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

            Dictionary<String, object> dic = new Dictionary<string, object>();
            DataTable dt = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetClubEditDetail(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt;
            return dic;
        }

        /// <remarks>
        /// 클럽명 변경 요청 목록
        /// 
        ///     {
        ///        "assIdx" : 1
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("ClubName")]
        public object Get(string filter)
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

            JObject root = JObject.Parse(filter);

            int assIdx = Convert.ToInt32(root["assIdx"].ToString());

            Dictionary<String, object> dic = new Dictionary<string, object>();

            DataSet ds = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetClubEditList(assIdx);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable dt = ds.Tables[0];
            DataTable cnt = ds.Tables[1];
            
            if(dt.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["data"] = new DataTable();
                dic["waitCnt"] = Convert.ToInt32(cnt.Rows[0]["waitCnt"].ToString());
            }
            else
            {
                dic["success"] = true;
                dic["data"] = dt;
                dic["waitCnt"] = Convert.ToInt32(cnt.Rows[0]["waitCnt"].ToString());

            }

            return dic;

        }



        /// <remarks>
        /// 클럽명 변경 신청
        /// 
        ///     {
        ///         "clubIdx" : 2,
        ///         "befClubName" : "a",
        ///         "aftClubName" : "B"
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("ClubName")]
        public object Post([FromBody] ClubName cn)
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
            int reqMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            int result = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();


            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.ReqClubNameChange(cn.clubIdx, cn.befClubName, cn.aftClubName, reqMember);
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
                dic["message"] = "정상 처리 되었습니다";
            }
            else 
            {
                dic["success"] = false;
                dic["message"] = "db error";
            }

            return dic;
        }


        /// <remarks>
        /// 클럽명 변경 승인/거절 
        /// 
        ///     {
        ///         "editIdx" : 1,
        ///         "isOk" : 1  
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("ClubName")]
        public object Patch([FromBody] ClubName cn)
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


            int result = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.ClubNameEdit(cn.editIdx, cn.isOk, editMember);
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
